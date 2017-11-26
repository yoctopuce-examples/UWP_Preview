/*********************************************************************
 *
 * $Id: YPower.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindPower(), the high-level API for Power functions
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

//--- (YPower return codes)
//--- (end of YPower return codes)
//--- (YPower class start)
/**
 * <summary>
 *   YPower Class: Power function interface
 * <para>
 *   The Yoctopuce class YPower allows you to read and configure Yoctopuce power
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   to register callback functions, to access the autonomous datalogger.
 *   This class adds the ability to access the energy counter and the power factor.
 * </para>
 * </summary>
 */
public class YPower : YSensor
{
//--- (end of YPower class start)
//--- (YPower definitions)
    /**
     * <summary>
     *   invalid cosPhi value
     * </summary>
     */
    public const  double COSPHI_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid meter value
     * </summary>
     */
    public const  double METER_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid meterTimer value
     * </summary>
     */
    public const  int METERTIMER_INVALID = YAPI.INVALID_UINT;
    protected double _cosPhi = COSPHI_INVALID;
    protected double _meter = METER_INVALID;
    protected int _meterTimer = METERTIMER_INVALID;
    protected ValueCallback _valueCallbackPower = null;
    protected TimedReportCallback _timedReportCallbackPower = null;

    public new delegate Task ValueCallback(YPower func, string value);
    public new delegate Task TimedReportCallback(YPower func, YMeasure measure);
    //--- (end of YPower definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPower(YAPIContext ctx, string func)
        : base(ctx, func, "Power")
    {
        //--- (YPower attributes initialization)
        //--- (end of YPower attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPower(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPower implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("cosPhi")) {
            _cosPhi = Math.Round(json_val.getDouble("cosPhi") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("meter")) {
            _meter = Math.Round(json_val.getDouble("meter") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("meterTimer")) {
            _meterTimer = json_val.getInt("meterTimer");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the power factor (the ratio between the real power consumed,
     *   measured in W, and the apparent power provided, measured in VA).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the power factor (the ratio between the real power consumed,
     *   measured in W, and the apparent power provided, measured in VA)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.COSPHI_INVALID</c>.
     * </para>
     */
    public async Task<double> get_cosPhi()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COSPHI_INVALID;
            }
        }
        res = _cosPhi;
        return res;
    }


    public async Task<int> set_meter(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("meter",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the energy counter, maintained by the wattmeter by integrating the power consumption over time.
     * <para>
     *   Note that this counter is reset at each start of the device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the energy counter, maintained by the wattmeter by
     *   integrating the power consumption over time
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.METER_INVALID</c>.
     * </para>
     */
    public async Task<double> get_meter()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return METER_INVALID;
            }
        }
        res = _meter;
        return res;
    }


    /**
     * <summary>
     *   Returns the elapsed time since last energy counter reset, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the elapsed time since last energy counter reset, in seconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.METERTIMER_INVALID</c>.
     * </para>
     */
    public async Task<int> get_meterTimer()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return METERTIMER_INVALID;
            }
        }
        res = _meterTimer;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a electrical power sensor for a given identifier.
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
     *   This function does not require that the electrical power sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPower.isOnline()</c> to test if the electrical power sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a electrical power sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the electrical power sensor
     * </param>
     * <returns>
     *   a <c>YPower</c> object allowing you to drive the electrical power sensor.
     * </returns>
     */
    public static YPower FindPower(string func)
    {
        YPower obj;
        obj = (YPower) YFunction._FindFromCache("Power", func);
        if (obj == null) {
            obj = new YPower(func);
            YFunction._AddToCache("Power",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a electrical power sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the electrical power sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPower.isOnline()</c> to test if the electrical power sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a electrical power sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the electrical power sensor
     * </param>
     * <returns>
     *   a <c>YPower</c> object allowing you to drive the electrical power sensor.
     * </returns>
     */
    public static YPower FindPowerInContext(YAPIContext yctx,string func)
    {
        YPower obj;
        obj = (YPower) YFunction._FindFromCacheInContext(yctx,  "Power", func);
        if (obj == null) {
            obj = new YPower(yctx, func);
            YFunction._AddToCache("Power",  func, obj);
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
        _valueCallbackPower = callback;
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
        if (_valueCallbackPower != null) {
            await _valueCallbackPower(this, value);
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
        _timedReportCallbackPower = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackPower != null) {
            await _timedReportCallbackPower(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Resets the energy counter.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> reset()
    {
        return await this.set_meter(0);
    }

    /**
     * <summary>
     *   Continues the enumeration of electrical power sensors started using <c>yFirstPower()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPower</c> object, corresponding to
     *   a electrical power sensor currently online, or a <c>null</c> pointer
     *   if there are no more electrical power sensors to enumerate.
     * </returns>
     */
    public YPower nextPower()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPowerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of electrical power sensors currently accessible.
     * <para>
     *   Use the method <c>YPower.nextPower()</c> to iterate on
     *   next electrical power sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPower</c> object, corresponding to
     *   the first electrical power sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPower FirstPower()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Power");
        if (next_hwid == null)  return null;
        return FindPowerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of electrical power sensors currently accessible.
     * <para>
     *   Use the method <c>YPower.nextPower()</c> to iterate on
     *   next electrical power sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPower</c> object, corresponding to
     *   the first electrical power sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPower FirstPowerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Power");
        if (next_hwid == null)  return null;
        return FindPowerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPower implementation)
}
}

