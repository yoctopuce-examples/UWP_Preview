using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace com.yoctopuce.YoctoAPI
{



    /// <summary>
    /// YSSDP Class: network discovery using ssdp
    /// <p/>
    /// this class is used to detect all YoctoHub using SSDP
    /// </summary>
    public class YSSDP
    {


        internal interface YSSDPReportInterface
        {
            /// <param name="serial">          : the serial number of the discovered Hub </param>
            /// <param name="urlToRegister">   : the new URL to register </param>
            /// <param name="urlToUnregister"> : the old URL to unregister </param>
            void HubDiscoveryCallback(string serial, string urlToRegister, string urlToUnregister);
        }

        private const int SSDP_PORT = 1900;
        private const string SSDP_MCAST_ADDR = "239.255.255.250";
        private const string SSDP_URN_YOCTOPUCE = "urn:yoctopuce-com:device:hub:1";
        private static readonly string SSDP_DISCOVERY_MESSAGE = "M-SEARCH * HTTP/1.1\r\n" + "HOST: " + SSDP_MCAST_ADDR + ":" + Convert.ToString(SSDP_PORT) + "\r\n" + "MAN: \"ssdp:discover\"\r\n" + "MX: 5\r\n" + "ST: " + SSDP_URN_YOCTOPUCE + "\r\n" + "\r\n";
        private const string SSDP_NOTIFY = "NOTIFY * HTTP/1.1";
        private const string SSDP_HTTP = "HTTP/1.1 200 OK";


        private readonly YAPIContext _yctx;
        //private readonly List<NetworkInterface> _netInterfaces = new List<NetworkInterface>(1);
        private readonly Dictionary<string, YSSDPCacheEntry> _cache = new Dictionary<string, YSSDPCacheEntry>();
        IPAddress mMcastAddr = IPAddress.Parse(SSDP_MCAST_ADDR);
        //private InetAddress mMcastAddr;
        private bool _Listening;
        private YSSDPReportInterface _callbacks;
        //private Thread[] _listenMSearchThread;
        //private Thread[] _listenBcastThread;

        internal YSSDP(YAPIContext yctx)
        {
            _yctx = yctx;
        }

        internal virtual void reset()
        {
            //_netInterfaces.Clear();
            _cache.Clear();
            //mMcastAddr = null;
            _Listening = false;
            _callbacks = null;
            //_listenMSearchThread = null;
            //_listenBcastThread = null;
        }

        internal virtual void addCallback(YSSDPReportInterface callback)
        {
            if (_callbacks == callback) {
                // already started
                return;
            }
            _callbacks = callback;
            if (!_Listening) {
                try {
                    startListening();
                } catch (IOException e) {
                    throw new YAPI_Exception(YAPI.IO_ERROR, "Unable to start SSDP thread : " + e.ToString());
                }
            }
        }


        private void updateCache(string uuid, string url, int cacheValidity)
        {
            YSSDPCacheEntry entry;
            if (cacheValidity <= 0) {
                cacheValidity = 1800;
            }
            cacheValidity *= 1000;

            if (_cache.ContainsKey(uuid)) {
                entry = _cache[uuid];
                if (!entry.URL.Equals(url)) {
                    _callbacks.HubDiscoveryCallback(entry.Serial, url, entry.URL);
                    entry.URL = url;
                } else {
                    _callbacks.HubDiscoveryCallback(entry.Serial, url, null);
                }
                entry.resetExpiration(cacheValidity);
                return;
            }
            entry = new YSSDPCacheEntry(uuid, url, cacheValidity);
            _cache[uuid] = entry;
            _callbacks.HubDiscoveryCallback(entry.Serial, entry.URL, null);
        }

        private void checkCacheExpiration()
        {
            List<string> to_remove = new List<string>();
            foreach (YSSDPCacheEntry entry in _cache.Values) {
                if (entry.hasExpired()) {
                    _callbacks.HubDiscoveryCallback(entry.Serial, null, entry.URL);
                    to_remove.Add(entry.UUID);
                }
            }
            foreach (string uuid in to_remove) {
                _cache.Remove(uuid);
            }
        }


        private void startListening()
        {

            _Listening = true;
        }


        internal virtual void Stop()
        {

            _cache.Clear();
        }


        private void parseIncomingMessage(string message)
        {
            int i = 0;
            string location = null;
            string usn = null;
            string cache = null;


            string[] lines = message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (!lines[i].Equals(SSDP_HTTP) && !lines[i].Equals(SSDP_NOTIFY)) {
                return;
            }
            for (i = 0; i < lines.Length; i++) {
                int pos = lines[i].IndexOf(":", StringComparison.Ordinal);
                if (pos <= 0) {
                    continue;
                }
                if (lines[i].StartsWith("LOCATION", StringComparison.Ordinal)) {
                    location = lines[i].Substring(pos + 1).Trim();
                } else if (lines[i].StartsWith("USN", StringComparison.Ordinal)) {
                    usn = lines[i].Substring(pos + 1).Trim();
                } else if (lines[i].StartsWith("CACHE-CONTROL", StringComparison.Ordinal)) {
                    cache = lines[i].Substring(pos + 1).Trim();
                }
            }
            if (location != null && usn != null && cache != null) {
                // parse USN
                int posuuid = usn.IndexOf(':');
                if (posuuid < 0) {
                    return;
                }
                posuuid++;
                int posurn = usn.IndexOf("::", posuuid, StringComparison.Ordinal);
                if (posurn < 0) {
                    return;
                }
                string uuid = usn.Substring(posuuid, posurn - posuuid).Trim();
                string urn = usn.Substring(posurn + 2).Trim();
                if (!urn.Equals(SSDP_URN_YOCTOPUCE)) {
                    return;
                }
                // parse Location
                if (location.StartsWith("http://", StringComparison.Ordinal)) {
                    location = location.Substring(7);
                }
                int posslash = location.IndexOf('/');
                if (posslash > 0) {
                    location = location.Substring(0, posslash);
                }

                int poscache = cache.IndexOf('=');
                if (poscache < 0) {
                    return;
                }
                cache = cache.Substring(poscache + 1).Trim();
                int cacheVal = Int32.Parse(cache);
                updateCache(uuid, location, cacheVal);
            }
        }

    }

}