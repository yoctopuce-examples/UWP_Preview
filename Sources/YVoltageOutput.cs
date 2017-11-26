/*********************************************************************
 *
 * $Id: YVoltageOutput.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindVoltageOutput(), the high-level API for VoltageOutput functions
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

//--- (YVoltageOutput return codes)
//--- (end of YVoltageOutput return codes)
//--- (YVoltageOutput class start)
/**
 * <summary>
 *   YVoltageOutput Class: VoltageOutput function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to change the value of the voltage output.
 * </para>
 * </summary>
 */
public class YVoltageOutput : YFunction
{
//--- (end of YVoltageOutput class start)
//--- (YVoltageOutput definitions)
    /**
     * <summary>
     *   invalid currentVoltage value
     * </summary>
     */
    public const  double CURRENTVOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid voltageTransition value
     * </summary>
     */
    public const  string VOLTAGETRANSITION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid voltageAtStartUp value
     * </summary>
     */
    public const  double VOLTAGEATSTARTUP_INVALID = YAPI.INVALID_DOUBLE;
    protected double _currentVoltage = CURRENTVOLTAGE_INVALID;
    protected string _voltageTransition = VOLTAGETRANSITION_INVALID;
    protected double _voltageAtStartUp = VOLTAGEATSTARTUP_INVALID;
    protected ValueCallback _valueCallbackVoltageOutput = null;

    public new delegate Task ValueCallback(YVoltageOutput func, string value);
    public new delegate Task TimedReportCallback(YVoltageOutput func, YMeasure measure);
    //--- (end of YVoltageOutput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVoltageOutput(YAPIContext ctx, string func)
        : base(ctx, func, "VoltageOutput")
    {
        //--- (YVoltageOutput attributes initialization)
        //--- (end of YVoltageOutput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVoltageOutput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YVoltageOutput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("currentVoltage")) {
            _currentVoltage = Math.Round(json_val.getDouble("currentVoltage") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("voltageTransition")) {
            _voltageTransition = json_val.getString("voltageTransition");
        }
        if (json_val.has("voltageAtStartUp")) {
            _voltageAtStartUp = Math.Round(json_val.getDouble("voltageAtStartUp") * 1000.0 / 65536.0) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the output voltage, in V.
     * <para>
     *   Valid range is from 0 to 10V.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the output voltage, in V
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
    public async Task<int> set_currentVoltage(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentVoltage",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the output voltage set point, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the output voltage set point, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YVoltageOutput.CURRENTVOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentVoltage()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTVOLTAGE_INVALID;
            }
        }
        res = _currentVoltage;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_voltageTransition()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGETRANSITION_INVALID;
            }
        }
        res = _voltageTransition;
        return res;
    }


    public async Task<int> set_voltageTransition(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("voltageTransition",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the output voltage at device start up.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call has no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the output voltage at device start up
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
    public async Task<int> set_voltageAtStartUp(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("voltageAtStartUp",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the selected voltage output at device startup, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the selected voltage output at device startup, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YVoltageOutput.VOLTAGEATSTARTUP_INVALID</c>.
     * </para>
     */
    public async Task<double> get_voltageAtStartUp()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGEATSTARTUP_INVALID;
            }
        }
        res = _voltageAtStartUp;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a voltage output for a given identifier.
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
     *   This function does not require that the voltage output is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVoltageOutput.isOnline()</c> to test if the voltage output is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage output by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the voltage output
     * </param>
     * <returns>
     *   a <c>YVoltageOutput</c> object allowing you to drive the voltage output.
     * </returns>
     */
    public static YVoltageOutput FindVoltageOutput(string func)
    {
        YVoltageOutput obj;
        obj = (YVoltageOutput) YFunction._FindFromCache("VoltageOutput", func);
        if (obj == null) {
            obj = new YVoltageOutput(func);
            YFunction._AddToCache("VoltageOutput",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a voltage output for a given identifier in a YAPI context.
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
     *   This function does not require that the voltage output is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVoltageOutput.isOnline()</c> to test if the voltage output is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a voltage output by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the voltage output
     * </param>
     * <returns>
     *   a <c>YVoltageOutput</c> object allowing you to drive the voltage output.
     * </returns>
     */
    public static YVoltageOutput FindVoltageOutputInContext(YAPIContext yctx,string func)
    {
        YVoltageOutput obj;
        obj = (YVoltageOutput) YFunction._FindFromCacheInContext(yctx,  "VoltageOutput", func);
        if (obj == null) {
            obj = new YVoltageOutput(yctx, func);
            YFunction._AddToCache("VoltageOutput",  func, obj);
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
        _valueCallbackVoltageOutput = callback;
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
        if (_valueCallbackVoltageOutput != null) {
            await _valueCallbackVoltageOutput(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Performs a smooth transistion of output voltage.
     * <para>
     *   Any explicit voltage
     *   change cancels any ongoing transition process.
     * </para>
     * </summary>
     * <param name="V_target">
     *   new output voltage value at the end of the transition
     *   (floating-point number, representing the end voltage in V)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     */
    public virtual async Task<int> voltageMove(double V_target,int ms_duration)
    {
        string newval;
        if (V_target < 0.0) {
            V_target  = 0.0;
        }
        if (V_target > 10.0) {
            V_target = 10.0;
        }
        newval = ""+Convert.ToString( (int) Math.Round(V_target*65536))+":"+Convert.ToString(ms_duration);

        return await this.set_voltageTransition(newval);
    }

    /**
     * <summary>
     *   Continues the enumeration of voltage outputs started using <c>yFirstVoltageOutput()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVoltageOutput</c> object, corresponding to
     *   a voltage output currently online, or a <c>null</c> pointer
     *   if there are no more voltage outputs to enumerate.
     * </returns>
     */
    public YVoltageOutput nextVoltageOutput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindVoltageOutputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of voltage outputs currently accessible.
     * <para>
     *   Use the method <c>YVoltageOutput.nextVoltageOutput()</c> to iterate on
     *   next voltage outputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVoltageOutput</c> object, corresponding to
     *   the first voltage output currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVoltageOutput FirstVoltageOutput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("VoltageOutput");
        if (next_hwid == null)  return null;
        return FindVoltageOutputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of voltage outputs currently accessible.
     * <para>
     *   Use the method <c>YVoltageOutput.nextVoltageOutput()</c> to iterate on
     *   next voltage outputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YVoltageOutput</c> object, corresponding to
     *   the first voltage output currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVoltageOutput FirstVoltageOutputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("VoltageOutput");
        if (next_hwid == null)  return null;
        return FindVoltageOutputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YVoltageOutput implementation)
}
}

