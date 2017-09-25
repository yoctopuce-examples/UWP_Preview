/*********************************************************************
 *
 * $Id: YWakeUpMonitor.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindWakeUpMonitor(), the high-level API for WakeUpMonitor functions
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

//--- (YWakeUpMonitor return codes)
//--- (end of YWakeUpMonitor return codes)
//--- (YWakeUpMonitor class start)
/**
 * <summary>
 *   YWakeUpMonitor Class: WakeUpMonitor function interface
 * <para>
 *   The WakeUpMonitor function handles globally all wake-up sources, as well
 *   as automated sleep mode.
 * </para>
 * </summary>
 */
public class YWakeUpMonitor : YFunction
{
//--- (end of YWakeUpMonitor class start)
//--- (YWakeUpMonitor definitions)
    /**
     * <summary>
     *   invalid powerDuration value
     * </summary>
     */
    public const  int POWERDURATION_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid sleepCountdown value
     * </summary>
     */
    public const  int SLEEPCOUNTDOWN_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid nextWakeUp value
     * </summary>
     */
    public const  long NEXTWAKEUP_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid wakeUpReason value
     * </summary>
     */
    public const int WAKEUPREASON_USBPOWER = 0;
    public const int WAKEUPREASON_EXTPOWER = 1;
    public const int WAKEUPREASON_ENDOFSLEEP = 2;
    public const int WAKEUPREASON_EXTSIG1 = 3;
    public const int WAKEUPREASON_SCHEDULE1 = 4;
    public const int WAKEUPREASON_SCHEDULE2 = 5;
    public const int WAKEUPREASON_INVALID = -1;
    /**
     * <summary>
     *   invalid wakeUpState value
     * </summary>
     */
    public const int WAKEUPSTATE_SLEEPING = 0;
    public const int WAKEUPSTATE_AWAKE = 1;
    public const int WAKEUPSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid rtcTime value
     * </summary>
     */
    public const  long RTCTIME_INVALID = YAPI.INVALID_LONG;
    protected int _powerDuration = POWERDURATION_INVALID;
    protected int _sleepCountdown = SLEEPCOUNTDOWN_INVALID;
    protected long _nextWakeUp = NEXTWAKEUP_INVALID;
    protected int _wakeUpReason = WAKEUPREASON_INVALID;
    protected int _wakeUpState = WAKEUPSTATE_INVALID;
    protected long _rtcTime = RTCTIME_INVALID;
    public const int _endOfTime = 2145960000;
    protected ValueCallback _valueCallbackWakeUpMonitor = null;

    public new delegate Task ValueCallback(YWakeUpMonitor func, string value);
    public new delegate Task TimedReportCallback(YWakeUpMonitor func, YMeasure measure);
    //--- (end of YWakeUpMonitor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWakeUpMonitor(YAPIContext ctx, string func)
        : base(ctx, func, "WakeUpMonitor")
    {
        //--- (YWakeUpMonitor attributes initialization)
        //--- (end of YWakeUpMonitor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWakeUpMonitor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YWakeUpMonitor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("powerDuration")) {
            _powerDuration = json_val.GetInt("powerDuration");
        }
        if (json_val.Has("sleepCountdown")) {
            _sleepCountdown = json_val.GetInt("sleepCountdown");
        }
        if (json_val.Has("nextWakeUp")) {
            _nextWakeUp = json_val.GetLong("nextWakeUp");
        }
        if (json_val.Has("wakeUpReason")) {
            _wakeUpReason = json_val.GetInt("wakeUpReason");
        }
        if (json_val.Has("wakeUpState")) {
            _wakeUpState = json_val.GetInt("wakeUpState");
        }
        if (json_val.Has("rtcTime")) {
            _rtcTime = json_val.GetLong("rtcTime");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the maximal wake up time (in seconds) before automatically going to sleep.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximal wake up time (in seconds) before automatically going to sleep
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpMonitor.POWERDURATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_powerDuration()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWERDURATION_INVALID;
            }
        }
        res = _powerDuration;
        return res;
    }


    /**
     * <summary>
     *   Changes the maximal wake up time (seconds) before automatically going to sleep.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the maximal wake up time (seconds) before automatically going to sleep
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
    public async Task<int> set_powerDuration(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("powerDuration",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the delay before the  next sleep period.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the delay before the  next sleep period
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpMonitor.SLEEPCOUNTDOWN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_sleepCountdown()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SLEEPCOUNTDOWN_INVALID;
            }
        }
        res = _sleepCountdown;
        return res;
    }


    /**
     * <summary>
     *   Changes the delay before the next sleep period.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the delay before the next sleep period
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
    public async Task<int> set_sleepCountdown(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("sleepCountdown",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the next scheduled wake up date/time (UNIX format).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the next scheduled wake up date/time (UNIX format)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpMonitor.NEXTWAKEUP_INVALID</c>.
     * </para>
     */
    public async Task<long> get_nextWakeUp()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return NEXTWAKEUP_INVALID;
            }
        }
        res = _nextWakeUp;
        return res;
    }


    /**
     * <summary>
     *   Changes the days of the week when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the days of the week when a wake up must take place
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
    public async Task<int> set_nextWakeUp(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("nextWakeUp",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the latest wake up reason.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YWakeUpMonitor.WAKEUPREASON_USBPOWER</c>, <c>YWakeUpMonitor.WAKEUPREASON_EXTPOWER</c>,
     *   <c>YWakeUpMonitor.WAKEUPREASON_ENDOFSLEEP</c>, <c>YWakeUpMonitor.WAKEUPREASON_EXTSIG1</c>,
     *   <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE1</c> and <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE2</c>
     *   corresponding to the latest wake up reason
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpMonitor.WAKEUPREASON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_wakeUpReason()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return WAKEUPREASON_INVALID;
            }
        }
        res = _wakeUpReason;
        return res;
    }


    /**
     * <summary>
     *   Returns  the current state of the monitor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YWakeUpMonitor.WAKEUPSTATE_SLEEPING</c> or <c>YWakeUpMonitor.WAKEUPSTATE_AWAKE</c>,
     *   according to  the current state of the monitor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpMonitor.WAKEUPSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_wakeUpState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return WAKEUPSTATE_INVALID;
            }
        }
        res = _wakeUpState;
        return res;
    }


    public async Task<int> set_wakeUpState(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("wakeUpState",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<long> get_rtcTime()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RTCTIME_INVALID;
            }
        }
        res = _rtcTime;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a monitor for a given identifier.
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
     *   This function does not require that the monitor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWakeUpMonitor.isOnline()</c> to test if the monitor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a monitor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the monitor
     * </param>
     * <returns>
     *   a <c>YWakeUpMonitor</c> object allowing you to drive the monitor.
     * </returns>
     */
    public static YWakeUpMonitor FindWakeUpMonitor(string func)
    {
        YWakeUpMonitor obj;
        obj = (YWakeUpMonitor) YFunction._FindFromCache("WakeUpMonitor", func);
        if (obj == null) {
            obj = new YWakeUpMonitor(func);
            YFunction._AddToCache("WakeUpMonitor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a monitor for a given identifier in a YAPI context.
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
     *   This function does not require that the monitor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWakeUpMonitor.isOnline()</c> to test if the monitor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a monitor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the monitor
     * </param>
     * <returns>
     *   a <c>YWakeUpMonitor</c> object allowing you to drive the monitor.
     * </returns>
     */
    public static YWakeUpMonitor FindWakeUpMonitorInContext(YAPIContext yctx,string func)
    {
        YWakeUpMonitor obj;
        obj = (YWakeUpMonitor) YFunction._FindFromCacheInContext(yctx,  "WakeUpMonitor", func);
        if (obj == null) {
            obj = new YWakeUpMonitor(yctx, func);
            YFunction._AddToCache("WakeUpMonitor",  func, obj);
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
        _valueCallbackWakeUpMonitor = callback;
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
        if (_valueCallbackWakeUpMonitor != null) {
            await _valueCallbackWakeUpMonitor(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Forces a wake up.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<int> wakeUp()
    {
        return await this.set_wakeUpState(WAKEUPSTATE_AWAKE);
    }

    /**
     * <summary>
     *   Goes to sleep until the next wake up condition is met,  the
     *   RTC time must have been set before calling this function.
     * <para>
     * </para>
     * </summary>
     * <param name="secBeforeSleep">
     *   number of seconds before going into sleep mode,
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> sleep(int secBeforeSleep)
    {
        int currTime;
        currTime = (int)(await this.get_rtcTime());
        if (!(currTime != 0)) { this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return YAPI.RTC_NOT_READY; }
        await this.set_nextWakeUp(_endOfTime);
        await this.set_sleepCountdown(secBeforeSleep);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Goes to sleep for a specific duration or until the next wake up condition is met, the
     *   RTC time must have been set before calling this function.
     * <para>
     *   The count down before sleep
     *   can be canceled with resetSleepCountDown.
     * </para>
     * </summary>
     * <param name="secUntilWakeUp">
     *   number of seconds before next wake up
     * </param>
     * <param name="secBeforeSleep">
     *   number of seconds before going into sleep mode
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> sleepFor(int secUntilWakeUp,int secBeforeSleep)
    {
        int currTime;
        currTime = (int)(await this.get_rtcTime());
        if (!(currTime != 0)) { this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return YAPI.RTC_NOT_READY; }
        await this.set_nextWakeUp(currTime+secUntilWakeUp);
        await this.set_sleepCountdown(secBeforeSleep);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Go to sleep until a specific date is reached or until the next wake up condition is met, the
     *   RTC time must have been set before calling this function.
     * <para>
     *   The count down before sleep
     *   can be canceled with resetSleepCountDown.
     * </para>
     * </summary>
     * <param name="wakeUpTime">
     *   wake-up datetime (UNIX format)
     * </param>
     * <param name="secBeforeSleep">
     *   number of seconds before going into sleep mode
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> sleepUntil(int wakeUpTime,int secBeforeSleep)
    {
        int currTime;
        currTime = (int)(await this.get_rtcTime());
        if (!(currTime != 0)) { this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return YAPI.RTC_NOT_READY; }
        await this.set_nextWakeUp(wakeUpTime);
        await this.set_sleepCountdown(secBeforeSleep);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Resets the sleep countdown.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> resetSleepCountDown()
    {
        await this.set_sleepCountdown(0);
        await this.set_nextWakeUp(0);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of monitors started using <c>yFirstWakeUpMonitor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWakeUpMonitor</c> object, corresponding to
     *   a monitor currently online, or a <c>null</c> pointer
     *   if there are no more monitors to enumerate.
     * </returns>
     */
    public YWakeUpMonitor nextWakeUpMonitor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindWakeUpMonitorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of monitors currently accessible.
     * <para>
     *   Use the method <c>YWakeUpMonitor.nextWakeUpMonitor()</c> to iterate on
     *   next monitors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWakeUpMonitor</c> object, corresponding to
     *   the first monitor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWakeUpMonitor FirstWakeUpMonitor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WakeUpMonitor");
        if (next_hwid == null)  return null;
        return FindWakeUpMonitorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of monitors currently accessible.
     * <para>
     *   Use the method <c>YWakeUpMonitor.nextWakeUpMonitor()</c> to iterate on
     *   next monitors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YWakeUpMonitor</c> object, corresponding to
     *   the first monitor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWakeUpMonitor FirstWakeUpMonitorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WakeUpMonitor");
        if (next_hwid == null)  return null;
        return FindWakeUpMonitorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YWakeUpMonitor implementation)
}
}

