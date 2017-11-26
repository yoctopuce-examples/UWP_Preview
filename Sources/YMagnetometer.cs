/*********************************************************************
 *
 * $Id: YMagnetometer.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindMagnetometer(), the high-level API for Magnetometer functions
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

//--- (YMagnetometer return codes)
//--- (end of YMagnetometer return codes)
//--- (YMagnetometer class start)
/**
 * <summary>
 *   YMagnetometer Class: Magnetometer function interface
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
public class YMagnetometer : YSensor
{
//--- (end of YMagnetometer class start)
//--- (YMagnetometer definitions)
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
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected double _xValue = XVALUE_INVALID;
    protected double _yValue = YVALUE_INVALID;
    protected double _zValue = ZVALUE_INVALID;
    protected ValueCallback _valueCallbackMagnetometer = null;
    protected TimedReportCallback _timedReportCallbackMagnetometer = null;

    public new delegate Task ValueCallback(YMagnetometer func, string value);
    public new delegate Task TimedReportCallback(YMagnetometer func, YMeasure measure);
    //--- (end of YMagnetometer definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMagnetometer(YAPIContext ctx, string func)
        : base(ctx, func, "Magnetometer")
    {
        //--- (YMagnetometer attributes initialization)
        //--- (end of YMagnetometer attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMagnetometer(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YMagnetometer implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("bandwidth")) {
            _bandwidth = json_val.getInt("bandwidth");
        }
        if (json_val.has("xValue")) {
            _xValue = Math.Round(json_val.getDouble("xValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("yValue")) {
            _yValue = Math.Round(json_val.getDouble("yValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("zValue")) {
            _zValue = Math.Round(json_val.getDouble("zValue") * 1000.0 / 65536.0) / 1000.0;
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
     *   On failure, throws an exception or returns <c>YMagnetometer.BANDWIDTH_INVALID</c>.
     * </para>
     */
    public async Task<int> get_bandwidth()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BANDWIDTH_INVALID;
            }
        }
        res = _bandwidth;
        return res;
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
     *   Returns the X component of the magnetic field, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the X component of the magnetic field, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMagnetometer.XVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_xValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return XVALUE_INVALID;
            }
        }
        res = _xValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the Y component of the magnetic field, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Y component of the magnetic field, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMagnetometer.YVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_yValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return YVALUE_INVALID;
            }
        }
        res = _yValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the Z component of the magnetic field, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Z component of the magnetic field, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMagnetometer.ZVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_zValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ZVALUE_INVALID;
            }
        }
        res = _zValue;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a magnetometer for a given identifier.
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
     *   This function does not require that the magnetometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMagnetometer.isOnline()</c> to test if the magnetometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a magnetometer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the magnetometer
     * </param>
     * <returns>
     *   a <c>YMagnetometer</c> object allowing you to drive the magnetometer.
     * </returns>
     */
    public static YMagnetometer FindMagnetometer(string func)
    {
        YMagnetometer obj;
        obj = (YMagnetometer) YFunction._FindFromCache("Magnetometer", func);
        if (obj == null) {
            obj = new YMagnetometer(func);
            YFunction._AddToCache("Magnetometer",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a magnetometer for a given identifier in a YAPI context.
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
     *   This function does not require that the magnetometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMagnetometer.isOnline()</c> to test if the magnetometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a magnetometer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the magnetometer
     * </param>
     * <returns>
     *   a <c>YMagnetometer</c> object allowing you to drive the magnetometer.
     * </returns>
     */
    public static YMagnetometer FindMagnetometerInContext(YAPIContext yctx,string func)
    {
        YMagnetometer obj;
        obj = (YMagnetometer) YFunction._FindFromCacheInContext(yctx,  "Magnetometer", func);
        if (obj == null) {
            obj = new YMagnetometer(yctx, func);
            YFunction._AddToCache("Magnetometer",  func, obj);
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
        _valueCallbackMagnetometer = callback;
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
        if (_valueCallbackMagnetometer != null) {
            await _valueCallbackMagnetometer(this, value);
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
        _timedReportCallbackMagnetometer = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackMagnetometer != null) {
            await _timedReportCallbackMagnetometer(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of magnetometers started using <c>yFirstMagnetometer()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMagnetometer</c> object, corresponding to
     *   a magnetometer currently online, or a <c>null</c> pointer
     *   if there are no more magnetometers to enumerate.
     * </returns>
     */
    public YMagnetometer nextMagnetometer()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindMagnetometerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of magnetometers currently accessible.
     * <para>
     *   Use the method <c>YMagnetometer.nextMagnetometer()</c> to iterate on
     *   next magnetometers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMagnetometer</c> object, corresponding to
     *   the first magnetometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMagnetometer FirstMagnetometer()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Magnetometer");
        if (next_hwid == null)  return null;
        return FindMagnetometerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of magnetometers currently accessible.
     * <para>
     *   Use the method <c>YMagnetometer.nextMagnetometer()</c> to iterate on
     *   next magnetometers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YMagnetometer</c> object, corresponding to
     *   the first magnetometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMagnetometer FirstMagnetometerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Magnetometer");
        if (next_hwid == null)  return null;
        return FindMagnetometerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YMagnetometer implementation)
}
}

