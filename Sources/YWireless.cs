/*********************************************************************
 *
 * $Id: YWireless.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindWireless(), the high-level API for Wireless functions
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
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

    //--- (generated code: YWireless return codes)
//--- (end of generated code: YWireless return codes)
    //--- (generated code: YWireless class start)
/**
 * <summary>
 *   YWireless Class: Wireless function interface
 * <para>
 *   YWireless functions provides control over wireless network parameters
 *   and status for devices that are wireless-enabled.
 * </para>
 * </summary>
 */
public class YWireless : YFunction
{
//--- (end of generated code: YWireless class start)
//--- (generated code: YWireless definitions)
    /**
     * <summary>
     *   invalid linkQuality value
     * </summary>
     */
    public const  int LINKQUALITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid ssid value
     * </summary>
     */
    public const  string SSID_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid channel value
     * </summary>
     */
    public const  int CHANNEL_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid security value
     * </summary>
     */
    public const int SECURITY_UNKNOWN = 0;
    public const int SECURITY_OPEN = 1;
    public const int SECURITY_WEP = 2;
    public const int SECURITY_WPA = 3;
    public const int SECURITY_WPA2 = 4;
    public const int SECURITY_INVALID = -1;
    /**
     * <summary>
     *   invalid message value
     * </summary>
     */
    public const  string MESSAGE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid wlanConfig value
     * </summary>
     */
    public const  string WLANCONFIG_INVALID = YAPI.INVALID_STRING;
    protected int _linkQuality = LINKQUALITY_INVALID;
    protected string _ssid = SSID_INVALID;
    protected int _channel = CHANNEL_INVALID;
    protected int _security = SECURITY_INVALID;
    protected string _message = MESSAGE_INVALID;
    protected string _wlanConfig = WLANCONFIG_INVALID;
    protected ValueCallback _valueCallbackWireless = null;

    public new delegate Task ValueCallback(YWireless func, string value);
    public new delegate Task TimedReportCallback(YWireless func, YMeasure measure);
    //--- (end of generated code: YWireless definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YWireless(YAPIContext ctx, String func)
            : base(ctx, func, "Wireless")
        {
            //--- (generated code: YWireless attributes initialization)
        //--- (end of generated code: YWireless attributes initialization)
        }

        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YWireless(String func)
            : this(YAPI.imm_GetYCtx(), func)
        {}

        //--- (generated code: YWireless implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("linkQuality")) {
            _linkQuality = json_val.GetInt("linkQuality");
        }
        if (json_val.Has("ssid")) {
            _ssid = json_val.GetString("ssid");
        }
        if (json_val.Has("channel")) {
            _channel = json_val.GetInt("channel");
        }
        if (json_val.Has("security")) {
            _security = json_val.GetInt("security");
        }
        if (json_val.Has("message")) {
            _message = json_val.GetString("message");
        }
        if (json_val.Has("wlanConfig")) {
            _wlanConfig = json_val.GetString("wlanConfig");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the link quality, expressed in percent.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the link quality, expressed in percent
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWireless.LINKQUALITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_linkQuality()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LINKQUALITY_INVALID;
            }
        }
        return _linkQuality;
    }


    /**
     * <summary>
     *   Returns the wireless network name (SSID).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the wireless network name (SSID)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWireless.SSID_INVALID</c>.
     * </para>
     */
    public async Task<string> get_ssid()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SSID_INVALID;
            }
        }
        return _ssid;
    }


    /**
     * <summary>
     *   Returns the 802.11 channel currently used, or 0 when the selected network has not been found.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the 802.11 channel currently used, or 0 when the selected network has not been found
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWireless.CHANNEL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_channel()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CHANNEL_INVALID;
            }
        }
        return _channel;
    }


    /**
     * <summary>
     *   Returns the security algorithm used by the selected wireless network.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YWireless.SECURITY_UNKNOWN</c>, <c>YWireless.SECURITY_OPEN</c>,
     *   <c>YWireless.SECURITY_WEP</c>, <c>YWireless.SECURITY_WPA</c> and <c>YWireless.SECURITY_WPA2</c>
     *   corresponding to the security algorithm used by the selected wireless network
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWireless.SECURITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_security()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SECURITY_INVALID;
            }
        }
        return _security;
    }


    /**
     * <summary>
     *   Returns the latest status message from the wireless interface.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest status message from the wireless interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWireless.MESSAGE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_message()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MESSAGE_INVALID;
            }
        }
        return _message;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_wlanConfig()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return WLANCONFIG_INVALID;
            }
        }
        return _wlanConfig;
    }


    public async Task<int> set_wlanConfig(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("wlanConfig",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a wireless lan interface for a given identifier.
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
     *   This function does not require that the wireless lan interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWireless.isOnline()</c> to test if the wireless lan interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a wireless lan interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the wireless lan interface
     * </param>
     * <returns>
     *   a <c>YWireless</c> object allowing you to drive the wireless lan interface.
     * </returns>
     */
    public static YWireless FindWireless(string func)
    {
        YWireless obj;
        obj = (YWireless) YFunction._FindFromCache("Wireless", func);
        if (obj == null) {
            obj = new YWireless(func);
            YFunction._AddToCache("Wireless",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a wireless lan interface for a given identifier in a YAPI context.
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
     *   This function does not require that the wireless lan interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWireless.isOnline()</c> to test if the wireless lan interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a wireless lan interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the wireless lan interface
     * </param>
     * <returns>
     *   a <c>YWireless</c> object allowing you to drive the wireless lan interface.
     * </returns>
     */
    public static YWireless FindWirelessInContext(YAPIContext yctx,string func)
    {
        YWireless obj;
        obj = (YWireless) YFunction._FindFromCacheInContext(yctx,  "Wireless", func);
        if (obj == null) {
            obj = new YWireless(yctx, func);
            YFunction._AddToCache("Wireless",  func, obj);
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
        _valueCallbackWireless = callback;
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
        if (_valueCallbackWireless != null) {
            await _valueCallbackWireless(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Changes the configuration of the wireless lan interface to connect to an existing
     *   access point (infrastructure mode).
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * </summary>
     * <param name="ssid">
     *   the name of the network to connect to
     * </param>
     * <param name="securityKey">
     *   the network key, as a character string
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> joinNetwork(string ssid,string securityKey)
    {
        return await this.set_wlanConfig("INFRA:"+ ssid+"\\"+securityKey);
    }

    /**
     * <summary>
     *   Changes the configuration of the wireless lan interface to create an ad-hoc
     *   wireless network, without using an access point.
     * <para>
     *   On the YoctoHub-Wireless-g,
     *   it is best to use softAPNetworkInstead(), which emulates an access point
     *   (Soft AP) which is more efficient and more widely supported than ad-hoc networks.
     * </para>
     * <para>
     *   When a security key is specified for an ad-hoc network, the network is protected
     *   by a WEP40 key (5 characters or 10 hexadecimal digits) or WEP128 key (13 characters
     *   or 26 hexadecimal digits). It is recommended to use a well-randomized WEP128 key
     *   using 26 hexadecimal digits to maximize security.
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module
     *   to apply this setting.
     * </para>
     * </summary>
     * <param name="ssid">
     *   the name of the network to connect to
     * </param>
     * <param name="securityKey">
     *   the network key, as a character string
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> adhocNetwork(string ssid,string securityKey)
    {
        return await this.set_wlanConfig("ADHOC:"+ ssid+"\\"+securityKey);
    }

    /**
     * <summary>
     *   Changes the configuration of the wireless lan interface to create a new wireless
     *   network by emulating a WiFi access point (Soft AP).
     * <para>
     *   This function can only be
     *   used with the YoctoHub-Wireless-g.
     * </para>
     * <para>
     *   When a security key is specified for a SoftAP network, the network is protected
     *   by a WEP40 key (5 characters or 10 hexadecimal digits) or WEP128 key (13 characters
     *   or 26 hexadecimal digits). It is recommended to use a well-randomized WEP128 key
     *   using 26 hexadecimal digits to maximize security.
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * </summary>
     * <param name="ssid">
     *   the name of the network to connect to
     * </param>
     * <param name="securityKey">
     *   the network key, as a character string
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> softAPNetwork(string ssid,string securityKey)
    {
        return await this.set_wlanConfig("SOFTAP:"+ ssid+"\\"+securityKey);
    }

    /**
     * <summary>
     *   Returns a list of YWlanRecord objects that describe detected Wireless networks.
     * <para>
     *   This list is not updated when the module is already connected to an acces point (infrastructure mode).
     *   To force an update of this list, <c>adhocNetwork()</c> must be called to disconnect
     *   the module from the current network. The returned list must be unallocated by the caller.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of <c>YWlanRecord</c> objects, containing the SSID, channel,
     *   link quality and the type of security of the wireless network.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list.
     * </para>
     */
    public virtual async Task<List<YWlanRecord>> get_detectedWlans()
    {
        byte[] json;
        List<string> wlanlist = new List<string>();
        List<YWlanRecord> res = new List<YWlanRecord>();
        // may throw an exception
        json = await this._download("wlan.json?by=name");
        wlanlist = this.imm_json_get_array(json);
        res.Clear();
        for (int ii = 0; ii < wlanlist.Count; ii++) {
            res.Add(new YWlanRecord(wlanlist[ii]));
        }
        return res;
    }

    /**
     * <summary>
     *   Continues the enumeration of wireless lan interfaces started using <c>yFirstWireless()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWireless</c> object, corresponding to
     *   a wireless lan interface currently online, or a <c>null</c> pointer
     *   if there are no more wireless lan interfaces to enumerate.
     * </returns>
     */
    public YWireless nextWireless()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindWirelessInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of wireless lan interfaces currently accessible.
     * <para>
     *   Use the method <c>YWireless.nextWireless()</c> to iterate on
     *   next wireless lan interfaces.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWireless</c> object, corresponding to
     *   the first wireless lan interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWireless FirstWireless()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Wireless");
        if (next_hwid == null)  return null;
        return FindWirelessInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of wireless lan interfaces currently accessible.
     * <para>
     *   Use the method <c>YWireless.nextWireless()</c> to iterate on
     *   next wireless lan interfaces.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YWireless</c> object, corresponding to
     *   the first wireless lan interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWireless FirstWirelessInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Wireless");
        if (next_hwid == null)  return null;
        return FindWirelessInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YWireless implementation)
    }

}