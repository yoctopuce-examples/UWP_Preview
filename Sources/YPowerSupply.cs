/*********************************************************************
 *
 * $Id: YPowerSupply.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindPowerSupply(), the high-level API for PowerSupply functions
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

//--- (YPowerSupply return codes)
//--- (end of YPowerSupply return codes)
//--- (YPowerSupply class start)
/**
 * <summary>
 *   YPowerSupply Class: PowerSupply function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to change the voltage set point,
 *   the current limit and the enable/disable the output.
 * </para>
 * </summary>
 */
public class YPowerSupply : YFunction
{
//--- (end of YPowerSupply class start)
//--- (YPowerSupply definitions)
    /**
     * <summary>
     *   invalid voltageSetPoint value
     * </summary>
     */
    public const  double VOLTAGESETPOINT_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid currentLimit value
     * </summary>
     */
    public const  double CURRENTLIMIT_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid powerOutput value
     * </summary>
     */
    public const int POWEROUTPUT_OFF = 0;
    public const int POWEROUTPUT_ON = 1;
    public const int POWEROUTPUT_INVALID = -1;
    /**
     * <summary>
     *   invalid voltageSense value
     * </summary>
     */
    public const int VOLTAGESENSE_INT = 0;
    public const int VOLTAGESENSE_EXT = 1;
    public const int VOLTAGESENSE_INVALID = -1;
    /**
     * <summary>
     *   invalid measuredVoltage value
     * </summary>
     */
    public const  double MEASUREDVOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid measuredCurrent value
     * </summary>
     */
    public const  double MEASUREDCURRENT_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid inputVoltage value
     * </summary>
     */
    public const  double INPUTVOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid vInt value
     * </summary>
     */
    public const  double VINT_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid ldoTemperature value
     * </summary>
     */
    public const  double LDOTEMPERATURE_INVALID = YAPI.INVALID_DOUBLE;
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
    /**
     * <summary>
     *   invalid currentAtStartUp value
     * </summary>
     */
    public const  double CURRENTATSTARTUP_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected double _voltageSetPoint = VOLTAGESETPOINT_INVALID;
    protected double _currentLimit = CURRENTLIMIT_INVALID;
    protected int _powerOutput = POWEROUTPUT_INVALID;
    protected int _voltageSense = VOLTAGESENSE_INVALID;
    protected double _measuredVoltage = MEASUREDVOLTAGE_INVALID;
    protected double _measuredCurrent = MEASUREDCURRENT_INVALID;
    protected double _inputVoltage = INPUTVOLTAGE_INVALID;
    protected double _vInt = VINT_INVALID;
    protected double _ldoTemperature = LDOTEMPERATURE_INVALID;
    protected string _voltageTransition = VOLTAGETRANSITION_INVALID;
    protected double _voltageAtStartUp = VOLTAGEATSTARTUP_INVALID;
    protected double _currentAtStartUp = CURRENTATSTARTUP_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackPowerSupply = null;

    public new delegate Task ValueCallback(YPowerSupply func, string value);
    public new delegate Task TimedReportCallback(YPowerSupply func, YMeasure measure);
    //--- (end of YPowerSupply definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPowerSupply(YAPIContext ctx, string func)
        : base(ctx, func, "PowerSupply")
    {
        //--- (YPowerSupply attributes initialization)
        //--- (end of YPowerSupply attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPowerSupply(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPowerSupply implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("voltageSetPoint")) {
            _voltageSetPoint = Math.Round(json_val.getDouble("voltageSetPoint") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("currentLimit")) {
            _currentLimit = Math.Round(json_val.getDouble("currentLimit") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("powerOutput")) {
            _powerOutput = json_val.getInt("powerOutput") > 0 ? 1 : 0;
        }
        if (json_val.has("voltageSense")) {
            _voltageSense = json_val.getInt("voltageSense");
        }
        if (json_val.has("measuredVoltage")) {
            _measuredVoltage = Math.Round(json_val.getDouble("measuredVoltage") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("measuredCurrent")) {
            _measuredCurrent = Math.Round(json_val.getDouble("measuredCurrent") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("inputVoltage")) {
            _inputVoltage = Math.Round(json_val.getDouble("inputVoltage") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("vInt")) {
            _vInt = Math.Round(json_val.getDouble("vInt") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("ldoTemperature")) {
            _ldoTemperature = Math.Round(json_val.getDouble("ldoTemperature") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("voltageTransition")) {
            _voltageTransition = json_val.getString("voltageTransition");
        }
        if (json_val.has("voltageAtStartUp")) {
            _voltageAtStartUp = Math.Round(json_val.getDouble("voltageAtStartUp") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("currentAtStartUp")) {
            _currentAtStartUp = Math.Round(json_val.getDouble("currentAtStartUp") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the voltage set point, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the voltage set point, in V
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
    public async Task<int> set_voltageSetPoint(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("voltageSetPoint",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the voltage set point, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the voltage set point, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.VOLTAGESETPOINT_INVALID</c>.
     * </para>
     */
    public async Task<double> get_voltageSetPoint()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGESETPOINT_INVALID;
            }
        }
        res = _voltageSetPoint;
        return res;
    }


    /**
     * <summary>
     *   Changes the current limit, in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current limit, in mA
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
    public async Task<int> set_currentLimit(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentLimit",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current limit, in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current limit, in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.CURRENTLIMIT_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentLimit()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTLIMIT_INVALID;
            }
        }
        res = _currentLimit;
        return res;
    }


    /**
     * <summary>
     *   Returns the power supply output switch state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPowerSupply.POWEROUTPUT_OFF</c> or <c>YPowerSupply.POWEROUTPUT_ON</c>, according to the
     *   power supply output switch state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.POWEROUTPUT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_powerOutput()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWEROUTPUT_INVALID;
            }
        }
        res = _powerOutput;
        return res;
    }


    /**
     * <summary>
     *   Changes the power supply output switch state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPowerSupply.POWEROUTPUT_OFF</c> or <c>YPowerSupply.POWEROUTPUT_ON</c>, according to the
     *   power supply output switch state
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
    public async Task<int> set_powerOutput(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("powerOutput",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the output voltage control point.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPowerSupply.VOLTAGESENSE_INT</c> or <c>YPowerSupply.VOLTAGESENSE_EXT</c>, according to
     *   the output voltage control point
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.VOLTAGESENSE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_voltageSense()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGESENSE_INVALID;
            }
        }
        res = _voltageSense;
        return res;
    }


    /**
     * <summary>
     *   Changes the voltage control point.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPowerSupply.VOLTAGESENSE_INT</c> or <c>YPowerSupply.VOLTAGESENSE_EXT</c>, according to
     *   the voltage control point
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
    public async Task<int> set_voltageSense(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("voltageSense",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the measured output voltage, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the measured output voltage, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.MEASUREDVOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_measuredVoltage()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MEASUREDVOLTAGE_INVALID;
            }
        }
        res = _measuredVoltage;
        return res;
    }


    /**
     * <summary>
     *   Returns the measured output current, in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the measured output current, in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.MEASUREDCURRENT_INVALID</c>.
     * </para>
     */
    public async Task<double> get_measuredCurrent()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MEASUREDCURRENT_INVALID;
            }
        }
        res = _measuredCurrent;
        return res;
    }


    /**
     * <summary>
     *   Returns the measured input voltage, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the measured input voltage, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.INPUTVOLTAGE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_inputVoltage()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return INPUTVOLTAGE_INVALID;
            }
        }
        res = _inputVoltage;
        return res;
    }


    /**
     * <summary>
     *   Returns the internal voltage, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the internal voltage, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.VINT_INVALID</c>.
     * </para>
     */
    public async Task<double> get_vInt()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VINT_INVALID;
            }
        }
        res = _vInt;
        return res;
    }


    /**
     * <summary>
     *   Returns the LDO temperature, in Celsius.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the LDO temperature, in Celsius
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.LDOTEMPERATURE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_ldoTemperature()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LDOTEMPERATURE_INVALID;
            }
        }
        res = _ldoTemperature;
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
     *   Changes the voltage set point at device start up.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call has no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the voltage set point at device start up
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
     *   Returns the selected voltage set point at device startup, in V.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the selected voltage set point at device startup, in V
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.VOLTAGEATSTARTUP_INVALID</c>.
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
     *   Changes the current limit at device start up.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call has no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current limit at device start up
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
     *   Returns the selected current limit at device startup, in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the selected current limit at device startup, in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPowerSupply.CURRENTATSTARTUP_INVALID</c>.
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
     *   Retrieves a regulated power supply for a given identifier.
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
     *   This function does not require that the regulated power supply is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPowerSupply.isOnline()</c> to test if the regulated power supply is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a regulated power supply by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the regulated power supply
     * </param>
     * <returns>
     *   a <c>YPowerSupply</c> object allowing you to drive the regulated power supply.
     * </returns>
     */
    public static YPowerSupply FindPowerSupply(string func)
    {
        YPowerSupply obj;
        obj = (YPowerSupply) YFunction._FindFromCache("PowerSupply", func);
        if (obj == null) {
            obj = new YPowerSupply(func);
            YFunction._AddToCache("PowerSupply",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a regulated power supply for a given identifier in a YAPI context.
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
     *   This function does not require that the regulated power supply is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPowerSupply.isOnline()</c> to test if the regulated power supply is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a regulated power supply by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the regulated power supply
     * </param>
     * <returns>
     *   a <c>YPowerSupply</c> object allowing you to drive the regulated power supply.
     * </returns>
     */
    public static YPowerSupply FindPowerSupplyInContext(YAPIContext yctx,string func)
    {
        YPowerSupply obj;
        obj = (YPowerSupply) YFunction._FindFromCacheInContext(yctx,  "PowerSupply", func);
        if (obj == null) {
            obj = new YPowerSupply(yctx, func);
            YFunction._AddToCache("PowerSupply",  func, obj);
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
        _valueCallbackPowerSupply = callback;
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
        if (_valueCallbackPowerSupply != null) {
            await _valueCallbackPowerSupply(this, value);
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
     *   <c>YAPI_SUCCESS</c> when the call succeeds.
     * </returns>
     */
    public virtual async Task<int> voltageMove(double V_target,int ms_duration)
    {
        string newval;
        if (V_target < 0.0) {
            V_target  = 0.0;
        }
        newval = ""+Convert.ToString( (int) Math.Round(V_target*65536))+":"+Convert.ToString(ms_duration);

        return await this.set_voltageTransition(newval);
    }

    /**
     * <summary>
     *   Continues the enumeration of regulated power supplies started using <c>yFirstPowerSupply()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPowerSupply</c> object, corresponding to
     *   a regulated power supply currently online, or a <c>null</c> pointer
     *   if there are no more regulated power supplies to enumerate.
     * </returns>
     */
    public YPowerSupply nextPowerSupply()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPowerSupplyInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of regulated power supplies currently accessible.
     * <para>
     *   Use the method <c>YPowerSupply.nextPowerSupply()</c> to iterate on
     *   next regulated power supplies.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPowerSupply</c> object, corresponding to
     *   the first regulated power supply currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPowerSupply FirstPowerSupply()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PowerSupply");
        if (next_hwid == null)  return null;
        return FindPowerSupplyInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of regulated power supplies currently accessible.
     * <para>
     *   Use the method <c>YPowerSupply.nextPowerSupply()</c> to iterate on
     *   next regulated power supplies.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPowerSupply</c> object, corresponding to
     *   the first regulated power supply currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPowerSupply FirstPowerSupplyInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PowerSupply");
        if (next_hwid == null)  return null;
        return FindPowerSupplyInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPowerSupply implementation)
}
}

