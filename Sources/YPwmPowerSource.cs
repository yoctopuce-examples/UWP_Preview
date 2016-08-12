/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindPwmPowerSource(), the high-level API for PwmPowerSource functions
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

//--- (YPwmPowerSource return codes)
//--- (end of YPwmPowerSource return codes)
//--- (YPwmPowerSource class start)
/**
 * <summary>
 *   YPwmPowerSource Class: PwmPowerSource function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to configure
 *   the voltage source used by all PWM on the same device.
 * </para>
 * </summary>
 */
public class YPwmPowerSource : YFunction
{
//--- (end of YPwmPowerSource class start)
//--- (YPwmPowerSource definitions)
    /**
     * <summary>
     *   invalid powerMode value
     * </summary>
     */
    public const int POWERMODE_USB_5V = 0;
    public const int POWERMODE_USB_3V = 1;
    public const int POWERMODE_EXT_V = 2;
    public const int POWERMODE_OPNDRN = 3;
    public const int POWERMODE_INVALID = -1;
    protected int _powerMode = POWERMODE_INVALID;
    protected ValueCallback _valueCallbackPwmPowerSource = null;

    public new delegate Task ValueCallback(YPwmPowerSource func, string value);
    public new delegate Task TimedReportCallback(YPwmPowerSource func, YMeasure measure);
    //--- (end of YPwmPowerSource definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmPowerSource(YAPIContext ctx, string func)
        : base(ctx, func, "PwmPowerSource")
    {
        //--- (YPwmPowerSource attributes initialization)
        //--- (end of YPwmPowerSource attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmPowerSource(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPwmPowerSource implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("powerMode")) {
            _powerMode = json_val.GetInt("powerMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the selected power source for the PWM on the same device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YPwmPowerSource.POWERMODE_USB_5V</c>, <c>YPwmPowerSource.POWERMODE_USB_3V</c>,
     *   <c>YPwmPowerSource.POWERMODE_EXT_V</c> and <c>YPwmPowerSource.POWERMODE_OPNDRN</c> corresponding to
     *   the selected power source for the PWM on the same device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmPowerSource.POWERMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_powerMode()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWERMODE_INVALID;
            }
        }
        return _powerMode;
    }


    /**
     * <summary>
     *   Changes  the PWM power source.
     * <para>
     *   PWM can use isolated 5V from USB, isolated 3V from USB or
     *   voltage from an external power source. The PWM can also work in open drain  mode. In that
     *   mode, the PWM actively pulls the line down.
     *   Warning: this setting is common to all PWM on the same device. If you change that parameter,
     *   all PWM located on the same device are  affected.
     *   If you want the change to be kept after a device reboot, make sure  to call the matching
     *   module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YPwmPowerSource.POWERMODE_USB_5V</c>, <c>YPwmPowerSource.POWERMODE_USB_3V</c>,
     *   <c>YPwmPowerSource.POWERMODE_EXT_V</c> and <c>YPwmPowerSource.POWERMODE_OPNDRN</c> corresponding to
     *    the PWM power source
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
    public async Task<int> set_powerMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("powerMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a voltage source for a given identifier.
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
     *   This function does not require that the voltage source is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmPowerSource.isOnline()</c> to test if the voltage source is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage source by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the voltage source
     * </param>
     * <returns>
     *   a <c>YPwmPowerSource</c> object allowing you to drive the voltage source.
     * </returns>
     */
    public static YPwmPowerSource FindPwmPowerSource(string func)
    {
        YPwmPowerSource obj;
        obj = (YPwmPowerSource) YFunction._FindFromCache("PwmPowerSource", func);
        if (obj == null) {
            obj = new YPwmPowerSource(func);
            YFunction._AddToCache("PwmPowerSource",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a voltage source for a given identifier in a YAPI context.
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
     *   This function does not require that the voltage source is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmPowerSource.isOnline()</c> to test if the voltage source is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage source by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the voltage source
     * </param>
     * <returns>
     *   a <c>YPwmPowerSource</c> object allowing you to drive the voltage source.
     * </returns>
     */
    public static YPwmPowerSource FindPwmPowerSourceInContext(YAPIContext yctx,string func)
    {
        YPwmPowerSource obj;
        obj = (YPwmPowerSource) YFunction._FindFromCacheInContext(yctx,  "PwmPowerSource", func);
        if (obj == null) {
            obj = new YPwmPowerSource(yctx, func);
            YFunction._AddToCache("PwmPowerSource",  func, obj);
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
        _valueCallbackPwmPowerSource = callback;
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
        if (_valueCallbackPwmPowerSource != null) {
            await _valueCallbackPwmPowerSource(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of Voltage sources started using <c>yFirstPwmPowerSource()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmPowerSource</c> object, corresponding to
     *   a voltage source currently online, or a <c>null</c> pointer
     *   if there are no more Voltage sources to enumerate.
     * </returns>
     */
    public YPwmPowerSource nextPwmPowerSource()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPwmPowerSourceInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of Voltage sources currently accessible.
     * <para>
     *   Use the method <c>YPwmPowerSource.nextPwmPowerSource()</c> to iterate on
     *   next Voltage sources.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmPowerSource</c> object, corresponding to
     *   the first source currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmPowerSource FirstPwmPowerSource()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmPowerSource");
        if (next_hwid == null)  return null;
        return FindPwmPowerSourceInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of Voltage sources currently accessible.
     * <para>
     *   Use the method <c>YPwmPowerSource.nextPwmPowerSource()</c> to iterate on
     *   next Voltage sources.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPwmPowerSource</c> object, corresponding to
     *   the first source currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmPowerSource FirstPwmPowerSourceInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmPowerSource");
        if (next_hwid == null)  return null;
        return FindPwmPowerSourceInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPwmPowerSource implementation)
}
}

