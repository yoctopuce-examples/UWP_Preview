/*********************************************************************
 *
 * $Id: YCurrentLoopOutput.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindCurrentLoopOutput(), the high-level API for CurrentLoopOutput functions
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

//--- (YCurrentLoopOutput return codes)
//--- (end of YCurrentLoopOutput return codes)
//--- (YCurrentLoopOutput class start)
/**
 * <summary>
 *   YCurrentLoopOutput Class: CurrentLoopOutput function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to change the value of the 4-20mA
 *   output as well as to know the current loop state.
 * </para>
 * </summary>
 */
public class YCurrentLoopOutput : YFunction
{
//--- (end of YCurrentLoopOutput class start)
//--- (YCurrentLoopOutput definitions)
    /**
     * <summary>
     *   invalid current value
     * </summary>
     */
    public const  double CURRENT_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid currentTransition value
     * </summary>
     */
    public const  string CURRENTTRANSITION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid currentAtStartUp value
     * </summary>
     */
    public const  double CURRENTATSTARTUP_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid loopPower value
     * </summary>
     */
    public const int LOOPPOWER_NOPWR = 0;
    public const int LOOPPOWER_LOWPWR = 1;
    public const int LOOPPOWER_POWEROK = 2;
    public const int LOOPPOWER_INVALID = -1;
    protected double _current = CURRENT_INVALID;
    protected string _currentTransition = CURRENTTRANSITION_INVALID;
    protected double _currentAtStartUp = CURRENTATSTARTUP_INVALID;
    protected int _loopPower = LOOPPOWER_INVALID;
    protected ValueCallback _valueCallbackCurrentLoopOutput = null;

    public new delegate Task ValueCallback(YCurrentLoopOutput func, string value);
    public new delegate Task TimedReportCallback(YCurrentLoopOutput func, YMeasure measure);
    //--- (end of YCurrentLoopOutput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCurrentLoopOutput(YAPIContext ctx, string func)
        : base(ctx, func, "CurrentLoopOutput")
    {
        //--- (YCurrentLoopOutput attributes initialization)
        //--- (end of YCurrentLoopOutput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCurrentLoopOutput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YCurrentLoopOutput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("current")) {
            _current = Math.Round(json_val.getDouble("current") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("currentTransition")) {
            _currentTransition = json_val.getString("currentTransition");
        }
        if (json_val.has("currentAtStartUp")) {
            _currentAtStartUp = Math.Round(json_val.getDouble("currentAtStartUp") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("loopPower")) {
            _loopPower = json_val.getInt("loopPower");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the current loop, the valid range is from 3 to 21mA.
     * <para>
     *   If the loop is
     *   not propely powered, the  target current is not reached and
     *   loopPower is set to LOWPWR.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current loop, the valid range is from 3 to 21mA
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
    public async Task<int> set_current(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("current",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the loop current set point in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the loop current set point in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCurrentLoopOutput.CURRENT_INVALID</c>.
     * </para>
     */
    public async Task<double> get_current()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENT_INVALID;
            }
        }
        res = _current;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_currentTransition()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTTRANSITION_INVALID;
            }
        }
        res = _currentTransition;
        return res;
    }


    public async Task<int> set_currentTransition(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("currentTransition",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the loop current at device start up.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call has no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the loop current at device start up
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
    public async Task<int> set_currentAtStartUp(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentAtStartUp",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current in the loop at device startup, in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current in the loop at device startup, in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCurrentLoopOutput.CURRENTATSTARTUP_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentAtStartUp()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTATSTARTUP_INVALID;
            }
        }
        res = _currentAtStartUp;
        return res;
    }


    /**
     * <summary>
     *   Returns the loop powerstate.
     * <para>
     *   POWEROK: the loop
     *   is powered. NOPWR: the loop in not powered. LOWPWR: the loop is not
     *   powered enough to maintain the current required (insufficient voltage).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YCurrentLoopOutput.LOOPPOWER_NOPWR</c>, <c>YCurrentLoopOutput.LOOPPOWER_LOWPWR</c>
     *   and <c>YCurrentLoopOutput.LOOPPOWER_POWEROK</c> corresponding to the loop powerstate
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCurrentLoopOutput.LOOPPOWER_INVALID</c>.
     * </para>
     */
    public async Task<int> get_loopPower()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LOOPPOWER_INVALID;
            }
        }
        res = _loopPower;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a 4-20mA output for a given identifier.
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
     *   This function does not require that the 4-20mA output is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCurrentLoopOutput.isOnline()</c> to test if the 4-20mA output is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a 4-20mA output by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the 4-20mA output
     * </param>
     * <returns>
     *   a <c>YCurrentLoopOutput</c> object allowing you to drive the 4-20mA output.
     * </returns>
     */
    public static YCurrentLoopOutput FindCurrentLoopOutput(string func)
    {
        YCurrentLoopOutput obj;
        obj = (YCurrentLoopOutput) YFunction._FindFromCache("CurrentLoopOutput", func);
        if (obj == null) {
            obj = new YCurrentLoopOutput(func);
            YFunction._AddToCache("CurrentLoopOutput",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a 4-20mA output for a given identifier in a YAPI context.
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
     *   This function does not require that the 4-20mA output is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCurrentLoopOutput.isOnline()</c> to test if the 4-20mA output is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a 4-20mA output by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the 4-20mA output
     * </param>
     * <returns>
     *   a <c>YCurrentLoopOutput</c> object allowing you to drive the 4-20mA output.
     * </returns>
     */
    public static YCurrentLoopOutput FindCurrentLoopOutputInContext(YAPIContext yctx,string func)
    {
        YCurrentLoopOutput obj;
        obj = (YCurrentLoopOutput) YFunction._FindFromCacheInContext(yctx,  "CurrentLoopOutput", func);
        if (obj == null) {
            obj = new YCurrentLoopOutput(yctx, func);
            YFunction._AddToCache("CurrentLoopOutput",  func, obj);
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
        _valueCallbackCurrentLoopOutput = callback;
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
        if (_valueCallbackCurrentLoopOutput != null) {
            await _valueCallbackCurrentLoopOutput(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Performs a smooth transistion of current flowing in the loop.
     * <para>
     *   Any current explicit
     *   change cancels any ongoing transition process.
     * </para>
     * </summary>
     * <param name="mA_target">
     *   new current value at the end of the transition
     *   (floating-point number, representing the end current in mA)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     */
    public virtual async Task<int> currentMove(double mA_target,int ms_duration)
    {
        string newval;
        if (mA_target < 3.0) {
            mA_target  = 3.0;
        }
        if (mA_target > 21.0) {
            mA_target = 21.0;
        }
        newval = ""+Convert.ToString( (int) Math.Round(mA_target*65536))+":"+Convert.ToString(ms_duration);

        return await this.set_currentTransition(newval);
    }

    /**
     * <summary>
     *   Continues the enumeration of 4-20mA outputs started using <c>yFirstCurrentLoopOutput()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCurrentLoopOutput</c> object, corresponding to
     *   a 4-20mA output currently online, or a <c>null</c> pointer
     *   if there are no more 4-20mA outputs to enumerate.
     * </returns>
     */
    public YCurrentLoopOutput nextCurrentLoopOutput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindCurrentLoopOutputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of 4-20mA outputs currently accessible.
     * <para>
     *   Use the method <c>YCurrentLoopOutput.nextCurrentLoopOutput()</c> to iterate on
     *   next 4-20mA outputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCurrentLoopOutput</c> object, corresponding to
     *   the first 4-20mA output currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCurrentLoopOutput FirstCurrentLoopOutput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("CurrentLoopOutput");
        if (next_hwid == null)  return null;
        return FindCurrentLoopOutputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of 4-20mA outputs currently accessible.
     * <para>
     *   Use the method <c>YCurrentLoopOutput.nextCurrentLoopOutput()</c> to iterate on
     *   next 4-20mA outputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YCurrentLoopOutput</c> object, corresponding to
     *   the first 4-20mA output currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCurrentLoopOutput FirstCurrentLoopOutputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("CurrentLoopOutput");
        if (next_hwid == null)  return null;
        return FindCurrentLoopOutputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YCurrentLoopOutput implementation)
}
}

