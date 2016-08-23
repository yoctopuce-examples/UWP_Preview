/*********************************************************************
 *
 * $Id: YAccelerometer.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindAccelerometer(), the high-level API for Accelerometer functions
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

//--- (YAccelerometer return codes)
//--- (end of YAccelerometer return codes)
//--- (YAccelerometer class start)
/**
 * <summary>
 *   YAccelerometer Class: Accelerometer function interface
 * <para>
 *   The YSensor class is the parent class for all Yoctopuce sensors. It can be
 *   used to read the current value and unit of any sensor, read the min/max
 *   value, configure autonomous recording frequency and access recorded data.
 *   It also provide a function to register a callback invoked each time the
 *   observed value changes, or at a predefined interval. Using this class rather
 *   than a specific subclass makes it possible to create generic applications
 *   that work with any Yoctopuce sensor, even those that do not yet exist.
 *   Note: The YAnButton class is the only analog input which does not inherit
 *   from YSensor.
 * </para>
 * </summary>
 */
public class YAccelerometer : YSensor
{
//--- (end of YAccelerometer class start)
//--- (YAccelerometer definitions)
    /**
     * <summary>
     *   invalid bandwidth value
     * </summary>
     */
    public const  int BANDWIDTH_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid xValue value
     * </summary>
     */
    public const  double XVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid yValue value
     * </summary>
     */
    public const  double YVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid zValue value
     * </summary>
     */
    public const  double ZVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid gravityCancellation value
     * </summary>
     */
    public const int GRAVITYCANCELLATION_OFF = 0;
    public const int GRAVITYCANCELLATION_ON = 1;
    public const int GRAVITYCANCELLATION_INVALID = -1;
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected double _xValue = XVALUE_INVALID;
    protected double _yValue = YVALUE_INVALID;
    protected double _zValue = ZVALUE_INVALID;
    protected int _gravityCancellation = GRAVITYCANCELLATION_INVALID;
    protected ValueCallback _valueCallbackAccelerometer = null;
    protected TimedReportCallback _timedReportCallbackAccelerometer = null;

    public new delegate Task ValueCallback(YAccelerometer func, string value);
    public new delegate Task TimedReportCallback(YAccelerometer func, YMeasure measure);
    //--- (end of YAccelerometer definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAccelerometer(YAPIContext ctx, string func)
        : base(ctx, func, "Accelerometer")
    {
        //--- (YAccelerometer attributes initialization)
        //--- (end of YAccelerometer attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAccelerometer(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAccelerometer implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("bandwidth")) {
            _bandwidth = json_val.GetInt("bandwidth");
        }
        if (json_val.Has("xValue")) {
            _xValue = Math.Round(json_val.GetDouble("xValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("yValue")) {
            _yValue = Math.Round(json_val.GetDouble("yValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("zValue")) {
            _zValue = Math.Round(json_val.GetDouble("zValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("gravityCancellation")) {
            _gravityCancellation = json_val.GetInt("gravityCancellation") > 0 ? 1 : 0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the measure update frequency, measured in Hz (Yocto-3D-V2 only).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the measure update frequency, measured in Hz (Yocto-3D-V2 only)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.BANDWIDTH_INVALID</c>.
     * </para>
     */
    public async Task<int> get_bandwidth()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BANDWIDTH_INVALID;
            }
        }
        return _bandwidth;
    }


    /**
     * <summary>
     *   Changes the measure update frequency, measured in Hz (Yocto-3D-V2 only).
     * <para>
     *   When the
     *   frequency is lower, the device performs averaging.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the measure update frequency, measured in Hz (Yocto-3D-V2 only)
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
    public async Task<int> set_bandwidth(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("bandwidth",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the X component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the X component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.XVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_xValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return XVALUE_INVALID;
            }
        }
        return _xValue;
    }


    /**
     * <summary>
     *   Returns the Y component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Y component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.YVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_yValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return YVALUE_INVALID;
            }
        }
        return _yValue;
    }


    /**
     * <summary>
     *   Returns the Z component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Z component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.ZVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_zValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ZVALUE_INVALID;
            }
        }
        return _zValue;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_gravityCancellation()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return GRAVITYCANCELLATION_INVALID;
            }
        }
        return _gravityCancellation;
    }


    public async Task<int> set_gravityCancellation(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("gravityCancellation",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves an accelerometer for a given identifier.
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
     *   This function does not require that the accelerometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAccelerometer.isOnline()</c> to test if the accelerometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an accelerometer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the accelerometer
     * </param>
     * <returns>
     *   a <c>YAccelerometer</c> object allowing you to drive the accelerometer.
     * </returns>
     */
    public static YAccelerometer FindAccelerometer(string func)
    {
        YAccelerometer obj;
        obj = (YAccelerometer) YFunction._FindFromCache("Accelerometer", func);
        if (obj == null) {
            obj = new YAccelerometer(func);
            YFunction._AddToCache("Accelerometer",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an accelerometer for a given identifier in a YAPI context.
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
     *   This function does not require that the accelerometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAccelerometer.isOnline()</c> to test if the accelerometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an accelerometer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the accelerometer
     * </param>
     * <returns>
     *   a <c>YAccelerometer</c> object allowing you to drive the accelerometer.
     * </returns>
     */
    public static YAccelerometer FindAccelerometerInContext(YAPIContext yctx,string func)
    {
        YAccelerometer obj;
        obj = (YAccelerometer) YFunction._FindFromCacheInContext(yctx,  "Accelerometer", func);
        if (obj == null) {
            obj = new YAccelerometer(yctx, func);
            YFunction._AddToCache("Accelerometer",  func, obj);
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
        _valueCallbackAccelerometer = callback;
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
        if (_valueCallbackAccelerometer != null) {
            await _valueCallbackAccelerometer(this, value);
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
        _timedReportCallbackAccelerometer = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackAccelerometer != null) {
            await _timedReportCallbackAccelerometer(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of accelerometers started using <c>yFirstAccelerometer()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   an accelerometer currently online, or a <c>null</c> pointer
     *   if there are no more accelerometers to enumerate.
     * </returns>
     */
    public YAccelerometer nextAccelerometer()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAccelerometerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of accelerometers currently accessible.
     * <para>
     *   Use the method <c>YAccelerometer.nextAccelerometer()</c> to iterate on
     *   next accelerometers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   the first accelerometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAccelerometer FirstAccelerometer()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Accelerometer");
        if (next_hwid == null)  return null;
        return FindAccelerometerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of accelerometers currently accessible.
     * <para>
     *   Use the method <c>YAccelerometer.nextAccelerometer()</c> to iterate on
     *   next accelerometers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   the first accelerometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAccelerometer FirstAccelerometerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Accelerometer");
        if (next_hwid == null)  return null;
        return FindAccelerometerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAccelerometer implementation)
}
}

