/*********************************************************************
 *
 * $Id: YLightSensor.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindLightSensor(), the high-level API for LightSensor functions
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

//--- (YLightSensor return codes)
//--- (end of YLightSensor return codes)
//--- (YLightSensor class start)
/**
 * <summary>
 *   YLightSensor Class: LightSensor function interface
 * <para>
 *   The Yoctopuce class YLightSensor allows you to read and configure Yoctopuce light
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   to register callback functions, to access the autonomous datalogger.
 *   This class adds the ability to easily perform a one-point linear calibration
 *   to compensate the effect of a glass or filter placed in front of the sensor.
 *   For some light sensors with several working modes, this class can select the
 *   desired working mode.
 * </para>
 * </summary>
 */
public class YLightSensor : YSensor
{
//--- (end of YLightSensor class start)
//--- (YLightSensor definitions)
    /**
     * <summary>
     *   invalid measureType value
     * </summary>
     */
    public const int MEASURETYPE_HUMAN_EYE = 0;
    public const int MEASURETYPE_WIDE_SPECTRUM = 1;
    public const int MEASURETYPE_INFRARED = 2;
    public const int MEASURETYPE_HIGH_RATE = 3;
    public const int MEASURETYPE_HIGH_ENERGY = 4;
    public const int MEASURETYPE_INVALID = -1;
    protected int _measureType = MEASURETYPE_INVALID;
    protected ValueCallback _valueCallbackLightSensor = null;
    protected TimedReportCallback _timedReportCallbackLightSensor = null;

    public new delegate Task ValueCallback(YLightSensor func, string value);
    public new delegate Task TimedReportCallback(YLightSensor func, YMeasure measure);
    //--- (end of YLightSensor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLightSensor(YAPIContext ctx, string func)
        : base(ctx, func, "LightSensor")
    {
        //--- (YLightSensor attributes initialization)
        //--- (end of YLightSensor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLightSensor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YLightSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("measureType")) {
            _measureType = json_val.GetInt("measureType");
        }
        base.imm_parseAttr(json_val);
    }

    public async Task<int> set_currentValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the sensor-specific calibration parameter so that the current value
     *   matches a desired target (linear scaling).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="calibratedVal">
     *   the desired target value.
     * </param>
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> calibrate(double calibratedVal)
    {
        string rest_val;
        rest_val = Math.Round(calibratedVal * 65536.0).ToString();
        await _setAttr("currentValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the type of light measure.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YLightSensor.MEASURETYPE_HUMAN_EYE</c>, <c>YLightSensor.MEASURETYPE_WIDE_SPECTRUM</c>,
     *   <c>YLightSensor.MEASURETYPE_INFRARED</c>, <c>YLightSensor.MEASURETYPE_HIGH_RATE</c> and
     *   <c>YLightSensor.MEASURETYPE_HIGH_ENERGY</c> corresponding to the type of light measure
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YLightSensor.MEASURETYPE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_measureType()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MEASURETYPE_INVALID;
            }
        }
        res = _measureType;
        return res;
    }


    /**
     * <summary>
     *   Changes the light sensor type used in the device.
     * <para>
     *   The measure can either
     *   approximate the response of the human eye, focus on a specific light
     *   spectrum, depending on the capabilities of the light-sensitive cell.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YLightSensor.MEASURETYPE_HUMAN_EYE</c>, <c>YLightSensor.MEASURETYPE_WIDE_SPECTRUM</c>,
     *   <c>YLightSensor.MEASURETYPE_INFRARED</c>, <c>YLightSensor.MEASURETYPE_HIGH_RATE</c> and
     *   <c>YLightSensor.MEASURETYPE_HIGH_ENERGY</c> corresponding to the light sensor type used in the device
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
    public async Task<int> set_measureType(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("measureType",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a light sensor for a given identifier.
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
     *   This function does not require that the light sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLightSensor.isOnline()</c> to test if the light sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a light sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the light sensor
     * </param>
     * <returns>
     *   a <c>YLightSensor</c> object allowing you to drive the light sensor.
     * </returns>
     */
    public static YLightSensor FindLightSensor(string func)
    {
        YLightSensor obj;
        obj = (YLightSensor) YFunction._FindFromCache("LightSensor", func);
        if (obj == null) {
            obj = new YLightSensor(func);
            YFunction._AddToCache("LightSensor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a light sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the light sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLightSensor.isOnline()</c> to test if the light sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a light sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the light sensor
     * </param>
     * <returns>
     *   a <c>YLightSensor</c> object allowing you to drive the light sensor.
     * </returns>
     */
    public static YLightSensor FindLightSensorInContext(YAPIContext yctx,string func)
    {
        YLightSensor obj;
        obj = (YLightSensor) YFunction._FindFromCacheInContext(yctx,  "LightSensor", func);
        if (obj == null) {
            obj = new YLightSensor(yctx, func);
            YFunction._AddToCache("LightSensor",  func, obj);
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
        _valueCallbackLightSensor = callback;
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
        if (_valueCallbackLightSensor != null) {
            await _valueCallbackLightSensor(this, value);
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
        _timedReportCallbackLightSensor = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackLightSensor != null) {
            await _timedReportCallbackLightSensor(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of light sensors started using <c>yFirstLightSensor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLightSensor</c> object, corresponding to
     *   a light sensor currently online, or a <c>null</c> pointer
     *   if there are no more light sensors to enumerate.
     * </returns>
     */
    public YLightSensor nextLightSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindLightSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of light sensors currently accessible.
     * <para>
     *   Use the method <c>YLightSensor.nextLightSensor()</c> to iterate on
     *   next light sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLightSensor</c> object, corresponding to
     *   the first light sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLightSensor FirstLightSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("LightSensor");
        if (next_hwid == null)  return null;
        return FindLightSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of light sensors currently accessible.
     * <para>
     *   Use the method <c>YLightSensor.nextLightSensor()</c> to iterate on
     *   next light sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YLightSensor</c> object, corresponding to
     *   the first light sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLightSensor FirstLightSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("LightSensor");
        if (next_hwid == null)  return null;
        return FindLightSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YLightSensor implementation)
}
}

