/*********************************************************************
 *
 * $Id: YServo.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindServo(), the high-level API for Servo functions
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

//--- (YServo return codes)
//--- (end of YServo return codes)
//--- (YServo class start)
/**
 * <summary>
 *   YServo Class: Servo function interface
 * <para>
 *   Yoctopuce application programming interface allows you not only to move
 *   a servo to a given position, but also to specify the time interval
 *   in which the move should be performed. This makes it possible to
 *   synchronize two servos involved in a same move.
 * </para>
 * </summary>
 */
public class YServo : YFunction
{
//--- (end of YServo class start)
//--- (YServo definitions)
    public class YMove
    {
        public int target = YAPI.INVALID_INT;
        public int ms = YAPI.INVALID_INT;
        public int moving = YAPI.INVALID_UINT;
        public YMove(){}
    }

    /**
     * <summary>
     *   invalid position value
     * </summary>
     */
    public const  int POSITION_INVALID = YAPI.INVALID_INT;
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
     *   invalid range value
     * </summary>
     */
    public const  int RANGE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid neutral value
     * </summary>
     */
    public const  int NEUTRAL_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid positionAtPowerOn value
     * </summary>
     */
    public const  int POSITIONATPOWERON_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid enabledAtPowerOn value
     * </summary>
     */
    public const int ENABLEDATPOWERON_FALSE = 0;
    public const int ENABLEDATPOWERON_TRUE = 1;
    public const int ENABLEDATPOWERON_INVALID = -1;
    public static readonly YMove MOVE_INVALID = null;
    protected int _position = POSITION_INVALID;
    protected int _enabled = ENABLED_INVALID;
    protected int _range = RANGE_INVALID;
    protected int _neutral = NEUTRAL_INVALID;
    protected YMove _move = new YMove();
    protected int _positionAtPowerOn = POSITIONATPOWERON_INVALID;
    protected int _enabledAtPowerOn = ENABLEDATPOWERON_INVALID;
    protected ValueCallback _valueCallbackServo = null;

    public new delegate Task ValueCallback(YServo func, string value);
    public new delegate Task TimedReportCallback(YServo func, YMeasure measure);
    //--- (end of YServo definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YServo(YAPIContext ctx, string func)
        : base(ctx, func, "Servo")
    {
        //--- (YServo attributes initialization)
        //--- (end of YServo attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YServo(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YServo implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("position")) {
            _position = json_val.GetInt("position");
        }
        if (json_val.Has("enabled")) {
            _enabled = json_val.GetInt("enabled") > 0 ? 1 : 0;
        }
        if (json_val.Has("range")) {
            _range = json_val.GetInt("range");
        }
        if (json_val.Has("neutral")) {
            _neutral = json_val.GetInt("neutral");
        }
        if (json_val.Has("positionAtPowerOn")) {
            _positionAtPowerOn = json_val.GetInt("positionAtPowerOn");
        }
        if (json_val.Has("enabledAtPowerOn")) {
            _enabledAtPowerOn = json_val.GetInt("enabledAtPowerOn") > 0 ? 1 : 0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   invalid move
     * </summary>
     */
    /**
     * <summary>
     *   Returns the current servo position.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current servo position
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.POSITION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_position()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POSITION_INVALID;
            }
        }
        return _position;
    }


    /**
     * <summary>
     *   Changes immediately the servo driving position.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to immediately the servo driving position
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
    public async Task<int> set_position(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("position",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the servos.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YServo.ENABLED_FALSE</c> or <c>YServo.ENABLED_TRUE</c>, according to the state of the servos
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.ENABLED_INVALID</c>.
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
     *   Stops or starts the servo.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YServo.ENABLED_FALSE</c> or <c>YServo.ENABLED_TRUE</c>
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
     *   Returns the current range of use of the servo.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current range of use of the servo
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.RANGE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_range()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RANGE_INVALID;
            }
        }
        return _range;
    }


    /**
     * <summary>
     *   Changes the range of use of the servo, specified in per cents.
     * <para>
     *   A range of 100% corresponds to a standard control signal, that varies
     *   from 1 [ms] to 2 [ms], When using a servo that supports a double range,
     *   from 0.5 [ms] to 2.5 [ms], you can select a range of 200%.
     *   Be aware that using a range higher than what is supported by the servo
     *   is likely to damage the servo. Remember to call the matching module
     *   <c>saveToFlash()</c> method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the range of use of the servo, specified in per cents
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
    public async Task<int> set_range(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("range",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the duration in microseconds of a neutral pulse for the servo.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the duration in microseconds of a neutral pulse for the servo
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.NEUTRAL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_neutral()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return NEUTRAL_INVALID;
            }
        }
        return _neutral;
    }


    /**
     * <summary>
     *   Changes the duration of the pulse corresponding to the neutral position of the servo.
     * <para>
     *   The duration is specified in microseconds, and the standard value is 1500 [us].
     *   This setting makes it possible to shift the range of use of the servo.
     *   Be aware that using a range higher than what is supported by the servo is
     *   likely to damage the servo. Remember to call the matching module
     *   <c>saveToFlash()</c> method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the duration of the pulse corresponding to the neutral position of the servo
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
    public async Task<int> set_neutral(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("neutral",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the servo position at device power up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the servo position at device power up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.POSITIONATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_positionAtPowerOn()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POSITIONATPOWERON_INVALID;
            }
        }
        return _positionAtPowerOn;
    }


    /**
     * <summary>
     *   Configure the servo position at device power up.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public async Task<int> set_positionAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("positionAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the servo signal generator state at power up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YServo.ENABLEDATPOWERON_FALSE</c> or <c>YServo.ENABLEDATPOWERON_TRUE</c>, according to
     *   the servo signal generator state at power up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YServo.ENABLEDATPOWERON_INVALID</c>.
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
     *   Configure the servo signal generator state at power up.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YServo.ENABLEDATPOWERON_FALSE</c> or <c>YServo.ENABLEDATPOWERON_TRUE</c>
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
     *   Retrieves a servo for a given identifier.
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
     *   This function does not require that the servo is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YServo.isOnline()</c> to test if the servo is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a servo by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the servo
     * </param>
     * <returns>
     *   a <c>YServo</c> object allowing you to drive the servo.
     * </returns>
     */
    public static YServo FindServo(string func)
    {
        YServo obj;
        obj = (YServo) YFunction._FindFromCache("Servo", func);
        if (obj == null) {
            obj = new YServo(func);
            YFunction._AddToCache("Servo",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a servo for a given identifier in a YAPI context.
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
     *   This function does not require that the servo is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YServo.isOnline()</c> to test if the servo is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a servo by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the servo
     * </param>
     * <returns>
     *   a <c>YServo</c> object allowing you to drive the servo.
     * </returns>
     */
    public static YServo FindServoInContext(YAPIContext yctx,string func)
    {
        YServo obj;
        obj = (YServo) YFunction._FindFromCacheInContext(yctx,  "Servo", func);
        if (obj == null) {
            obj = new YServo(yctx, func);
            YFunction._AddToCache("Servo",  func, obj);
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
        _valueCallbackServo = callback;
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
        if (_valueCallbackServo != null) {
            await _valueCallbackServo(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of servos started using <c>yFirstServo()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YServo</c> object, corresponding to
     *   a servo currently online, or a <c>null</c> pointer
     *   if there are no more servos to enumerate.
     * </returns>
     */
    public YServo nextServo()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindServoInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of servos currently accessible.
     * <para>
     *   Use the method <c>YServo.nextServo()</c> to iterate on
     *   next servos.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YServo</c> object, corresponding to
     *   the first servo currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YServo FirstServo()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Servo");
        if (next_hwid == null)  return null;
        return FindServoInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of servos currently accessible.
     * <para>
     *   Use the method <c>YServo.nextServo()</c> to iterate on
     *   next servos.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YServo</c> object, corresponding to
     *   the first servo currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YServo FirstServoInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Servo");
        if (next_hwid == null)  return null;
        return FindServoInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YServo implementation)
}
}

