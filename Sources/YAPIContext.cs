/*********************************************************************
 *
 * $Id: YAPIContext.cs 28987 2017-10-23 09:39:15Z seb $
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
using System.Threading;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{
    public class YAPIContext
    {
        internal class DataEvent
        {
            internal readonly YFunction _fun;
            internal readonly string _value;
            internal readonly List<int> _report;
            internal readonly double _timestamp;


            public DataEvent(YFunction fun, string value)
            {
                _fun = fun;
                _value = value;
                _report = null;
                _timestamp = 0;
            }

            public DataEvent(YFunction fun, double timestamp, List<int> report)
            {
                _fun = fun;
                _value = null;
                _timestamp = timestamp;
                _report = report;
            }

            public virtual async Task invoke()
            {
                if (_value == null) {
                    YSensor sensor = (YSensor) _fun;
                    YMeasure mesure = await sensor._decodeTimedReport(_timestamp, _report);
                    await sensor._invokeTimedReportCallback(mesure);
                } else {
                    // new value
                    await _fun._invokeValueCallback(_value);
                }
            }
        }

        internal class PlugEvent
        {
            public enum Event
            {
                PLUG,
                UNPLUG,
                CHANGE
            }

            public Event ev;
            public YModule module;

            public PlugEvent(YAPIContext yctx, Event ev, string serial)
            {
                this.ev = ev;
                this.module = YModule.FindModuleInContext(yctx, serial + ".module");
            }
        }


        private static double[] decExp = new double[] {1.0e-6, 1.0e-5, 1.0e-4, 1.0e-3, 1.0e-2, 1.0e-1, 1.0, 1.0e1, 1.0e2, 1.0e3, 1.0e4, 1.0e5, 1.0e6, 1.0e7, 1.0e8, 1.0e9};

        // Convert Yoctopuce 16-bit decimal floats to standard double-precision floats
        //
        internal static double imm_decimalToDouble(int val)
        {
            bool negate = false;
            double res;
            int mantis = val & 2047;
            if (mantis == 0)
                return 0.0;
            if (val > 32767) {
                negate = true;
                val = 65536 - val;
            } else if (val < 0) {
                negate = true;
                val = -val;
            }
            int exp = val >> 11;
            res = (double) (mantis) * decExp[exp];
            return (negate ? -res : res);
        }

        // Convert standard double-precision floats to Yoctopuce 16-bit decimal floats
        //
        internal static long imm_doubleToDecimal(double val)
        {
            int negate = 0;
            double comp, mant;
            int decpow;
            long res;

            if (val == 0.0) {
                return 0;
            }
            if (val < 0) {
                negate = 1;
                val = -val;
            }
            comp = val / 1999.0;
            decpow = 0;
            while (comp > decExp[decpow] && decpow < 15) {
                decpow++;
            }
            mant = val / decExp[decpow];
            if (decpow == 15 && mant > 2047.0) {
                res = (15 << 11) + 2047; // overflow
            } else {
                res = (decpow << 11) + Convert.ToInt32(mant);
            }
            return (negate != 0 ? -res : res);
        }


        internal static List<int> imm_decodeWords(string sdat)
        {
            List<int> udat = new List<int>();

            for (int p = 0; p < sdat.Length;) {
                uint val;
                uint c = sdat[p++];
                if (c == '*') {
                    val = 0;
                } else if (c == 'X') {
                    val = 0xffff;
                } else if (c == 'Y') {
                    val = 0x7fff;
                } else if (c >= 'a') {
                    int srcpos = (int) (udat.Count - 1 - (c - 'a'));
                    if (srcpos < 0) {
                        val = 0;
                    } else {
                        val = (uint) udat[srcpos];
                    }
                } else {
                    if (p + 2 > sdat.Length) {
                        return udat;
                    }
                    val = (c - '0');
                    c = sdat[p++];
                    val += (c - '0') << 5;
                    c = sdat[p++];
                    if (c == 'z')
                        c = '\\';
                    val += (c - '0') << 10;
                }
                udat.Add((int) val);
            }
            return udat;
        }

        internal static List<int> imm_decodeFloats(string sdat)
        {
            List<int> idat = new List<int>();

            for (int p = 0; p < sdat.Length;) {
                int val = 0;
                int sign = 1;
                int dec = 0;
                int decInc = 0;
                int c = sdat[p++];
                while (c != (int) '-' && (c < (int) '0' || c > (int) '9')) {
                    if (p >= sdat.Length) {
                        return idat;
                    }
                    c = sdat[p++];
                }
                if (c == '-') {
                    if (p >= sdat.Length) {
                        return idat;
                    }
                    sign = -sign;
                    c = sdat[p++];
                }
                while ((c >= '0' && c <= '9') || c == '.') {
                    if (c == '.') {
                        decInc = 1;
                    } else if (dec < 3) {
                        val = val * 10 + (c - '0');
                        dec += decInc;
                    }
                    if (p < sdat.Length) {
                        c = sdat[p++];
                    } else {
                        c = 0;
                    }
                }
                if (dec < 3) {
                    if (dec == 0)
                        val *= 1000;
                    else if (dec == 1)
                        val *= 100;
                    else
                        val *= 10;
                }
                idat.Add(sign * val);
            }
            return idat;
        }

        // helper function to find pattern in byte[]
        // todo: look if there is a more efficient c# rewrite
        internal static int imm_find_in_bytes(byte[] source, byte[] match)
        {
            // sanity checks
            if (source == null || match == null) {
                return -1;
            }
            if (source.Length == 0 || match.Length == 0) {
                return -1;
            }
            int ret = -1;
            int spos = 0;
            int mpos = 0;
            byte m = match[mpos];
            for (; spos < source.Length; spos++) {
                if (m == source[spos]) {
                    // starting match
                    if (mpos == 0) {
                        ret = spos;
                    } // finishing match
                    else if (mpos == match.Length - 1) {
                        return ret;
                    }
                    mpos++;
                    m = match[mpos];
                } else {
                    ret = -1;
                    mpos = 0;
                    m = match[mpos];
                }
            }
            return ret;
        }

        internal static string imm_floatToStr(double value)
        {
            int rounded = (int) Math.Round(value * 1000);
            string res = "";
            if (rounded < 0) {
                res += "-";
                rounded = -rounded;
            }
            res += Convert.ToString((int) (rounded / 1000));
            int decim = rounded % 1000;
            if (decim > 0) {
                res += ".";
                if (decim < 100)
                    res += "0";
                if (decim < 10)
                    res += "0";
                if ((decim % 10) == 0)
                    decim /= 10;
                if ((decim % 10) == 0)
                    decim /= 10;
                res += Convert.ToString(decim);
            }
            return res;
        }

        internal static int imm_atoi(string val)
        {
            int p = 0;
            while (p < val.Length && Char.IsWhiteSpace(val[p])) {
                p++;
            }
            int start = p;
            if (p < val.Length && (val[p] == '-' || val[p] == '+'))
                p++;
            while (p < val.Length && Char.IsDigit(val[p])) {
                p++;
            }
            if (start < p) {
                return int.Parse(val.Substring(start, p - start));
            }
            return 0;
        }

        protected const string _hexArray = "0123456789ABCDEF";

        internal static string imm_bytesToHexStr(byte[] bytes, int offset, int len)
        {
            char[] hexChars = new char[len * 2];
            for (int j = 0; j < len; j++) {
                int v = bytes[offset + j] & 0xFF;
                hexChars[j * 2] = _hexArray[v >> 4];
                hexChars[j * 2 + 1] = _hexArray[v & 0x0F];
            }
            return new string(hexChars);
        }

        internal static byte[] imm_hexStrToBin(string hex_str)
        {
            int len = hex_str.Length / 2;
            byte[] res = new byte[len];
            for (int i = 0; i < len; i++) {
                int val = 0;
                for (int n = 0; n < 2; n++) {
                    char c = hex_str[i * 2 + n];
                    val <<= 4;
                    if (c <= '9') {
                        val += c - '0';
                    } else if (c <= 'F') {
                        val += c - 'A' + 10;
                    } else {
                        val += c - 'a' + 10;
                    }
                }
                res[i] = (byte) val;
            }
            return res;
        }

        internal static byte[] imm_bytesMerge(byte[] array_a, byte[] array_b)
        {
            byte[] res = new byte[array_a.Length + array_b.Length];
            System.Buffer.BlockCopy(array_a, 0, res, 0, array_a.Length);
            System.Buffer.BlockCopy(array_b, 0, res, array_a.Length, array_b.Length);
            return res;
        }


        // Return the class name for a given function ID or full Hardware Id
        internal static string imm_functionClass(string funcid)
        {
            int dotpos = funcid.IndexOf('.');

            if (dotpos >= 0) {
                funcid = funcid.Substring(dotpos + 1);
            }
            int classlen = funcid.Length;

            while (funcid[classlen - 1] <= 57) {
                classlen--;
            }

            return funcid.Substring(0, 1).ToUpperInvariant() + funcid.Substring(1, classlen - 1);
        }


        public ulong DefaultCacheValidity = 5;

        //todo: Replace global encding to the YAPIContext one
        //internal string _defaultEncoding = YAPI.DefaultEncoding;
        private int _apiMode;

        internal bool _exceptionsDisabled = false;
        internal readonly List<YGenericHub> _hubs = new List<YGenericHub>(1); // array of root urls
        private bool _firstArrival;
        private readonly LinkedList<PlugEvent> _pendingCallbacks = new LinkedList<PlugEvent>();
        private readonly LinkedList<DataEvent> _data_events = new LinkedList<DataEvent>();

        public event YAPI.DeviceUpdateHandler _arrivalCallback;
        private event YAPI.DeviceUpdateHandler _namechgCallback;
        public event YAPI.DeviceUpdateHandler _removalCallback;
        public event YAPI.LogHandler _logCallback;
        private event YAPI.HubDiscoveryHandler _HubDiscoveryCallback;

        private readonly Dictionary<int, YAPI.CalibrationHandler> _calibHandlers = new Dictionary<int, YAPI.CalibrationHandler>();

        private readonly YSSDP _ssdp;
        internal readonly YHash _yHash;
        private readonly List<YFunction> _ValueCallbackList = new List<YFunction>();
        private readonly List<YFunction> _TimedReportCallbackList = new List<YFunction>();
        
        internal readonly Dictionary<string, YPEntry.BaseClass> _BaseType = new Dictionary<string, YPEntry.BaseClass>();

       

        // fixme: review SSDP code        
        internal async void HubDiscoveryCallback(string serial, string urlToRegister, string urlToUnregister)
        {
            if (urlToRegister != null) {
                if (_HubDiscoveryCallback != null) {
                    await _HubDiscoveryCallback(serial, urlToRegister);
                }
            }
            if ((this._apiMode & YAPI.DETECT_NET) != 0) {
                if (urlToRegister != null) {
                    if (urlToUnregister != null) {
                        await this.UnregisterHub(urlToUnregister);
                    }
                    try {
                        await this.PreregisterHub(urlToRegister);
                    } catch (YAPI_Exception ex) {
                        this._Log("Unable to register hub " + urlToRegister + " detected by SSDP:" + ex.ToString());
                    }
                }
            }
        }


        internal double linearCalibrationHandler(double rawValue, int calibType, List<int> param, List<double> rawValues, List<double> refValues)
        {
            // calibration types n=1..10 and 11.20 are meant for linear calibration using n points
            int npt;
            double x = rawValues[0];
            double adj = refValues[0] - x;
            int i = 0;

            if (calibType < YAPI.YOCTO_CALIB_TYPE_OFS) {
                npt = calibType % 10;
                if (npt > rawValues.Count) {
                    npt = rawValues.Count;
                }
                if (npt > refValues.Count) {
                    npt = refValues.Count;
                }
            } else {
                npt = refValues.Count;
            }
            while (rawValue > rawValues[i] && ++i < npt) {
                double x2 = x;
                double adj2 = adj;

                x = rawValues[i];
                adj = refValues[i] - x;

                if (rawValue < x && x > x2) {
                    adj = adj2 + (adj - adj2) * (rawValue - x2) / (x - x2);
                }
            }
            return rawValue + adj;
        }

        //INTERNAL METHOD:

        public YAPIContext()
        {
            _yHash = new YHash(this);
            _ssdp = new YSSDP(this);
            imm_resetContext();
        }

        private void imm_resetContext()
        {
            _apiMode = 0;
            _firstArrival = true;
            _pendingCallbacks.Clear();
            _data_events.Clear();
            _arrivalCallback = null;
            _namechgCallback = null;
            _removalCallback = null;
            _logCallback = null;
            _HubDiscoveryCallback = null;
            _hubs.Clear();
            _calibHandlers.Clear();
            _ssdp.reset();
            _yHash.imm_reset();
            _ValueCallbackList.Clear();
            _TimedReportCallbackList.Clear();
            for (int i = 1; i <= 20; i++) {
                _calibHandlers[i] = linearCalibrationHandler;
            }
            _calibHandlers[YAPI.YOCTO_CALIB_TYPE_OFS] = linearCalibrationHandler;
            _BaseType.Clear();
            _BaseType["Function"] = YPEntry.BaseClass.Function;
            _BaseType["Sensor"] = YPEntry.BaseClass.Sensor;

        }

        internal void _pushPlugEvent(PlugEvent.Event ev, string serial)
        {
            _pendingCallbacks.AddLast(new PlugEvent(this, ev, serial));
        }


        // Queue a function data event (timed report of notification value)
        internal void _PushDataEvent(DataEvent ev)
        {
            _data_events.AddLast(ev);
        }

        /*
        * Return a the calibration handler for a given type
        */
        internal YAPI.CalibrationHandler imm_getCalibrationHandler(int calibType)
        {
            if (!_calibHandlers.ContainsKey(calibType)) {
                return null;
            }
            return _calibHandlers[calibType];
        }


        internal async Task<YDevice> funcGetDevice(string className, string func)
        {
            string resolved;
            try {
                resolved = _yHash.imm_resolveSerial(className, func);
            } catch (YAPI_Exception ex) {
                if (ex.errorType == YAPI.DEVICE_NOT_FOUND && _hubs.Count == 0) {
                    throw new YAPI_Exception(ex.errorType, "Impossible to contact any device because no hub has been registered");
                } else {
                    await _updateDeviceList_internal(true, false);
                    resolved = _yHash.imm_resolveSerial(className, func);
                }
            }
            YDevice dev = _yHash.imm_getDevice(resolved);
            if (dev == null) {
                // try to force a device list update to check if the device arrived
                // in between
                await _updateDeviceList_internal(true, false);
                dev = _yHash.imm_getDevice(resolved);
                if (dev == null) {
                    throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "Device [" + resolved + "] not online");
                }
            }
            return dev;
        }


        internal async Task _UpdateValueCallbackList(YFunction func, bool add)
        {
            if (add) {
                await func.isOnline();
                if (!_ValueCallbackList.Contains(func)) {
                    _ValueCallbackList.Add(func);
                }
            } else {
                _ValueCallbackList.Remove(func);
            }
        }

        internal YFunction _GetValueCallback(string hwid)
        {
            foreach (YFunction func in _ValueCallbackList) {
                try {
                    string fhwid = func.imm_get_hardwareId();
                    if (fhwid != null && fhwid.Equals(hwid)) {
                        return func;
                    }
                } catch (YAPI_Exception) { }
            }
            return null;
        }


        internal async Task _UpdateTimedReportCallbackList(YFunction func, bool add)
        {
            if (add) {
                await func.isOnline();
                if (!_TimedReportCallbackList.Contains(func)) {
                    _TimedReportCallbackList.Add(func);
                }
            } else {
                _TimedReportCallbackList.Remove(func);
            }
        }

        internal YFunction _GetTimedReportCallback(string hwid)
        {
            foreach (YFunction func in _TimedReportCallbackList) {
                try {
                    string fhwid = func.imm_get_hardwareId();
                    if (fhwid != null && fhwid.Equals(hwid)) {
                        return func;
                    }
                } catch (YAPI_Exception) { }
            }
            return null;
        }

        private async Task<int> AddNewHub(string url, bool reportConnnectionLost, System.IO.Stream request, System.IO.Stream response, object session)
        {
            foreach (YGenericHub h in _hubs) {
                if (h.imm_isSameHub(url, request, response, session)) {
                    return YAPI.SUCCESS;
                }
            }
            YGenericHub newhub;
            YGenericHub.HTTPParams parsedurl;
            parsedurl = new YGenericHub.HTTPParams(url);
            // Add hub to known list
            if (url.Equals("usb")) {
                newhub = new YUSBHub(this, _hubs.Count, true);
            } else if (url.Equals("usb_silent")) {
                newhub = new YUSBHub(this, _hubs.Count, false);
            } else if (url.Equals("net")) {
                if ((_apiMode & YAPI.DETECT_NET) == 0) {
                    _apiMode |= YAPI.DETECT_NET;
                    // todo: review ssdp callback
                    //_ssdp.addCallback(_ssdpCallback);
                }
                return YAPI.SUCCESS;
            } else if (parsedurl.Host.Equals("callback")) {
                //todo: add SUPPORT FOR CALLBACK
                throw new YAPI_Exception(YAPI.NOT_SUPPORTED, "callback is not yet supported");
            } else {
                newhub = new YHTTPHub(this, _hubs.Count, parsedurl, reportConnnectionLost);
            }
            _hubs.Add(newhub);
            await newhub.startNotifications();
            return YAPI.SUCCESS;
        }


        private async Task _updateDeviceList_internal(bool forceupdate, bool invokecallbacks)
        {
            if (_firstArrival && invokecallbacks && _arrivalCallback != null) {
                forceupdate = true;
            }

            // Rescan all hubs and update list of online devices
            foreach (YGenericHub h in _hubs) {
                await h.updateDeviceListAsync(forceupdate);
            }

            // after processing all hubs, invoke pending callbacks if required
            if (invokecallbacks) {
                while (true) {
                    PlugEvent evt;
                    if (_pendingCallbacks.Count == 0) {
                        break;
                    }
                    evt = _pendingCallbacks.First.Value;
                    _pendingCallbacks.RemoveFirst();
                    switch (evt.ev) {
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.PLUG:
                            if (_arrivalCallback != null) {
                                await _arrivalCallback(evt.module);
                            }
                            break;
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.CHANGE:
                            if (_namechgCallback != null) {
                                await _namechgCallback(evt.module);
                            }
                            break;
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.UNPLUG:
                            if (_removalCallback != null) {
                                await _removalCallback(evt.module);
                            }
                            _yHash.imm_forgetDevice(await evt.module.get_serialNumber());
                            break;
                    }
                }
                if (_arrivalCallback != null && _firstArrival) {
                    _firstArrival = false;
                }
            }
        }

        internal void _Log(string message)
        {
            Debug.Write(message);
            if (_logCallback != null) {
                _logCallback(message);
            }
        }


        //PUBLIC METHOD:


        /**
        *
        */
        public void DisableExceptions()
        {
            _exceptionsDisabled = true;
        }

        /**
         *
         */
        public void EnableExceptions()
        {
            _exceptionsDisabled = false;
        }


        /**
         *
         */
        public static string GetAPIVersion()
        {
            return YAPI.GetAPIVersion();
        }


        /**
         *
         */
        public async Task<int> InitAPI(int mode)
        {
            int res = YAPI.SUCCESS;
            if ((mode & YAPI.DETECT_NET) != 0) {
                res = await RegisterHub("net");
                if (res != YAPI.SUCCESS) {
                    return res;
                }
            }
            if ((mode & YAPI.RESEND_MISSING_PKT) != 0) {
                YAPI.pktAckDelay = YAPI.DEFAULT_PKT_RESEND_DELAY;
            }
            if ((mode & YAPI.DETECT_USB) != 0) {
                res = await RegisterHub("usb");
            }
            return res;
        }

        /**
         *
         */
        public void FreeAPI()
        {
            if ((_apiMode & YAPI.DETECT_NET) != 0) {
                _ssdp.Stop();
            }
            foreach (YGenericHub h in _hubs) {
                h.stopNotifications();
                h.imm_release();
            }
            imm_resetContext();
        }


        /**
         *
         */
        public async Task<int> RegisterHub(string url)
        {
            await AddNewHub(url, true, null, null, null);
            // Register device list
            await _updateDeviceList_internal(true, false);
            return YAPI.SUCCESS;
        }


        /**
         *
         */
        public async Task<int> PreregisterHub(string url)
        {
            try {
                await AddNewHub(url, false, null, null, null);
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }
            return YAPI.SUCCESS;
        }

        /**
         *
         */
        public async Task UnregisterHub(string url)
        {
            if (url.Equals("net")) {
                _apiMode &= ~YAPI.DETECT_NET;
                return;
            }
            await unregisterHubEx(url, null, null, null);
        }

        private async Task unregisterHubEx(string url, System.IO.Stream request, System.IO.Stream response, object session)
        {
            foreach (YGenericHub h in _hubs) {
                if (h.imm_isSameHub(url, request, response, session)) {
                    await h.stopNotifications();
                    foreach (string serial in h._serialByYdx.Values) {
                        _yHash.imm_forgetDevice(serial);
                    }
                    h.imm_release();
                    _hubs.Remove(h);
                    return;
                }
            }
        }


        /**
         *
         */
        public async Task<int> TestHub(string url, uint mstimeout)
        {
            YGenericHub newhub;
            YGenericHub.HTTPParams parsedurl = new YGenericHub.HTTPParams(url);
            // Add hub to known list
            if (url.Equals("usb")) {
                newhub = new YUSBHub(this, 0, true);
            } else if (url.Equals("net")) {
                return YAPI.SUCCESS;
            } else if (parsedurl.Host.Equals("callback")) {
                throw new YAPI_Exception(YAPI.NOT_SUPPORTED, "Not yet supported");
            } else {
                newhub = new YHTTPHub(this, 0, parsedurl, true);
            }
            return await newhub.ping(mstimeout);
        }


        /**
         *
         */
        public async Task<int> UpdateDeviceList()
        {
            await _updateDeviceList_internal(false, true);
            return YAPI.SUCCESS;
        }

        /**
         *
         */
        public async Task<int> HandleEvents()
        {
            try {
                // handle pending events
                while (true) {
                    DataEvent pv;
                    if (_data_events.Count == 0) {
                        break;
                    }
                    pv = _data_events.First.Value;
                    _data_events.RemoveFirst();
                    if (pv != null) {
                        await pv.invoke();
                    }
                }
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }
            return YAPI.SUCCESS;
        }

        /**
        *
        */
        public async Task<int> Sleep(ulong ms_duration)
        {
            try {
                ulong end = GetTickCount() + ms_duration;

                do {
                    await HandleEvents();
                    if (end > GetTickCount()) {
                        await Task.Delay(new TimeSpan(0, 0, 0, 0, 2));
                    }
                } while (end > GetTickCount());
                return YAPI.SUCCESS;
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }
        }


        /**
         *
         */
        public Task<int> TriggerHubDiscovery()
        {
            // Register device list
            //todo: add ssd support
            //_ssdp.addCallback(_ssdpCallback);
            return Task.FromResult<int>(YAPI.SUCCESS);
        }

        /**
         *
         */
        public static ulong GetTickCount()
        {
            return (ulong) DateTime.Now.Ticks / 10000;
        }

        /**
         *
         */
        public bool CheckLogicalName(string name)
        {
            return YAPI.CheckLogicalName(name);
        }

        /**
         *
         */
        public void RegisterDeviceArrivalCallback(YAPI.DeviceUpdateHandler arrivalCallback)
        {
            _arrivalCallback = arrivalCallback;
        }

        public void RegisterDeviceChangeCallback(YAPI.DeviceUpdateHandler changeCallback)
        {
            _namechgCallback = changeCallback;
        }

        /**
         *
         */
        public void RegisterDeviceRemovalCallback(YAPI.DeviceUpdateHandler removalCallback)
        {
            _removalCallback += removalCallback;
        }

        /**
         *
         */
        public async Task RegisterHubDiscoveryCallback(YAPI.HubDiscoveryHandler hubDiscoveryCallback)
        {
            _HubDiscoveryCallback = hubDiscoveryCallback;
            try {
                await TriggerHubDiscovery();
            } catch (YAPI_Exception) { }
        }

        /**
         *
         */
        public void RegisterLogFunction(YAPI.LogHandler logfun)
        {
            _logCallback = logfun;
        }


        public string get_debugMsg(string serial)
        {
            string res = "";
            foreach (YGenericHub h in _hubs) {
                res += h.get_debugMsg(serial);
            }
            return res;
        }
    }
}