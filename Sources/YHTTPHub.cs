/*********************************************************************
 *
 * $Id: YHTTPHub.cs 25246 2016-08-22 15:28:10Z seb $
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
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace com.yoctopuce.YoctoAPI
{


    internal class YHTTPHub : YGenericHub
    {
        public const int YIO_DEFAULT_TCP_TIMEOUT = 20000;
        public const int YIO_1_MINUTE_TCP_TIMEOUT = 60000;
        public const int YIO_10_MINUTES_TCP_TIMEOUT = 600000;

        private NotificationHandler _notificationHandler;
        private string _http_realm = "";
        private string _nounce = "";
        private string _serial = "";
        private int _nounce_count;
        private string _ha1 = "";
        private string _opaque = "";
        private readonly Random _randGen = new Random();
        private int _authRetryCount;
        internal bool _writeProtected;

        internal virtual bool imm_needRetryWithAuth()
        {
            return _http_params.User.Length != 0 && _http_params.Pass.Length != 0 && _authRetryCount++ <= 3;
        }

        internal virtual void imm_authSucceded()
        {
            _authRetryCount = 0;
        }

        // Update the hub internal variables according
        // to a received header with WWW-Authenticate
        //todo: verify authentification
        internal virtual void imm_parseWWWAuthenticate(string header)
        {
            int pos = header.IndexOf("\r\nWWW-Authenticate:", StringComparison.Ordinal);
            if (pos == -1) {
                return;
            }
            header = header.Substring(pos + 19);
            int eol = header.IndexOf('\r');
            if (eol >= 0) {
                header = header.Substring(0, eol);
            }
            _http_realm = "";
            _nounce = "";
            _opaque = "";
            _nounce_count = 0;

            string[] tags = header.Split(' ');
            char[] delim = {
                    '[',
                    '=',
                    '\"',
                    ',',
                    ']'
                };
            foreach (string tag in tags) {
                string[] parts = tag.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                string name, value;
                if (parts.Length == 2) {
                    name = parts[0];
                    value = parts[1];
                } else if (parts.Length == 3) {
                    name = parts[0];
                    value = parts[2];
                } else {
                    continue;
                }
                switch (name) {
                    case "realm":
                        _http_realm = value;
                        break;
                    case "nonce":
                        _nounce = value;
                        break;
                    case "opaque":
                        _opaque = value;
                        break;
                }
            }

            string plaintext = _http_params.User + ":" + _http_realm + ":" + _http_params.Pass;
            byte[] bytes_to_hash = YAPI.DefaultEncoding.GetBytes(plaintext);
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer md5digest = alg.HashData(bytes_to_hash.AsBuffer());
            byte[] digest = md5digest.ToArray();
            _ha1 = YAPIContext.imm_bytesToHexStr(digest, 0, digest.Length).ToLower();
        }


        // Return an Authorization header for a given request
        internal virtual string imm_getAuthorization(string request)
        {
            if (_http_params.User.Length == 0 || _http_realm.Length == 0) {
                return "";
            }
            _nounce_count++;
            int pos = request.IndexOf(' ');
            string method = request.Substring(0, pos);
            int enduri = request.IndexOf(' ', pos + 1);
            if (enduri < 0) {
                enduri = request.Length;
            }
            string uri = request.Substring(pos + 1, enduri - (pos + 1));
            string nc = string.Format("{0:x8}", _nounce_count);
            string cnonce = string.Format("{0:x8}", _randGen.Next());

            string plaintext = method + ":" + uri;
            byte[] bytes_to_hash = YAPI.DefaultEncoding.GetBytes(plaintext);
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer md5digest = alg.HashData(bytes_to_hash.AsBuffer());
            byte[] digest = md5digest.ToArray();
            string ha2 = YAPIContext.imm_bytesToHexStr(digest, 0, digest.Length).ToLower();

            plaintext = _ha1 + ":" + _nounce + ":" + nc + ":" + cnonce + ":auth:" + ha2;
            bytes_to_hash = YAPI.DefaultEncoding.GetBytes(plaintext);
            alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            md5digest = alg.HashData(bytes_to_hash.AsBuffer());
            digest = md5digest.ToArray();
            string response = YAPIContext.imm_bytesToHexStr(digest, 0, digest.Length).ToLower();
            //System.out.print(String.format("Auth Resp ha1=%s nonce=%s nc=%s cnouce=%s ha2=%s -> %s\n", _ha1, _nounce, nc, cnonce, ha2, response));
            return
                string.Format(
                    "Authorization: Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", qop=auth, nc={4}, cnonce=\"{5}\", response=\"{6}\", opaque=\"{7}\"\r\n",
                    _http_params.User, _http_realm, _nounce, uri, nc, cnonce, response, _opaque);
        }


        internal YHTTPHub(YAPIContext yctx, int idx, HTTPParams httpParams, bool reportConnnectionLost)
            : base(yctx, httpParams, idx, reportConnnectionLost)
        {
        }


        internal override async Task startNotifications()
        {
            if (_notificationHandler != null) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "notification already started");
            }
            if (_http_params.WebSocket) {
                _notificationHandler = new WSNotificationHandler(this);
            } else {
                _notificationHandler = new TCPNotificationHandler(this);
            }
            await _notificationHandler.Start();
        }

        internal override async Task stopNotifications()
        {
            if (_notificationHandler != null) {
                bool requestsUnfinished = await _notificationHandler.Stop(YHTTPRequest.MAX_REQUEST_MS);
                if (requestsUnfinished) {
                    _yctx._Log(string.Format("Stop hub {0} before all async request has ended", Host));
                }
                _notificationHandler = null;
            }
        }

        internal override void imm_release()
        {

        }

        internal override string RootUrl {
            get { return _http_params.Url; }
        }

        internal override bool imm_isSameHub(string url, object request, object response, object session)
        {
            HTTPParams param = new HTTPParams(url);
            bool url_equals = param.imm_getUrl(false, false).Equals(_http_params.imm_getUrl(false, false));
            return url_equals;
        }


        internal override async Task updateDeviceListAsync(bool forceupdate)
        {

            ulong now = YAPI.GetTickCount();
            if (forceupdate) {
                _devListExpires = 0;
            }
            if (_devListExpires > now) {
                return;
            }
            if (!_notificationHandler.Connected) {
                if (_reportConnnectionLost) {
                    throw new YAPI_Exception(YAPI.TIMEOUT, "hub " + _http_params.Url + " is not reachable");
                } else {
                    return;
                }
            }

            string json_data;
            try {
                byte[] data = await _notificationHandler.hubRequestSync("GET /api.json", null,
                    YIO_DEFAULT_TCP_TIMEOUT);
                json_data = YAPI.DefaultEncoding.GetString(data);
            } catch (YAPI_Exception) {
                if (_reportConnnectionLost) {
                    throw;
                }
                return;
            }

            Dictionary<string, List<YPEntry>> yellowPages = new Dictionary<string, List<YPEntry>>();
            List<WPEntry> whitePages = new List<WPEntry>();

            YJSONObject loadval = new YJSONObject(json_data);
            try {
                loadval.Parse();
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
            if (!loadval.Has("services") || !loadval.GetYJSONObject("services").Has("whitePages")) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Device " + _http_params.Host + " is not a hub");
            }
            _serial = loadval.GetYJSONObject("module").GetString("serialNumber");
            YJSONArray whitePages_json = loadval.GetYJSONObject("services").GetYJSONArray("whitePages");
            YJSONObject yellowPages_json = loadval.GetYJSONObject("services").GetYJSONObject("yellowPages");
            if (loadval.Has("network")) {
                string adminpass = loadval.GetYJSONObject("network").GetString("adminPassword");
                _writeProtected = adminpass.Length > 0;
            }
            // Reindex all functions from yellow pages
            //HashMap<String, Boolean> refresh = new HashMap<String, Boolean>();
            List<string> keys = yellowPages_json.Keys();
            foreach (string classname in keys) {
                YJSONArray yprecs_json = yellowPages_json.GetYJSONArray(classname);
                List<YPEntry> yprecs_arr = new List<YPEntry>(yprecs_json.Length);
                for (int i = 0; i < yprecs_json.Length; i++) {
                    YPEntry yprec = new YPEntry(yprecs_json.GetYJSONObject(i));
                    yprecs_arr.Add(yprec);
                }
                yellowPages[classname] = yprecs_arr;
            }

            _serialByYdx.Clear();
            // Reindex all devices from white pages
            for (int i = 0; i < whitePages_json.Length; i++) {
                YJSONObject jsonObject = whitePages_json.GetYJSONObject(i);
                WPEntry devinfo = new WPEntry(jsonObject);
                int index = jsonObject.GetInt("index");
                _serialByYdx[index] = devinfo.SerialNumber;
                whitePages.Add(devinfo);
            }
            await updateFromWpAndYp(whitePages, yellowPages);

            // reset device list cache timeout for this hub
            now = YAPI.GetTickCount();
            _devListExpires = now + _devListValidity;
        }

        internal override async Task<List<string>> firmwareUpdate(string serial, YFirmwareFile firmware, byte[] settings,
            UpdateProgress progress)
        {
            bool use_self_flash = false;
            string baseurl = "";
            bool need_reboot = true;
            if (_serial.StartsWith("VIRTHUB", StringComparison.Ordinal)) {
                use_self_flash = false;
            } else if (serial.Equals(_serial)) {
                use_self_flash = true;
            } else {
                // check if subdevice support self flashing
                try {
                    await _notificationHandler.hubRequestSync("GET /bySerial/" + serial + "/flash.json?a=state", null,
                        YIO_DEFAULT_TCP_TIMEOUT);
                    baseurl = "/bySerial/" + serial;
                    use_self_flash = true;
                } catch (YAPI_Exception) { }
            }
            //5% -> 10%
            await progress(5, "Enter in bootloader");
            List<string> bootloaders = await getBootloaders();
            bool is_shield = serial.StartsWith("YHUBSHL1", StringComparison.Ordinal);
            foreach (string bl in bootloaders) {
                if (bl.Equals(serial)) {
                    need_reboot = false;
                } else if (is_shield) {
                    if (bl.StartsWith("YHUBSHL1", StringComparison.Ordinal)) {
                        throw new YAPI_Exception(YAPI.IO_ERROR, "Only one YoctoHub-Shield is allowed in update mode");
                    }
                }
            }
            if (!use_self_flash && need_reboot && bootloaders.Count >= 4) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Too many devices in update mode");
            }
            // ensure flash engine is not busy
            byte[] bytes = await _notificationHandler.hubRequestSync("GET" + baseurl + "/flash.json?a=state", null,
                YIO_DEFAULT_TCP_TIMEOUT);
            string uploadstate = YAPI.DefaultEncoding.GetString(bytes);
            YJSONObject uploadres = new YJSONObject(uploadstate);
            uploadres.Parse();
            string state = uploadres.GetYJSONString("state").GetString();
            if (state.Equals("uploading") || state.Equals("flashing")) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Cannot start firmware update: busy (" + state + ")");
            }
            // start firmware upload
            //10% -> 40%
            await progress(10, "Send firmware file");
            byte[] head_body = YDevice.imm_formatHTTPUpload("firmware", firmware.Data);
            await _notificationHandler.hubRequestSync("POST " + baseurl + "/upload.html", head_body, 0);
            //check firmware upload result
            bytes = await _notificationHandler.hubRequestSync("GET " + baseurl + "/flash.json?a=state", null,
                YIO_10_MINUTES_TCP_TIMEOUT);
            string uploadresstr = YAPI.DefaultEncoding.GetString(bytes);
            uploadres = new YJSONObject(uploadresstr);
            uploadres.Parse();
            state = uploadres.GetString("state");
            if (state != "valid") {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Upload of firmware failed: invalid firmware(" + state + ")");
            }
            if (uploadres.GetInt("progress") != 100) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Upload of firmware failed: incomplete upload");
            }
            if (use_self_flash) {
                byte[] startupConf;
                string json = YAPI.DefaultEncoding.GetString(settings);
                YJSONObject jsonObject = new YJSONObject(json);
                jsonObject.Parse();
                YJSONObject settingsOnly = jsonObject.GetYJSONObject("api");
                settingsOnly.Remove("services");
                string startupConfStr = settingsOnly.ToString();
                startupConf = YAPI.DefaultEncoding.GetBytes(startupConfStr);
                await progress(20, "Upload startupConf.json");
                head_body = YDevice.imm_formatHTTPUpload("startupConf.json", startupConf);
                await _notificationHandler.hubRequestSync("POST " + baseurl + "/upload.html", head_body,
                    YIO_10_MINUTES_TCP_TIMEOUT);
                await progress(20, "Upload firmwareConf");
                head_body = YDevice.imm_formatHTTPUpload("firmwareConf", startupConf);
                await _notificationHandler.hubRequestSync("POST " + baseurl + "/upload.html", head_body,
                    YIO_10_MINUTES_TCP_TIMEOUT);
            }

            //40%-> 80%
            if (use_self_flash) {
                await progress(40, "Flash firmware");
                // the hub itself -> reboot in autoflash mode
                await _notificationHandler.hubRequestSync(
                    "GET " + baseurl + "/api/module/rebootCountdown?rebootCountdown=-1003", null, YIO_DEFAULT_TCP_TIMEOUT);
                await Task.Delay(TimeSpan.FromSeconds(7));
            } else {
                // reboot device to bootloader if needed
                if (need_reboot) {
                    // reboot subdevice
                    await _notificationHandler.hubRequestSync(
                        "GET /bySerial/" + serial + "/api/module/rebootCountdown?rebootCountdown=-2", null,
                        YIO_DEFAULT_TCP_TIMEOUT);
                }
                // verify that the device is in bootloader
                ulong timeout = YAPI.GetTickCount() + YPROG_BOOTLOADER_TIMEOUT;
                byte[] res;
                bool found = false;
                await progress(40, "Wait for device to be in bootloader");
                do {
                    List<string> list = await getBootloaders();
                    foreach (string bl in list) {
                        if (bl.Equals(serial)) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        //fixme: replace by async
                        //Thread.Sleep(100);
                    }
                } while (!found && YAPI.GetTickCount() < timeout);
                //start flash
                await progress(45, "Flash firmware");
                res = await _notificationHandler.hubRequestSync("GET /flash.json?a=flash&s=" + serial, null,
                    YIO_10_MINUTES_TCP_TIMEOUT);
                string jsonstr = YAPI.DefaultEncoding.GetString(res);
                YJSONObject flashres = new YJSONObject(jsonstr);
                flashres.Parse();
                YJSONArray logslist = flashres.GetYJSONArray("logs");
                List<string> logs = new List<string>(logslist.Length);
                for (int i = 0; i < logslist.Length; i++) {
                    logs.Add(logslist.GetString(i));
                }
                return logs;

            }

            return null;
        }

        internal override async Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body,
            RequestAsyncResult asyncResult, object asyncContext)
        {
            if (!_notificationHandler.Connected) {
                throw new YAPI_Exception(YAPI.TIMEOUT, "hub " + _http_params.Url + " is not reachable");
            }
            if (_writeProtected && !_notificationHandler.hasRwAccess()) {
                throw new YAPI_Exception(YAPI.UNAUTHORIZED, "Access denied: admin credentials required");
            }

            await _notificationHandler.devRequestAsync(device, req_first_line, req_head_and_body, asyncResult,
                asyncContext);
        }

        internal override async Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body,
            RequestProgress progress, object context)
        {
            if (!_notificationHandler.Connected) {
                throw new YAPI_Exception(YAPI.TIMEOUT, "hub " + _http_params.Url + " is not reachable");
            }
            // Setup timeout counter
            uint tcpTimeout = YIO_DEFAULT_TCP_TIMEOUT;
            if (req_first_line.Contains("/testcb.txt") || req_first_line.Contains("/rxmsg.json") ||
                req_first_line.Contains("/files.json") || req_first_line.Contains("/upload.html")) {
                tcpTimeout = YIO_1_MINUTE_TCP_TIMEOUT;
            } else if (req_first_line.Contains("/flash.json")) {
                tcpTimeout = YIO_10_MINUTES_TCP_TIMEOUT;
            }
            return await _notificationHandler.devRequestSync(device, req_first_line, req_head_and_body, tcpTimeout,
                progress, context);
        }

        internal virtual string Host {
            get { return _http_params.Host; }
        }

        internal virtual int Port {
            get { return _http_params.Port; }
        }

        public override async Task<List<string>> getBootloaders()
        {
            List<string> res = new List<string>();
            byte[] raw_data = await _notificationHandler.hubRequestSync("GET /flash.json?a=list", null, YIO_DEFAULT_TCP_TIMEOUT);
            string jsonstr = YAPI.DefaultEncoding.GetString(raw_data);
            YJSONObject flashres = new YJSONObject(jsonstr);
            flashres.Parse();
            YJSONArray list = flashres.GetYJSONArray("list");
            for (int i = 0; i < list.Length; i++) {
                res.Add(list.GetString(i));
            }
            return res;

        }

        internal override async Task<int> ping(uint mstimeout)
        {
            // ping dot not use Notification handler but a one shot http request
            YHTTPRequest req = new YHTTPRequest(this, "ping");
            await req.RequestSync("GET /api/module/firmwareRelease.json", null, mstimeout);
            return YAPI.SUCCESS;
        }


    }

}