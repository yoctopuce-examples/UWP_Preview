/*********************************************************************
 *
 * $Id: YFunction.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * YFunction Class (virtual class, used internally)
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
using System.Threading.Tasks;


namespace com.yoctopuce.YoctoAPI
{




    //--- (generated code: YFunction class start)
/**
 * <summary>
 *   YFunction Class: Common function interface
 * <para>
 *   This is the parent class for all public objects representing device functions documented in
 *   the high-level programming API. This abstract class does all the real job, but without
 *   knowledge of the specific function attributes.
 * </para>
 * <para>
 *   Instantiating a child class of YFunction does not cause any communication.
 *   The instance simply keeps track of its function identifier, and will dynamically bind
 *   to a matching device at the time it is really being used to read or set an attribute.
 *   In order to allow true hot-plug replacement of one device by another, the binding stay
 *   dynamic through the life of the object.
 * </para>
 * <para>
 *   The YFunction class implements a generic high-level cache for the attribute values of
 *   the specified function, pre-parsed from the REST API string.
 * </para>
 * </summary>
 */
public class YFunction
{
//--- (end of generated code: YFunction class start)

        public const string FUNCTIONDESCRIPTOR_INVALID = "!INVALID!";
        protected internal readonly YAPIContext _yapi;
        protected internal readonly string _className;
        protected internal readonly string _func;
        protected internal int _lastErrorType;
        protected internal string _lastErrorMsg;
        protected internal object _userData;
        protected internal readonly Dictionary<string, YDataStream> _dataStreams;
        //--- (generated code: YFunction definitions)
    /**
     * <summary>
     *   invalid logicalName value
     * </summary>
     */
    public const  string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid advertisedValue value
     * </summary>
     */
    public const  string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
    protected string _logicalName = LOGICALNAME_INVALID;
    protected string _advertisedValue = ADVERTISEDVALUE_INVALID;
    protected ValueCallback _valueCallbackFunction = null;
    protected ulong _cacheExpiration = 0;
    protected string _serial;
    protected string _funId;
    protected string _hwId;

    public delegate Task ValueCallback(YFunction func, string value);
    public delegate Task TimedReportCallback(YFunction func, YMeasure measure);
    //--- (end of generated code: YFunction definitions)


        protected internal YFunction(YAPIContext yctx, string func, string classname)
        {
            _yapi = yctx;
            _className = classname;
            _func = func;
            _lastErrorType = YAPI.SUCCESS;
            _lastErrorMsg = "";
            _userData = null;
            _dataStreams = new Dictionary<string, YDataStream>();
            //--- (generated code: YFunction attributes initialization)
        //--- (end of generated code: YFunction attributes initialization)
        }

        protected internal YFunction(YAPIContext yctx, string func) : this(yctx, func, "Function")
        { }

        public YFunction(string func) : this(YAPI.imm_GetYCtx(), func, "Function")
        { }

        public int _throw(int errType, string errMsg)
        {
            _lastErrorType = errType;
            _lastErrorMsg = errMsg;
            if (!(_yapi._exceptionsDisabled)) {
                throw new YAPI_Exception(errType, "YoctoApi error : " + errMsg);
            }
            return errType;
        }


        protected internal static YFunction _FindFromCacheInContext(YAPIContext yctx, string className, string func)
        {
            return yctx._yHash.imm_getFunction(className, func);
        }

        protected internal static YFunction _FindFromCache(string className, string func)
        {
            YAPIContext ctx = YAPI.imm_GetYCtx();
            return ctx._yHash.imm_getFunction(className, func);
        }


        protected internal static void _AddToCache(string className, string func, YFunction obj)
        {
            obj._yapi._yHash.imm_setFunction(className, func, obj);
        }

        protected internal static async Task _UpdateValueCallbackList(YFunction func, bool add)
        {
            await func._yapi._UpdateValueCallbackList(func, add);
        }

        protected internal static async Task _UpdateTimedReportCallbackList(YFunction func, bool add)
        {
            await func._yapi._UpdateTimedReportCallbackList(func, add);
        }

        //--- (generated code: YFunction implementation)
#pragma warning disable 1998
    internal virtual void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("logicalName")) {
            _logicalName = json_val.GetString("logicalName");
        }
        if (json_val.Has("advertisedValue")) {
            _advertisedValue = json_val.GetString("advertisedValue");
        }
    }

    /**
     * <summary>
     *   Returns the logical name of the function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the logical name of the function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YFunction.LOGICALNAME_INVALID</c>.
     * </para>
     */
    public async Task<string> get_logicalName()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LOGICALNAME_INVALID;
            }
        }
        res = _logicalName;
        return res;
    }


    /**
     * <summary>
     *   Changes the logical name of the function.
     * <para>
     *   You can use <c>yCheckLogicalName()</c>
     *   prior to this call to make sure that your parameter is valid.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the logical name of the function
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
    public async Task<int> set_logicalName(string  newval)
    {
        string rest_val;
        if (!YAPI.CheckLogicalName(newval))
            _throw(YAPI.INVALID_ARGUMENT,"Invalid name :" + newval);
        rest_val = newval;
        await _setAttr("logicalName",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a short string representing the current state of the function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to a short string representing the current state of the function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YFunction.ADVERTISEDVALUE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_advertisedValue()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ADVERTISEDVALUE_INVALID;
            }
        }
        res = _advertisedValue;
        return res;
    }


    public async Task<int> set_advertisedValue(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("advertisedValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a function for a given identifier.
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
     *   This function does not require that the function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YFunction.isOnline()</c> to test if the function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a function by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the function
     * </param>
     * <returns>
     *   a <c>YFunction</c> object allowing you to drive the function.
     * </returns>
     */
    public static YFunction FindFunction(string func)
    {
        YFunction obj;
        obj = (YFunction) YFunction._FindFromCache("Function", func);
        if (obj == null) {
            obj = new YFunction(func);
            YFunction._AddToCache("Function",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a function for a given identifier in a YAPI context.
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
     *   This function does not require that the function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YFunction.isOnline()</c> to test if the function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a function by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the function
     * </param>
     * <returns>
     *   a <c>YFunction</c> object allowing you to drive the function.
     * </returns>
     */
    public static YFunction FindFunctionInContext(YAPIContext yctx,string func)
    {
        YFunction obj;
        obj = (YFunction) YFunction._FindFromCacheInContext(yctx,  "Function", func);
        if (obj == null) {
            obj = new YFunction(yctx, func);
            YFunction._AddToCache("Function",  func, obj);
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
    public virtual async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackFunction = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public virtual async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackFunction != null) {
            await _valueCallbackFunction(this, value);
        } else {
        }
        return 0;
    }

    /**
     * <summary>
     *   Disables the propagation of every new advertised value to the parent hub.
     * <para>
     *   You can use this function to save bandwidth and CPU on computers with limited
     *   resources, or to prevent unwanted invocations of the HTTP callback.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> muteValueCallbacks()
    {
        return await this.set_advertisedValue("SILENT");
    }

    /**
     * <summary>
     *   Re-enables the propagation of every new advertised value to the parent hub.
     * <para>
     *   This function reverts the effect of a previous call to <c>muteValueCallbacks()</c>.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> unmuteValueCallbacks()
    {
        return await this.set_advertisedValue("");
    }

    /**
     * <summary>
     *   Returns the current value of a single function attribute, as a text string, as quickly as
     *   possible but without using the cached value.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="attrName">
     *   the name of the requested attribute
     * </param>
     * <returns>
     *   a string with the value of the the attribute
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual async Task<string> loadAttribute(string attrName)
    {
        string url;
        byte[] attrVal;
        url = "api/"+ await this.get_functionId()+"/"+attrName;
        attrVal = await this._download(url);
        return YAPI.DefaultEncoding.GetString(attrVal);
    }

    public virtual async Task<int> _parserHelper()
    {
        return 0;
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public YFunction nextFunction()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindFunctionInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YFunction FirstFunction()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Function");
        if (next_hwid == null)  return null;
        return FindFunctionInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YFunction FirstFunctionInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Function");
        if (next_hwid == null)  return null;
        return FindFunctionInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YFunction implementation)

        /// <summary>
        ///  Returns a short text that describes unambiguously the instance of the function in the form
        /// TYPE(NAME)=SERIAL&#46;FUNCTIONID.
        /// More precisely,
        /// TYPE       is the type of the function,
        /// NAME       it the name used for the first access to the function,
        /// SERIAL     is the serial number of the module if the module is connected or "unresolved", and
        /// FUNCTIONID is  the hardware identifier of the function if the module is connected.
        /// For example, this method returns Relay(MyCustomName.relay1)=RELAYLO1-123456.relay1 if the
        /// module is already connected or Relay(BadCustomeName.relay1)=unresolved if the module has
        /// not yet been connected. This method does not trigger any USB or TCP transaction and can therefore be used in
        /// a debugger.
        /// </summary>
        /// <returns> a string that describes the function
        ///         (ex: Relay(MyCustomName.relay1)=RELAYLO1-123456.relay1) </returns>
        public virtual string describe()
        {
            try {
                string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
                return _className + "(" + _func + ")=" + hwid;
            } catch (YAPI_Exception) {
            }
            return _className + "(" + _func + ")=unresolved";

        }

#pragma warning disable 1998

        /// <summary>
        /// Returns the unique hardware identifier of the function in the form SERIAL.FUNCTIONID.
        /// The unique hardware identifier is composed of the device serial
        /// number and of the hardware identifier of the function (for example RELAYLO1-123456.relay1).
        /// </summary>
        /// <returns> a string that uniquely identifies the function (ex: RELAYLO1-123456.relay1)
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> get_hardwareId()
        {
            //fixme: add fallback to isOnline to check that device is present
            return _yapi._yHash.imm_resolveHwID(_className, _func);
        }

        internal virtual string imm_get_hardwareId()
        {
            return _yapi._yHash.imm_resolveHwID(_className, _func);
        }


        /// <summary>
        /// Returns the hardware identifier of the function, without reference to the module. For example
        /// relay1
        /// </summary>
        /// <returns> a string that identifies the function (ex: relay1)
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> get_functionId()
        {
            return _yapi._yHash.imm_resolveFuncId(_className, _func);
        }

        internal virtual string imm_get_functionId()
        {
            return _yapi._yHash.imm_resolveFuncId(_className, _func);
        }


        /// <summary>
        /// Returns a global identifier of the function in the format MODULE_NAME&#46;FUNCTION_NAME.
        /// The returned string uses the logical names of the module and of the function if they are defined,
        /// otherwise the serial number of the module and the hardware identifier of the function
        /// (for example: MyCustomName.relay1)
        /// </summary>
        /// <returns> a string that uniquely identifies the function using logical names
        ///         (ex: MyCustomName.relay1)
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<string> get_friendlyName()
        {
            YPEntry yp = _yapi._yHash.imm_resolveFunction(_className, _func);
            return yp.getFriendlyName(_yapi);

        }

#pragma warning restore 1998

        public override string ToString()
        {
            return describe();
        }

        internal virtual async void _parse(YJSONObject json, ulong msValidity)
        {
            _cacheExpiration = YAPI.GetTickCount() + msValidity;
            imm_parseAttr(json);
            await _parserHelper();
        }

        protected string imm_escapeAttr(string changeval)
        {
            string espcaped = "";
            int i = 0;
            char c = '\0';
            string h = null;
            for (i = 0; i < changeval.Length; i++) {
                c = changeval[i];
                if (c <= ' ' || (c > 'z' && c != '~') || c == '"' || c == '%' || c == '&' ||
                           c == '+' || c == '<' || c == '=' || c == '>' || c == '\\' || c == '^' || c == '`') {
                    int hh;
                    if ((c == 0xc2 || c == 0xc3) && (i + 1 < changeval.Length) && (changeval[i + 1] & 0xc0) == 0x80) {
                        // UTF8-encoded ISO-8859-1 character: translate to plain ISO-8859-1
                        hh = (c & 1) * 0x40;
                        i++;
                        hh += changeval[i];
                    } else {
                        hh = c;
                    }
                    h = hh.ToString("X");
                    if ((h.Length < 2))
                        h = "0" + h;
                    espcaped += "%" + h;
                } else {
                    espcaped += c;
                }
            }
            return espcaped;
        }

        // Change the value of an attribute on a device, and update cache on the fly
        // Note: the function cache is a typed (parsed) cache, contrarily to the agnostic device cache
        protected internal virtual async Task<int> _setAttr(string attr, string newval)
        {
            if (newval == null) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Undefined value to set for attribute " + attr);
            }
            string attrname = imm_escapeAttr(attr);
            string extra = "/" + attrname + "?" + attrname + "=" + imm_escapeAttr(newval) + "&.";
            await _devRequest(extra);
            if (_cacheExpiration != 0) {
                _cacheExpiration = YAPI.GetTickCount();
            }
            return YAPI.SUCCESS;
        }

        private async Task<byte[]> _request(string req_first_line, byte[] req_head_and_body)
        {
            YDevice dev = await getYDevice();
            return await dev.requestHTTPSync(req_first_line, req_head_and_body);
        }

        protected internal virtual async Task<int> _upload(string path, byte[] content)
        {
            YDevice dev = await getYDevice();
            return await dev.requestHTTPUpload(path, content);
        }

        protected internal virtual async Task<int> _upload(string pathname, string content)
        {
            return await this._upload(pathname, YAPI.DefaultEncoding.GetBytes(content));
        }

        protected internal virtual async Task<byte[]> _download(string url)
        {
            string request = "GET /" + url + " HTTP/1.1\r\n\r\n";
            return await _request(request, null);
        }


        protected internal virtual string imm_json_get_key(byte[] json, string key)
        {
            YJSONObject obj = new YJSONObject(YAPI.DefaultEncoding.GetString(json));
            obj.Parse();
            if (obj.Has(key)) {
                string val = obj.GetString(key);
                if (val == null) {
                    val = obj.ToString();
                }
                return val;
            }
            throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "No key " + key + "in JSON struct");
        }

        protected internal virtual string imm_json_get_string(byte[] json)
        {
            string s = YAPI.DefaultEncoding.GetString(json);
            YJSONString jstring = new YJSONString(s, 0, s.Length);
            jstring.Parse();
            return jstring.GetString();
        }

        protected internal virtual List<string> imm_json_get_array(byte[] json)
        {
            YJSONArray array = new YJSONArray(YAPI.DefaultEncoding.GetString(json));
            array.Parse();
            List<string> list = new List<string>();
            int len = array.Length;
            for (int i = 0; i < len; i++) {
                YJSONContent o = array.Get(i);
                list.Add(o.ToJSON());
            }
            return list;
        }


        internal virtual string imm_get_json_path_struct(YJSONObject jsonObject, string[] paths, int ofs)
        {

            string key = paths[ofs];
            if (!jsonObject.Has(key)) {
                return "";
            }

            YJSONContent obj = jsonObject.Get(key);
            if (obj != null) {
                if (paths.Length == ofs + 1) {
                    return obj.ToJSON();
                }

                if (obj is YJSONArray) {
                    return imm_get_json_path_array(jsonObject.GetYJSONArray(key), paths, ofs + 1);
                } else if (obj is YJSONObject) {
                    return imm_get_json_path_struct(jsonObject.GetYJSONObject(key), paths, ofs + 1);
                }
            }
            return "";
        }

        private string imm_get_json_path_array(YJSONArray jsonArray, string[] paths, int ofs)
        {
            int key = Convert.ToInt32(paths[ofs]);
            if (jsonArray.Length <= key) {
                return "";
            }

            YJSONContent obj = jsonArray.Get(key);
            if (obj != null) {
                if (paths.Length == ofs + 1) {
                    return obj.ToString();
                }

                if (obj is YJSONArray) {
                    return imm_get_json_path_array(jsonArray.GetYJSONArray(key), paths, ofs + 1);
                } else if (obj is YJSONObject) {
                    return imm_get_json_path_struct(jsonArray.GetYJSONObject(key), paths, ofs + 1);
                }
            }
            return "";
        }


        protected internal virtual string imm_get_json_path(string json, string path)
        {
            YJSONObject jsonObject = null;
            jsonObject = new YJSONObject(json);
            jsonObject.Parse();
            string[] split = path.Split(new char[] { '\\', '|' });
            return imm_get_json_path_struct(jsonObject, split, 0);
        }

        internal virtual string imm_decode_json_string(string json)
        {
            YJSONString ystr = new YJSONString(json, 0, json.Length);
            ystr.Parse();
            return ystr.GetString();
        }

        // Load and parse the REST API for a function given by class name and
        // identifier, possibly applying changes
        // Device cache will be preloaded when loading function "module" and
        // leveraged for other modules
        internal virtual async Task<YJSONObject> _devRequest(string extra)
        {
            YDevice dev = await getYDevice();
            _hwId = _yapi._yHash.imm_resolveHwID(_className, _func);
            string[] split = _hwId.Split(new char[] { '\\', '.' });
            _funId = split[1];
            _serial = split[0];
            YJSONObject loadval = null;
            if (extra.Equals("")) {
                // use a cached API string, without reloading unless module is
                // requested
                string yreq = await dev.requestAPI();
                YJSONObject jsonval = new YJSONObject(yreq);
                jsonval.Parse();
                loadval = jsonval.GetYJSONObject(_funId);

            } else {
                dev.imm_clearCache();
            }
            if (loadval == null) {
                // request specified function only to minimize traffic
                if (extra.Equals("")) {
                    string httpreq = "GET /api/" + _funId + ".json";
                    string yreq = await dev.requestHTTPSyncAsString(httpreq, null);
                    loadval = new YJSONObject(yreq);
                    loadval.Parse();
                } else {
                    string httpreq = "GET /api/" + _funId + extra;
                    await dev.requestHTTPAsync(httpreq, null, null, null);
                    return null;
                }
            }
            return loadval;
        }

        internal virtual async Task<YDevice> getYDevice()
        {
            return await _yapi.funcGetDevice(_className, _func);
        }

        // Method used to cache DataStream objects (new DataLogger)
        internal virtual YDataStream imm_findDataStream(YDataSet dataset, string def)
        {
            string key = dataset.get_functionId() + ":" + def;
            if (_dataStreams.ContainsKey(key)) {
                return _dataStreams[key];
            }

            YDataStream newDataStream = new YDataStream(this, dataset, YAPIContext.imm_decodeWords(def));
            _dataStreams[key] = newDataStream;
            return newDataStream;
        }

        // Method used to clear cache of DataStream object (undocumented)
        public virtual void imm_clearDataStreamCache()
        {
            _dataStreams.Clear();
        }

        /// <summary>
        /// Checks if the function is currently reachable, without raising any error.
        /// If there is a cached value for the function in cache, that has not yet
        /// expired, the device is considered reachable.
        /// No exception is raised if there is an error while trying to contact the
        /// device hosting the function.
        /// </summary>
        /// <returns> true if the function can be reached, and false otherwise </returns>
        public virtual async Task<bool> isOnline()
        {
            // A valid value in cache means that the device is online
            if (_cacheExpiration > YAPI.GetTickCount()) {
                return true;
            }
            try {
                // Check that the function is available without throwing exceptions
                await load(_yapi.DefaultCacheValidity);
            } catch (YAPI_Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the numerical error code of the latest error with the function.
        /// This method is mostly useful when using the Yoctopuce library with
        /// exceptions disabled.
        /// </summary>
        /// <returns> a number corresponding to the code of the latest error that occurred while
        ///         using the function object </returns>
        public virtual int get_errorType()
        {
            return _lastErrorType;
        }

        public virtual int ErrorType {
            get {
                return _lastErrorType;
            }
        }

        public virtual int errorType()
        {
            return _lastErrorType;
        }

        public virtual int errType()
        {
            return _lastErrorType;
        }

        /// <summary>
        /// Returns the error message of the latest error with the function.
        /// This method is mostly useful when using the Yoctopuce library with
        /// exceptions disabled.
        /// </summary>
        /// <returns> a string corresponding to the latest error message that occured while
        ///         using the function object </returns>
        public virtual string get_errorMessage()
        {
            return _lastErrorMsg;
        }

        public virtual string ErrorMessage {
            get {
                return _lastErrorMsg;
            }
        }

        public virtual string errorMessage()
        {
            return _lastErrorMsg;
        }

        public virtual string errMessage()
        {
            return _lastErrorMsg;
        }


        /// <summary>
        /// Invalidates the cache. Invalidates the cache of the function attributes. Forces the
        /// next call to get_xxx() or loadxxx() to use values that come from the device.
        /// 
        /// 
        /// </summary>
        public virtual async Task clearCache()
        {
            try {
                YDevice dev = await getYDevice();
                dev.imm_clearCache();
            } catch (YAPI_Exception) {
            }
            if (_cacheExpiration != 0) {
                _cacheExpiration = YAPI.GetTickCount();
            }
        }


        /// <summary>
        /// Preloads the function cache with a specified validity duration.
        /// By default, whenever accessing a device, all function attributes
        /// are kept in cache for the standard duration (5 ms). This method can be
        /// used to temporarily mark the cache as valid for a longer period, in order
        /// to reduce network traffic for instance.
        /// </summary>
        /// <param name="msValidity"> : an integer corresponding to the validity attributed to the
        ///         loaded function parameters, in milliseconds
        /// </param>
        /// <returns> YAPI.SUCCESS when the call succeeds.
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<int> load(ulong msValidity)
        {
            YJSONObject json_obj = await _devRequest("");
            _parse(json_obj, msValidity);
            return YAPI.SUCCESS;
        }

        /// <summary>
        /// Gets the YModule object for the device on which the function is located.
        /// If the function cannot be located on any module, the returned instance of
        /// YModule is not shown as on-line.
        /// </summary>
        /// <returns> an instance of YModule </returns>
        public virtual async Task<YModule> get_module()
        {
            // try to resolve the function name to a device id without query
            if (_serial != null && !_serial.Equals("")) {
                return YModule.FindModuleInContext(_yapi, _serial + ".module");
            }
            if (_func.IndexOf('.') == -1) {
                try {
                    string serial = _yapi._yHash.imm_resolveSerial(_className, _func);
                    return YModule.FindModuleInContext(_yapi, serial + ".module");
                } catch (YAPI_Exception) {
                }
            }
            try {
                // device not resolved for now, force a communication for a last chance resolution
                if (await load(YAPI.DefaultCacheValidity) == YAPI.SUCCESS) {
                    string serial = _yapi._yHash.imm_resolveSerial(_className, _func);
                    return YModule.FindModuleInContext(_yapi, serial + ".module");
                }
            } catch (YAPI_Exception) {
            }
            // return a true yFindModule object even if it is not a module valid for communicating
            return YModule.FindModuleInContext(_yapi, "module_of_" + _className + "_" + _func);
        }


        public virtual async Task<YModule> module()
        {
            return await get_module();
        }

#pragma warning disable 1998

        /// <summary>
        /// Returns a unique identifier of type YFUN_DESCR corresponding to the function.
        /// This identifier can be used to test if two instances of YFunction reference the same
        /// physical function on the same physical device.
        /// </summary>
        /// <returns> an identifier of type YFUN_DESCR.
        /// 
        /// If the function has never been contacted, the returned value is YFunction.FUNCTIONDESCRIPTOR_INVALID. </returns>
        public virtual async Task<string> get_functionDescriptor()
        {
            // try to resolve the function name to a device id without query
            try {
                return _yapi._yHash.imm_resolveHwID(_className, _func);
            } catch (YAPI_Exception) {
                return FUNCTIONDESCRIPTOR_INVALID;
            }
        }



        /// <summary>
        /// Returns the value of the userData attribute, as previously stored using method
        /// set_userData.
        /// This attribute is never touched directly by the API, and is at disposal of the caller to
        /// store a context.
        /// </summary>
        /// <returns> the object stored previously by the caller. </returns>
        public virtual async Task<object> get_userData()
        {
            return _userData;
        }



        /// <summary>
        /// Stores a user context provided as argument in the userData attribute of the function.
        /// This attribute is never touched by the API, and is at disposal of the caller to store a context.
        /// </summary>
        /// <param name="data"> : any kind of object to be stored
        ///  </param>
        public virtual async Task set_userData(object data)
        {
            _userData = data;
        }
#pragma warning restore 1998
    }

}