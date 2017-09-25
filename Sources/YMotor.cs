/*********************************************************************
 *
 * $Id: YMotor.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindMotor(), the high-level API for Motor functions
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

//--- (YMotor return codes)
//--- (end of YMotor return codes)
//--- (YMotor class start)
/**
 * <summary>
 *   YMotor Class: Motor function interface
 * <para>
 *   Yoctopuce application programming interface allows you to drive the
 *   power sent to the motor to make it turn both ways, but also to drive accelerations
 *   and decelerations. The motor will then accelerate automatically: you will not
 *   have to monitor it. The API also allows to slow down the motor by shortening
 *   its terminals: the motor will then act as an electromagnetic brake.
 * </para>
 * </summary>
 */
public class YMotor : YFunction
{
//--- (end of YMotor class start)
//--- (YMotor definitions)
    /**
     * <summary>
     *   invalid motorStatus value
     * </summary>
     */
    public const int MOTORSTATUS_IDLE = 0;
    public const int MOTORSTATUS_BRAKE = 1;
    public const int MOTORSTATUS_FORWD = 2;
    public const int MOTORSTATUS_BACKWD = 3;
    public const int MOTORSTATUS_LOVOLT = 4;
    public const int MOTORSTATUS_HICURR = 5;
    public const int MOTORSTATUS_HIHEAT = 6;
    public const int MOTORSTATUS_FAILSF = 7;
    public const int MOTORSTATUS_INVALID = -1;
    /**
     * <summary>
     *   invalid drivingForce value
     * </summary>
     */
    public const  double DRIVINGFORCE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid brakingForce value
     * </summary>
     */
    public const  double BRAKINGFORCE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid cutOffVoltage value
     * </summary>
     */
    public const  double CUTOFFVOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid overCurrentLimit value
     * </summary>
     */
    public const  int OVERCURRENTLIMIT_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid frequency value
     * </summary>
     */
    public const  double FREQUENCY_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid starterTime value
     * </summary>
     */
    public const  int STARTERTIME_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid failSafeTimeout value
     * </summary>
     */
    public const  int FAILSAFETIMEOUT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _motorStatus = MOTORSTATUS_INVALID;
    protected double _drivingForce = DRIVINGFORCE_INVALID;
    protected double _brakingForce = BRAKINGFORCE_INVALID;
    protected double _cutOffVoltage = CUTOFFVOLTAGE_INVALID;
    protected int _overCurrentLimit = OVERCURRENTLIMIT_INVALID;
    protected double _frequency = FREQUENCY_INVALID;
    protected int _starterTime = STARTERTIME_INVALID;
    protected int _failSafeTimeout = FAILSAFETIMEOUT_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackMotor = null;

    public new delegate Task ValueCallback(YMotor func, string value);
    public new delegate Task TimedReportCallback(YMotor func, YMeasure measure);
    //--- (end of YMotor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMotor(YAPIContext ctx, string func)
        : base(ctx, func, "Motor")
    {
        //--- (YMotor attributes initialization)
        //--- (end of YMotor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMotor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YMotor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("motorStatus")) {
            _motorStatus = json_val.GetInt("motorStatus");
        }
        if (json_val.Has("drivingForce")) {
            _drivingForce = Math.Round(json_val.GetDouble("drivingForce") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("brakingForce")) {
            _brakingForce = Math.Round(json_val.GetDouble("brakingForce") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("cutOffVoltage")) {
            _cutOffVoltage = Math.Round(json_val.GetDouble("cutOffVoltage") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("overCurrentLimit")) {
            _overCurrentLimit = json_val.GetInt("overCurrentLimit");
        }
        if (json_val.Has("frequency")) {
            _frequency = Math.Round(json_val.GetDouble("frequency") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("starterTime")) {
            _starterTime = json_val.GetInt("starterTime");
        }
        if (json_val.Has("failSafeTimeout")) {
            _failSafeTimeout = json_val.GetInt("failSafeTimeout");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Return the controller state.
     * <para>
     *   Possible states are:
     *   IDLE   when the motor is stopped/in free wheel, ready to start;
     *   FORWD  when the controller is driving the motor forward;
     *   BACKWD when the controller is driving the motor backward;
     *   BRAKE  when the controller is braking;
     *   LOVOLT when the controller has detected a low voltage condition;
     *   HICURR when the controller has detected an overcurrent condition;
     *   HIHEAT when the controller has detected an overheat condition;
     *   FAILSF when the controller switched on the failsafe security.
     * </para>
     * <para>
     *   When an error condition occurred (LOVOLT, HICURR, HIHEAT, FAILSF), the controller
     *   status must be explicitly reset using the <c>resetStatus</c> function.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YMotor.MOTORSTATUS_IDLE</c>, <c>YMotor.MOTORSTATUS_BRAKE</c>,
     *   <c>YMotor.MOTORSTATUS_FORWD</c>, <c>YMotor.MOTORSTATUS_BACKWD</c>,
     *   <c>YMotor.MOTORSTATUS_LOVOLT</c>, <c>YMotor.MOTORSTATUS_HICURR</c>,
     *   <c>YMotor.MOTORSTATUS_HIHEAT</c> and <c>YMotor.MOTORSTATUS_FAILSF</c>
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.MOTORSTATUS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_motorStatus()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MOTORSTATUS_INVALID;
            }
        }
        res = _motorStatus;
        return res;
    }


    public async Task<int> set_motorStatus(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("motorStatus",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes immediately the power sent to the motor.
     * <para>
     *   The value is a percentage between -100%
     *   to 100%. If you want go easy on your mechanics and avoid excessive current consumption,
     *   try to avoid brutal power changes. For example, immediate transition from forward full power
     *   to reverse full power is a very bad idea. Each time the driving power is modified, the
     *   braking power is set to zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to immediately the power sent to the motor
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
    public async Task<int> set_drivingForce(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("drivingForce",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the power sent to the motor, as a percentage between -100% and +100%.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the power sent to the motor, as a percentage between -100% and +100%
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.DRIVINGFORCE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_drivingForce()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DRIVINGFORCE_INVALID;
            }
        }
        res = _drivingForce;
        return res;
    }


    /**
     * <summary>
     *   Changes immediately the braking force applied to the motor (in percents).
     * <para>
     *   The value 0 corresponds to no braking (free wheel). When the braking force
     *   is changed, the driving power is set to zero. The value is a percentage.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to immediately the braking force applied to the motor (in percents)
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
    public async Task<int> set_brakingForce(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("brakingForce",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the braking force applied to the motor, as a percentage.
     * <para>
     *   The value 0 corresponds to no braking (free wheel).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the braking force applied to the motor, as a percentage
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.BRAKINGFORCE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_brakingForce()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BRAKINGFORCE_INVALID;
            }
        }
        res = _brakingForce;
        return res;
    }


    /**
     * <summary>
     *   Changes the threshold voltage under which the controller automatically switches to error state
     *   and prevents further current draw.
     * <para>
     *   This setting prevent damage to a battery that can
     *   occur when drawing current from an "empty" battery.
     *   Note that whatever the cutoff threshold, the controller switches to undervoltage
     *   error state if the power supply goes under 3V, even for a very brief time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the threshold voltage under which the controller
     *   automatically switches to error state
     *   and prevents further current draw
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
    public async Task<int> set_cutOffVoltage(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("cutOffVoltage",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the threshold voltage under which the controller automatically switches to error state
     *   and prevents further current draw.
     * <para>
     *   This setting prevents damage to a battery that can
     *   occur when drawing current from an "empty" battery.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the threshold voltage under which the controller
     *   automatically switches to error state
     *   and prevents further current draw
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.CUTOFFVOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_cutOffVoltage()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CUTOFFVOLTAGE_INVALID;
            }
        }
        res = _cutOffVoltage;
        return res;
    }


    /**
     * <summary>
     *   Returns the current threshold (in mA) above which the controller automatically
     *   switches to error state.
     * <para>
     *   A zero value means that there is no limit.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current threshold (in mA) above which the controller automatically
     *   switches to error state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.OVERCURRENTLIMIT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_overCurrentLimit()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return OVERCURRENTLIMIT_INVALID;
            }
        }
        res = _overCurrentLimit;
        return res;
    }


    /**
     * <summary>
     *   Changes the current threshold (in mA) above which the controller automatically
     *   switches to error state.
     * <para>
     *   A zero value means that there is no limit. Note that whatever the
     *   current limit is, the controller switches to OVERCURRENT status if the current
     *   goes above 32A, even for a very brief time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current threshold (in mA) above which the controller automatically
     *   switches to error state
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
    public async Task<int> set_overCurrentLimit(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("overCurrentLimit",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the PWM frequency used to control the motor.
     * <para>
     *   Low frequency is usually
     *   more efficient and may help the motor to start, but an audible noise might be
     *   generated. A higher frequency reduces the noise, but more energy is converted
     *   into heat.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM frequency used to control the motor
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
     *   Returns the PWM frequency used to control the motor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM frequency used to control the motor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.FREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<double> get_frequency()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FREQUENCY_INVALID;
            }
        }
        res = _frequency;
        return res;
    }


    /**
     * <summary>
     *   Returns the duration (in ms) during which the motor is driven at low frequency to help
     *   it start up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the duration (in ms) during which the motor is driven at low frequency to help
     *   it start up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.STARTERTIME_INVALID</c>.
     * </para>
     */
    public async Task<int> get_starterTime()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STARTERTIME_INVALID;
            }
        }
        res = _starterTime;
        return res;
    }


    /**
     * <summary>
     *   Changes the duration (in ms) during which the motor is driven at low frequency to help
     *   it start up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the duration (in ms) during which the motor is driven at low frequency to help
     *   it start up
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
    public async Task<int> set_starterTime(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("starterTime",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the delay in milliseconds allowed for the controller to run autonomously without
     *   receiving any instruction from the control process.
     * <para>
     *   When this delay has elapsed,
     *   the controller automatically stops the motor and switches to FAILSAFE error.
     *   Failsafe security is disabled when the value is zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the delay in milliseconds allowed for the controller to run autonomously without
     *   receiving any instruction from the control process
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMotor.FAILSAFETIMEOUT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_failSafeTimeout()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FAILSAFETIMEOUT_INVALID;
            }
        }
        res = _failSafeTimeout;
        return res;
    }


    /**
     * <summary>
     *   Changes the delay in milliseconds allowed for the controller to run autonomously without
     *   receiving any instruction from the control process.
     * <para>
     *   When this delay has elapsed,
     *   the controller automatically stops the motor and switches to FAILSAFE error.
     *   Failsafe security is disabled when the value is zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the delay in milliseconds allowed for the controller to run autonomously without
     *   receiving any instruction from the control process
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
    public async Task<int> set_failSafeTimeout(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("failSafeTimeout",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        res = _command;
        return res;
    }


    public async Task<int> set_command(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("command",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a motor for a given identifier.
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
     *   This function does not require that the motor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMotor.isOnline()</c> to test if the motor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a motor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the motor
     * </param>
     * <returns>
     *   a <c>YMotor</c> object allowing you to drive the motor.
     * </returns>
     */
    public static YMotor FindMotor(string func)
    {
        YMotor obj;
        obj = (YMotor) YFunction._FindFromCache("Motor", func);
        if (obj == null) {
            obj = new YMotor(func);
            YFunction._AddToCache("Motor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a motor for a given identifier in a YAPI context.
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
     *   This function does not require that the motor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMotor.isOnline()</c> to test if the motor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a motor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the motor
     * </param>
     * <returns>
     *   a <c>YMotor</c> object allowing you to drive the motor.
     * </returns>
     */
    public static YMotor FindMotorInContext(YAPIContext yctx,string func)
    {
        YMotor obj;
        obj = (YMotor) YFunction._FindFromCacheInContext(yctx,  "Motor", func);
        if (obj == null) {
            obj = new YMotor(yctx, func);
            YFunction._AddToCache("Motor",  func, obj);
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
        _valueCallbackMotor = callback;
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
        if (_valueCallbackMotor != null) {
            await _valueCallbackMotor(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Rearms the controller failsafe timer.
     * <para>
     *   When the motor is running and the failsafe feature
     *   is active, this function should be called periodically to prove that the control process
     *   is running properly. Otherwise, the motor is automatically stopped after the specified
     *   timeout. Calling a motor <i>set</i> function implicitely rearms the failsafe timer.
     * </para>
     * </summary>
     */
    public virtual async Task<int> keepALive()
    {
        return await this.set_command("K");
    }

    /**
     * <summary>
     *   Reset the controller state to IDLE.
     * <para>
     *   This function must be invoked explicitely
     *   after any error condition is signaled.
     * </para>
     * </summary>
     */
    public virtual async Task<int> resetStatus()
    {
        return await this.set_motorStatus(MOTORSTATUS_IDLE);
    }

    /**
     * <summary>
     *   Changes progressively the power sent to the moteur for a specific duration.
     * <para>
     * </para>
     * </summary>
     * <param name="targetPower">
     *   desired motor power, in percents (between -100% and +100%)
     * </param>
     * <param name="delay">
     *   duration (in ms) of the transition
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drivingForceMove(double targetPower,int delay)
    {
        return await this.set_command("P"+Convert.ToString((int) Math.Round(targetPower*10))+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Changes progressively the braking force applied to the motor for a specific duration.
     * <para>
     * </para>
     * </summary>
     * <param name="targetPower">
     *   desired braking force, in percents
     * </param>
     * <param name="delay">
     *   duration (in ms) of the transition
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> brakingForceMove(double targetPower,int delay)
    {
        return await this.set_command("B"+Convert.ToString((int) Math.Round(targetPower*10))+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Continues the enumeration of motors started using <c>yFirstMotor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMotor</c> object, corresponding to
     *   a motor currently online, or a <c>null</c> pointer
     *   if there are no more motors to enumerate.
     * </returns>
     */
    public YMotor nextMotor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindMotorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of motors currently accessible.
     * <para>
     *   Use the method <c>YMotor.nextMotor()</c> to iterate on
     *   next motors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMotor</c> object, corresponding to
     *   the first motor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMotor FirstMotor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Motor");
        if (next_hwid == null)  return null;
        return FindMotorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of motors currently accessible.
     * <para>
     *   Use the method <c>YMotor.nextMotor()</c> to iterate on
     *   next motors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YMotor</c> object, corresponding to
     *   the first motor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMotor FirstMotorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Motor");
        if (next_hwid == null)  return null;
        return FindMotorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YMotor implementation)
}
}

