/*********************************************************************
 *
 * $Id: YProximity.cs 28559 2017-09-15 15:01:38Z seb $
 *
 * Implements FindProximity(), the high-level API for Proximity functions
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

//--- (YProximity return codes)
//--- (end of YProximity return codes)
//--- (YProximity class start)
/**
 * <summary>
 *   YProximity Class: Proximity function interface
 * <para>
 *   The Yoctopuce class YProximity allows you to use and configure Yoctopuce proximity
 *   sensors. It inherits from the YSensor class the core functions to read measurements,
 *   to register callback functions, to access the autonomous datalogger.
 *   This class adds the ability to easily perform a one-point linear calibration
 *   to compensate the effect of a glass or filter placed in front of the sensor.
 * </para>
 * </summary>
 */
public class YProximity : YSensor
{
//--- (end of YProximity class start)
//--- (YProximity definitions)
    /**
     * <summary>
     *   invalid signalValue value
     * </summary>
     */
    public const  double SIGNALVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid detectionThreshold value
     * </summary>
     */
    public const  int DETECTIONTHRESHOLD_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid isPresent value
     * </summary>
     */
    public const int ISPRESENT_FALSE = 0;
    public const int ISPRESENT_TRUE = 1;
    public const int ISPRESENT_INVALID = -1;
    /**
     * <summary>
     *   invalid lastTimeApproached value
     * </summary>
     */
    public const  long LASTTIMEAPPROACHED_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid lastTimeRemoved value
     * </summary>
     */
    public const  long LASTTIMEREMOVED_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid pulseCounter value
     * </summary>
     */
    public const  long PULSECOUNTER_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid pulseTimer value
     * </summary>
     */
    public const  long PULSETIMER_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid proximityReportMode value
     * </summary>
     */
    public const int PROXIMITYREPORTMODE_NUMERIC = 0;
    public const int PROXIMITYREPORTMODE_PRESENCE = 1;
    public const int PROXIMITYREPORTMODE_PULSECOUNT = 2;
    public const int PROXIMITYREPORTMODE_INVALID = -1;
    protected double _signalValue = SIGNALVALUE_INVALID;
    protected int _detectionThreshold = DETECTIONTHRESHOLD_INVALID;
    protected int _isPresent = ISPRESENT_INVALID;
    protected long _lastTimeApproached = LASTTIMEAPPROACHED_INVALID;
    protected long _lastTimeRemoved = LASTTIMEREMOVED_INVALID;
    protected long _pulseCounter = PULSECOUNTER_INVALID;
    protected long _pulseTimer = PULSETIMER_INVALID;
    protected int _proximityReportMode = PROXIMITYREPORTMODE_INVALID;
    protected ValueCallback _valueCallbackProximity = null;
    protected TimedReportCallback _timedReportCallbackProximity = null;

    public new delegate Task ValueCallback(YProximity func, string value);
    public new delegate Task TimedReportCallback(YProximity func, YMeasure measure);
    //--- (end of YProximity definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YProximity(YAPIContext ctx, string func)
        : base(ctx, func, "Proximity")
    {
        //--- (YProximity attributes initialization)
        //--- (end of YProximity attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YProximity(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YProximity implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("signalValue")) {
            _signalValue = Math.Round(json_val.GetDouble("signalValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("detectionThreshold")) {
            _detectionThreshold = json_val.GetInt("detectionThreshold");
        }
        if (json_val.Has("isPresent")) {
            _isPresent = json_val.GetInt("isPresent") > 0 ? 1 : 0;
        }
        if (json_val.Has("lastTimeApproached")) {
            _lastTimeApproached = json_val.GetLong("lastTimeApproached");
        }
        if (json_val.Has("lastTimeRemoved")) {
            _lastTimeRemoved = json_val.GetLong("lastTimeRemoved");
        }
        if (json_val.Has("pulseCounter")) {
            _pulseCounter = json_val.GetLong("pulseCounter");
        }
        if (json_val.Has("pulseTimer")) {
            _pulseTimer = json_val.GetLong("pulseTimer");
        }
        if (json_val.Has("proximityReportMode")) {
            _proximityReportMode = json_val.GetInt("proximityReportMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current value of signal measured by the proximity sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current value of signal measured by the proximity sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.SIGNALVALUE_INVALID</c>.
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
     *   Returns the threshold used to determine the logical state of the proximity sensor, when considered
     *   as a binary input (on/off).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the threshold used to determine the logical state of the proximity
     *   sensor, when considered
     *   as a binary input (on/off)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.DETECTIONTHRESHOLD_INVALID</c>.
     * </para>
     */
    public async Task<int> get_detectionThreshold()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DETECTIONTHRESHOLD_INVALID;
            }
        }
        res = _detectionThreshold;
        return res;
    }


    /**
     * <summary>
     *   Changes the threshold used to determine the logical state of the proximity sensor, when considered
     *   as a binary input (on/off).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the threshold used to determine the logical state of the proximity
     *   sensor, when considered
     *   as a binary input (on/off)
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
    public async Task<int> set_detectionThreshold(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("detectionThreshold",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns true if the input (considered as binary) is active (detection value is smaller than the specified <c>threshold</c>), and false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YProximity.ISPRESENT_FALSE</c> or <c>YProximity.ISPRESENT_TRUE</c>, according to true if
     *   the input (considered as binary) is active (detection value is smaller than the specified
     *   <c>threshold</c>), and false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.ISPRESENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_isPresent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ISPRESENT_INVALID;
            }
        }
        res = _isPresent;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on and the last observed
     *   detection (the input contact transitioned from absent to present).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last observed
     *   detection (the input contact transitioned from absent to present)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.LASTTIMEAPPROACHED_INVALID</c>.
     * </para>
     */
    public async Task<long> get_lastTimeApproached()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LASTTIMEAPPROACHED_INVALID;
            }
        }
        res = _lastTimeApproached;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on and the last observed
     *   detection (the input contact transitioned from present to absent).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last observed
     *   detection (the input contact transitioned from present to absent)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.LASTTIMEREMOVED_INVALID</c>.
     * </para>
     */
    public async Task<long> get_lastTimeRemoved()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LASTTIMEREMOVED_INVALID;
            }
        }
        res = _lastTimeRemoved;
        return res;
    }


    /**
     * <summary>
     *   Returns the pulse counter value.
     * <para>
     *   The value is a 32 bit integer. In case
     *   of overflow (>=2^32), the counter will wrap. To reset the counter, just
     *   call the resetCounter() method.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the pulse counter value
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.PULSECOUNTER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseCounter()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULSECOUNTER_INVALID;
            }
        }
        res = _pulseCounter;
        return res;
    }


    public async Task<int> set_pulseCounter(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pulseCounter",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the timer of the pulse counter (ms).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the timer of the pulse counter (ms)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.PULSETIMER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseTimer()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULSETIMER_INVALID;
            }
        }
        res = _pulseTimer;
        return res;
    }


    /**
     * <summary>
     *   Returns the parameter (sensor value, presence or pulse count) returned by the get_currentValue function and callbacks.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YProximity.PROXIMITYREPORTMODE_NUMERIC</c>,
     *   <c>YProximity.PROXIMITYREPORTMODE_PRESENCE</c> and <c>YProximity.PROXIMITYREPORTMODE_PULSECOUNT</c>
     *   corresponding to the parameter (sensor value, presence or pulse count) returned by the
     *   get_currentValue function and callbacks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YProximity.PROXIMITYREPORTMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_proximityReportMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PROXIMITYREPORTMODE_INVALID;
            }
        }
        res = _proximityReportMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the  parameter  type (sensor value, presence or pulse count) returned by the get_currentValue function and callbacks.
     * <para>
     *   The edge count value is limited to the 6 lowest digits. For values greater than one million, use
     *   get_pulseCounter().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YProximity.PROXIMITYREPORTMODE_NUMERIC</c>,
     *   <c>YProximity.PROXIMITYREPORTMODE_PRESENCE</c> and <c>YProximity.PROXIMITYREPORTMODE_PULSECOUNT</c>
     *   corresponding to the  parameter  type (sensor value, presence or pulse count) returned by the
     *   get_currentValue function and callbacks
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
    public async Task<int> set_proximityReportMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("proximityReportMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a proximity sensor for a given identifier.
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
     *   This function does not require that the proximity sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YProximity.isOnline()</c> to test if the proximity sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a proximity sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the proximity sensor
     * </param>
     * <returns>
     *   a <c>YProximity</c> object allowing you to drive the proximity sensor.
     * </returns>
     */
    public static YProximity FindProximity(string func)
    {
        YProximity obj;
        obj = (YProximity) YFunction._FindFromCache("Proximity", func);
        if (obj == null) {
            obj = new YProximity(func);
            YFunction._AddToCache("Proximity",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a proximity sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the proximity sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YProximity.isOnline()</c> to test if the proximity sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a proximity sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the proximity sensor
     * </param>
     * <returns>
     *   a <c>YProximity</c> object allowing you to drive the proximity sensor.
     * </returns>
     */
    public static YProximity FindProximityInContext(YAPIContext yctx,string func)
    {
        YProximity obj;
        obj = (YProximity) YFunction._FindFromCacheInContext(yctx,  "Proximity", func);
        if (obj == null) {
            obj = new YProximity(yctx, func);
            YFunction._AddToCache("Proximity",  func, obj);
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
        _valueCallbackProximity = callback;
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
        if (_valueCallbackProximity != null) {
            await _valueCallbackProximity(this, value);
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
        _timedReportCallbackProximity = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackProximity != null) {
            await _timedReportCallbackProximity(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Resets the pulse counter value as well as its timer.
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
    public virtual async Task<int> resetCounter()
    {
        return await this.set_pulseCounter(0);
    }

    /**
     * <summary>
     *   Continues the enumeration of proximity sensors started using <c>yFirstProximity()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YProximity</c> object, corresponding to
     *   a proximity sensor currently online, or a <c>null</c> pointer
     *   if there are no more proximity sensors to enumerate.
     * </returns>
     */
    public YProximity nextProximity()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindProximityInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of proximity sensors currently accessible.
     * <para>
     *   Use the method <c>YProximity.nextProximity()</c> to iterate on
     *   next proximity sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YProximity</c> object, corresponding to
     *   the first proximity sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YProximity FirstProximity()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Proximity");
        if (next_hwid == null)  return null;
        return FindProximityInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of proximity sensors currently accessible.
     * <para>
     *   Use the method <c>YProximity.nextProximity()</c> to iterate on
     *   next proximity sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YProximity</c> object, corresponding to
     *   the first proximity sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YProximity FirstProximityInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Proximity");
        if (next_hwid == null)  return null;
        return FindProximityInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YProximity implementation)
}
}

