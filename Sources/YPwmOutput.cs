/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindPwmOutput(), the high-level API for PwmOutput functions
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

//--- (YPwmOutput return codes)
//--- (end of YPwmOutput return codes)
//--- (YPwmOutput class start)
/**
 * <summary>
 *   YPwmOutput Class: PwmOutput function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to configure, start, and stop the PWM.
 * </para>
 * </summary>
 */
public class YPwmOutput : YFunction
{
//--- (end of YPwmOutput class start)
//--- (YPwmOutput definitions)
    /**
     * <summary>
     *   invalid enabled value
     * </summary>
     */
    public const int ENABLED_FALSE = 0;
    public const int ENABLED_TRUE = 1;
    public const int ENABLED_INVALID = -1;
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
     *   invalid pwmTransition value
     * </summary>
     */
    public const  string PWMTRANSITION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid enabledAtPowerOn value
     * </summary>
     */
    public const int ENABLEDATPOWERON_FALSE = 0;
    public const int ENABLEDATPOWERON_TRUE = 1;
    public const int ENABLEDATPOWERON_INVALID = -1;
    /**
     * <summary>
     *   invalid dutyCycleAtPowerOn value
     * </summary>
     */
    public const  double DUTYCYCLEATPOWERON_INVALID = YAPI.INVALID_DOUBLE;
    protected int _enabled = ENABLED_INVALID;
    protected double _frequency = FREQUENCY_INVALID;
    protected double _period = PERIOD_INVALID;
    protected double _dutyCycle = DUTYCYCLE_INVALID;
    protected double _pulseDuration = PULSEDURATION_INVALID;
    protected string _pwmTransition = PWMTRANSITION_INVALID;
    protected int _enabledAtPowerOn = ENABLEDATPOWERON_INVALID;
    protected double _dutyCycleAtPowerOn = DUTYCYCLEATPOWERON_INVALID;
    protected ValueCallback _valueCallbackPwmOutput = null;

    public new delegate Task ValueCallback(YPwmOutput func, string value);
    public new delegate Task TimedReportCallback(YPwmOutput func, YMeasure measure);
    //--- (end of YPwmOutput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmOutput(YAPIContext ctx, string func)
        : base(ctx, func, "PwmOutput")
    {
        //--- (YPwmOutput attributes initialization)
        //--- (end of YPwmOutput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmOutput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPwmOutput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("enabled")) {
            _enabled = json_val.GetInt("enabled") > 0 ? 1 : 0;
        }
        if (json_val.Has("frequency")) {
            _frequency = Math.Round(json_val.GetDouble("frequency") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("period")) {
            _period = Math.Round(json_val.GetDouble("period") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("dutyCycle")) {
            _dutyCycle = Math.Round(json_val.GetDouble("dutyCycle") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("pulseDuration")) {
            _pulseDuration = Math.Round(json_val.GetDouble("pulseDuration") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("pwmTransition")) {
            _pwmTransition = json_val.GetString("pwmTransition");
        }
        if (json_val.Has("enabledAtPowerOn")) {
            _enabledAtPowerOn = json_val.GetInt("enabledAtPowerOn") > 0 ? 1 : 0;
        }
        if (json_val.Has("dutyCycleAtPowerOn")) {
            _dutyCycleAtPowerOn = Math.Round(json_val.GetDouble("dutyCycleAtPowerOn") * 1000.0 / 65536.0) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the state of the PWMs.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPwmOutput.ENABLED_FALSE</c> or <c>YPwmOutput.ENABLED_TRUE</c>, according to the state of the PWMs
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.ENABLED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_enabled()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ENABLED_INVALID;
            }
        }
        return _enabled;
    }


    /**
     * <summary>
     *   Stops or starts the PWM.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPwmOutput.ENABLED_FALSE</c> or <c>YPwmOutput.ENABLED_TRUE</c>
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
    public async Task<int> set_enabled(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("enabled",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the PWM frequency.
     * <para>
     *   The duty cycle is kept unchanged thanks to an
     *   automatic pulse width change.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM frequency
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
    public async Task<int> set_frequency(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("frequency",rest_val);
        return YAPI.SUCCESS;
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
     *   On failure, throws an exception or returns <c>YPwmOutput.FREQUENCY_INVALID</c>.
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
     *   Changes the PWM period in milliseconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM period in milliseconds
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
    public async Task<int> set_period(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("period",rest_val);
        return YAPI.SUCCESS;
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
     *   On failure, throws an exception or returns <c>YPwmOutput.PERIOD_INVALID</c>.
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
     *   Changes the PWM duty cycle, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM duty cycle, in per cents
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
    public async Task<int> set_dutyCycle(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("dutyCycle",rest_val);
        return YAPI.SUCCESS;
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
     *   On failure, throws an exception or returns <c>YPwmOutput.DUTYCYCLE_INVALID</c>.
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
     *   Changes the PWM pulse length, in milliseconds.
     * <para>
     *   A pulse length cannot be longer than period, otherwise it is truncated.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM pulse length, in milliseconds
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
    public async Task<int> set_pulseDuration(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("pulseDuration",rest_val);
        return YAPI.SUCCESS;
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
     *   On failure, throws an exception or returns <c>YPwmOutput.PULSEDURATION_INVALID</c>.
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
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_pwmTransition()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PWMTRANSITION_INVALID;
            }
        }
        return _pwmTransition;
    }


    public async Task<int> set_pwmTransition(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("pwmTransition",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the PWM at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPwmOutput.ENABLEDATPOWERON_FALSE</c> or <c>YPwmOutput.ENABLEDATPOWERON_TRUE</c>,
     *   according to the state of the PWM at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.ENABLEDATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_enabledAtPowerOn()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ENABLEDATPOWERON_INVALID;
            }
        }
        return _enabledAtPowerOn;
    }


    /**
     * <summary>
     *   Changes the state of the PWM at device power on.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPwmOutput.ENABLEDATPOWERON_FALSE</c> or <c>YPwmOutput.ENABLEDATPOWERON_TRUE</c>,
     *   according to the state of the PWM at device power on
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
    public async Task<int> set_enabledAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("enabledAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the PWM duty cycle at device power on.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM duty cycle at device power on
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
    public async Task<int> set_dutyCycleAtPowerOn(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("dutyCycleAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWMs duty cycle at device power on as a floating point number between 0 and 100.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWMs duty cycle at device power on as a floating point
     *   number between 0 and 100
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.DUTYCYCLEATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<double> get_dutyCycleAtPowerOn()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DUTYCYCLEATPOWERON_INVALID;
            }
        }
        return _dutyCycleAtPowerOn;
    }


    /**
     * <summary>
     *   Retrieves a PWM for a given identifier.
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
     *   This function does not require that the PWM is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmOutput.isOnline()</c> to test if the PWM is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the PWM
     * </param>
     * <returns>
     *   a <c>YPwmOutput</c> object allowing you to drive the PWM.
     * </returns>
     */
    public static YPwmOutput FindPwmOutput(string func)
    {
        YPwmOutput obj;
        obj = (YPwmOutput) YFunction._FindFromCache("PwmOutput", func);
        if (obj == null) {
            obj = new YPwmOutput(func);
            YFunction._AddToCache("PwmOutput",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a PWM for a given identifier in a YAPI context.
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
     *   This function does not require that the PWM is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmOutput.isOnline()</c> to test if the PWM is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the PWM
     * </param>
     * <returns>
     *   a <c>YPwmOutput</c> object allowing you to drive the PWM.
     * </returns>
     */
    public static YPwmOutput FindPwmOutputInContext(YAPIContext yctx,string func)
    {
        YPwmOutput obj;
        obj = (YPwmOutput) YFunction._FindFromCacheInContext(yctx,  "PwmOutput", func);
        if (obj == null) {
            obj = new YPwmOutput(yctx, func);
            YFunction._AddToCache("PwmOutput",  func, obj);
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
        _valueCallbackPwmOutput = callback;
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
        if (_valueCallbackPwmOutput != null) {
            await _valueCallbackPwmOutput(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Performs a smooth transistion of the pulse duration toward a given value.
     * <para>
     *   Any period,
     *   frequency, duty cycle or pulse width change will cancel any ongoing transition process.
     * </para>
     * </summary>
     * <param name="ms_target">
     *   new pulse duration at the end of the transition
     *   (floating-point number, representing the pulse duration in milliseconds)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> pulseDurationMove(double ms_target,int ms_duration)
    {
        string newval;
        if (ms_target < 0.0) {
            ms_target = 0.0;
        }
        newval = ""+Convert.ToString( (int) Math.Round(ms_target*65536))+"ms:"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Performs a smooth change of the pulse duration toward a given value.
     * <para>
     * </para>
     * </summary>
     * <param name="target">
     *   new duty cycle at the end of the transition
     *   (floating-point number, between 0 and 1)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> dutyCycleMove(double target,int ms_duration)
    {
        string newval;
        if (target < 0.0) {
            target = 0.0;
        }
        if (target > 100.0) {
            target = 100.0;
        }
        newval = ""+Convert.ToString( (int) Math.Round(target*65536))+":"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Continues the enumeration of PWMs started using <c>yFirstPwmOutput()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   a PWM currently online, or a <c>null</c> pointer
     *   if there are no more PWMs to enumerate.
     * </returns>
     */
    public YPwmOutput nextPwmOutput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPwmOutputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWMs currently accessible.
     * <para>
     *   Use the method <c>YPwmOutput.nextPwmOutput()</c> to iterate on
     *   next PWMs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   the first PWM currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmOutput FirstPwmOutput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmOutput");
        if (next_hwid == null)  return null;
        return FindPwmOutputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWMs currently accessible.
     * <para>
     *   Use the method <c>YPwmOutput.nextPwmOutput()</c> to iterate on
     *   next PWMs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   the first PWM currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmOutput FirstPwmOutputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmOutput");
        if (next_hwid == null)  return null;
        return FindPwmOutputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPwmOutput implementation)
}
}

