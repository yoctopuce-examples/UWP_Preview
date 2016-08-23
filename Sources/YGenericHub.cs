/*********************************************************************
 *
 * $Id: YGenericHub.cs 25204 2016-08-17 13:52:16Z seb $
 *
 * High-level programming interface, common to all modules
 *
 * - - - - - - - - - License information: - - - - - - - - -
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing
 *  with Yoctopuce products.
 *
 *  You may reproduce and distribute copies of this file in
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    internal abstract class YGenericHub
    {

        internal const int NOTIFY_V2_LEGACY = 0; // unused (reserved for compatibility with legacy notifications)
        internal const int NOTIFY_V2_6RAWBYTES = 1; // largest type: data is always 6 bytes
        internal const int NOTIFY_V2_TYPEDDATA = 2; // other types: first data byte holds the decoding format
        internal const int NOTIFY_V2_FLUSHGROUP = 3; // no data associated

        // stream type
        internal const int YSTREAM_EMPTY = 0;
        internal const int YSTREAM_TCP = 1;
        internal const int YSTREAM_TCP_CLOSE = 2;
        internal const int YSTREAM_NOTICE = 3;
        internal const int YSTREAM_REPORT = 4;
        internal const int YSTREAM_META = 5;
        internal const int YSTREAM_REPORT_V2 = 6;
        internal const int YSTREAM_NOTICE_V2 = 7;
        internal const int YSTREAM_TCP_NOTIF = 8;
        internal const int YSTREAM_TCP_ASYNCCLOSE = 9;


        internal const int USB_META_UTCTIME = 1;
        internal const int USB_META_DLFLUSH = 2;
        internal const int USB_META_ACK_D2H_PACKET = 3;
        internal const int USB_META_WS_ANNOUNCE = 4;
        internal const int USB_META_WS_AUTHENTICATION = 5;
        internal const int USB_META_WS_ERROR = 6;
        internal const int USB_META_ACK_UPLOAD = 7;

        internal const int USB_META_UTCTIME_SIZE = 5;
        internal const int USB_META_DLFLUSH_SIZE = 1;
        internal const int USB_META_ACK_D2H_PACKET_SIZE = 2;
        internal static readonly int USB_META_WS_ANNOUNCE_SIZE = 8 + YAPI.YOCTO_SERIAL_LEN;
        internal const int USB_META_WS_AUTHENTICATION_SIZE = 28;
        internal const int USB_META_WS_ERROR_SIZE = 6;
        internal const int USB_META_ACK_UPLOAD_SIZE = 6;

        internal const int USB_META_WS_PROTO_V1 = 1; // adding authentication support
        internal const int USB_META_WS_PROTO_V2 = 2; // adding API packets throttling
        internal const int VERSION_SUPPORT_ASYNC_CLOSE = 1;


        internal const int USB_META_WS_VALID_SHA1 = 1;
        internal const int USB_META_WS_AUTH_FLAGS_RW = 2;


        private const int PUBVAL_LEGACY = 0; // 0-6 ASCII characters (normally sent as YSTREAM_NOTICE)
        private const int PUBVAL_1RAWBYTE = 1; // 1 raw byte  (=2 characters)
        private const int PUBVAL_2RAWBYTES = 2; // 2 raw bytes (=4 characters)
        private const int PUBVAL_3RAWBYTES = 3; // 3 raw bytes (=6 characters)
        private const int PUBVAL_4RAWBYTES = 4; // 4 raw bytes (=8 characters)
        private const int PUBVAL_5RAWBYTES = 5; // 5 raw bytes (=10 characters)
        private const int PUBVAL_6RAWBYTES = 6; // 6 hex bytes (=12 characters) (sent as V2_6RAWBYTES)
        private const int PUBVAL_C_LONG = 7; // 32-bit C signed integer
        private const int PUBVAL_C_FLOAT = 8; // 32-bit C float
        private const int PUBVAL_YOCTO_FLOAT_E3 = 9; // 32-bit Yocto fixed-point format (e-3)
        private const int PUBVAL_YOCTO_FLOAT_E6 = 10; // 32-bit Yocto fixed-point format (e-6)

        public const long YPROG_BOOTLOADER_TIMEOUT = 10000;
        protected internal readonly YAPIContext _yctx;
        internal readonly HTTPParams _http_params;
        protected internal int _hubidx;
        protected internal long _notifyTrigger = 0;
        protected internal object _notifyHandle = null;
        protected internal ulong _devListValidity = 500;
        protected internal ulong _devListExpires = 0;
        protected internal readonly ConcurrentDictionary<int, string> _serialByYdx = new ConcurrentDictionary<int, string>();
        protected internal Dictionary<string, YDevice> _devices = new Dictionary<string, YDevice>();
        protected internal readonly bool _reportConnnectionLost;
        private string _hubSerialNumber = null;

        public YGenericHub(YAPIContext yctx, HTTPParams httpParams, int idx, bool reportConnnectionLost)
        {
            _yctx = yctx;
            _hubidx = idx;
            _reportConnnectionLost = reportConnnectionLost;
            _http_params = httpParams;
        }

        internal abstract void imm_release();

        internal abstract string RootUrl { get; }

        internal abstract bool imm_isSameHub(string url, object request, object response, object session);

        internal abstract Task startNotifications();

        internal abstract Task stopNotifications();


        internal static string imm_decodePubVal(int typeV2, byte[] funcval, int ofs, int funcvallen)
        {
            string buffer = "";
            int endp;

            if (typeV2 == NOTIFY_V2_6RAWBYTES || typeV2 == NOTIFY_V2_TYPEDDATA) {
                int funcValType;

                if (typeV2 == NOTIFY_V2_6RAWBYTES) {
                    funcValType = PUBVAL_6RAWBYTES;
                } else {
                    funcValType = funcval[ofs++] & 0xff;
                }
                switch (funcValType) {
                    case PUBVAL_LEGACY:
                        // fallback to legacy handling, just in case
                        break;
                    case PUBVAL_1RAWBYTE:
                    case PUBVAL_2RAWBYTES:
                    case PUBVAL_3RAWBYTES:
                    case PUBVAL_4RAWBYTES:
                    case PUBVAL_5RAWBYTES:
                    case PUBVAL_6RAWBYTES:
                        // 1..5 hex bytes
                        for (int i = 0; i < funcValType; i++) {
                            int c = funcval[ofs++] & 0xff;
                            int b = c >> 4;
                            buffer += (b > 9) ? b + 'a' - 10 : b + '0';
                            b = c & 0xf;
                            buffer += (b > 9) ? b + 'a' - 10 : b + '0';
                        }
                        return buffer;
                    case PUBVAL_C_LONG:
                    case PUBVAL_YOCTO_FLOAT_E3:
                        // 32bit integer in little endian format or Yoctopuce 10-3 format
                        int numVal = funcval[ofs++] & 0xff;
                        numVal += (int)(funcval[ofs++] & 0xff) << 8;
                        numVal += (int)(funcval[ofs++] & 0xff) << 16;
                        numVal += (int)(funcval[ofs++] & 0xff) << 24;
                        if (funcValType == PUBVAL_C_LONG) {
                            return string.Format("{0:D}", numVal);
                        } else {
                            buffer = string.Format("{0:F3}", numVal / 1000.0);
                            endp = buffer.Length;
                            while (endp > 0 && buffer[endp - 1] == '0') {
                                --endp;
                            }
                            if (endp > 0 && buffer[endp - 1] == '.') {
                                --endp;
                                buffer = buffer.Substring(0, endp);
                            }
                            return buffer;
                        }
                    case PUBVAL_C_FLOAT:
                        //todo: Verifiy 32bits notification decoding (ex: Yocto-GPS)
                        // 32bit (short) float
                        float floatVal = System.BitConverter.ToSingle(funcval, 0);
                        buffer = string.Format("{0:F6}", floatVal);
                        endp = buffer.Length;
                        while (endp > 0 && buffer[endp - 1] == '0') {
                            --endp;
                        }
                        if (endp > 0 && buffer[endp - 1] == '.') {
                            --endp;
                            buffer = buffer.Substring(0, endp);
                        }
                        return buffer;
                    default:
                        return "?";
                }
            }

            // Legacy handling: just pad with NULL up to 7 chars
            int len = 0;
            while (len < YAPI.YOCTO_PUBVAL_SIZE && len < funcvallen) {
                if (funcval[len + ofs] == 0) {
                    break;
                }
                len++;
            }
            return YAPI.DefaultEncoding.GetString(funcval, ofs, len);
        }


        protected internal virtual async Task updateFromWpAndYp(List<WPEntry> whitePages, Dictionary<string, List<YPEntry>> yellowPages)
        {

            // by default consider all known device as unplugged
            List<YDevice> toRemove = new List<YDevice>(_devices.Values);

            foreach (WPEntry wp in whitePages) {
                string serial = wp.SerialNumber;
                if (_devices.ContainsKey(serial)) {
                    // already there
                    YDevice currdev = _devices[serial];
                    if (!currdev.LogicalName.Equals(wp.LogicalName)) {
                        // Reindex device from its own data
                        await currdev.refresh();
                        _yctx._pushPlugEvent(YAPIContext.PlugEvent.Event.CHANGE, serial);
                    } else if (currdev.Beacon > 0 != wp.Beacon > 0) {
                        await currdev.refresh();
                    }
                    toRemove.Remove(currdev);
                } else {
                    YDevice dev = new YDevice(this, wp, yellowPages);
                    _yctx._yHash.imm_reindexDevice(dev);
                    _devices[serial] = dev;
                    _yctx._pushPlugEvent(YAPIContext.PlugEvent.Event.PLUG, serial);
                    _yctx._Log("HUB: device " + serial + " has been plugged\n");
                }
            }

            foreach (YDevice dev in toRemove) {
                string serial = dev.SerialNumber;
                _yctx._pushPlugEvent(YAPIContext.PlugEvent.Event.UNPLUG, serial);
                _yctx._Log("HUB: device " + serial + " has been unplugged\n");
                _devices.Remove(serial);
            }

            if (_hubSerialNumber == null) {
                foreach (WPEntry wp in whitePages) {
                    if (wp.NetworkUrl.Equals("")) {
                        _hubSerialNumber = wp.SerialNumber;
                    }
                }
            }
            _yctx._yHash.imm_reindexYellowPages(yellowPages);

        }

        internal virtual string SerialNumber {
            get {
                return _hubSerialNumber;
            }
        }

        public virtual string imm_get_urlOf(string serialNumber)
        {
            foreach (YDevice dev in _devices.Values) {
                string devSerialNumber = dev.SerialNumber;
                if (devSerialNumber.Equals(serialNumber)) {
                    return _http_params.imm_getUrl(true, false) + dev._wpRec.NetworkUrl;
                }
            }
            return _http_params.imm_getUrl(true, false);
        }

        public virtual List<string> imm_get_subDeviceOf(string serialNumber)
        {
            List<string> res = new List<string>();
            foreach (YDevice dev in _devices.Values) {
                string devSerialNumber = dev.SerialNumber;
                if (devSerialNumber.Equals(serialNumber)) {
                    if (!dev._wpRec.NetworkUrl.Equals("")) {
                        //
                        res.Clear();
                        return res;
                    }
                }
                res.Add(devSerialNumber);
            }
            return res;
        }

        protected internal virtual void imm_handleValueNotification(string serial, string funcid, string value)
        {
            string hwid = serial + "." + funcid;

            _yctx._yHash.imm_setFunctionValue(hwid, value);
            YFunction conn_fn = _yctx._GetValueCallback(hwid);
            if (conn_fn != null) {
                _yctx._PushDataEvent(new YAPIContext.DataEvent(conn_fn, value));
            }
        }

        //called from Jni
        protected internal virtual void imm_handleTimedNotification(string serial, string funcid, double deviceTime, sbyte[] report)
        {
            List<int> arrayList = new List<int>(report.Length);
            foreach (sbyte b in report) {
                int i = b & 0xff;
                arrayList.Add(i);
            }
            imm_handleTimedNotification(serial, funcid, deviceTime, arrayList);
        }


        protected internal virtual void imm_handleTimedNotification(string serial, string funcid, double deviceTime, List<int> report)
        {
            string hwid = serial + "." + funcid;
            YFunction func = _yctx._GetTimedReportCallback(hwid);
            if (func != null) {
                _yctx._PushDataEvent(new YAPIContext.DataEvent(func, deviceTime, report));
            }
        }

        internal abstract Task updateDeviceListAsync(bool forceupdate);

        public abstract Task<List<string>> getBootloaders();

        internal abstract Task<int> ping(uint mstimeout);

        public static string imm_getAPIVersion()
        {
            return "";
        }

        internal delegate Task UpdateProgress(int percent, string message);

        internal abstract Task<List<string>> firmwareUpdate(string serial, YFirmwareFile firmware, byte[] settings, UpdateProgress progress);

        internal delegate void RequestAsyncResult(object context, byte[] result, int error, string errmsg);

        internal delegate void RequestProgress(object context, int acked, int total);

        internal abstract Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, RequestAsyncResult asyncResult, object asyncContext);

        internal abstract Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, RequestProgress progress, object context);


        protected internal class HTTPParams
        {

            internal readonly string _host;
            internal readonly int _port;
            internal readonly string _user;
            internal readonly string _pass;
            internal readonly string _proto;

            public HTTPParams(string url)
            {
                int pos = 0;
                if (url.StartsWith("ws://", StringComparison.Ordinal)) {
                    pos = 5;
                    _proto = "ws";
                } else if (url.StartsWith("usb://", StringComparison.Ordinal)) {
                    pos = 6;
                    _proto = "usb";
                } else {
                    _proto = "http";
                    if (url.StartsWith("http://", StringComparison.Ordinal)) {
                        pos = 7;
                    }
                }
                int end_auth = url.IndexOf('@', pos);
                int end_user = url.IndexOf(':', pos);
                if (end_auth >= 0 && end_user >= 0 && end_user < end_auth) {
                    _user = url.Substring(pos, end_user - pos);
                    _pass = url.Substring(end_user + 1, end_auth - (end_user + 1));
                    pos = end_auth + 1;
                } else {
                    _user = "";
                    _pass = "";
                }
                if (url.Length > pos && url[pos] == '@') {
                    pos++;
                }
                int end_url = url.IndexOf('/', pos);
                if (end_url < 0) {
                    end_url = url.Length;
                }
                int portpos = url.IndexOf(':', pos);
                if (portpos > 0 && portpos < end_url) {
                    _host = url.Substring(pos, portpos - pos);
                    _port = int.Parse(url.Substring(portpos + 1, end_url - (portpos + 1)));
                } else {
                    _host = url.Substring(pos, end_url - pos);
                    _port = 4444;
                }
            }

            internal virtual string Host {
                get {
                    return _host;
                }
            }

            internal virtual string Pass {
                get {
                    return _pass;
                }
            }

            internal virtual int Port {
                get {
                    return _port;
                }
            }

            internal virtual string User {
                get {
                    return _user;
                }
            }

            internal virtual string Url {
                get {
                    return imm_getUrl(false, true);
                }
            }


            internal virtual string imm_getUrl(bool withProto, bool withUserPass)
            {
                StringBuilder url = new StringBuilder();
                if (withProto) {
                    url.Append(_proto).Append("://");
                }
                if (withUserPass && !_user.Equals("")) {
                    url.Append(_user);
                    if (!_pass.Equals("")) {
                        url.Append(":");
                        url.Append(_pass);
                    }
                    url.Append("@");
                }
                url.Append(_host);
                url.Append(":");
                url.Append(_port);
                return url.ToString();
            }

            public virtual bool WebSocket {
                get {
                    return _proto.Equals("ws");
                }
            }

            public virtual bool imm_hasAuthParam()
            {
                return !_user.Equals("");
            }
        }
    }

}