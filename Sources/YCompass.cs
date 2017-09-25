/*********************************************************************
 *
 * $Id: YCompass.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindCompass(), the high-level API for Compass functions
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

//--- (YCompass return codes)
//--- (end of YCompass return codes)
//--- (YCompass class start)
/**
 * <summary>
 *   YCompass Class: Compass function interface
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
public class YCompass : YSensor
{
//--- (end of YCompass class start)
//--- (YCompass definitions)
    /**
     * <summary>
     *   invalid bandwidth value
     * </summary>
     */
    public const  int BANDWIDTH_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid axis value
     * </summary>
     */
    public const int AXIS_X = 0;
    public const int AXIS_Y = 1;
    public const int AXIS_Z = 2;
    public const int AXIS_INVALID = -1;
    /**
     * <summary>
     *   invalid magneticHeading value
     * </summary>
     */
    public const  double MAGNETICHEADING_INVALID = YAPI.INVALID_DOUBLE;
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected int _axis = AXIS_INVALID;
    protected double _magneticHeading = MAGNETICHEADING_INVALID;
    protected ValueCallback _valueCallbackCompass = null;
    protected TimedReportCallback _timedReportCallbackCompass = null;

    public new delegate Task ValueCallback(YCompass func, string value);
    public new delegate Task TimedReportCallback(YCompass func, YMeasure measure);
    //--- (end of YCompass definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCompass(YAPIContext ctx, string func)
        : base(ctx, func, "Compass")
    {
        //--- (YCompass attributes initialization)
        //--- (end of YCompass attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCompass(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YCompass implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("bandwidth")) {
            _bandwidth = json_val.GetInt("bandwidth");
        }
        if (json_val.Has("axis")) {
            _axis = json_val.GetInt("axis");
        }
        if (json_val.Has("magneticHeading")) {
            _magneticHeading = Math.Round(json_val.GetDouble("magneticHeading") * 1000.0 / 65536.0) / 1000.0;
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
     *   On failure, throws an exception or returns <c>YCompass.BANDWIDTH_INVALID</c>.
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
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_axis()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return AXIS_INVALID;
            }
        }
        res = _axis;
        return res;
    }


    /**
     * <summary>
     *   Returns the magnetic heading, regardless of the configured bearing.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the magnetic heading, regardless of the configured bearing
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCompass.MAGNETICHEADING_INVALID</c>.
     * </para>
     */
    public async Task<double> get_magneticHeading()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAGNETICHEADING_INVALID;
            }
        }
        res = _magneticHeading;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a compass for a given identifier.
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
     *   This function does not require that the compass is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCompass.isOnline()</c> to test if the compass is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a compass by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the compass
     * </param>
     * <returns>
     *   a <c>YCompass</c> object allowing you to drive the compass.
     * </returns>
     */
    public static YCompass FindCompass(string func)
    {
        YCompass obj;
        obj = (YCompass) YFunction._FindFromCache("Compass", func);
        if (obj == null) {
            obj = new YCompass(func);
            YFunction._AddToCache("Compass",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a compass for a given identifier in a YAPI context.
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
     *   This function does not require that the compass is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCompass.isOnline()</c> to test if the compass is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a compass by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the compass
     * </param>
     * <returns>
     *   a <c>YCompass</c> object allowing you to drive the compass.
     * </returns>
     */
    public static YCompass FindCompassInContext(YAPIContext yctx,string func)
    {
        YCompass obj;
        obj = (YCompass) YFunction._FindFromCacheInContext(yctx,  "Compass", func);
        if (obj == null) {
            obj = new YCompass(yctx, func);
            YFunction._AddToCache("Compass",  func, obj);
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
        _valueCallbackCompass = callback;
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
        if (_valueCallbackCompass != null) {
            await _valueCallbackCompass(this, value);
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
        _timedReportCallbackCompass = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackCompass != null) {
            await _timedReportCallbackCompass(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of compasses started using <c>yFirstCompass()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   a compass currently online, or a <c>null</c> pointer
     *   if there are no more compasses to enumerate.
     * </returns>
     */
    public YCompass nextCompass()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindCompassInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of compasses currently accessible.
     * <para>
     *   Use the method <c>YCompass.nextCompass()</c> to iterate on
     *   next compasses.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   the first compass currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCompass FirstCompass()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Compass");
        if (next_hwid == null)  return null;
        return FindCompassInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of compasses currently accessible.
     * <para>
     *   Use the method <c>YCompass.nextCompass()</c> to iterate on
     *   next compasses.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   the first compass currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCompass FirstCompassInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Compass");
        if (next_hwid == null)  return null;
        return FindCompassInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YCompass implementation)
}
}

