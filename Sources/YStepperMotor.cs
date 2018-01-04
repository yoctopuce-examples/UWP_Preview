/*********************************************************************
 *
 * $Id: YStepperMotor.cs 29507 2017-12-28 14:14:56Z mvuilleu $
 *
 * Implements FindStepperMotor(), the high-level API for StepperMotor functions
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

//--- (YStepperMotor return codes)
//--- (end of YStepperMotor return codes)
//--- (YStepperMotor class start)
/**
 * <summary>
 *   YStepperMotor Class: StepperMotor function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to drive a stepper motor.
 * </para>
 * </summary>
 */
public class YStepperMotor : YFunction
{
//--- (end of YStepperMotor class start)
//--- (YStepperMotor definitions)
    /**
     * <summary>
     *   invalid motorState value
     * </summary>
     */
    public const int MOTORSTATE_ABSENT = 0;
    public const int MOTORSTATE_ALERT = 1;
    public const int MOTORSTATE_HI_Z = 2;
    public const int MOTORSTATE_STOP = 3;
    public const int MOTORSTATE_RUN = 4;
    public const int MOTORSTATE_BATCH = 5;
    public const int MOTORSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid diags value
     * </summary>
     */
    public const  int DIAGS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid stepPos value
     * </summary>
     */
    public const  double STEPPOS_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid speed value
     * </summary>
     */
    public const  double SPEED_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid pullinSpeed value
     * </summary>
     */
    public const  double PULLINSPEED_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid maxAccel value
     * </summary>
     */
    public const  double MAXACCEL_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid maxSpeed value
     * </summary>
     */
    public const  double MAXSPEED_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid stepping value
     * </summary>
     */
    public const int STEPPING_MICROSTEP16 = 0;
    public const int STEPPING_MICROSTEP8 = 1;
    public const int STEPPING_MICROSTEP4 = 2;
    public const int STEPPING_HALFSTEP = 3;
    public const int STEPPING_FULLSTEP = 4;
    public const int STEPPING_INVALID = -1;
    /**
     * <summary>
     *   invalid overcurrent value
     * </summary>
     */
    public const  int OVERCURRENT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid tCurrStop value
     * </summary>
     */
    public const  int TCURRSTOP_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid tCurrRun value
     * </summary>
     */
    public const  int TCURRRUN_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid alertMode value
     * </summary>
     */
    public const  string ALERTMODE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid auxMode value
     * </summary>
     */
    public const  string AUXMODE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid auxSignal value
     * </summary>
     */
    public const  int AUXSIGNAL_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _motorState = MOTORSTATE_INVALID;
    protected int _diags = DIAGS_INVALID;
    protected double _stepPos = STEPPOS_INVALID;
    protected double _speed = SPEED_INVALID;
    protected double _pullinSpeed = PULLINSPEED_INVALID;
    protected double _maxAccel = MAXACCEL_INVALID;
    protected double _maxSpeed = MAXSPEED_INVALID;
    protected int _stepping = STEPPING_INVALID;
    protected int _overcurrent = OVERCURRENT_INVALID;
    protected int _tCurrStop = TCURRSTOP_INVALID;
    protected int _tCurrRun = TCURRRUN_INVALID;
    protected string _alertMode = ALERTMODE_INVALID;
    protected string _auxMode = AUXMODE_INVALID;
    protected int _auxSignal = AUXSIGNAL_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackStepperMotor = null;

    public new delegate Task ValueCallback(YStepperMotor func, string value);
    public new delegate Task TimedReportCallback(YStepperMotor func, YMeasure measure);
    //--- (end of YStepperMotor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YStepperMotor(YAPIContext ctx, string func)
        : base(ctx, func, "StepperMotor")
    {
        //--- (YStepperMotor attributes initialization)
        //--- (end of YStepperMotor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YStepperMotor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YStepperMotor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("motorState")) {
            _motorState = json_val.getInt("motorState");
        }
        if (json_val.has("diags")) {
            _diags = json_val.getInt("diags");
        }
        if (json_val.has("stepPos")) {
            _stepPos = json_val.getDouble("stepPos") / 16.0;
        }
        if (json_val.has("speed")) {
            _speed = Math.Round(json_val.getDouble("speed") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("pullinSpeed")) {
            _pullinSpeed = Math.Round(json_val.getDouble("pullinSpeed") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("maxAccel")) {
            _maxAccel = Math.Round(json_val.getDouble("maxAccel") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("maxSpeed")) {
            _maxSpeed = Math.Round(json_val.getDouble("maxSpeed") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("stepping")) {
            _stepping = json_val.getInt("stepping");
        }
        if (json_val.has("overcurrent")) {
            _overcurrent = json_val.getInt("overcurrent");
        }
        if (json_val.has("tCurrStop")) {
            _tCurrStop = json_val.getInt("tCurrStop");
        }
        if (json_val.has("tCurrRun")) {
            _tCurrRun = json_val.getInt("tCurrRun");
        }
        if (json_val.has("alertMode")) {
            _alertMode = json_val.getString("alertMode");
        }
        if (json_val.has("auxMode")) {
            _auxMode = json_val.getString("auxMode");
        }
        if (json_val.has("auxSignal")) {
            _auxSignal = json_val.getInt("auxSignal");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the motor working state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YStepperMotor.MOTORSTATE_ABSENT</c>, <c>YStepperMotor.MOTORSTATE_ALERT</c>,
     *   <c>YStepperMotor.MOTORSTATE_HI_Z</c>, <c>YStepperMotor.MOTORSTATE_STOP</c>,
     *   <c>YStepperMotor.MOTORSTATE_RUN</c> and <c>YStepperMotor.MOTORSTATE_BATCH</c> corresponding to the
     *   motor working state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MOTORSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_motorState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MOTORSTATE_INVALID;
            }
        }
        res = _motorState;
        return res;
    }


    /**
     * <summary>
     *   Returns the stepper motor controller diagnostics, as a bitmap.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the stepper motor controller diagnostics, as a bitmap
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.DIAGS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_diags()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DIAGS_INVALID;
            }
        }
        res = _diags;
        return res;
    }


    /**
     * <summary>
     *   Changes the current logical motor position, measured in steps.
     * <para>
     *   This command does not cause any motor move, as its purpose is only to setup
     *   the origin of the position counter. The fractional part of the position,
     *   that corresponds to the physical position of the rotor, is not changed.
     *   To trigger a motor move, use methods <c>moveTo()</c> or <c>moveRel()</c>
     *   instead.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current logical motor position, measured in steps
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
    public async Task<int> set_stepPos(double  newval)
    {
        string rest_val;
        rest_val = (Math.Round(newval * 100.0) / 100.0).ToString();
        await _setAttr("stepPos",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current logical motor position, measured in steps.
     * <para>
     *   The value may include a fractional part when micro-stepping is in use.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current logical motor position, measured in steps
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.STEPPOS_INVALID</c>.
     * </para>
     */
    public async Task<double> get_stepPos()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STEPPOS_INVALID;
            }
        }
        res = _stepPos;
        return res;
    }


    /**
     * <summary>
     *   Returns current motor speed, measured in steps per second.
     * <para>
     *   To change speed, use method <c>changeSpeed()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to current motor speed, measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.SPEED_INVALID</c>.
     * </para>
     */
    public async Task<double> get_speed()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SPEED_INVALID;
            }
        }
        res = _speed;
        return res;
    }


    /**
     * <summary>
     *   Changes the motor speed immediately reachable from stop state, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the motor speed immediately reachable from stop state,
     *   measured in steps per second
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
    public async Task<int> set_pullinSpeed(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("pullinSpeed",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the motor speed immediately reachable from stop state, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the motor speed immediately reachable from stop state,
     *   measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.PULLINSPEED_INVALID</c>.
     * </para>
     */
    public async Task<double> get_pullinSpeed()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULLINSPEED_INVALID;
            }
        }
        res = _pullinSpeed;
        return res;
    }


    /**
     * <summary>
     *   Changes the maximal motor acceleration, measured in steps per second^2.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the maximal motor acceleration, measured in steps per second^2
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
    public async Task<int> set_maxAccel(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("maxAccel",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximal motor acceleration, measured in steps per second^2.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the maximal motor acceleration, measured in steps per second^2
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MAXACCEL_INVALID</c>.
     * </para>
     */
    public async Task<double> get_maxAccel()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXACCEL_INVALID;
            }
        }
        res = _maxAccel;
        return res;
    }


    /**
     * <summary>
     *   Changes the maximal motor speed, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the maximal motor speed, measured in steps per second
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
    public async Task<int> set_maxSpeed(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("maxSpeed",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximal motor speed, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the maximal motor speed, measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MAXSPEED_INVALID</c>.
     * </para>
     */
    public async Task<double> get_maxSpeed()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXSPEED_INVALID;
            }
        }
        res = _maxSpeed;
        return res;
    }


    /**
     * <summary>
     *   Returns the stepping mode used to drive the motor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YStepperMotor.STEPPING_MICROSTEP16</c>, <c>YStepperMotor.STEPPING_MICROSTEP8</c>,
     *   <c>YStepperMotor.STEPPING_MICROSTEP4</c>, <c>YStepperMotor.STEPPING_HALFSTEP</c> and
     *   <c>YStepperMotor.STEPPING_FULLSTEP</c> corresponding to the stepping mode used to drive the motor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.STEPPING_INVALID</c>.
     * </para>
     */
    public async Task<int> get_stepping()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STEPPING_INVALID;
            }
        }
        res = _stepping;
        return res;
    }


    /**
     * <summary>
     *   Changes the stepping mode used to drive the motor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YStepperMotor.STEPPING_MICROSTEP16</c>, <c>YStepperMotor.STEPPING_MICROSTEP8</c>,
     *   <c>YStepperMotor.STEPPING_MICROSTEP4</c>, <c>YStepperMotor.STEPPING_HALFSTEP</c> and
     *   <c>YStepperMotor.STEPPING_FULLSTEP</c> corresponding to the stepping mode used to drive the motor
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
    public async Task<int> set_stepping(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("stepping",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the overcurrent alert and emergency stop threshold, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the overcurrent alert and emergency stop threshold, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.OVERCURRENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_overcurrent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return OVERCURRENT_INVALID;
            }
        }
        res = _overcurrent;
        return res;
    }


    /**
     * <summary>
     *   Changes the overcurrent alert and emergency stop threshold, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the overcurrent alert and emergency stop threshold, measured in mA
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
    public async Task<int> set_overcurrent(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("overcurrent",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the torque regulation current when the motor is stopped, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the torque regulation current when the motor is stopped, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.TCURRSTOP_INVALID</c>.
     * </para>
     */
    public async Task<int> get_tCurrStop()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TCURRSTOP_INVALID;
            }
        }
        res = _tCurrStop;
        return res;
    }


    /**
     * <summary>
     *   Changes the torque regulation current when the motor is stopped, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the torque regulation current when the motor is stopped, measured in mA
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
    public async Task<int> set_tCurrStop(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("tCurrStop",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the torque regulation current when the motor is running, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the torque regulation current when the motor is running, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.TCURRRUN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_tCurrRun()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TCURRRUN_INVALID;
            }
        }
        res = _tCurrRun;
        return res;
    }


    /**
     * <summary>
     *   Changes the torque regulation current when the motor is running, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the torque regulation current when the motor is running, measured in mA
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
    public async Task<int> set_tCurrRun(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("tCurrRun",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_alertMode()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ALERTMODE_INVALID;
            }
        }
        res = _alertMode;
        return res;
    }


    public async Task<int> set_alertMode(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("alertMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_auxMode()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return AUXMODE_INVALID;
            }
        }
        res = _auxMode;
        return res;
    }


    public async Task<int> set_auxMode(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("auxMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current value of the signal generated on the auxiliary output.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current value of the signal generated on the auxiliary output
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.AUXSIGNAL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_auxSignal()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return AUXSIGNAL_INVALID;
            }
        }
        res = _auxSignal;
        return res;
    }


    /**
     * <summary>
     *   Changes the value of the signal generated on the auxiliary output.
     * <para>
     *   Acceptable values depend on the auxiliary output signal type configured.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the value of the signal generated on the auxiliary output
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
    public async Task<int> set_auxSignal(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("auxSignal",rest_val);
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
     *   Retrieves a stepper motor for a given identifier.
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
     *   This function does not require that the stepper motor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YStepperMotor.isOnline()</c> to test if the stepper motor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a stepper motor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the stepper motor
     * </param>
     * <returns>
     *   a <c>YStepperMotor</c> object allowing you to drive the stepper motor.
     * </returns>
     */
    public static YStepperMotor FindStepperMotor(string func)
    {
        YStepperMotor obj;
        obj = (YStepperMotor) YFunction._FindFromCache("StepperMotor", func);
        if (obj == null) {
            obj = new YStepperMotor(func);
            YFunction._AddToCache("StepperMotor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a stepper motor for a given identifier in a YAPI context.
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
     *   This function does not require that the stepper motor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YStepperMotor.isOnline()</c> to test if the stepper motor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a stepper motor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the stepper motor
     * </param>
     * <returns>
     *   a <c>YStepperMotor</c> object allowing you to drive the stepper motor.
     * </returns>
     */
    public static YStepperMotor FindStepperMotorInContext(YAPIContext yctx,string func)
    {
        YStepperMotor obj;
        obj = (YStepperMotor) YFunction._FindFromCacheInContext(yctx,  "StepperMotor", func);
        if (obj == null) {
            obj = new YStepperMotor(yctx, func);
            YFunction._AddToCache("StepperMotor",  func, obj);
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
        _valueCallbackStepperMotor = callback;
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
        if (_valueCallbackStepperMotor != null) {
            await _valueCallbackStepperMotor(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> sendCommand(string command)
    {
        string id;
        string url;
        byte[] retBin;
        int res;
        id = await this.get_functionId();
        id = (id).Substring( 12, 1);
        url = "cmd.txt?"+ id+"="+command;
        //may throw an exception
        retBin = await this._download(url);
        res = retBin[0];
        if (res == 49) {
            if (!(res == 48)) { this._throw( YAPI.DEVICE_BUSY, "Motor command pipeline is full, try again later"); return YAPI.DEVICE_BUSY; }
        } else {
            if (!(res == 48)) { this._throw( YAPI.IO_ERROR, "Motor command failed permanently"); return YAPI.IO_ERROR; }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Reinitialize the controller and clear all alert flags.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> reset()
    {
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Starts the motor backward at the specified speed, to search for the motor home position.
     * <para>
     * </para>
     * </summary>
     * <param name="speed">
     *   desired speed, in steps per second.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> findHomePosition(double speed)
    {
        return await this.sendCommand("H"+Convert.ToString((int) Math.Round(1000*speed)));
    }

    /**
     * <summary>
     *   Starts the motor at a given speed.
     * <para>
     *   The time needed to reach the requested speed
     *   will depend on the acceleration parameters configured for the motor.
     * </para>
     * </summary>
     * <param name="speed">
     *   desired speed, in steps per second. The minimal non-zero speed
     *   is 0.001 pulse per second.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> changeSpeed(double speed)
    {
        return await this.sendCommand("R"+Convert.ToString((int) Math.Round(1000*speed)));
    }

    /**
     * <summary>
     *   Starts the motor to reach a given absolute position.
     * <para>
     *   The time needed to reach the requested
     *   position will depend on the acceleration and max speed parameters configured for
     *   the motor.
     * </para>
     * </summary>
     * <param name="absPos">
     *   absolute position, measured in steps from the origin.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> moveTo(double absPos)
    {
        return await this.sendCommand("M"+Convert.ToString((int) Math.Round(16*absPos)));
    }

    /**
     * <summary>
     *   Starts the motor to reach a given relative position.
     * <para>
     *   The time needed to reach the requested
     *   position will depend on the acceleration and max speed parameters configured for
     *   the motor.
     * </para>
     * </summary>
     * <param name="relPos">
     *   relative position, measured in steps from the current position.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> moveRel(double relPos)
    {
        return await this.sendCommand("m"+Convert.ToString((int) Math.Round(16*relPos)));
    }

    /**
     * <summary>
     *   Starts the motor to reach a given relative position, keeping the speed under the
     *   specified limit.
     * <para>
     *   The time needed to reach the requested position will depend on
     *   the acceleration parameters configured for the motor.
     * </para>
     * </summary>
     * <param name="relPos">
     *   relative position, measured in steps from the current position.
     * </param>
     * <param name="maxSpeed">
     *   limit speed, in steps per second.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> moveRelSlow(double relPos,double maxSpeed)
    {
        return await this.sendCommand("m"+Convert.ToString((int) Math.Round(16*relPos))+"@"+Convert.ToString((int) Math.Round(1000*maxSpeed)));
    }

    /**
     * <summary>
     *   Keep the motor in the same state for the specified amount of time, before processing next command.
     * <para>
     * </para>
     * </summary>
     * <param name="waitMs">
     *   wait time, specified in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> pause(int waitMs)
    {
        return await this.sendCommand("_"+Convert.ToString(waitMs));
    }

    /**
     * <summary>
     *   Stops the motor with an emergency alert, without taking any additional precaution.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> emergencyStop()
    {
        return await this.sendCommand("!");
    }

    /**
     * <summary>
     *   Move one step in the direction opposite the direction set when the most recent alert was raised.
     * <para>
     *   The move occures even if the system is still in alert mode (end switch depressed). Caution.
     *   use this function with great care as it may cause mechanical damages !
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> alertStepOut()
    {
        return await this.sendCommand(".");
    }

    /**
     * <summary>
     *   Move one single step in the selected direction without regards to end switches.
     * <para>
     *   The move occures even if the system is still in alert mode (end switch depressed). Caution.
     *   use this function with great care as it may cause mechanical damages !
     * </para>
     * </summary>
     * <param name="dir">
     *   Value +1 ou -1, according to the desired direction of the move
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> alertStepDir(int dir)
    {
        if (!(dir != 0)) { this._throw( YAPI.INVALID_ARGUMENT, "direction must be +1 or -1"); return YAPI.INVALID_ARGUMENT; }
        if (dir > 0) {
            return await this.sendCommand(".+");
        }
        return await this.sendCommand(".-");
    }

    /**
     * <summary>
     *   Stops the motor smoothly as soon as possible, without waiting for ongoing move completion.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> abortAndBrake()
    {
        return await this.sendCommand("B");
    }

    /**
     * <summary>
     *   Turn the controller into Hi-Z mode immediately, without waiting for ongoing move completion.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> abortAndHiZ()
    {
        return await this.sendCommand("z");
    }

    /**
     * <summary>
     *   Continues the enumeration of stepper motors started using <c>yFirstStepperMotor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YStepperMotor</c> object, corresponding to
     *   a stepper motor currently online, or a <c>null</c> pointer
     *   if there are no more stepper motors to enumerate.
     * </returns>
     */
    public YStepperMotor nextStepperMotor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindStepperMotorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of stepper motors currently accessible.
     * <para>
     *   Use the method <c>YStepperMotor.nextStepperMotor()</c> to iterate on
     *   next stepper motors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YStepperMotor</c> object, corresponding to
     *   the first stepper motor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YStepperMotor FirstStepperMotor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("StepperMotor");
        if (next_hwid == null)  return null;
        return FindStepperMotorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of stepper motors currently accessible.
     * <para>
     *   Use the method <c>YStepperMotor.nextStepperMotor()</c> to iterate on
     *   next stepper motors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YStepperMotor</c> object, corresponding to
     *   the first stepper motor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YStepperMotor FirstStepperMotorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("StepperMotor");
        if (next_hwid == null)  return null;
        return FindStepperMotorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YStepperMotor implementation)
}
}

