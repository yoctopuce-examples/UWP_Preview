/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindPowerOutput(), the high-level API for PowerOutput functions
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

//--- (YPowerOutput return codes)
//--- (end of YPowerOutput return codes)
//--- (YPowerOutput class start)
/**
 * <summary>
 *   YPowerOutput Class: External power supply control interface
 * <para>
 *   Yoctopuce application programming interface allows you to control
 *   the power ouput featured on some devices such as the Yocto-Serial.
 * </para>
 * </summary>
 */
public class YPowerOutput : YFunction
{
//--- (end of YPowerOutput class start)
//--- (YPowerOutput definitions)
    /**
     * <summary>
     *   invalid voltage value
     * </summary>
     */
    public const int VOLTAGE_OFF = 0;
    public const int VOLTAGE_OUT3V3 = 1;
    public const int VOLTAGE_OUT5V = 2;
    public const int VOLTAGE_INVALID = -1;
    protected int _voltage = VOLTAGE_INVALID;
    protected ValueCallback _valueCallbackPowerOutput = null;

    public new delegate Task ValueCallback(YPowerOutput func, string value);
    public new delegate Task TimedReportCallback(YPowerOutput func, YMeasure measure);
    //--- (end of YPowerOutput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPowerOutput(YAPIContext ctx, string func)
        : base(ctx, func, "PowerOutput")
    {
        //--- (YPowerOutput attributes initialization)
        //--- (end of YPowerOutput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPowerOutput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPowerOutput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("voltage")) {
            _voltage = json_val.GetInt("voltage");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the voltage on the power ouput featured by
     *   the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YPowerOutput.VOLTAGE_OFF</c>, <c>YPowerOutput.VOLTAGE_OUT3V3</c> and
     *   <c>YPowerOutput.VOLTAGE_OUT5V</c> corresponding to the voltage on the power ouput featured by
     *   the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerOutput.VOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_voltage()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGE_INVALID;
            }
        }
        return _voltage;
    }


    /**
     * <summary>
     *   Changes the voltage on the power output provided by the
     *   module.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YPowerOutput.VOLTAGE_OFF</c>, <c>YPowerOutput.VOLTAGE_OUT3V3</c> and
     *   <c>YPowerOutput.VOLTAGE_OUT5V</c> corresponding to the voltage on the power output provided by the
     *   module
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
    public async Task<int> set_voltage(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("voltage",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a dual power  ouput control for a given identifier.
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
     *   This function does not require that the power ouput control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPowerOutput.isOnline()</c> to test if the power ouput control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a dual power  ouput control by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the power ouput control
     * </param>
     * <returns>
     *   a <c>YPowerOutput</c> object allowing you to drive the power ouput control.
     * </returns>
     */
    public static YPowerOutput FindPowerOutput(string func)
    {
        YPowerOutput obj;
        obj = (YPowerOutput) YFunction._FindFromCache("PowerOutput", func);
        if (obj == null) {
            obj = new YPowerOutput(func);
            YFunction._AddToCache("PowerOutput",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a dual power  ouput control for a given identifier in a YAPI context.
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
     *   This function does not require that the power ouput control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPowerOutput.isOnline()</c> to test if the power ouput control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a dual power  ouput control by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the power ouput control
     * </param>
     * <returns>
     *   a <c>YPowerOutput</c> object allowing you to drive the power ouput control.
     * </returns>
     */
    public static YPowerOutput FindPowerOutputInContext(YAPIContext yctx,string func)
    {
        YPowerOutput obj;
        obj = (YPowerOutput) YFunction._FindFromCacheInContext(yctx,  "PowerOutput", func);
        if (obj == null) {
            obj = new YPowerOutput(yctx, func);
            YFunction._AddToCache("PowerOutput",  func, obj);
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
        _valueCallbackPowerOutput = callback;
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
        if (_valueCallbackPowerOutput != null) {
            await _valueCallbackPowerOutput(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of dual power ouput controls started using <c>yFirstPowerOutput()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPowerOutput</c> object, corresponding to
     *   a dual power  ouput control currently online, or a <c>null</c> pointer
     *   if there are no more dual power ouput controls to enumerate.
     * </returns>
     */
    public YPowerOutput nextPowerOutput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPowerOutputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of dual power ouput controls currently accessible.
     * <para>
     *   Use the method <c>YPowerOutput.nextPowerOutput()</c> to iterate on
     *   next dual power ouput controls.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPowerOutput</c> object, corresponding to
     *   the first dual power ouput control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPowerOutput FirstPowerOutput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PowerOutput");
        if (next_hwid == null)  return null;
        return FindPowerOutputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of dual power ouput controls currently accessible.
     * <para>
     *   Use the method <c>YPowerOutput.nextPowerOutput()</c> to iterate on
     *   next dual power ouput controls.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPowerOutput</c> object, corresponding to
     *   the first dual power ouput control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPowerOutput FirstPowerOutputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PowerOutput");
        if (next_hwid == null)  return null;
        return FindPowerOutputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPowerOutput implementation)
}
}

