/*********************************************************************
 *
 * $Id: YGenericSensor.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindGenericSensor(), the high-level API for GenericSensor functions
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

//--- (YGenericSensor return codes)
//--- (end of YGenericSensor return codes)
//--- (YGenericSensor class start)
/**
 * <summary>
 *   YGenericSensor Class: GenericSensor function interface
 * <para>
 *   The YGenericSensor class allows you to read and configure Yoctopuce signal
 *   transducers. It inherits from YSensor class the core functions to read measurements,
 *   to register callback functions, to access the autonomous datalogger.
 *   This class adds the ability to configure the automatic conversion between the
 *   measured signal and the corresponding engineering unit.
 * </para>
 * </summary>
 */
public class YGenericSensor : YSensor
{
//--- (end of YGenericSensor class start)
//--- (YGenericSensor definitions)
    /**
     * <summary>
     *   invalid signalValue value
     * </summary>
     */
    public const  double SIGNALVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid signalUnit value
     * </summary>
     */
    public const  string SIGNALUNIT_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid signalRange value
     * </summary>
     */
    public const  string SIGNALRANGE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid valueRange value
     * </summary>
     */
    public const  string VALUERANGE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid signalBias value
     * </summary>
     */
    public const  double SIGNALBIAS_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid signalSampling value
     * </summary>
     */
    public const int SIGNALSAMPLING_HIGH_RATE = 0;
    public const int SIGNALSAMPLING_HIGH_RATE_FILTERED = 1;
    public const int SIGNALSAMPLING_LOW_NOISE = 2;
    public const int SIGNALSAMPLING_LOW_NOISE_FILTERED = 3;
    public const int SIGNALSAMPLING_INVALID = -1;
    protected double _signalValue = SIGNALVALUE_INVALID;
    protected string _signalUnit = SIGNALUNIT_INVALID;
    protected string _signalRange = SIGNALRANGE_INVALID;
    protected string _valueRange = VALUERANGE_INVALID;
    protected double _signalBias = SIGNALBIAS_INVALID;
    protected int _signalSampling = SIGNALSAMPLING_INVALID;
    protected ValueCallback _valueCallbackGenericSensor = null;
    protected TimedReportCallback _timedReportCallbackGenericSensor = null;

    public new delegate Task ValueCallback(YGenericSensor func, string value);
    public new delegate Task TimedReportCallback(YGenericSensor func, YMeasure measure);
    //--- (end of YGenericSensor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YGenericSensor(YAPIContext ctx, string func)
        : base(ctx, func, "GenericSensor")
    {
        //--- (YGenericSensor attributes initialization)
        //--- (end of YGenericSensor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YGenericSensor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YGenericSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("signalValue")) {
            _signalValue = Math.Round(json_val.GetDouble("signalValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("signalUnit")) {
            _signalUnit = json_val.GetString("signalUnit");
        }
        if (json_val.Has("signalRange")) {
            _signalRange = json_val.GetString("signalRange");
        }
        if (json_val.Has("valueRange")) {
            _valueRange = json_val.GetString("valueRange");
        }
        if (json_val.Has("signalBias")) {
            _signalBias = Math.Round(json_val.GetDouble("signalBias") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("signalSampling")) {
            _signalSampling = json_val.GetInt("signalSampling");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured value.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured value
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
    public async Task<int> set_unit(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("unit",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current value of the electrical signal measured by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current value of the electrical signal measured by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_signalValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALVALUE_INVALID;
            }
        }
        res = Math.Round(_signalValue * 1000) / 1000;
        return res;
    }


    /**
     * <summary>
     *   Returns the measuring unit of the electrical signal used by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the measuring unit of the electrical signal used by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALUNIT_INVALID</c>.
     * </para>
     */
    public async Task<string> get_signalUnit()
    {
        string res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALUNIT_INVALID;
            }
        }
        res = _signalUnit;
        return res;
    }


    /**
     * <summary>
     *   Returns the electric signal range used by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the electric signal range used by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALRANGE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_signalRange()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALRANGE_INVALID;
            }
        }
        res = _signalRange;
        return res;
    }


    /**
     * <summary>
     *   Changes the electric signal range used by the sensor.
     * <para>
     *   Default value is "-999999.999...999999.999".
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the electric signal range used by the sensor
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
    public async Task<int> set_signalRange(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("signalRange",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the physical value range measured by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the physical value range measured by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.VALUERANGE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_valueRange()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VALUERANGE_INVALID;
            }
        }
        res = _valueRange;
        return res;
    }


    /**
     * <summary>
     *   Changes the physical value range measured by the sensor.
     * <para>
     *   As a side effect, the range modification may
     *   automatically modify the display resolution. Default value is "-999999.999...999999.999".
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the physical value range measured by the sensor
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
    public async Task<int> set_valueRange(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("valueRange",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the electric signal bias for zero shift adjustment.
     * <para>
     *   If your electric signal reads positif when it should be zero, setup
     *   a positive signalBias of the same value to fix the zero shift.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the electric signal bias for zero shift adjustment
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
    public async Task<int> set_signalBias(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("signalBias",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the electric signal bias for zero shift adjustment.
     * <para>
     *   A positive bias means that the signal is over-reporting the measure,
     *   while a negative bias means that the signal is underreporting the measure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the electric signal bias for zero shift adjustment
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALBIAS_INVALID</c>.
     * </para>
     */
    public async Task<double> get_signalBias()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALBIAS_INVALID;
            }
        }
        res = _signalBias;
        return res;
    }


    /**
     * <summary>
     *   Returns the electric signal sampling method to use.
     * <para>
     *   The <c>HIGH_RATE</c> method uses the highest sampling frequency, without any filtering.
     *   The <c>HIGH_RATE_FILTERED</c> method adds a windowed 7-sample median filter.
     *   The <c>LOW_NOISE</c> method uses a reduced acquisition frequency to reduce noise.
     *   The <c>LOW_NOISE_FILTERED</c> method combines a reduced frequency with the median filter
     *   to get measures as stable as possible when working on a noisy signal.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YGenericSensor.SIGNALSAMPLING_HIGH_RATE</c>,
     *   <c>YGenericSensor.SIGNALSAMPLING_HIGH_RATE_FILTERED</c>, <c>YGenericSensor.SIGNALSAMPLING_LOW_NOISE</c>
     *   and <c>YGenericSensor.SIGNALSAMPLING_LOW_NOISE_FILTERED</c> corresponding to the electric signal
     *   sampling method to use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALSAMPLING_INVALID</c>.
     * </para>
     */
    public async Task<int> get_signalSampling()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALSAMPLING_INVALID;
            }
        }
        res = _signalSampling;
        return res;
    }


    /**
     * <summary>
     *   Changes the electric signal sampling method to use.
     * <para>
     *   The <c>HIGH_RATE</c> method uses the highest sampling frequency, without any filtering.
     *   The <c>HIGH_RATE_FILTERED</c> method adds a windowed 7-sample median filter.
     *   The <c>LOW_NOISE</c> method uses a reduced acquisition frequency to reduce noise.
     *   The <c>LOW_NOISE_FILTERED</c> method combines a reduced frequency with the median filter
     *   to get measures as stable as possible when working on a noisy signal.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YGenericSensor.SIGNALSAMPLING_HIGH_RATE</c>,
     *   <c>YGenericSensor.SIGNALSAMPLING_HIGH_RATE_FILTERED</c>, <c>YGenericSensor.SIGNALSAMPLING_LOW_NOISE</c>
     *   and <c>YGenericSensor.SIGNALSAMPLING_LOW_NOISE_FILTERED</c> corresponding to the electric signal
     *   sampling method to use
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
    public async Task<int> set_signalSampling(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("signalSampling",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a generic sensor for a given identifier.
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
     *   This function does not require that the generic sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGenericSensor.isOnline()</c> to test if the generic sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a generic sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the generic sensor
     * </param>
     * <returns>
     *   a <c>YGenericSensor</c> object allowing you to drive the generic sensor.
     * </returns>
     */
    public static YGenericSensor FindGenericSensor(string func)
    {
        YGenericSensor obj;
        obj = (YGenericSensor) YFunction._FindFromCache("GenericSensor", func);
        if (obj == null) {
            obj = new YGenericSensor(func);
            YFunction._AddToCache("GenericSensor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a generic sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the generic sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGenericSensor.isOnline()</c> to test if the generic sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a generic sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the generic sensor
     * </param>
     * <returns>
     *   a <c>YGenericSensor</c> object allowing you to drive the generic sensor.
     * </returns>
     */
    public static YGenericSensor FindGenericSensorInContext(YAPIContext yctx,string func)
    {
        YGenericSensor obj;
        obj = (YGenericSensor) YFunction._FindFromCacheInContext(yctx,  "GenericSensor", func);
        if (obj == null) {
            obj = new YGenericSensor(yctx, func);
            YFunction._AddToCache("GenericSensor",  func, obj);
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
        _valueCallbackGenericSensor = callback;
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
        if (_valueCallbackGenericSensor != null) {
            await _valueCallbackGenericSensor(this, value);
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
        _timedReportCallbackGenericSensor = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackGenericSensor != null) {
            await _timedReportCallbackGenericSensor(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Adjusts the signal bias so that the current signal value is need
     *   precisely as zero.
     * <para>
     * </para>
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
    public virtual async Task<int> zeroAdjust()
    {
        double currSignal;
        double currBias;
        currSignal = await this.get_signalValue();
        currBias = await this.get_signalBias();
        return await this.set_signalBias(currSignal + currBias);
    }

    /**
     * <summary>
     *   Continues the enumeration of generic sensors started using <c>yFirstGenericSensor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGenericSensor</c> object, corresponding to
     *   a generic sensor currently online, or a <c>null</c> pointer
     *   if there are no more generic sensors to enumerate.
     * </returns>
     */
    public YGenericSensor nextGenericSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindGenericSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of generic sensors currently accessible.
     * <para>
     *   Use the method <c>YGenericSensor.nextGenericSensor()</c> to iterate on
     *   next generic sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGenericSensor</c> object, corresponding to
     *   the first generic sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGenericSensor FirstGenericSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("GenericSensor");
        if (next_hwid == null)  return null;
        return FindGenericSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of generic sensors currently accessible.
     * <para>
     *   Use the method <c>YGenericSensor.nextGenericSensor()</c> to iterate on
     *   next generic sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YGenericSensor</c> object, corresponding to
     *   the first generic sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGenericSensor FirstGenericSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("GenericSensor");
        if (next_hwid == null)  return null;
        return FindGenericSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YGenericSensor implementation)
}
}

