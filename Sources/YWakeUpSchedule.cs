/*********************************************************************
 *
 * $Id: YWakeUpSchedule.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindWakeUpSchedule(), the high-level API for WakeUpSchedule functions
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

//--- (YWakeUpSchedule return codes)
//--- (end of YWakeUpSchedule return codes)
//--- (YWakeUpSchedule class start)
/**
 * <summary>
 *   YWakeUpSchedule Class: WakeUpSchedule function interface
 * <para>
 *   The WakeUpSchedule function implements a wake up condition. The wake up time is
 *   specified as a set of months and/or days and/or hours and/or minutes when the
 *   wake up should happen.
 * </para>
 * </summary>
 */
public class YWakeUpSchedule : YFunction
{
//--- (end of YWakeUpSchedule class start)
//--- (YWakeUpSchedule definitions)
    /**
     * <summary>
     *   invalid minutesA value
     * </summary>
     */
    public const  int MINUTESA_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid minutesB value
     * </summary>
     */
    public const  int MINUTESB_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid hours value
     * </summary>
     */
    public const  int HOURS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid weekDays value
     * </summary>
     */
    public const  int WEEKDAYS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid monthDays value
     * </summary>
     */
    public const  int MONTHDAYS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid months value
     * </summary>
     */
    public const  int MONTHS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid nextOccurence value
     * </summary>
     */
    public const  long NEXTOCCURENCE_INVALID = YAPI.INVALID_LONG;
    protected int _minutesA = MINUTESA_INVALID;
    protected int _minutesB = MINUTESB_INVALID;
    protected int _hours = HOURS_INVALID;
    protected int _weekDays = WEEKDAYS_INVALID;
    protected int _monthDays = MONTHDAYS_INVALID;
    protected int _months = MONTHS_INVALID;
    protected long _nextOccurence = NEXTOCCURENCE_INVALID;
    protected ValueCallback _valueCallbackWakeUpSchedule = null;

    public new delegate Task ValueCallback(YWakeUpSchedule func, string value);
    public new delegate Task TimedReportCallback(YWakeUpSchedule func, YMeasure measure);
    //--- (end of YWakeUpSchedule definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWakeUpSchedule(YAPIContext ctx, string func)
        : base(ctx, func, "WakeUpSchedule")
    {
        //--- (YWakeUpSchedule attributes initialization)
        //--- (end of YWakeUpSchedule attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWakeUpSchedule(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YWakeUpSchedule implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("minutesA")) {
            _minutesA = json_val.GetInt("minutesA");
        }
        if (json_val.Has("minutesB")) {
            _minutesB = json_val.GetInt("minutesB");
        }
        if (json_val.Has("hours")) {
            _hours = json_val.GetInt("hours");
        }
        if (json_val.Has("weekDays")) {
            _weekDays = json_val.GetInt("weekDays");
        }
        if (json_val.Has("monthDays")) {
            _monthDays = json_val.GetInt("monthDays");
        }
        if (json_val.Has("months")) {
            _months = json_val.GetInt("months");
        }
        if (json_val.Has("nextOccurence")) {
            _nextOccurence = json_val.GetLong("nextOccurence");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the minutes in the 00-29 interval of each hour scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the minutes in the 00-29 interval of each hour scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.MINUTESA_INVALID</c>.
     * </para>
     */
    public async Task<int> get_minutesA()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MINUTESA_INVALID;
            }
        }
        res = _minutesA;
        return res;
    }


    /**
     * <summary>
     *   Changes the minutes in the 00-29 interval when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the minutes in the 00-29 interval when a wake up must take place
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
    public async Task<int> set_minutesA(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("minutesA",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the minutes in the 30-59 intervalof each hour scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the minutes in the 30-59 intervalof each hour scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.MINUTESB_INVALID</c>.
     * </para>
     */
    public async Task<int> get_minutesB()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MINUTESB_INVALID;
            }
        }
        res = _minutesB;
        return res;
    }


    /**
     * <summary>
     *   Changes the minutes in the 30-59 interval when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the minutes in the 30-59 interval when a wake up must take place
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
    public async Task<int> set_minutesB(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("minutesB",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the hours scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the hours scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.HOURS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_hours()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HOURS_INVALID;
            }
        }
        res = _hours;
        return res;
    }


    /**
     * <summary>
     *   Changes the hours when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the hours when a wake up must take place
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
    public async Task<int> set_hours(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("hours",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the days of the week scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the days of the week scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.WEEKDAYS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_weekDays()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return WEEKDAYS_INVALID;
            }
        }
        res = _weekDays;
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
    public async Task<int> set_weekDays(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("weekDays",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the days of the month scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the days of the month scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.MONTHDAYS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_monthDays()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MONTHDAYS_INVALID;
            }
        }
        res = _monthDays;
        return res;
    }


    /**
     * <summary>
     *   Changes the days of the month when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the days of the month when a wake up must take place
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
    public async Task<int> set_monthDays(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("monthDays",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the months scheduled for wake up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the months scheduled for wake up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.MONTHS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_months()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MONTHS_INVALID;
            }
        }
        res = _months;
        return res;
    }


    /**
     * <summary>
     *   Changes the months when a wake up must take place.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the months when a wake up must take place
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
    public async Task<int> set_months(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("months",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the date/time (seconds) of the next wake up occurence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the date/time (seconds) of the next wake up occurence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWakeUpSchedule.NEXTOCCURENCE_INVALID</c>.
     * </para>
     */
    public async Task<long> get_nextOccurence()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return NEXTOCCURENCE_INVALID;
            }
        }
        res = _nextOccurence;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a wake up schedule for a given identifier.
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
     *   This function does not require that the wake up schedule is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWakeUpSchedule.isOnline()</c> to test if the wake up schedule is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a wake up schedule by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the wake up schedule
     * </param>
     * <returns>
     *   a <c>YWakeUpSchedule</c> object allowing you to drive the wake up schedule.
     * </returns>
     */
    public static YWakeUpSchedule FindWakeUpSchedule(string func)
    {
        YWakeUpSchedule obj;
        obj = (YWakeUpSchedule) YFunction._FindFromCache("WakeUpSchedule", func);
        if (obj == null) {
            obj = new YWakeUpSchedule(func);
            YFunction._AddToCache("WakeUpSchedule",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a wake up schedule for a given identifier in a YAPI context.
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
     *   This function does not require that the wake up schedule is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWakeUpSchedule.isOnline()</c> to test if the wake up schedule is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a wake up schedule by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the wake up schedule
     * </param>
     * <returns>
     *   a <c>YWakeUpSchedule</c> object allowing you to drive the wake up schedule.
     * </returns>
     */
    public static YWakeUpSchedule FindWakeUpScheduleInContext(YAPIContext yctx,string func)
    {
        YWakeUpSchedule obj;
        obj = (YWakeUpSchedule) YFunction._FindFromCacheInContext(yctx,  "WakeUpSchedule", func);
        if (obj == null) {
            obj = new YWakeUpSchedule(yctx, func);
            YFunction._AddToCache("WakeUpSchedule",  func, obj);
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
        _valueCallbackWakeUpSchedule = callback;
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
        if (_valueCallbackWakeUpSchedule != null) {
            await _valueCallbackWakeUpSchedule(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Returns all the minutes of each hour that are scheduled for wake up.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<long> get_minutes()
    {
        long res;

        res = await this.get_minutesB();
        res = ((res) << (30));
        res = res + await this.get_minutesA();
        return res;
    }

    /**
     * <summary>
     *   Changes all the minutes where a wake up must take place.
     * <para>
     * </para>
     * </summary>
     * <param name="bitmap">
     *   Minutes 00-59 of each hour scheduled for wake up.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_minutes(long bitmap)
    {
        await this.set_minutesA((int)(((bitmap) & (0x3fffffff))));
        bitmap = ((bitmap) >> (30));
        return await this.set_minutesB((int)(((bitmap) & (0x3fffffff))));
    }

    /**
     * <summary>
     *   Continues the enumeration of wake up schedules started using <c>yFirstWakeUpSchedule()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWakeUpSchedule</c> object, corresponding to
     *   a wake up schedule currently online, or a <c>null</c> pointer
     *   if there are no more wake up schedules to enumerate.
     * </returns>
     */
    public YWakeUpSchedule nextWakeUpSchedule()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindWakeUpScheduleInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of wake up schedules currently accessible.
     * <para>
     *   Use the method <c>YWakeUpSchedule.nextWakeUpSchedule()</c> to iterate on
     *   next wake up schedules.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWakeUpSchedule</c> object, corresponding to
     *   the first wake up schedule currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWakeUpSchedule FirstWakeUpSchedule()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WakeUpSchedule");
        if (next_hwid == null)  return null;
        return FindWakeUpScheduleInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of wake up schedules currently accessible.
     * <para>
     *   Use the method <c>YWakeUpSchedule.nextWakeUpSchedule()</c> to iterate on
     *   next wake up schedules.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YWakeUpSchedule</c> object, corresponding to
     *   the first wake up schedule currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWakeUpSchedule FirstWakeUpScheduleInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WakeUpSchedule");
        if (next_hwid == null)  return null;
        return FindWakeUpScheduleInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YWakeUpSchedule implementation)
}
}

