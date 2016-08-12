/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindPwmInput(), the high-level API for PwmInput functions
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

//--- (YPwmInput return codes)
//--- (end of YPwmInput return codes)
//--- (YPwmInput class start)
/**
 * <summary>
 *   YPwmInput Class: PwmInput function interface
 * <para>
 *   The Yoctopuce class YPwmInput allows you to read and configure Yoctopuce PWM
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   register callback functions, access to the autonomous datalogger.
 *   This class adds the ability to configure the signal parameter used to transmit
 *   information: the duty cycle, the frequency or the pulse width.
 * </para>
 * </summary>
 */
public class YPwmInput : YSensor
{
//--- (end of YPwmInput class start)
//--- (YPwmInput definitions)
    /**
     * <summary>
     *   invalid dutyCycle value
     * </summary>
     */
    public const  double DUTYCYCLE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid pulseDuration value
     * </summary>
     */
    public const  double PULSEDURATION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid frequency value
     * </summary>
     */
    public const  double FREQUENCY_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid period value
     * </summary>
     */
    public const  double PERIOD_INVALID = YAPI.INVALID_DOUBLE;
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
     *   invalid pwmReportMode value
     * </summary>
     */
    public const int PWMREPORTMODE_PWM_DUTYCYCLE = 0;
    public const int PWMREPORTMODE_PWM_FREQUENCY = 1;
    public const int PWMREPORTMODE_PWM_PULSEDURATION = 2;
    public const int PWMREPORTMODE_PWM_EDGECOUNT = 3;
    public const int PWMREPORTMODE_INVALID = -1;
    protected double _dutyCycle = DUTYCYCLE_INVALID;
    protected double _pulseDuration = PULSEDURATION_INVALID;
    protected double _frequency = FREQUENCY_INVALID;
    protected double _period = PERIOD_INVALID;
    protected long _pulseCounter = PULSECOUNTER_INVALID;
    protected long _pulseTimer = PULSETIMER_INVALID;
    protected int _pwmReportMode = PWMREPORTMODE_INVALID;
    protected ValueCallback _valueCallbackPwmInput = null;
    protected TimedReportCallback _timedReportCallbackPwmInput = null;

    public new delegate Task ValueCallback(YPwmInput func, string value);
    public new delegate Task TimedReportCallback(YPwmInput func, YMeasure measure);
    //--- (end of YPwmInput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmInput(YAPIContext ctx, string func)
        : base(ctx, func, "PwmInput")
    {
        //--- (YPwmInput attributes initialization)
        //--- (end of YPwmInput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmInput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPwmInput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("dutyCycle")) {
            _dutyCycle = Math.Round(json_val.GetDouble("dutyCycle") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("pulseDuration")) {
            _pulseDuration = Math.Round(json_val.GetDouble("pulseDuration") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("frequency")) {
            _frequency = Math.Round(json_val.GetDouble("frequency") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("period")) {
            _period = Math.Round(json_val.GetDouble("period") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("pulseCounter")) {
            _pulseCounter = json_val.GetLong("pulseCounter");
        }
        if (json_val.Has("pulseTimer")) {
            _pulseTimer = json_val.GetLong("pulseTimer");
        }
        if (json_val.Has("pwmReportMode")) {
            _pwmReportMode = json_val.GetInt("pwmReportMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the PWM duty cycle, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM duty cycle, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.DUTYCYCLE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_dutyCycle()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DUTYCYCLE_INVALID;
            }
        }
        return _dutyCycle;
    }


    /**
     * <summary>
     *   Returns the PWM pulse length in milliseconds, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM pulse length in milliseconds, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PULSEDURATION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_pulseDuration()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULSEDURATION_INVALID;
            }
        }
        return _pulseDuration;
    }


    /**
     * <summary>
     *   Returns the PWM frequency in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM frequency in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.FREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<double> get_frequency()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FREQUENCY_INVALID;
            }
        }
        return _frequency;
    }


    /**
     * <summary>
     *   Returns the PWM period in milliseconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM period in milliseconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PERIOD_INVALID</c>.
     * </para>
     */
    public async Task<double> get_period()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PERIOD_INVALID;
            }
        }
        return _period;
    }


    /**
     * <summary>
     *   Returns the pulse counter value.
     * <para>
     *   Actually that
     *   counter is incremented twice per period. That counter is
     *   limited  to 1 billion
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the pulse counter value
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PULSECOUNTER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseCounter()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULSECOUNTER_INVALID;
            }
        }
        return _pulseCounter;
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
     *   Returns the timer of the pulses counter (ms).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the timer of the pulses counter (ms)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PULSETIMER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseTimer()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULSETIMER_INVALID;
            }
        }
        return _pulseTimer;
    }


    /**
     * <summary>
     *   Returns the parameter (frequency/duty cycle, pulse width, edges count) returned by the get_currentValue function and callbacks.
     * <para>
     *   Attention
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YPwmInput.PWMREPORTMODE_PWM_DUTYCYCLE</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQUENCY</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSEDURATION</c> and <c>YPwmInput.PWMREPORTMODE_PWM_EDGECOUNT</c>
     *   corresponding to the parameter (frequency/duty cycle, pulse width, edges count) returned by the
     *   get_currentValue function and callbacks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PWMREPORTMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_pwmReportMode()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PWMREPORTMODE_INVALID;
            }
        }
        return _pwmReportMode;
    }


    /**
     * <summary>
     *   Modifies the  parameter  type (frequency/duty cycle, pulse width, or edge count) returned by the get_currentValue function and callbacks.
     * <para>
     *   The edge count value is limited to the 6 lowest digits. For values greater than one million, use
     *   get_pulseCounter().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YPwmInput.PWMREPORTMODE_PWM_DUTYCYCLE</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQUENCY</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSEDURATION</c> and <c>YPwmInput.PWMREPORTMODE_PWM_EDGECOUNT</c>
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
    public async Task<int> set_pwmReportMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pwmReportMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a PWM input for a given identifier.
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
     *   This function does not require that the PWM input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmInput.isOnline()</c> to test if the PWM input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the PWM input
     * </param>
     * <returns>
     *   a <c>YPwmInput</c> object allowing you to drive the PWM input.
     * </returns>
     */
    public static YPwmInput FindPwmInput(string func)
    {
        YPwmInput obj;
        obj = (YPwmInput) YFunction._FindFromCache("PwmInput", func);
        if (obj == null) {
            obj = new YPwmInput(func);
            YFunction._AddToCache("PwmInput",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a PWM input for a given identifier in a YAPI context.
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
     *   This function does not require that the PWM input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmInput.isOnline()</c> to test if the PWM input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the PWM input
     * </param>
     * <returns>
     *   a <c>YPwmInput</c> object allowing you to drive the PWM input.
     * </returns>
     */
    public static YPwmInput FindPwmInputInContext(YAPIContext yctx,string func)
    {
        YPwmInput obj;
        obj = (YPwmInput) YFunction._FindFromCacheInContext(yctx,  "PwmInput", func);
        if (obj == null) {
            obj = new YPwmInput(yctx, func);
            YFunction._AddToCache("PwmInput",  func, obj);
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
        _valueCallbackPwmInput = callback;
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
        if (_valueCallbackPwmInput != null) {
            await _valueCallbackPwmInput(this, value);
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
        _timedReportCallbackPwmInput = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackPwmInput != null) {
            await _timedReportCallbackPwmInput(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Returns the pulse counter value as well as its timer.
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
     *   Continues the enumeration of PWM inputs started using <c>yFirstPwmInput()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   a PWM input currently online, or a <c>null</c> pointer
     *   if there are no more PWM inputs to enumerate.
     * </returns>
     */
    public YPwmInput nextPwmInput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPwmInputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM inputs currently accessible.
     * <para>
     *   Use the method <c>YPwmInput.nextPwmInput()</c> to iterate on
     *   next PWM inputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   the first PWM input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmInput FirstPwmInput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmInput");
        if (next_hwid == null)  return null;
        return FindPwmInputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM inputs currently accessible.
     * <para>
     *   Use the method <c>YPwmInput.nextPwmInput()</c> to iterate on
     *   next PWM inputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   the first PWM input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmInput FirstPwmInputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmInput");
        if (next_hwid == null)  return null;
        return FindPwmInputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPwmInput implementation)
}
}

