/*********************************************************************
 *
 * $Id: YModule.cs 29558 2018-01-04 15:44:28Z seb $
 *
 * YModule Class: Module control interface
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
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{
    //--- (generated code: YModule class start)
/**
 * <summary>
 *   YModule Class: Module control interface
 * <para>
 *   This interface is identical for all Yoctopuce USB modules.
 *   It can be used to control the module global parameters, and
 *   to enumerate the functions provided by each module.
 * </para>
 * </summary>
 */
public class YModule : YFunction
{
//--- (end of generated code: YModule class start)


        // --- (generated code: YModule definitions)
    /**
     * <summary>
     *   invalid productName value
     * </summary>
     */
    public const  string PRODUCTNAME_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid serialNumber value
     * </summary>
     */
    public const  string SERIALNUMBER_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid productId value
     * </summary>
     */
    public const  int PRODUCTID_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid productRelease value
     * </summary>
     */
    public const  int PRODUCTRELEASE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid firmwareRelease value
     * </summary>
     */
    public const  string FIRMWARERELEASE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid persistentSettings value
     * </summary>
     */
    public const int PERSISTENTSETTINGS_LOADED = 0;
    public const int PERSISTENTSETTINGS_SAVED = 1;
    public const int PERSISTENTSETTINGS_MODIFIED = 2;
    public const int PERSISTENTSETTINGS_INVALID = -1;
    /**
     * <summary>
     *   invalid luminosity value
     * </summary>
     */
    public const  int LUMINOSITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid beacon value
     * </summary>
     */
    public const int BEACON_OFF = 0;
    public const int BEACON_ON = 1;
    public const int BEACON_INVALID = -1;
    /**
     * <summary>
     *   invalid upTime value
     * </summary>
     */
    public const  long UPTIME_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid usbCurrent value
     * </summary>
     */
    public const  int USBCURRENT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid rebootCountdown value
     * </summary>
     */
    public const  int REBOOTCOUNTDOWN_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid userVar value
     * </summary>
     */
    public const  int USERVAR_INVALID = YAPI.INVALID_INT;
    protected string _productName = PRODUCTNAME_INVALID;
    protected string _serialNumber = SERIALNUMBER_INVALID;
    protected int _productId = PRODUCTID_INVALID;
    protected int _productRelease = PRODUCTRELEASE_INVALID;
    protected string _firmwareRelease = FIRMWARERELEASE_INVALID;
    protected int _persistentSettings = PERSISTENTSETTINGS_INVALID;
    protected int _luminosity = LUMINOSITY_INVALID;
    protected int _beacon = BEACON_INVALID;
    protected long _upTime = UPTIME_INVALID;
    protected int _usbCurrent = USBCURRENT_INVALID;
    protected int _rebootCountdown = REBOOTCOUNTDOWN_INVALID;
    protected int _userVar = USERVAR_INVALID;
    protected ValueCallback _valueCallbackModule = null;
    protected LogCallback _logCallback = null;

    public delegate Task LogCallback(YModule module, string logline);
    public new delegate Task ValueCallback(YModule func, string value);
    public new delegate Task TimedReportCallback(YModule func, YMeasure measure);
    //--- (end of generated code: YModule definitions)


        // Return the internal device object hosting the function
        protected internal virtual YDevice imm_getDev()
        {
            string devid = _func;
            int dotidx = devid.IndexOf('.');
            if (dotidx >= 0) {
                devid = devid.Substring(0, dotidx);
            }
            YDevice dev = _yapi._yHash.imm_getDevice(devid);
            if (dev == null) {
                throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "Device [" + devid + "] is not online");
            }
            return dev;
        }

        /// <param name="func"> : functionid </param>
        protected internal YModule(YAPIContext yctx, string func) : base(yctx, func, "Module")
        {
            //--- (generated code: YModule attributes initialization)
        //--- (end of generated code: YModule attributes initialization)
        }

        protected internal YModule(string func) : this(YAPI.imm_GetYCtx(), func)
        { }
#pragma warning disable 1998

        /// <summary>
        /// Returns the number of functions (beside the "module" interface) available on the module.
        /// </summary>
        /// <returns> the number of functions on the module
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<int> functionCount()
        {
            YDevice dev = imm_getDev();
            return dev.imm_functionCount();
        }

        /// <summary>
        /// Retrieves the hardware identifier of the <i>n</i>th function on the module.
        /// </summary>
        ///  <param name="functionIndex"> : the index of the function for which the information is desired, starting at
        /// 0 for the first function.
        /// </param>
        /// <returns> a string corresponding to the unambiguous hardware identifier of the requested module function
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> functionId(int functionIndex)
        {
            YDevice dev = imm_getDev();
            return dev.imm_getYPEntry(functionIndex).FuncId;
        }

        /// <summary>
        /// Retrieves the type of the <i>n</i>th function on the module.
        /// </summary>
        ///  <param name="functionIndex"> : the index of the function for which the information is desired, starting at
        /// 0 for the first function.
        /// </param>
        /// <returns> a the type of the function
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> functionType(int functionIndex)
        {
            YDevice dev = imm_getDev();
            return dev.imm_getYPEntry(functionIndex).Classname;
        }

        /// <summary>
        /// Retrieve the function base type of the nth function (beside "module") in the device </summary>
        ///  <param name="functionIndex"> : the index of the function for which the information is desired, starting at
        /// 0 for the first function.
        /// </param>
        /// <returns> a the type of the function
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> functionBaseType(int functionIndex)
        {
            YDevice dev = imm_getDev();
            return dev.imm_getYPEntry(functionIndex).BaseType;
        }

        /// <summary>
        /// Retrieves the logical name of the <i>n</i>th function on the module.
        /// </summary>
        ///  <param name="functionIndex"> : the index of the function for which the information is desired, starting at
        /// 0 for the first function.
        /// </param>
        /// <returns> a string corresponding to the logical name of the requested module function
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> functionName(int functionIndex)
        {
            YDevice dev = imm_getDev();
            return dev.imm_getYPEntry(functionIndex).LogicalName;
        }

        /// <summary>
        /// Retrieves the advertised value of the <i>n</i>th function on the module.
        /// </summary>
        ///  <param name="functionIndex"> : the index of the function for which the information is desired, starting at
        /// 0 for the first function.
        /// </param>
        ///  <returns> a short string (up to 6 characters) corresponding to the advertised value of the requested
        /// module function
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> functionValue(int functionIndex)
        {
            YDevice dev = imm_getDev();
            return dev.imm_getYPEntry(functionIndex).AdvertisedValue;
        }


        /// <summary>
        /// Registers a device log callback function. This callback will be called each time
        /// that a module sends a new log message. Mostly useful to debug a Yoctopuce module.
        /// </summary>
        /// <param name="callback"> : the callback function to call, or a null pointer. The callback function should take two
        ///         arguments: the module object that emitted the log message, and the character string containing the log.
        ///  </param>
        public virtual async Task registerLogCallback(LogCallback callback)
        {
            _logCallback = callback;
            YDevice ydev = _yapi._yHash.imm_getDevice(_serialNumber);
            if (ydev != null) {
                await ydev.registerLogCallback(callback);
            }
        }


        internal virtual async Task<LogCallback> get_logCallback()
        {
            return _logCallback;
        }

        private byte[] imm_flattenJsonStruct(byte[] actualSettings)
        {
            YJSONArray jsonout = new YJSONArray();
            string accutalSettingsStr = YAPI.DefaultEncoding.GetString(actualSettings);
            YJSONObject json = new YJSONObject(accutalSettingsStr);
            json.parse();
            List<string> functionList = json.keys();
            foreach (string fun_key in functionList) {
                if (!fun_key.Equals("services")) {
                    YJSONObject functionJson = json.getYJSONObject(fun_key);
                    if (functionJson == null) {
                        continue;
                    }
                    List<string> attr_keys = functionJson.keys();
                    foreach (string attr_key in attr_keys) {
                        if (!functionJson.has(attr_key)) {
                            continue;
                        }
                        YJSONContent value = functionJson.get(attr_key);
                        if (value == null) {
                            continue;
                        }
                        string flat_attr = fun_key + "/" + attr_key + "=" + value.ToString();
                        jsonout.put(flat_attr);
                    }
                }
            }
            return YAPI.DefaultEncoding.GetBytes(jsonout.toJSON());
        }


        /// <summary>
        /// Returns a list of all the modules that are plugged into the current module.
        /// This method only makes sense when called for a YoctoHub/VirtualHub.
        /// Otherwise, an empty array will be returned.
        /// </summary>
        /// <returns> an array of strings containing the sub modules. </returns>
        public virtual async Task<List<string>> get_subDevices()
        {
            YDevice dev = imm_getDev();
            YGenericHub hub = dev.Hub;
            return hub.imm_get_subDeviceOf(_serialNumber);
        }


        /// <summary>
        /// Returns the serial number of the YoctoHub on which this module is connected.
        /// If the module is connected by USB, or if the module is the root YoctoHub, an
        /// empty string is returned.
        /// </summary>
        /// <returns> a string with the serial number of the YoctoHub or an empty string </returns>
        public virtual async Task<string> get_parentHub()
        {
            YDevice dev = imm_getDev();
            YGenericHub hub = dev.Hub;
            string hubSerial = hub.SerialNumber;
            if (hubSerial.Equals(_serialNumber)) {
                return "";
            }
            return hubSerial;
        }


        /// <summary>
        /// Returns the URL used to access the module. If the module is connected by USB, the
        /// string 'usb' is returned.
        /// </summary>
        /// <returns> a string with the URL of the module. </returns>
        public virtual async Task<string> get_url()
        {
            YDevice dev = imm_getDev();
            YGenericHub hub = dev.Hub;
            return hub.imm_get_urlOf(_serialNumber);
        }
#pragma warning restore 1998


        // --- (generated code: YModule implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("productName")) {
            _productName = json_val.getString("productName");
        }
        if (json_val.has("serialNumber")) {
            _serialNumber = json_val.getString("serialNumber");
        }
        if (json_val.has("productId")) {
            _productId = json_val.getInt("productId");
        }
        if (json_val.has("productRelease")) {
            _productRelease = json_val.getInt("productRelease");
        }
        if (json_val.has("firmwareRelease")) {
            _firmwareRelease = json_val.getString("firmwareRelease");
        }
        if (json_val.has("persistentSettings")) {
            _persistentSettings = json_val.getInt("persistentSettings");
        }
        if (json_val.has("luminosity")) {
            _luminosity = json_val.getInt("luminosity");
        }
        if (json_val.has("beacon")) {
            _beacon = json_val.getInt("beacon") > 0 ? 1 : 0;
        }
        if (json_val.has("upTime")) {
            _upTime = json_val.getLong("upTime");
        }
        if (json_val.has("usbCurrent")) {
            _usbCurrent = json_val.getInt("usbCurrent");
        }
        if (json_val.has("rebootCountdown")) {
            _rebootCountdown = json_val.getInt("rebootCountdown");
        }
        if (json_val.has("userVar")) {
            _userVar = json_val.getInt("userVar");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the commercial name of the module, as set by the factory.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the commercial name of the module, as set by the factory
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.PRODUCTNAME_INVALID</c>.
     * </para>
     */
    public async Task<string> get_productName()
    {
        string res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PRODUCTNAME_INVALID;
            }
        }
        res = _productName;
        return res;
    }


    /**
     * <summary>
     *   Returns the serial number of the module, as set by the factory.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the serial number of the module, as set by the factory
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.SERIALNUMBER_INVALID</c>.
     * </para>
     */
    public async Task<string> get_serialNumber()
    {
        string res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SERIALNUMBER_INVALID;
            }
        }
        res = _serialNumber;
        return res;
    }


    /**
     * <summary>
     *   Returns the USB device identifier of the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the USB device identifier of the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.PRODUCTID_INVALID</c>.
     * </para>
     */
    public async Task<int> get_productId()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PRODUCTID_INVALID;
            }
        }
        res = _productId;
        return res;
    }


    /**
     * <summary>
     *   Returns the hardware release version of the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the hardware release version of the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.PRODUCTRELEASE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_productRelease()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PRODUCTRELEASE_INVALID;
            }
        }
        res = _productRelease;
        return res;
    }


    /**
     * <summary>
     *   Returns the version of the firmware embedded in the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the version of the firmware embedded in the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.FIRMWARERELEASE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_firmwareRelease()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FIRMWARERELEASE_INVALID;
            }
        }
        res = _firmwareRelease;
        return res;
    }


    /**
     * <summary>
     *   Returns the current state of persistent module settings.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YModule.PERSISTENTSETTINGS_LOADED</c>, <c>YModule.PERSISTENTSETTINGS_SAVED</c> and
     *   <c>YModule.PERSISTENTSETTINGS_MODIFIED</c> corresponding to the current state of persistent module settings
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.PERSISTENTSETTINGS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_persistentSettings()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PERSISTENTSETTINGS_INVALID;
            }
        }
        res = _persistentSettings;
        return res;
    }


    public async Task<int> set_persistentSettings(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("persistentSettings",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the luminosity of the  module informative leds (from 0 to 100).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the luminosity of the  module informative leds (from 0 to 100)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.LUMINOSITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_luminosity()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LUMINOSITY_INVALID;
            }
        }
        res = _luminosity;
        return res;
    }


    /**
     * <summary>
     *   Changes the luminosity of the module informative leds.
     * <para>
     *   The parameter is a
     *   value between 0 and 100.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the luminosity of the module informative leds
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_luminosity(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("luminosity",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the localization beacon.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YModule.BEACON_OFF</c> or <c>YModule.BEACON_ON</c>, according to the state of the localization beacon
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.BEACON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_beacon()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BEACON_INVALID;
            }
        }
        res = _beacon;
        return res;
    }


    /**
     * <summary>
     *   Turns on or off the module localization beacon.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YModule.BEACON_OFF</c> or <c>YModule.BEACON_ON</c>
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_beacon(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("beacon",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the number of milliseconds spent since the module was powered on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of milliseconds spent since the module was powered on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.UPTIME_INVALID</c>.
     * </para>
     */
    public async Task<long> get_upTime()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return UPTIME_INVALID;
            }
        }
        res = _upTime;
        return res;
    }


    /**
     * <summary>
     *   Returns the current consumed by the module on the USB bus, in milli-amps.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current consumed by the module on the USB bus, in milli-amps
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.USBCURRENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_usbCurrent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return USBCURRENT_INVALID;
            }
        }
        res = _usbCurrent;
        return res;
    }


    /**
     * <summary>
     *   Returns the remaining number of seconds before the module restarts, or zero when no
     *   reboot has been scheduled.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the remaining number of seconds before the module restarts, or zero when no
     *   reboot has been scheduled
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.REBOOTCOUNTDOWN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rebootCountdown()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REBOOTCOUNTDOWN_INVALID;
            }
        }
        res = _rebootCountdown;
        return res;
    }


    public async Task<int> set_rebootCountdown(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("rebootCountdown",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the value previously stored in this attribute.
     * <para>
     *   On startup and after a device reboot, the value is always reset to zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the value previously stored in this attribute
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YModule.USERVAR_INVALID</c>.
     * </para>
     */
    public async Task<int> get_userVar()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return USERVAR_INVALID;
            }
        }
        res = _userVar;
        return res;
    }


    /**
     * <summary>
     *   Stores a 32 bit value in the device RAM.
     * <para>
     *   This attribute is at programmer disposal,
     *   should he need to store a state variable.
     *   On startup and after a device reboot, the value is always reset to zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_userVar(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("userVar",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Allows you to find a module from its serial number or from its logical name.
     * <para>
     * </para>
     * <para>
     *   This function does not require that the module is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YModule.isOnline()</c> to test if the module is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a module by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string containing either the serial number or
     *   the logical name of the desired module
     * </param>
     * <returns>
     *   a <c>YModule</c> object allowing you to drive the module
     *   or get additional information on the module.
     * </returns>
     */
    public static YModule FindModule(string func)
    {
        YModule obj;
        obj = (YModule) YFunction._FindFromCache("Module", func);
        if (obj == null) {
            obj = new YModule(func);
            YFunction._AddToCache("Module",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a module for a given identifier in a YAPI context.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the module is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YModule.isOnline()</c> to test if the module is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a module by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the module
     * </param>
     * <returns>
     *   a <c>YModule</c> object allowing you to drive the module.
     * </returns>
     */
    public static YModule FindModuleInContext(YAPIContext yctx,string func)
    {
        YModule obj;
        obj = (YModule) YFunction._FindFromCacheInContext(yctx,  "Module", func);
        if (obj == null) {
            obj = new YModule(yctx, func);
            YFunction._AddToCache("Module",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackModule = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public override async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackModule != null) {
            await _valueCallbackModule(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Saves current settings in the nonvolatile memory of the module.
     * <para>
     *   Warning: the number of allowed save operations during a module life is
     *   limited (about 100000 cycles). Do not call this function within a loop.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> saveToFlash()
    {
        return await this.set_persistentSettings(PERSISTENTSETTINGS_SAVED);
    }

    /**
     * <summary>
     *   Reloads the settings stored in the nonvolatile memory, as
     *   when the module is powered on.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> revertFromFlash()
    {
        return await this.set_persistentSettings(PERSISTENTSETTINGS_LOADED);
    }

    /**
     * <summary>
     *   Schedules a simple module reboot after the given number of seconds.
     * <para>
     * </para>
     * </summary>
     * <param name="secBeforeReboot">
     *   number of seconds before rebooting
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> reboot(int secBeforeReboot)
    {
        return await this.set_rebootCountdown(secBeforeReboot);
    }

    /**
     * <summary>
     *   Schedules a module reboot into special firmware update mode.
     * <para>
     * </para>
     * </summary>
     * <param name="secBeforeReboot">
     *   number of seconds before rebooting
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> triggerFirmwareUpdate(int secBeforeReboot)
    {
        return await this.set_rebootCountdown(-secBeforeReboot);
    }

    /**
     * <summary>
     *   Tests whether the byn file is valid for this module.
     * <para>
     *   This method is useful to test if the module needs to be updated.
     *   It is possible to pass a directory as argument instead of a file. In this case, this method returns
     *   the path of the most recent
     *   appropriate <c>.byn</c> file. If the parameter <c>onlynew</c> is true, the function discards
     *   firmwares that are older or
     *   equal to the installed firmware.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="path">
     *   the path of a byn file or a directory that contains byn files
     * </param>
     * <param name="onlynew">
     *   returns only files that are strictly newer
     * </param>
     * <para>
     * </para>
     * <returns>
     *   the path of the byn file to use or a empty string if no byn files matches the requirement
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a string that start with "error:".
     * </para>
     */
    public virtual async Task<string> checkFirmware(string path,bool onlynew)
    {
        string serial;
        int release;
        string tmp_res;
        if (onlynew) {
            release = YAPIContext.imm_atoi(await this.get_firmwareRelease());
        } else {
            release = 0;
        }
        //may throw an exception
        serial = await this.get_serialNumber();
        tmp_res = await YFirmwareUpdate.CheckFirmware(serial, path, release);
        if ((tmp_res).IndexOf("error:") == 0) {
            this._throw(YAPI.INVALID_ARGUMENT, tmp_res);
        }
        return tmp_res;
    }

    /**
     * <summary>
     *   Prepares a firmware update of the module.
     * <para>
     *   This method returns a <c>YFirmwareUpdate</c> object which
     *   handles the firmware update process.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="path">
     *   the path of the <c>.byn</c> file to use.
     * </param>
     * <param name="force">
     *   true to force the firmware update even if some prerequisites appear not to be met
     * </param>
     * <returns>
     *   a <c>YFirmwareUpdate</c> object or NULL on error.
     * </returns>
     */
    public virtual async Task<YFirmwareUpdate> updateFirmwareEx(string path,bool force)
    {
        string serial;
        byte[] settings;

        serial = await this.get_serialNumber();
        settings = await this.get_allSettings();
        if ((settings).Length == 0) {
            this._throw(YAPI.IO_ERROR, "Unable to get device settings");
            settings = YAPI.DefaultEncoding.GetBytes("error:Unable to get device settings");
        }
        return new YFirmwareUpdate(_yapi, serial, path, settings, force);
    }

    /**
     * <summary>
     *   Prepares a firmware update of the module.
     * <para>
     *   This method returns a <c>YFirmwareUpdate</c> object which
     *   handles the firmware update process.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="path">
     *   the path of the <c>.byn</c> file to use.
     * </param>
     * <returns>
     *   a <c>YFirmwareUpdate</c> object or NULL on error.
     * </returns>
     */
    public virtual async Task<YFirmwareUpdate> updateFirmware(string path)
    {
        return await this.updateFirmwareEx(path, false);
    }

    /**
     * <summary>
     *   Returns all the settings and uploaded files of the module.
     * <para>
     *   Useful to backup all the
     *   logical names, calibrations parameters, and uploaded files of a device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a binary buffer with all the settings.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an binary object of size 0.
     * </para>
     */
    public virtual async Task<byte[]> get_allSettings()
    {
        byte[] settings;
        byte[] json;
        byte[] res;
        string sep;
        string name;
        string item;
        string t_type;
        string id;
        string url;
        string file_data;
        byte[] file_data_bin;
        byte[] temp_data_bin;
        string ext_settings;
        List<string> filelist = new List<string>();
        List<string> templist = new List<string>();

        settings = await this._download("api.json");
        if ((settings).Length == 0) {
            return settings;
        }
        ext_settings = ", \"extras\":[";
        templist = await this.get_functionIds("Temperature");
        sep = "";
        for (int ii = 0; ii <  templist.Count; ii++) {
            if (YAPIContext.imm_atoi(await this.get_firmwareRelease()) > 9000) {
                url = "api/"+ templist[ii]+"/sensorType";
                t_type = YAPI.DefaultEncoding.GetString(await this._download(url));
                if (t_type == "RES_NTC") {
                    id = ( templist[ii]).Substring( 11, ( templist[ii]).Length - 11);
                    temp_data_bin = await this._download("extra.json?page="+id);
                    if ((temp_data_bin).Length == 0) {
                        return temp_data_bin;
                    }
                    item = ""+ sep+"{\"fid\":\""+  templist[ii]+"\", \"json\":"+YAPI.DefaultEncoding.GetString(temp_data_bin)+"}\n";
                    ext_settings = ext_settings + item;
                    sep = ",";
                }
            }
        }
        ext_settings = ext_settings + "],\n\"files\":[";
        if (await this.hasFunction("files")) {
            json = await this._download("files.json?a=dir&f=");
            if ((json).Length == 0) {
                return json;
            }
            filelist = this.imm_json_get_array(json);
            sep = "";
            for (int ii = 0; ii <  filelist.Count; ii++) {
                name = this.imm_json_get_key(YAPI.DefaultEncoding.GetBytes( filelist[ii]), "name");
                if (((name).Length > 0) && !(name == "startupConf.json")) {
                    file_data_bin = await this._download(this.imm_escapeAttr(name));
                    file_data = YAPIContext.imm_bytesToHexStr(file_data_bin, 0, file_data_bin.Length);
                    item = ""+ sep+"{\"name\":\""+ name+"\", \"data\":\""+file_data+"\"}\n";
                    ext_settings = ext_settings + item;
                    sep = ",";
                }
            }
        }
        res = YAPI.DefaultEncoding.GetBytes("{ \"api\":" + YAPI.DefaultEncoding.GetString(settings) + ext_settings + "]}");
        return res;
    }

    public virtual async Task<int> loadThermistorExtra(string funcId,string jsonExtra)
    {
        List<string> values = new List<string>();
        string url;
        string curr;
        string currTemp;
        int ofs;
        int size;
        url = "api/" + funcId + ".json?command=Z";

        await this._download(url);
        // add records in growing resistance value
        values = this.imm_json_get_array(YAPI.DefaultEncoding.GetBytes(jsonExtra));
        ofs = 0;
        size = values.Count;
        while (ofs + 1 < size) {
            curr = values[ofs];
            currTemp = values[ofs + 1];
            url = "api/"+  funcId+"/.json?command=m"+ curr+":"+currTemp;
            await this._download(url);
            ofs = ofs + 2;
        }
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_extraSettings(string jsonExtra)
    {
        List<string> extras = new List<string>();
        string functionId;
        string data;
        extras = this.imm_json_get_array(YAPI.DefaultEncoding.GetBytes(jsonExtra));
        for (int ii = 0; ii <  extras.Count; ii++) {
            functionId = this.imm_get_json_path( extras[ii], "fid");
            functionId = this.imm_decode_json_string(functionId);
            data = this.imm_get_json_path( extras[ii], "json");
            if (await this.hasFunction(functionId)) {
                await this.loadThermistorExtra(functionId, data);
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Restores all the settings and uploaded files to the module.
     * <para>
     *   This method is useful to restore all the logical names and calibrations parameters,
     *   uploaded files etc. of a device from a backup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modifications must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="settings">
     *   a binary buffer with all the settings.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_allSettingsAndFiles(byte[] settings)
    {
        byte[] down;
        string json;
        string json_api;
        string json_files;
        string json_extra;
        json = YAPI.DefaultEncoding.GetString(settings);
        json_api = this.imm_get_json_path(json, "api");
        if (json_api == "") {
            return await this.set_allSettings(settings);
        }
        json_extra = this.imm_get_json_path(json, "extras");
        if (!(json_extra == "")) {
            await this.set_extraSettings(json_extra);
        }
        await this.set_allSettings(YAPI.DefaultEncoding.GetBytes(json_api));
        if (await this.hasFunction("files")) {
            List<string> files = new List<string>();
            string res;
            string name;
            string data;
            down = await this._download("files.json?a=format");
            res = this.imm_get_json_path(YAPI.DefaultEncoding.GetString(down), "res");
            res = this.imm_decode_json_string(res);
            if (!(res == "ok")) { this._throw( YAPI.IO_ERROR, "format failed"); return YAPI.IO_ERROR; }
            json_files = this.imm_get_json_path(json, "files");
            files = this.imm_json_get_array(YAPI.DefaultEncoding.GetBytes(json_files));
            for (int ii = 0; ii <  files.Count; ii++) {
                name = this.imm_get_json_path( files[ii], "name");
                name = this.imm_decode_json_string(name);
                data = this.imm_get_json_path( files[ii], "data");
                data = this.imm_decode_json_string(data);
                await this._upload(name, YAPIContext.imm_hexStrToBin(data));
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Tests if the device includes a specific function.
     * <para>
     *   This method takes a function identifier
     *   and returns a boolean.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="funcId">
     *   the requested function identifier
     * </param>
     * <returns>
     *   true if the device has the function identifier
     * </returns>
     */
    public virtual async Task<bool> hasFunction(string funcId)
    {
        int count;
        int i;
        string fid;

        count  = await this.functionCount();
        i = 0;
        while (i < count) {
            fid  = await this.functionId(i);
            if (fid == funcId) {
                return true;
            }
            i = i + 1;
        }
        return false;
    }

    /**
     * <summary>
     *   Retrieve all hardware identifier that match the type passed in argument.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="funType">
     *   The type of function (Relay, LightSensor, Voltage,...)
     * </param>
     * <returns>
     *   an array of strings.
     * </returns>
     */
    public virtual async Task<List<string>> get_functionIds(string funType)
    {
        int count;
        int i;
        string ftype;
        List<string> res = new List<string>();

        count = await this.functionCount();
        i = 0;
        while (i < count) {
            ftype = await this.functionType(i);
            if (ftype == funType) {
                res.Add(await this.functionId(i));
            } else {
                ftype = await this.functionBaseType(i);
                if (ftype == funType) {
                    res.Add(await this.functionId(i));
                }
            }
            i = i + 1;
        }
        return res;
    }

    //cannot be generated for Java:
    //public virtual byte[] imm_flattenJsonStruct(byte[] jsoncomplex)

    public virtual async Task<int> calibVersion(string cparams)
    {
        if (cparams == "0,") {
            return 3;
        }
        if ((cparams).IndexOf(",") >= 0) {
            if ((cparams).IndexOf(" ") > 0) {
                return 3;
            } else {
                return 1;
            }
        }
        if (cparams == "" || cparams == "0") {
            return 1;
        }
        if (((cparams).Length < 2) || ((cparams).IndexOf(".") >= 0)) {
            return 0;
        } else {
            return 2;
        }
    }

    public virtual async Task<int> calibScale(string unit_name,string sensorType)
    {
        if (unit_name == "g" || unit_name == "gauss" || unit_name == "W") {
            return 1000;
        }
        if (unit_name == "C") {
            if (sensorType == "") {
                return 16;
            }
            if (YAPIContext.imm_atoi(sensorType) < 8) {
                return 16;
            } else {
                return 100;
            }
        }
        if (unit_name == "m" || unit_name == "deg") {
            return 10;
        }
        return 1;
    }

    public virtual async Task<int> calibOffset(string unit_name)
    {
        if (unit_name == "% RH" || unit_name == "mbar" || unit_name == "lx") {
            return 0;
        }
        return 32767;
    }

    public virtual async Task<string> calibConvert(string param,string currentFuncValue,string unit_name,string sensorType)
    {
        int paramVer;
        int funVer;
        int funScale;
        int funOffset;
        int paramScale;
        int paramOffset;
        List<int> words = new List<int>();
        List<string> words_str = new List<string>();
        List<double> calibData = new List<double>();
        List<int> iCalib = new List<int>();
        int calibType;
        int i;
        int maxSize;
        double ratio;
        int nPoints;
        double wordVal;
        // Initial guess for parameter encoding
        paramVer = await this.calibVersion(param);
        funVer = await this.calibVersion(currentFuncValue);
        funScale = await this.calibScale(unit_name, sensorType);
        funOffset = await this.calibOffset(unit_name);
        paramScale = funScale;
        paramOffset = funOffset;
        if (funVer < 3) {
            // Read the effective device scale if available
            if (funVer == 2) {
                words = YAPIContext.imm_decodeWords(currentFuncValue);
                if ((words[0] == 1366) && (words[1] == 12500)) {
                    // Yocto-3D RefFrame used a special encoding
                    funScale = 1;
                    funOffset = 0;
                } else {
                    funScale = words[1];
                    funOffset = words[0];
                }
            } else {
                if (funVer == 1) {
                    if (currentFuncValue == "" || (YAPIContext.imm_atoi(currentFuncValue) > 10)) {
                        funScale = 0;
                    }
                }
            }
        }
        calibData.Clear();
        calibType = 0;
        if (paramVer < 3) {
            // Handle old 16 bit parameters formats
            if (paramVer == 2) {
                words = YAPIContext.imm_decodeWords(param);
                if ((words[0] == 1366) && (words[1] == 12500)) {
                    // Yocto-3D RefFrame used a special encoding
                    paramScale = 1;
                    paramOffset = 0;
                } else {
                    paramScale = words[1];
                    paramOffset = words[0];
                }
                if ((words.Count >= 3) && (words[2] > 0)) {
                    maxSize = 3 + 2 * ((words[2]) % (10));
                    if (maxSize > words.Count) {
                        maxSize = words.Count;
                    }
                    i = 3;
                    while (i < maxSize) {
                        calibData.Add((double) words[i]);
                        i = i + 1;
                    }
                }
            } else {
                if (paramVer == 1) {
                    words_str = new List<string>(param.Split(new char[] {','}));
                    for (int ii = 0; ii < words_str.Count; ii++) {
                        words.Add(YAPIContext.imm_atoi(words_str[ii]));
                    }
                    if (param == "" || (words[0] > 10)) {
                        paramScale = 0;
                    }
                    if ((words.Count > 0) && (words[0] > 0)) {
                        maxSize = 1 + 2 * ((words[0]) % (10));
                        if (maxSize > words.Count) {
                            maxSize = words.Count;
                        }
                        i = 1;
                        while (i < maxSize) {
                            calibData.Add((double) words[i]);
                            i = i + 1;
                        }
                    }
                } else {
                    if (paramVer == 0) {
                        ratio = Double.Parse(param);
                        if (ratio > 0) {
                            calibData.Add(0.0);
                            calibData.Add(0.0);
                            calibData.Add(Math.Round(65535 / ratio));
                            calibData.Add(65535.0);
                        }
                    }
                }
            }
            i = 0;
            while (i < calibData.Count) {
                if (paramScale > 0) {
                    // scalar decoding
                    calibData[i] = (calibData[i] - paramOffset) / paramScale;
                } else {
                    // floating-point decoding
                    calibData[i] = YAPIContext.imm_decimalToDouble((int) Math.Round(calibData[i]));
                }
                i = i + 1;
            }
        } else {
            // Handle latest 32bit parameter format
            iCalib = YAPIContext.imm_decodeFloats(param);
            calibType = (int) Math.Round(iCalib[0] / 1000.0);
            if (calibType >= 30) {
                calibType = calibType - 30;
            }
            i = 1;
            while (i < iCalib.Count) {
                calibData.Add(iCalib[i] / 1000.0);
                i = i + 1;
            }
        }
        if (funVer >= 3) {
            // Encode parameters in new format
            if (calibData.Count == 0) {
                param = "0,";
            } else {
                param = (30 + calibType).ToString();
                i = 0;
                while (i < calibData.Count) {
                    if (((i) & (1)) > 0) {
                        param = param + ":";
                    } else {
                        param = param + " ";
                    }
                    param = param + ((int) Math.Round(calibData[i] * 1000.0 / 1000.0)).ToString();
                    i = i + 1;
                }
                param = param + ",";
            }
        } else {
            if (funVer >= 1) {
                // Encode parameters for older devices
                nPoints = ((calibData.Count) / (2));
                param = (nPoints).ToString();
                i = 0;
                while (i < 2 * nPoints) {
                    if (funScale == 0) {
                        wordVal = YAPIContext.imm_doubleToDecimal((int) Math.Round(calibData[i]));
                    } else {
                        wordVal = calibData[i] * funScale + funOffset;
                    }
                    param = param + "," + (Math.Round(wordVal)).ToString();
                    i = i + 1;
                }
            } else {
                // Initial V0 encoding used for old Yocto-Light
                if (calibData.Count == 4) {
                    param = (Math.Round(1000 * (calibData[3] - calibData[1]) / calibData[2] - calibData[0])).ToString();
                }
            }
        }
        return param;
    }

    /**
     * <summary>
     *   Restores all the settings of the device.
     * <para>
     *   Useful to restore all the logical names and calibrations parameters
     *   of a module from a backup.Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modifications must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="settings">
     *   a binary buffer with all the settings.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_allSettings(byte[] settings)
    {
        List<string> restoreLast = new List<string>();
        byte[] old_json_flat;
        List<string> old_dslist = new List<string>();
        List<string> old_jpath = new List<string>();
        List<int> old_jpath_len = new List<int>();
        List<string> old_val_arr = new List<string>();
        byte[] actualSettings;
        List<string> new_dslist = new List<string>();
        List<string> new_jpath = new List<string>();
        List<int> new_jpath_len = new List<int>();
        List<string> new_val_arr = new List<string>();
        int cpos;
        int eqpos;
        int leng;
        int i;
        int j;
        string njpath;
        string jpath;
        string fun;
        string attr;
        string value;
        string url;
        string tmp;
        string new_calib;
        string sensorType;
        string unit_name;
        string newval;
        string oldval;
        string old_calib;
        string each_str;
        bool do_update;
        bool found;
        tmp = YAPI.DefaultEncoding.GetString(settings);
        tmp = this.imm_get_json_path(tmp, "api");
        if (!(tmp == "")) {
            settings = YAPI.DefaultEncoding.GetBytes(tmp);
        }
        oldval = "";
        newval = "";
        old_json_flat = this.imm_flattenJsonStruct(settings);
        old_dslist = this.imm_json_get_array(old_json_flat);
        for (int ii = 0; ii < old_dslist.Count; ii++) {
            each_str = this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(old_dslist[ii]));
            // split json path and attr
            leng = (each_str).Length;
            eqpos = (each_str).IndexOf("=");
            if ((eqpos < 0) || (leng == 0)) {
                this._throw(YAPI.INVALID_ARGUMENT, "Invalid settings");
                return YAPI.INVALID_ARGUMENT;
            }
            jpath = (each_str).Substring( 0, eqpos);
            eqpos = eqpos + 1;
            value = (each_str).Substring( eqpos, leng - eqpos);
            old_jpath.Add(jpath);
            old_jpath_len.Add((jpath).Length);
            old_val_arr.Add(value);
        }

        actualSettings = await this._download("api.json");
        actualSettings = this.imm_flattenJsonStruct(actualSettings);
        new_dslist = this.imm_json_get_array(actualSettings);
        for (int ii = 0; ii < new_dslist.Count; ii++) {
            // remove quotes
            each_str = this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(new_dslist[ii]));
            // split json path and attr
            leng = (each_str).Length;
            eqpos = (each_str).IndexOf("=");
            if ((eqpos < 0) || (leng == 0)) {
                this._throw(YAPI.INVALID_ARGUMENT, "Invalid settings");
                return YAPI.INVALID_ARGUMENT;
            }
            jpath = (each_str).Substring( 0, eqpos);
            eqpos = eqpos + 1;
            value = (each_str).Substring( eqpos, leng - eqpos);
            new_jpath.Add(jpath);
            new_jpath_len.Add((jpath).Length);
            new_val_arr.Add(value);
        }
        i = 0;
        while (i < new_jpath.Count) {
            njpath = new_jpath[i];
            leng = (njpath).Length;
            cpos = (njpath).IndexOf("/");
            if ((cpos < 0) || (leng == 0)) {
                continue;
            }
            fun = (njpath).Substring( 0, cpos);
            cpos = cpos + 1;
            attr = (njpath).Substring( cpos, leng - cpos);
            do_update = true;
            if (fun == "services") {
                do_update = false;
            }
            if ((do_update) && (attr == "firmwareRelease")) {
                do_update = false;
            }
            if ((do_update) && (attr == "usbCurrent")) {
                do_update = false;
            }
            if ((do_update) && (attr == "upTime")) {
                do_update = false;
            }
            if ((do_update) && (attr == "persistentSettings")) {
                do_update = false;
            }
            if ((do_update) && (attr == "adminPassword")) {
                do_update = false;
            }
            if ((do_update) && (attr == "userPassword")) {
                do_update = false;
            }
            if ((do_update) && (attr == "rebootCountdown")) {
                do_update = false;
            }
            if ((do_update) && (attr == "advertisedValue")) {
                do_update = false;
            }
            if ((do_update) && (attr == "poeCurrent")) {
                do_update = false;
            }
            if ((do_update) && (attr == "readiness")) {
                do_update = false;
            }
            if ((do_update) && (attr == "ipAddress")) {
                do_update = false;
            }
            if ((do_update) && (attr == "subnetMask")) {
                do_update = false;
            }
            if ((do_update) && (attr == "router")) {
                do_update = false;
            }
            if ((do_update) && (attr == "linkQuality")) {
                do_update = false;
            }
            if ((do_update) && (attr == "ssid")) {
                do_update = false;
            }
            if ((do_update) && (attr == "channel")) {
                do_update = false;
            }
            if ((do_update) && (attr == "security")) {
                do_update = false;
            }
            if ((do_update) && (attr == "message")) {
                do_update = false;
            }
            if ((do_update) && (attr == "currentValue")) {
                do_update = false;
            }
            if ((do_update) && (attr == "currentRawValue")) {
                do_update = false;
            }
            if ((do_update) && (attr == "currentRunIndex")) {
                do_update = false;
            }
            if ((do_update) && (attr == "pulseTimer")) {
                do_update = false;
            }
            if ((do_update) && (attr == "lastTimePressed")) {
                do_update = false;
            }
            if ((do_update) && (attr == "lastTimeReleased")) {
                do_update = false;
            }
            if ((do_update) && (attr == "filesCount")) {
                do_update = false;
            }
            if ((do_update) && (attr == "freeSpace")) {
                do_update = false;
            }
            if ((do_update) && (attr == "timeUTC")) {
                do_update = false;
            }
            if ((do_update) && (attr == "rtcTime")) {
                do_update = false;
            }
            if ((do_update) && (attr == "unixTime")) {
                do_update = false;
            }
            if ((do_update) && (attr == "dateTime")) {
                do_update = false;
            }
            if ((do_update) && (attr == "rawValue")) {
                do_update = false;
            }
            if ((do_update) && (attr == "lastMsg")) {
                do_update = false;
            }
            if ((do_update) && (attr == "delayedPulseTimer")) {
                do_update = false;
            }
            if ((do_update) && (attr == "rxCount")) {
                do_update = false;
            }
            if ((do_update) && (attr == "txCount")) {
                do_update = false;
            }
            if ((do_update) && (attr == "msgCount")) {
                do_update = false;
            }
            if (do_update) {
                do_update = false;
                newval = new_val_arr[i];
                j = 0;
                found = false;
                while ((j < old_jpath.Count) && !(found)) {
                    if ((new_jpath_len[i] == old_jpath_len[j]) && (new_jpath[i] == old_jpath[j])) {
                        found = true;
                        oldval = old_val_arr[j];
                        if (!(newval == oldval)) {
                            do_update = true;
                        }
                    }
                    j = j + 1;
                }
            }
            if (do_update) {
                if (attr == "calibrationParam") {
                    old_calib = "";
                    unit_name = "";
                    sensorType = "";
                    new_calib = newval;
                    j = 0;
                    found = false;
                    while ((j < old_jpath.Count) && !(found)) {
                        if ((new_jpath_len[i] == old_jpath_len[j]) && (new_jpath[i] == old_jpath[j])) {
                            found = true;
                            old_calib = old_val_arr[j];
                        }
                        j = j + 1;
                    }
                    tmp = fun + "/unit";
                    j = 0;
                    found = false;
                    while ((j < new_jpath.Count) && !(found)) {
                        if (tmp == new_jpath[j]) {
                            found = true;
                            unit_name = new_val_arr[j];
                        }
                        j = j + 1;
                    }
                    tmp = fun + "/sensorType";
                    j = 0;
                    found = false;
                    while ((j < new_jpath.Count) && !(found)) {
                        if (tmp == new_jpath[j]) {
                            found = true;
                            sensorType = new_val_arr[j];
                        }
                        j = j + 1;
                    }
                    newval = await this.calibConvert(old_calib,  new_val_arr[i],  unit_name, sensorType);
                    url = "api/" + fun + ".json?" + attr + "=" + this.imm_escapeAttr(newval);
                    await this._download(url);
                } else {
                    url = "api/" + fun + ".json?" + attr + "=" + this.imm_escapeAttr(oldval);
                    if (attr == "resolution") {
                        restoreLast.Add(url);
                    } else {
                        await this._download(url);
                    }
                }
            }
            i = i + 1;
        }
        for (int ii = 0; ii < restoreLast.Count; ii++) {
            await this._download(restoreLast[ii]);
        }
        await this.clearCache();
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Downloads the specified built-in file and returns a binary buffer with its content.
     * <para>
     * </para>
     * </summary>
     * <param name="pathname">
     *   name of the new file to load
     * </param>
     * <returns>
     *   a binary buffer with the file content
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YAPI.INVALID_STRING</c>.
     * </para>
     */
    public virtual async Task<byte[]> download(string pathname)
    {
        return await this._download(pathname);
    }

    /**
     * <summary>
     *   Returns the icon of the module.
     * <para>
     *   The icon is a PNG image and does not
     *   exceeds 1536 bytes.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a binary buffer with module icon, in png format.
     *   On failure, throws an exception or returns  <c>YAPI.INVALID_STRING</c>.
     * </returns>
     */
    public virtual async Task<byte[]> get_icon2d()
    {
        return await this._download("icon2d.png");
    }

    /**
     * <summary>
     *   Returns a string with last logs of the module.
     * <para>
     *   This method return only
     *   logs that are still in the module.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with last logs of the module.
     *   On failure, throws an exception or returns  <c>YAPI.INVALID_STRING</c>.
     * </returns>
     */
    public virtual async Task<string> get_lastLogs()
    {
        byte[] content;

        content = await this._download("logs.txt");
        return YAPI.DefaultEncoding.GetString(content);
    }

    /**
     * <summary>
     *   Adds a text message to the device logs.
     * <para>
     *   This function is useful in
     *   particular to trace the execution of HTTP callbacks. If a newline
     *   is desired after the message, it must be included in the string.
     * </para>
     * </summary>
     * <param name="text">
     *   the string to append to the logs.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> log(string text)
    {
        return await this._upload("logs.txt", YAPI.DefaultEncoding.GetBytes(text));
    }

    //cannot be generated for Java:
    //public virtual async Task<List<string>> get_subDevices()

    //cannot be generated for Java:
    //public virtual async Task<string> get_parentHub()

    //cannot be generated for Java:
    //public virtual async Task<string> get_url()

    /**
     * <summary>
     *   Continues the module enumeration started using <c>yFirstModule()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YModule</c> object, corresponding to
     *   the next module found, or a <c>null</c> pointer
     *   if there are no more modules to enumerate.
     * </returns>
     */
    public YModule nextModule()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindModuleInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of modules currently accessible.
     * <para>
     *   Use the method <c>YModule.nextModule()</c> to iterate on the
     *   next modules.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YModule</c> object, corresponding to
     *   the first module currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YModule FirstModule()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Module");
        if (next_hwid == null)  return null;
        return FindModuleInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YModule FirstModuleInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Module");
        if (next_hwid == null)  return null;
        return FindModuleInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YModule implementation)
    }
}