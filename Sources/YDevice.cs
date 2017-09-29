/*********************************************************************
 *
 * $Id: YDevice.cs 28648 2017-09-26 12:32:59Z seb $
 *
 * Internal YDevice class
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
using System.Linq;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    //
    // YDevice Class (used internally)
    //
    // This class is used to store everything we know about connected Yocto-Devices.
    // Instances are created when devices are discovered in the white pages
    // (or registered manually, for root hubs) and then used to keep track of
    // device naming changes. When a device or a function is renamed, this
    // object forces the local indexes to be immediately updated, even if not
    // yet fully propagated through the yellow pages of the device hub.
    //
    // In order to regroup multiple function queries on the same physical device,
    // this class implements a device-wide API string cache (agnostic of API content).
    // This is in addition to the function-specific cache implemented in YFunction.
    //
    public class YDevice
    {
        private YGenericHub _hub;
        internal WPEntry _wpRec;
        private ulong _cache_expiration;
        private string _cache_json;
        private readonly Dictionary<int?, YPEntry> _ypRecs;
        private double _deviceTime;
        private YPEntry _moduleYPEntry;
        private YModule.LogCallback _logCallback = null;
        private int _logpos = 0;
        private bool _logIsPulling = false;

        // Device constructor. Automatically call the YAPI functin to reindex device
        internal YDevice(YGenericHub hub, WPEntry wpRec, Dictionary<string, List<YPEntry>> ypRecs) {
            // private attributes
            _hub = hub;
            _wpRec = wpRec;
            _cache_expiration = 0;
            _cache_json = "";
            _moduleYPEntry = new YPEntry(wpRec.SerialNumber, "module", YPEntry.BaseClass.Function);
            _moduleYPEntry.LogicalName = wpRec.LogicalName;
            _ypRecs = new Dictionary<int?, YPEntry>();
            List<string> keySet = ypRecs.Keys.ToList();
            foreach (string categ in keySet) {
                foreach (YPEntry rec in ypRecs[categ]) {
                    if (rec.Serial.Equals(wpRec.SerialNumber)) {
                        int funydx = rec.Index;
                        _ypRecs[funydx] = rec;
                    }
                }
            }
        }

        internal virtual YGenericHub Hub {
            get {
                return _hub;
            }
        }

        // Return the serial number of the device, as found during discovery
        public virtual string SerialNumber {
            get {
                return _wpRec.SerialNumber;
            }
        }

        // Return the logical name of the device, as found during discovery
        public virtual string LogicalName {
            get {
                return _wpRec.LogicalName;
            }
        }

        // Return the product name of the device, as found during discovery
        public virtual string ProductName {
            get {
                return _wpRec.ProductName;
            }
        }

        // Return the product Id of the device, as found during discovery
        public virtual int ProductId {
            get {
                return _wpRec.ProductId;
            }
        }

        internal virtual string RelativePath {
            get {
                return _wpRec.NetworkUrl;
            }
        }

        // Return the beacon state of the device, as found during discovery
        public virtual int Beacon {
            get {
                return _wpRec.Beacon;
            }
        }

        // Get the whole REST API string for a device, from cache if possible
        public virtual async Task<string> requestAPI() {
            if (_cache_expiration > YAPI.GetTickCount()) {
                return _cache_json;
            }
            string yreq = await requestHTTPSyncAsString("GET /api.json", null);
            this._cache_expiration = YAPI.GetTickCount() + YAPI.DefaultCacheValidity;
            this._cache_json = yreq;
            return yreq;
        }

        // Reload a device API (store in cache), and update YAPI function lists accordingly
        // Intended to be called within UpdateDeviceList only
        public virtual async Task<int> refresh() {
            string result = await this.requestAPI();
            YJSONObject loadval;
            bool? reindex = false;
            loadval = new YJSONObject(result);
            loadval.Parse();

            _cache_expiration = YAPI.GetTickCount() + YAPI.DefaultCacheValidity;
            _cache_json = result;
            // parse module and refresh names if needed
            List<string> keys = loadval.Keys();
            foreach (string key in keys) {
                if (key.Equals("module")) {
                    YJSONObject module = loadval.GetYJSONObject("module");
                    if (!_wpRec.LogicalName.Equals(module.GetString("logicalName"))) {
                        _wpRec.LogicalName = module.GetString("logicalName");
                        _moduleYPEntry.LogicalName = _wpRec.LogicalName;
                        reindex = true;
                    }
                    _wpRec.Beacon = module.GetInt("beacon");
                } else if (!key.Equals("services")) {
                    YJSONObject func = loadval.GetYJSONObject(key);
                    string name;
                    if (func.Has("logicalName")) {
                        name = func.GetString("logicalName");
                    } else {
                        name = _wpRec.LogicalName;
                    }
                    if (func.Has("advertisedValue")) {
                        string pubval = func.GetString("advertisedValue");
                        _hub._yctx._yHash.imm_setFunctionValue(_wpRec.SerialNumber+"."+key, pubval);
                    }
                    for (int f = 0; f < _ypRecs.Count; f++) {
                        if (_ypRecs[f].FuncId.Equals(key)) {
                            if (!_ypRecs[f].LogicalName.Equals(name)) {
                                _ypRecs[f].LogicalName = name;
                                reindex = true;
                            }
                            break;
                        }
                    }
                }
            }

            if (reindex.Value) {
                _hub._yctx._yHash.imm_reindexDevice(this);
            }
            return YAPI.SUCCESS;
        }

        // Force the REST API string in cache to expire immediately
        public virtual void imm_clearCache() {
            _cache_expiration = 0;
        }

        // Retrieve the number of functions (beside "module") in the device
        protected internal virtual int imm_functionCount() {
            return _ypRecs.Count;
        }

        internal virtual YPEntry imm_getYPEntry(int idx) {
            if (idx < _ypRecs.Count) {
                return _ypRecs[idx];
            }
            return null;
        }

        internal virtual async Task<byte[]> requestHTTPSync(string request, byte[] rest_of_request) {
            string shortRequest = imm_formatRequest(request);
            return await _hub.devRequestSync(this, shortRequest, rest_of_request, null, null);
        }

        internal virtual async Task<string> requestHTTPSyncAsString(string request, byte[] rest_of_request) {
            byte[] bytes = await requestHTTPSync(request, rest_of_request);
            return YAPI.DefaultEncoding.GetString(bytes);
        }

        internal virtual async Task requestHTTPAsync(string request, byte[] rest_of_request, YGenericHub.RequestAsyncResult asyncResult, object context) {
            string shortRequest = imm_formatRequest(request);
            await _hub.devRequestAsync(this, shortRequest, rest_of_request, asyncResult, context);
        }

        private string imm_formatRequest(string request) {
            string[] words = request.Split(' ');
            if (words.Length < 2) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Invalid request, not enough words; expected a method name and a URL");
            }
            string relativeUrl = words[1];
            if (relativeUrl[0] != '/') {
                relativeUrl = "/" + relativeUrl;
            }
            return string.Format("{0} {1}{2}", words[0], _wpRec.NetworkUrl, relativeUrl);
        }


        public virtual double imm_getDeviceTime() {
            return _deviceTime;
        }

        public virtual void imm_setDeviceTime(byte[] data) {
            double time = data[0] + 0x100 * data[1] + 0x10000 * data[2] + 0x1000000 * data[3];
            _deviceTime = time + data[4] / 250.0;
        }

        internal virtual YPEntry ModuleYPEntry {
            get {
                return _moduleYPEntry;
            }
        }

        private void imm_logCallbackHandle(object context, byte[] result, int error, string errmsg) {
            if (result == null) {
                _logIsPulling = false;
                return;
            }
            if (_logCallback == null) {
                _logIsPulling = false;
                return;
            }
            string resultStr = YAPI.DefaultEncoding.GetString(result);
            int pos = resultStr.LastIndexOf("@", StringComparison.Ordinal);
            if (pos < 0) {
                _logIsPulling = false;
                return;
            }
            string logs = resultStr.Substring(0, pos);
            string posStr = resultStr.Substring(pos + 1);
            _logpos = Convert.ToInt32(posStr);
            YModule module = YModule.FindModuleInContext(_hub._yctx, SerialNumber);
            string[] lines = logs.Split('\n');
            foreach (string line in lines) {
                _logCallback(module, line);
            }
            _logIsPulling = false;
        }

        internal virtual async Task triggerLogPull() {
            if (_logCallback == null || _logIsPulling) {
                return;
            }
            _logIsPulling = true;
            string request = "GET logs.txt?pos=" + _logpos;
            try {
                await requestHTTPAsync(request, null, imm_logCallbackHandle, _logpos);
            } catch (YAPI_Exception ex) {
                _hub._yctx._Log("LOG error:" + ex.Message);
            }
        }

        internal virtual async Task registerLogCallback(YModule.LogCallback callback) {
            _logCallback = callback;
            await triggerLogPull();
        }

        //todo: look if we can rewrite this function in c# to be more efficent
        internal static byte[] imm_formatHTTPUpload(string path, byte[] content) {
            Random randomGenerator = new Random();
            string boundary;
            string mp_header = "Content-Disposition: form-data; name=\"" + path + "\"; filename=\"api\"\r\n" + "Content-Type: application/octet-stream\r\n" + "Content-Transfer-Encoding: binary\r\n\r\n";
            // find a valid boundary
            do {
                boundary = string.Format("Zz{0:x6}zZ", randomGenerator.Next(0x1000000));
            } while (mp_header.Contains(boundary) && YAPIContext.imm_find_in_bytes(content, YAPI.DefaultEncoding.GetBytes(boundary)) >= 0);
            //construct header parts
            string header_start = "Content-Type: multipart/form-data; boundary=" + boundary + "\r\n\r\n--" + boundary + "\r\n" + mp_header;
            string header_stop = "\r\n--" + boundary + "--\r\n";
            byte[] head_body = new byte[header_start.Length + content.Length + header_stop.Length];
            int pos = 0;
            int len = header_start.Length;
            Array.Copy(YAPI.DefaultEncoding.GetBytes(header_start), 0, head_body, pos, len);

            pos += len;
            len = content.Length;
            Array.Copy(content, 0, head_body, pos, len);

            pos += len;
            len = header_stop.Length;
            Array.Copy(YAPI.DefaultEncoding.GetBytes(header_stop), 0, head_body, pos, len);
            Array.Copy(YAPI.DefaultEncoding.GetBytes(header_stop), 0, head_body, pos, len);
            return head_body;
        }

        internal virtual async Task<int> requestHTTPUpload(string path, byte[] content) {
            string request = "POST /upload.html";
            byte[] head_body = YDevice.imm_formatHTTPUpload(path, content);
            await requestHTTPSync(request, head_body);
            return YAPI.SUCCESS;
        }


        public string get_debugMsg()
        {
            return _hub.get_debugMsg(SerialNumber);
        }
    }

}