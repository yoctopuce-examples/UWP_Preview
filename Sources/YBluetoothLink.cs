/*********************************************************************
 *
 * $Id: YBluetoothLink.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindBluetoothLink(), the high-level API for BluetoothLink functions
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

//--- (YBluetoothLink return codes)
//--- (end of YBluetoothLink return codes)
//--- (YBluetoothLink class start)
/**
 * <summary>
 *   YBluetoothLink Class: BluetoothLink function interface
 * <para>
 *   BluetoothLink function provides control over bluetooth link
 *   and status for devices that are bluetooth-enabled.
 * </para>
 * </summary>
 */
public class YBluetoothLink : YFunction
{
//--- (end of YBluetoothLink class start)
//--- (YBluetoothLink definitions)
    /**
     * <summary>
     *   invalid ownAddress value
     * </summary>
     */
    public const  string OWNADDRESS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid pairingPin value
     * </summary>
     */
    public const  string PAIRINGPIN_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid remoteAddress value
     * </summary>
     */
    public const  string REMOTEADDRESS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid remoteName value
     * </summary>
     */
    public const  string REMOTENAME_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid mute value
     * </summary>
     */
    public const int MUTE_FALSE = 0;
    public const int MUTE_TRUE = 1;
    public const int MUTE_INVALID = -1;
    /**
     * <summary>
     *   invalid preAmplifier value
     * </summary>
     */
    public const  int PREAMPLIFIER_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid volume value
     * </summary>
     */
    public const  int VOLUME_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid linkState value
     * </summary>
     */
    public const int LINKSTATE_DOWN = 0;
    public const int LINKSTATE_FREE = 1;
    public const int LINKSTATE_SEARCH = 2;
    public const int LINKSTATE_EXISTS = 3;
    public const int LINKSTATE_LINKED = 4;
    public const int LINKSTATE_PLAY = 5;
    public const int LINKSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid linkQuality value
     * </summary>
     */
    public const  int LINKQUALITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected string _ownAddress = OWNADDRESS_INVALID;
    protected string _pairingPin = PAIRINGPIN_INVALID;
    protected string _remoteAddress = REMOTEADDRESS_INVALID;
    protected string _remoteName = REMOTENAME_INVALID;
    protected int _mute = MUTE_INVALID;
    protected int _preAmplifier = PREAMPLIFIER_INVALID;
    protected int _volume = VOLUME_INVALID;
    protected int _linkState = LINKSTATE_INVALID;
    protected int _linkQuality = LINKQUALITY_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackBluetoothLink = null;

    public new delegate Task ValueCallback(YBluetoothLink func, string value);
    public new delegate Task TimedReportCallback(YBluetoothLink func, YMeasure measure);
    //--- (end of YBluetoothLink definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YBluetoothLink(YAPIContext ctx, string func)
        : base(ctx, func, "BluetoothLink")
    {
        //--- (YBluetoothLink attributes initialization)
        //--- (end of YBluetoothLink attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YBluetoothLink(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YBluetoothLink implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("ownAddress")) {
            _ownAddress = json_val.GetString("ownAddress");
        }
        if (json_val.Has("pairingPin")) {
            _pairingPin = json_val.GetString("pairingPin");
        }
        if (json_val.Has("remoteAddress")) {
            _remoteAddress = json_val.GetString("remoteAddress");
        }
        if (json_val.Has("remoteName")) {
            _remoteName = json_val.GetString("remoteName");
        }
        if (json_val.Has("mute")) {
            _mute = json_val.GetInt("mute") > 0 ? 1 : 0;
        }
        if (json_val.Has("preAmplifier")) {
            _preAmplifier = json_val.GetInt("preAmplifier");
        }
        if (json_val.Has("volume")) {
            _volume = json_val.GetInt("volume");
        }
        if (json_val.Has("linkState")) {
            _linkState = json_val.GetInt("linkState");
        }
        if (json_val.Has("linkQuality")) {
            _linkQuality = json_val.GetInt("linkQuality");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the MAC-48 address of the bluetooth interface, which is unique on the bluetooth network.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the MAC-48 address of the bluetooth interface, which is unique on the
     *   bluetooth network
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.OWNADDRESS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_ownAddress()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return OWNADDRESS_INVALID;
            }
        }
        return _ownAddress;
    }


    /**
     * <summary>
     *   Returns an opaque string if a PIN code has been configured in the device to access
     *   the SIM card, or an empty string if none has been configured or if the code provided
     *   was rejected by the SIM card.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to an opaque string if a PIN code has been configured in the device to access
     *   the SIM card, or an empty string if none has been configured or if the code provided
     *   was rejected by the SIM card
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.PAIRINGPIN_INVALID</c>.
     * </para>
     */
    public async Task<string> get_pairingPin()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PAIRINGPIN_INVALID;
            }
        }
        return _pairingPin;
    }


    /**
     * <summary>
     *   Changes the PIN code used by the module for bluetooth pairing.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module to save the
     *   new value in the device flash.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the PIN code used by the module for bluetooth pairing
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
    public async Task<int> set_pairingPin(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("pairingPin",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the MAC-48 address of the remote device to connect to.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the MAC-48 address of the remote device to connect to
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.REMOTEADDRESS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_remoteAddress()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REMOTEADDRESS_INVALID;
            }
        }
        return _remoteAddress;
    }


    /**
     * <summary>
     *   Changes the MAC-48 address defining which remote device to connect to.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the MAC-48 address defining which remote device to connect to
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
    public async Task<int> set_remoteAddress(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("remoteAddress",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the bluetooth name the remote device, if found on the bluetooth network.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the bluetooth name the remote device, if found on the bluetooth network
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.REMOTENAME_INVALID</c>.
     * </para>
     */
    public async Task<string> get_remoteName()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REMOTENAME_INVALID;
            }
        }
        return _remoteName;
    }


    /**
     * <summary>
     *   Returns the state of the mute function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YBluetoothLink.MUTE_FALSE</c> or <c>YBluetoothLink.MUTE_TRUE</c>, according to the state
     *   of the mute function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.MUTE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_mute()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MUTE_INVALID;
            }
        }
        return _mute;
    }


    /**
     * <summary>
     *   Changes the state of the mute function.
     * <para>
     *   Remember to call the matching module
     *   <c>saveToFlash()</c> method to save the setting permanently.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YBluetoothLink.MUTE_FALSE</c> or <c>YBluetoothLink.MUTE_TRUE</c>, according to the state
     *   of the mute function
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
    public async Task<int> set_mute(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("mute",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the audio pre-amplifier volume, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the audio pre-amplifier volume, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.PREAMPLIFIER_INVALID</c>.
     * </para>
     */
    public async Task<int> get_preAmplifier()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PREAMPLIFIER_INVALID;
            }
        }
        return _preAmplifier;
    }


    /**
     * <summary>
     *   Changes the audio pre-amplifier volume, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the audio pre-amplifier volume, in per cents
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
    public async Task<int> set_preAmplifier(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("preAmplifier",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the connected headset volume, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the connected headset volume, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.VOLUME_INVALID</c>.
     * </para>
     */
    public async Task<int> get_volume()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLUME_INVALID;
            }
        }
        return _volume;
    }


    /**
     * <summary>
     *   Changes the connected headset volume, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the connected headset volume, in per cents
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
    public async Task<int> set_volume(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("volume",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the bluetooth link state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YBluetoothLink.LINKSTATE_DOWN</c>, <c>YBluetoothLink.LINKSTATE_FREE</c>,
     *   <c>YBluetoothLink.LINKSTATE_SEARCH</c>, <c>YBluetoothLink.LINKSTATE_EXISTS</c>,
     *   <c>YBluetoothLink.LINKSTATE_LINKED</c> and <c>YBluetoothLink.LINKSTATE_PLAY</c> corresponding to
     *   the bluetooth link state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.LINKSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_linkState()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LINKSTATE_INVALID;
            }
        }
        return _linkState;
    }


    /**
     * <summary>
     *   Returns the bluetooth receiver signal strength, in pourcents, or 0 if no connection is established.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the bluetooth receiver signal strength, in pourcents, or 0 if no
     *   connection is established
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.LINKQUALITY_INVALID</c>.
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
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        return _command;
    }


    public async Task<int> set_command(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("command",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a cellular interface for a given identifier.
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
     *   This function does not require that the cellular interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBluetoothLink.isOnline()</c> to test if the cellular interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a cellular interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the cellular interface
     * </param>
     * <returns>
     *   a <c>YBluetoothLink</c> object allowing you to drive the cellular interface.
     * </returns>
     */
    public static YBluetoothLink FindBluetoothLink(string func)
    {
        YBluetoothLink obj;
        obj = (YBluetoothLink) YFunction._FindFromCache("BluetoothLink", func);
        if (obj == null) {
            obj = new YBluetoothLink(func);
            YFunction._AddToCache("BluetoothLink",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a cellular interface for a given identifier in a YAPI context.
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
     *   This function does not require that the cellular interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBluetoothLink.isOnline()</c> to test if the cellular interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a cellular interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the cellular interface
     * </param>
     * <returns>
     *   a <c>YBluetoothLink</c> object allowing you to drive the cellular interface.
     * </returns>
     */
    public static YBluetoothLink FindBluetoothLinkInContext(YAPIContext yctx,string func)
    {
        YBluetoothLink obj;
        obj = (YBluetoothLink) YFunction._FindFromCacheInContext(yctx,  "BluetoothLink", func);
        if (obj == null) {
            obj = new YBluetoothLink(yctx, func);
            YFunction._AddToCache("BluetoothLink",  func, obj);
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
        _valueCallbackBluetoothLink = callback;
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
        if (_valueCallbackBluetoothLink != null) {
            await _valueCallbackBluetoothLink(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Attempt to connect to the previously selected remote device.
     * <para>
     * </para>
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
    public virtual async Task<int> connect()
    {
        return await this.set_command("C");
    }

    /**
     * <summary>
     *   Disconnect from the previously selected remote device.
     * <para>
     * </para>
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
    public virtual async Task<int> disconnect()
    {
        return await this.set_command("D");
    }

    /**
     * <summary>
     *   Continues the enumeration of cellular interfaces started using <c>yFirstBluetoothLink()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBluetoothLink</c> object, corresponding to
     *   a cellular interface currently online, or a <c>null</c> pointer
     *   if there are no more cellular interfaces to enumerate.
     * </returns>
     */
    public YBluetoothLink nextBluetoothLink()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindBluetoothLinkInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of cellular interfaces currently accessible.
     * <para>
     *   Use the method <c>YBluetoothLink.nextBluetoothLink()</c> to iterate on
     *   next cellular interfaces.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBluetoothLink</c> object, corresponding to
     *   the first cellular interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBluetoothLink FirstBluetoothLink()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("BluetoothLink");
        if (next_hwid == null)  return null;
        return FindBluetoothLinkInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of cellular interfaces currently accessible.
     * <para>
     *   Use the method <c>YBluetoothLink.nextBluetoothLink()</c> to iterate on
     *   next cellular interfaces.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YBluetoothLink</c> object, corresponding to
     *   the first cellular interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBluetoothLink FirstBluetoothLinkInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("BluetoothLink");
        if (next_hwid == null)  return null;
        return FindBluetoothLinkInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YBluetoothLink implementation)
}
}

