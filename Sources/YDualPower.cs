/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindDualPower(), the high-level API for DualPower functions
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

//--- (YDualPower return codes)
//--- (end of YDualPower return codes)
//--- (YDualPower class start)
/**
 * <summary>
 *   YDualPower Class: External power supply control interface
 * <para>
 *   Yoctopuce application programming interface allows you to control
 *   the power source to use for module functions that require high current.
 *   The module can also automatically disconnect the external power
 *   when a voltage drop is observed on the external power source
 *   (external battery running out of power).
 * </para>
 * </summary>
 */
public class YDualPower : YFunction
{
//--- (end of YDualPower class start)
//--- (YDualPower definitions)
    /**
     * <summary>
     *   invalid powerState value
     * </summary>
     */
    public const int POWERSTATE_OFF = 0;
    public const int POWERSTATE_FROM_USB = 1;
    public const int POWERSTATE_FROM_EXT = 2;
    public const int POWERSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid powerControl value
     * </summary>
     */
    public const int POWERCONTROL_AUTO = 0;
    public const int POWERCONTROL_FROM_USB = 1;
    public const int POWERCONTROL_FROM_EXT = 2;
    public const int POWERCONTROL_OFF = 3;
    public const int POWERCONTROL_INVALID = -1;
    /**
     * <summary>
     *   invalid extVoltage value
     * </summary>
     */
    public const  int EXTVOLTAGE_INVALID = YAPI.INVALID_UINT;
    protected int _powerState = POWERSTATE_INVALID;
    protected int _powerControl = POWERCONTROL_INVALID;
    protected int _extVoltage = EXTVOLTAGE_INVALID;
    protected ValueCallback _valueCallbackDualPower = null;

    public new delegate Task ValueCallback(YDualPower func, string value);
    public new delegate Task TimedReportCallback(YDualPower func, YMeasure measure);
    //--- (end of YDualPower definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YDualPower(YAPIContext ctx, string func)
        : base(ctx, func, "DualPower")
    {
        //--- (YDualPower attributes initialization)
        //--- (end of YDualPower attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YDualPower(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YDualPower implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("powerState")) {
            _powerState = json_val.GetInt("powerState");
        }
        if (json_val.Has("powerControl")) {
            _powerControl = json_val.GetInt("powerControl");
        }
        if (json_val.Has("extVoltage")) {
            _extVoltage = json_val.GetInt("extVoltage");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current power source for module functions that require lots of current.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDualPower.POWERSTATE_OFF</c>, <c>YDualPower.POWERSTATE_FROM_USB</c> and
     *   <c>YDualPower.POWERSTATE_FROM_EXT</c> corresponding to the current power source for module
     *   functions that require lots of current
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDualPower.POWERSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_powerState()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWERSTATE_INVALID;
            }
        }
        return _powerState;
    }


    /**
     * <summary>
     *   Returns the selected power source for module functions that require lots of current.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDualPower.POWERCONTROL_AUTO</c>, <c>YDualPower.POWERCONTROL_FROM_USB</c>,
     *   <c>YDualPower.POWERCONTROL_FROM_EXT</c> and <c>YDualPower.POWERCONTROL_OFF</c> corresponding to the
     *   selected power source for module functions that require lots of current
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDualPower.POWERCONTROL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_powerControl()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWERCONTROL_INVALID;
            }
        }
        return _powerControl;
    }


    /**
     * <summary>
     *   Changes the selected power source for module functions that require lots of current.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YDualPower.POWERCONTROL_AUTO</c>, <c>YDualPower.POWERCONTROL_FROM_USB</c>,
     *   <c>YDualPower.POWERCONTROL_FROM_EXT</c> and <c>YDualPower.POWERCONTROL_OFF</c> corresponding to the
     *   selected power source for module functions that require lots of current
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
    public async Task<int> set_powerControl(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("powerControl",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the measured voltage on the external power source, in millivolts.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the measured voltage on the external power source, in millivolts
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDualPower.EXTVOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_extVoltage()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return EXTVOLTAGE_INVALID;
            }
        }
        return _extVoltage;
    }


    /**
     * <summary>
     *   Retrieves a dual power control for a given identifier.
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
     *   This function does not require that the power control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDualPower.isOnline()</c> to test if the power control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a dual power control by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the power control
     * </param>
     * <returns>
     *   a <c>YDualPower</c> object allowing you to drive the power control.
     * </returns>
     */
    public static YDualPower FindDualPower(string func)
    {
        YDualPower obj;
        obj = (YDualPower) YFunction._FindFromCache("DualPower", func);
        if (obj == null) {
            obj = new YDualPower(func);
            YFunction._AddToCache("DualPower",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a dual power control for a given identifier in a YAPI context.
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
     *   This function does not require that the power control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDualPower.isOnline()</c> to test if the power control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a dual power control by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the power control
     * </param>
     * <returns>
     *   a <c>YDualPower</c> object allowing you to drive the power control.
     * </returns>
     */
    public static YDualPower FindDualPowerInContext(YAPIContext yctx,string func)
    {
        YDualPower obj;
        obj = (YDualPower) YFunction._FindFromCacheInContext(yctx,  "DualPower", func);
        if (obj == null) {
            obj = new YDualPower(yctx, func);
            YFunction._AddToCache("DualPower",  func, obj);
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
        _valueCallbackDualPower = callback;
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
        if (_valueCallbackDualPower != null) {
            await _valueCallbackDualPower(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of dual power controls started using <c>yFirstDualPower()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDualPower</c> object, corresponding to
     *   a dual power control currently online, or a <c>null</c> pointer
     *   if there are no more dual power controls to enumerate.
     * </returns>
     */
    public YDualPower nextDualPower()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindDualPowerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of dual power controls currently accessible.
     * <para>
     *   Use the method <c>YDualPower.nextDualPower()</c> to iterate on
     *   next dual power controls.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDualPower</c> object, corresponding to
     *   the first dual power control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDualPower FirstDualPower()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DualPower");
        if (next_hwid == null)  return null;
        return FindDualPowerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of dual power controls currently accessible.
     * <para>
     *   Use the method <c>YDualPower.nextDualPower()</c> to iterate on
     *   next dual power controls.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YDualPower</c> object, corresponding to
     *   the first dual power control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDualPower FirstDualPowerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DualPower");
        if (next_hwid == null)  return null;
        return FindDualPowerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YDualPower implementation)
}
}

