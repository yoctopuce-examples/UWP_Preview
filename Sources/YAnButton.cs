/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindAnButton(), the high-level API for AnButton functions
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

//--- (YAnButton return codes)
//--- (end of YAnButton return codes)
//--- (YAnButton class start)
/**
 * <summary>
 *   YAnButton Class: AnButton function interface
 * <para>
 *   Yoctopuce application programming interface allows you to measure the state
 *   of a simple button as well as to read an analog potentiometer (variable resistance).
 *   This can be use for instance with a continuous rotating knob, a throttle grip
 *   or a joystick. The module is capable to calibrate itself on min and max values,
 *   in order to compute a calibrated value that varies proportionally with the
 *   potentiometer position, regardless of its total resistance.
 * </para>
 * </summary>
 */
public class YAnButton : YFunction
{
//--- (end of YAnButton class start)
//--- (YAnButton definitions)
    /**
     * <summary>
     *   invalid calibratedValue value
     * </summary>
     */
    public const  int CALIBRATEDVALUE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid rawValue value
     * </summary>
     */
    public const  int RAWVALUE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid analogCalibration value
     * </summary>
     */
    public const int ANALOGCALIBRATION_OFF = 0;
    public const int ANALOGCALIBRATION_ON = 1;
    public const int ANALOGCALIBRATION_INVALID = -1;
    /**
     * <summary>
     *   invalid calibrationMax value
     * </summary>
     */
    public const  int CALIBRATIONMAX_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid calibrationMin value
     * </summary>
     */
    public const  int CALIBRATIONMIN_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid sensitivity value
     * </summary>
     */
    public const  int SENSITIVITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid isPressed value
     * </summary>
     */
    public const int ISPRESSED_FALSE = 0;
    public const int ISPRESSED_TRUE = 1;
    public const int ISPRESSED_INVALID = -1;
    /**
     * <summary>
     *   invalid lastTimePressed value
     * </summary>
     */
    public const  long LASTTIMEPRESSED_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid lastTimeReleased value
     * </summary>
     */
    public const  long LASTTIMERELEASED_INVALID = YAPI.INVALID_LONG;
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
    protected int _calibratedValue = CALIBRATEDVALUE_INVALID;
    protected int _rawValue = RAWVALUE_INVALID;
    protected int _analogCalibration = ANALOGCALIBRATION_INVALID;
    protected int _calibrationMax = CALIBRATIONMAX_INVALID;
    protected int _calibrationMin = CALIBRATIONMIN_INVALID;
    protected int _sensitivity = SENSITIVITY_INVALID;
    protected int _isPressed = ISPRESSED_INVALID;
    protected long _lastTimePressed = LASTTIMEPRESSED_INVALID;
    protected long _lastTimeReleased = LASTTIMERELEASED_INVALID;
    protected long _pulseCounter = PULSECOUNTER_INVALID;
    protected long _pulseTimer = PULSETIMER_INVALID;
    protected ValueCallback _valueCallbackAnButton = null;

    public new delegate Task ValueCallback(YAnButton func, string value);
    public new delegate Task TimedReportCallback(YAnButton func, YMeasure measure);
    //--- (end of YAnButton definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAnButton(YAPIContext ctx, string func)
        : base(ctx, func, "AnButton")
    {
        //--- (YAnButton attributes initialization)
        //--- (end of YAnButton attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAnButton(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAnButton implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("calibratedValue")) {
            _calibratedValue = json_val.GetInt("calibratedValue");
        }
        if (json_val.Has("rawValue")) {
            _rawValue = json_val.GetInt("rawValue");
        }
        if (json_val.Has("analogCalibration")) {
            _analogCalibration = json_val.GetInt("analogCalibration") > 0 ? 1 : 0;
        }
        if (json_val.Has("calibrationMax")) {
            _calibrationMax = json_val.GetInt("calibrationMax");
        }
        if (json_val.Has("calibrationMin")) {
            _calibrationMin = json_val.GetInt("calibrationMin");
        }
        if (json_val.Has("sensitivity")) {
            _sensitivity = json_val.GetInt("sensitivity");
        }
        if (json_val.Has("isPressed")) {
            _isPressed = json_val.GetInt("isPressed") > 0 ? 1 : 0;
        }
        if (json_val.Has("lastTimePressed")) {
            _lastTimePressed = json_val.GetLong("lastTimePressed");
        }
        if (json_val.Has("lastTimeReleased")) {
            _lastTimeReleased = json_val.GetLong("lastTimeReleased");
        }
        if (json_val.Has("pulseCounter")) {
            _pulseCounter = json_val.GetLong("pulseCounter");
        }
        if (json_val.Has("pulseTimer")) {
            _pulseTimer = json_val.GetLong("pulseTimer");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current calibrated input value (between 0 and 1000, included).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current calibrated input value (between 0 and 1000, included)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.CALIBRATEDVALUE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_calibratedValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALIBRATEDVALUE_INVALID;
            }
        }
        return _calibratedValue;
    }


    /**
     * <summary>
     *   Returns the current measured input value as-is (between 0 and 4095, included).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current measured input value as-is (between 0 and 4095, included)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.RAWVALUE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rawValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RAWVALUE_INVALID;
            }
        }
        return _rawValue;
    }


    /**
     * <summary>
     *   Tells if a calibration process is currently ongoing.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YAnButton.ANALOGCALIBRATION_OFF</c> or <c>YAnButton.ANALOGCALIBRATION_ON</c>
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.ANALOGCALIBRATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_analogCalibration()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ANALOGCALIBRATION_INVALID;
            }
        }
        return _analogCalibration;
    }


    /**
     * <summary>
     *   Starts or stops the calibration process.
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module at the end of the calibration if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YAnButton.ANALOGCALIBRATION_OFF</c> or <c>YAnButton.ANALOGCALIBRATION_ON</c>
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
    public async Task<int> set_analogCalibration(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("analogCalibration",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximal value measured during the calibration (between 0 and 4095, included).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximal value measured during the calibration (between 0 and 4095, included)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.CALIBRATIONMAX_INVALID</c>.
     * </para>
     */
    public async Task<int> get_calibrationMax()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALIBRATIONMAX_INVALID;
            }
        }
        return _calibrationMax;
    }


    /**
     * <summary>
     *   Changes the maximal calibration value for the input (between 0 and 4095, included), without actually
     *   starting the automated calibration.
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the maximal calibration value for the input (between 0 and 4095,
     *   included), without actually
     *   starting the automated calibration
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
    public async Task<int> set_calibrationMax(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("calibrationMax",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the minimal value measured during the calibration (between 0 and 4095, included).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the minimal value measured during the calibration (between 0 and 4095, included)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.CALIBRATIONMIN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_calibrationMin()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALIBRATIONMIN_INVALID;
            }
        }
        return _calibrationMin;
    }


    /**
     * <summary>
     *   Changes the minimal calibration value for the input (between 0 and 4095, included), without actually
     *   starting the automated calibration.
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the minimal calibration value for the input (between 0 and 4095,
     *   included), without actually
     *   starting the automated calibration
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
    public async Task<int> set_calibrationMin(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("calibrationMin",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the sensibility for the input (between 1 and 1000) for triggering user callbacks.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the sensibility for the input (between 1 and 1000) for triggering user callbacks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.SENSITIVITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_sensitivity()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SENSITIVITY_INVALID;
            }
        }
        return _sensitivity;
    }


    /**
     * <summary>
     *   Changes the sensibility for the input (between 1 and 1000) for triggering user callbacks.
     * <para>
     *   The sensibility is used to filter variations around a fixed value, but does not preclude the
     *   transmission of events when the input value evolves constantly in the same direction.
     *   Special case: when the value 1000 is used, the callback will only be thrown when the logical state
     *   of the input switches from pressed to released and back.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the sensibility for the input (between 1 and 1000) for triggering user callbacks
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
    public async Task<int> set_sensitivity(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("sensitivity",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns true if the input (considered as binary) is active (closed contact), and false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YAnButton.ISPRESSED_FALSE</c> or <c>YAnButton.ISPRESSED_TRUE</c>, according to true if
     *   the input (considered as binary) is active (closed contact), and false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.ISPRESSED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_isPressed()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ISPRESSED_INVALID;
            }
        }
        return _isPressed;
    }


    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on and the last time
     *   the input button was pressed (the input contact transitioned from open to closed).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last time
     *   the input button was pressed (the input contact transitioned from open to closed)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.LASTTIMEPRESSED_INVALID</c>.
     * </para>
     */
    public async Task<long> get_lastTimePressed()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LASTTIMEPRESSED_INVALID;
            }
        }
        return _lastTimePressed;
    }


    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on and the last time
     *   the input button was released (the input contact transitioned from closed to open).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last time
     *   the input button was released (the input contact transitioned from closed to open)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAnButton.LASTTIMERELEASED_INVALID</c>.
     * </para>
     */
    public async Task<long> get_lastTimeReleased()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LASTTIMERELEASED_INVALID;
            }
        }
        return _lastTimeReleased;
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
     *   On failure, throws an exception or returns <c>YAnButton.PULSECOUNTER_INVALID</c>.
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
     *   On failure, throws an exception or returns <c>YAnButton.PULSETIMER_INVALID</c>.
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
     *   Retrieves an analog input for a given identifier.
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
     *   This function does not require that the analog input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAnButton.isOnline()</c> to test if the analog input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an analog input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the analog input
     * </param>
     * <returns>
     *   a <c>YAnButton</c> object allowing you to drive the analog input.
     * </returns>
     */
    public static YAnButton FindAnButton(string func)
    {
        YAnButton obj;
        obj = (YAnButton) YFunction._FindFromCache("AnButton", func);
        if (obj == null) {
            obj = new YAnButton(func);
            YFunction._AddToCache("AnButton",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an analog input for a given identifier in a YAPI context.
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
     *   This function does not require that the analog input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAnButton.isOnline()</c> to test if the analog input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an analog input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the analog input
     * </param>
     * <returns>
     *   a <c>YAnButton</c> object allowing you to drive the analog input.
     * </returns>
     */
    public static YAnButton FindAnButtonInContext(YAPIContext yctx,string func)
    {
        YAnButton obj;
        obj = (YAnButton) YFunction._FindFromCacheInContext(yctx,  "AnButton", func);
        if (obj == null) {
            obj = new YAnButton(yctx, func);
            YFunction._AddToCache("AnButton",  func, obj);
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
        _valueCallbackAnButton = callback;
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
        if (_valueCallbackAnButton != null) {
            await _valueCallbackAnButton(this, value);
        } else {
            await base._invokeValueCallback(value);
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
     *   Continues the enumeration of analog inputs started using <c>yFirstAnButton()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAnButton</c> object, corresponding to
     *   an analog input currently online, or a <c>null</c> pointer
     *   if there are no more analog inputs to enumerate.
     * </returns>
     */
    public YAnButton nextAnButton()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAnButtonInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of analog inputs currently accessible.
     * <para>
     *   Use the method <c>YAnButton.nextAnButton()</c> to iterate on
     *   next analog inputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAnButton</c> object, corresponding to
     *   the first analog input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAnButton FirstAnButton()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AnButton");
        if (next_hwid == null)  return null;
        return FindAnButtonInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of analog inputs currently accessible.
     * <para>
     *   Use the method <c>YAnButton.nextAnButton()</c> to iterate on
     *   next analog inputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAnButton</c> object, corresponding to
     *   the first analog input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAnButton FirstAnButtonInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AnButton");
        if (next_hwid == null)  return null;
        return FindAnButtonInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAnButton implementation)
}
}

