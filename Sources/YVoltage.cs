/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindVoltage(), the high-level API for Voltage functions
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

//--- (YVoltage return codes)
//--- (end of YVoltage return codes)
//--- (YVoltage class start)
/**
 * <summary>
 *   YVoltage Class: Voltage function interface
 * <para>
 *   The Yoctopuce class YVoltage allows you to read and configure Yoctopuce voltage
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   register callback functions, access to the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YVoltage : YSensor
{
//--- (end of YVoltage class start)
//--- (YVoltage definitions)
    protected ValueCallback _valueCallbackVoltage = null;
    protected TimedReportCallback _timedReportCallbackVoltage = null;

    public new delegate Task ValueCallback(YVoltage func, string value);
    public new delegate Task TimedReportCallback(YVoltage func, YMeasure measure);
    //--- (end of YVoltage definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVoltage(YAPIContext ctx, string func)
        : base(ctx, func, "Voltage")
    {
        //--- (YVoltage attributes initialization)
        //--- (end of YVoltage attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVoltage(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YVoltage implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves a voltage sensor for a given identifier.
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
     *   This function does not require that the voltage sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVoltage.isOnline()</c> to test if the voltage sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the voltage sensor
     * </param>
     * <returns>
     *   a <c>YVoltage</c> object allowing you to drive the voltage sensor.
     * </returns>
     */
    public static YVoltage FindVoltage(string func)
    {
        YVoltage obj;
        obj = (YVoltage) YFunction._FindFromCache("Voltage", func);
        if (obj == null) {
            obj = new YVoltage(func);
            YFunction._AddToCache("Voltage",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a voltage sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the voltage sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVoltage.isOnline()</c> to test if the voltage sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the voltage sensor
     * </param>
     * <returns>
     *   a <c>YVoltage</c> object allowing you to drive the voltage sensor.
     * </returns>
     */
    public static YVoltage FindVoltageInContext(YAPIContext yctx,string func)
    {
        YVoltage obj;
        obj = (YVoltage) YFunction._FindFromCacheInContext(yctx,  "Voltage", func);
        if (obj == null) {
            obj = new YVoltage(yctx, func);
            YFunction._AddToCache("Voltage",  func, obj);
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
        _valueCallbackVoltage = callback;
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
        if (_valueCallbackVoltage != null) {
            await _valueCallbackVoltage(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every periodic timed notification.
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
     *   arguments: the function object of which the value has changed, and an YMeasure object describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerTimedReportCallback(TimedReportCallback callback)
    {
        YSensor sensor;
        sensor = this;
        if (callback != null) {
            await YFunction._UpdateTimedReportCallbackList(sensor, true);
        } else {
            await YFunction._UpdateTimedReportCallbackList(sensor, false);
        }
        _timedReportCallbackVoltage = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackVoltage != null) {
            await _timedReportCallbackVoltage(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of voltage sensors started using <c>yFirstVoltage()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVoltage</c> object, corresponding to
     *   a voltage sensor currently online, or a <c>null</c> pointer
     *   if there are no more voltage sensors to enumerate.
     * </returns>
     */
    public YVoltage nextVoltage()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindVoltageInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of voltage sensors currently accessible.
     * <para>
     *   Use the method <c>YVoltage.nextVoltage()</c> to iterate on
     *   next voltage sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVoltage</c> object, corresponding to
     *   the first voltage sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVoltage FirstVoltage()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Voltage");
        if (next_hwid == null)  return null;
        return FindVoltageInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of voltage sensors currently accessible.
     * <para>
     *   Use the method <c>YVoltage.nextVoltage()</c> to iterate on
     *   next voltage sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YVoltage</c> object, corresponding to
     *   the first voltage sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVoltage FirstVoltageInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Voltage");
        if (next_hwid == null)  return null;
        return FindVoltageInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YVoltage implementation)
}
}

