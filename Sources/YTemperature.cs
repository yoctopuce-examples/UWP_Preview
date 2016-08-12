/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindTemperature(), the high-level API for Temperature functions
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

//--- (YTemperature return codes)
//--- (end of YTemperature return codes)
//--- (YTemperature class start)
/**
 * <summary>
 *   YTemperature Class: Temperature function interface
 * <para>
 *   The Yoctopuce class YTemperature allows you to read and configure Yoctopuce temperature
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   register callback functions, access to the autonomous datalogger.
 *   This class adds the ability to configure some specific parameters for some
 *   sensors (connection type, temperature mapping table).
 * </para>
 * </summary>
 */
public class YTemperature : YSensor
{
//--- (end of YTemperature class start)
//--- (YTemperature definitions)
    /**
     * <summary>
     *   invalid sensorType value
     * </summary>
     */
    public const int SENSORTYPE_DIGITAL = 0;
    public const int SENSORTYPE_TYPE_K = 1;
    public const int SENSORTYPE_TYPE_E = 2;
    public const int SENSORTYPE_TYPE_J = 3;
    public const int SENSORTYPE_TYPE_N = 4;
    public const int SENSORTYPE_TYPE_R = 5;
    public const int SENSORTYPE_TYPE_S = 6;
    public const int SENSORTYPE_TYPE_T = 7;
    public const int SENSORTYPE_PT100_4WIRES = 8;
    public const int SENSORTYPE_PT100_3WIRES = 9;
    public const int SENSORTYPE_PT100_2WIRES = 10;
    public const int SENSORTYPE_RES_OHM = 11;
    public const int SENSORTYPE_RES_NTC = 12;
    public const int SENSORTYPE_RES_LINEAR = 13;
    public const int SENSORTYPE_INVALID = -1;
    /**
     * <summary>
     *   invalid signalValue value
     * </summary>
     */
    public const  double SIGNALVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid signalUnit value
     * </summary>
     */
    public const  string SIGNALUNIT_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _sensorType = SENSORTYPE_INVALID;
    protected double _signalValue = SIGNALVALUE_INVALID;
    protected string _signalUnit = SIGNALUNIT_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackTemperature = null;
    protected TimedReportCallback _timedReportCallbackTemperature = null;

    public new delegate Task ValueCallback(YTemperature func, string value);
    public new delegate Task TimedReportCallback(YTemperature func, YMeasure measure);
    //--- (end of YTemperature definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YTemperature(YAPIContext ctx, string func)
        : base(ctx, func, "Temperature")
    {
        //--- (YTemperature attributes initialization)
        //--- (end of YTemperature attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YTemperature(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YTemperature implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("sensorType")) {
            _sensorType = json_val.GetInt("sensorType");
        }
        if (json_val.Has("signalValue")) {
            _signalValue = Math.Round(json_val.GetDouble("signalValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("signalUnit")) {
            _signalUnit = json_val.GetString("signalUnit");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured temperature.
     * <para>
     *   That unit is a string.
     *   If that strings end with the letter F all temperatures values will returned in
     *   Fahrenheit degrees. If that String ends with the letter K all values will be
     *   returned in Kelvin degrees. If that string ends with the letter C all values will be
     *   returned in Celsius degrees.  If the string ends with any other character the
     *   change will be ignored. Remember to call the
     *   <c>saveToFlash()</c> method of the module if the modification must be kept.
     *   WARNING: if a specific calibration is defined for the temperature function, a
     *   unit system change will probably break it.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured temperature
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
    public async Task<int> set_unit(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("unit",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the temperature sensor type.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YTemperature.SENSORTYPE_DIGITAL</c>, <c>YTemperature.SENSORTYPE_TYPE_K</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_E</c>, <c>YTemperature.SENSORTYPE_TYPE_J</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_N</c>, <c>YTemperature.SENSORTYPE_TYPE_R</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_S</c>, <c>YTemperature.SENSORTYPE_TYPE_T</c>,
     *   <c>YTemperature.SENSORTYPE_PT100_4WIRES</c>, <c>YTemperature.SENSORTYPE_PT100_3WIRES</c>,
     *   <c>YTemperature.SENSORTYPE_PT100_2WIRES</c>, <c>YTemperature.SENSORTYPE_RES_OHM</c>,
     *   <c>YTemperature.SENSORTYPE_RES_NTC</c> and <c>YTemperature.SENSORTYPE_RES_LINEAR</c> corresponding
     *   to the temperature sensor type
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YTemperature.SENSORTYPE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_sensorType()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SENSORTYPE_INVALID;
            }
        }
        return _sensorType;
    }


    /**
     * <summary>
     *   Modifies the temperature sensor type.
     * <para>
     *   This function is used
     *   to define the type of thermocouple (K,E...) used with the device.
     *   It has no effect if module is using a digital sensor or a thermistor.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YTemperature.SENSORTYPE_DIGITAL</c>, <c>YTemperature.SENSORTYPE_TYPE_K</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_E</c>, <c>YTemperature.SENSORTYPE_TYPE_J</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_N</c>, <c>YTemperature.SENSORTYPE_TYPE_R</c>,
     *   <c>YTemperature.SENSORTYPE_TYPE_S</c>, <c>YTemperature.SENSORTYPE_TYPE_T</c>,
     *   <c>YTemperature.SENSORTYPE_PT100_4WIRES</c>, <c>YTemperature.SENSORTYPE_PT100_3WIRES</c>,
     *   <c>YTemperature.SENSORTYPE_PT100_2WIRES</c>, <c>YTemperature.SENSORTYPE_RES_OHM</c>,
     *   <c>YTemperature.SENSORTYPE_RES_NTC</c> and <c>YTemperature.SENSORTYPE_RES_LINEAR</c>
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
    public async Task<int> set_sensorType(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("sensorType",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current value of the electrical signal measured by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current value of the electrical signal measured by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YTemperature.SIGNALVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_signalValue()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALVALUE_INVALID;
            }
        }
        return Math.Round(_signalValue * 1000) / 1000;
    }


    /**
     * <summary>
     *   Returns the measuring unit of the electrical signal used by the sensor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the measuring unit of the electrical signal used by the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YTemperature.SIGNALUNIT_INVALID</c>.
     * </para>
     */
    public async Task<string> get_signalUnit()
    {
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNALUNIT_INVALID;
            }
        }
        return _signalUnit;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        return _command;
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
     *   Retrieves a temperature sensor for a given identifier.
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
     *   This function does not require that the temperature sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YTemperature.isOnline()</c> to test if the temperature sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a temperature sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the temperature sensor
     * </param>
     * <returns>
     *   a <c>YTemperature</c> object allowing you to drive the temperature sensor.
     * </returns>
     */
    public static YTemperature FindTemperature(string func)
    {
        YTemperature obj;
        obj = (YTemperature) YFunction._FindFromCache("Temperature", func);
        if (obj == null) {
            obj = new YTemperature(func);
            YFunction._AddToCache("Temperature",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a temperature sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the temperature sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YTemperature.isOnline()</c> to test if the temperature sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a temperature sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the temperature sensor
     * </param>
     * <returns>
     *   a <c>YTemperature</c> object allowing you to drive the temperature sensor.
     * </returns>
     */
    public static YTemperature FindTemperatureInContext(YAPIContext yctx,string func)
    {
        YTemperature obj;
        obj = (YTemperature) YFunction._FindFromCacheInContext(yctx,  "Temperature", func);
        if (obj == null) {
            obj = new YTemperature(yctx, func);
            YFunction._AddToCache("Temperature",  func, obj);
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
        _valueCallbackTemperature = callback;
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
        if (_valueCallbackTemperature != null) {
            await _valueCallbackTemperature(this, value);
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
        _timedReportCallbackTemperature = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackTemperature != null) {
            await _timedReportCallbackTemperature(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Configure NTC thermistor parameters in order to properly compute the temperature from
     *   the measured resistance.
     * <para>
     *   For increased precision, you can enter a complete mapping
     *   table using set_thermistorResponseTable. This function can only be used with a
     *   temperature sensor based on thermistors.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="res25">
     *   thermistor resistance at 25 degrees Celsius
     * </param>
     * <param name="beta">
     *   Beta value
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_ntcParameters(double res25,double beta)
    {
        double t0;
        double t1;
        double res100;
        List<double> tempValues = new List<double>();
        List<double> resValues = new List<double>();
        t0 = 25.0+275.15;
        t1 = 100.0+275.15;
        res100 = res25 * Math.Exp(beta*(1.0/t1 - 1.0/t0));
        tempValues.Clear();
        resValues.Clear();
        tempValues.Add(25.0);
        resValues.Add(res25);
        tempValues.Add(100.0);
        resValues.Add(res100);
        return await this.set_thermistorResponseTable(tempValues, resValues);
    }

    /**
     * <summary>
     *   Records a thermistor response table, in order to interpolate the temperature from
     *   the measured resistance.
     * <para>
     *   This function can only be used with a temperature
     *   sensor based on thermistors.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, corresponding to all
     *   temperatures (in degrees Celcius) for which the resistance of the
     *   thermistor is specified.
     * </param>
     * <param name="resValues">
     *   array of floating point numbers, corresponding to the resistance
     *   values (in Ohms) for each of the temperature included in the first
     *   argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_thermistorResponseTable(List<double> tempValues,List<double> resValues)
    {
        int siz;
        int res;
        int idx;
        int found;
        double prev;
        double curr;
        double currTemp;
        double idxres;
        siz = tempValues.Count;
        if (!(siz >= 2)) { this._throw( YAPI.INVALID_ARGUMENT, "thermistor response table must have at least two points"); return YAPI.INVALID_ARGUMENT; }
        if (!(siz == resValues.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "table sizes mismatch"); return YAPI.INVALID_ARGUMENT; }
        // may throw an exception
        res = await this.set_command("Z");
        if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to reset thermistor parameters"); return YAPI.IO_ERROR; }
        // add records in growing resistance value
        found = 1;
        prev = 0.0;
        while (found > 0) {
            found = 0;
            curr = 99999999.0;
            currTemp = -999999.0;
            idx = 0;
            while (idx < siz) {
                idxres = resValues[idx];
                if ((idxres > prev) && (idxres < curr)) {
                    curr = idxres;
                    currTemp = tempValues[idx];
                    found = 1;
                }
                idx = idx + 1;
            }
            if (found > 0) {
                res = await this.set_command("m"+Convert.ToString( (int) Math.Round(1000*curr))+":"+Convert.ToString((int) Math.Round(1000*currTemp)));
                if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to reset thermistor parameters"); return YAPI.IO_ERROR; }
                prev = curr;
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves the thermistor response table previously configured using the
     *   <c>set_thermistorResponseTable</c> function.
     * <para>
     *   This function can only be used with a
     *   temperature sensor based on thermistors.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, that is filled by the function
     *   with all temperatures (in degrees Celcius) for which the resistance
     *   of the thermistor is specified.
     * </param>
     * <param name="resValues">
     *   array of floating point numbers, that is filled by the function
     *   with the value (in Ohms) for each of the temperature included in the
     *   first argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadThermistorResponseTable(List<double> tempValues,List<double> resValues)
    {
        string id;
        byte[] bin_json;
        List<string> paramlist = new List<string>();
        List<double> templist = new List<double>();
        int siz;
        int idx;
        double temp;
        int found;
        double prev;
        double curr;
        double currRes;
        tempValues.Clear();
        resValues.Clear();
        // may throw an exception
        id = await this.get_functionId();
        id = (id).Substring( 11, (id).Length - 11);
        bin_json = await this._download("extra.json?page="+id);
        paramlist = this.imm_json_get_array(bin_json);
        // first convert all temperatures to float
        siz = ((paramlist.Count) >> (1));
        templist.Clear();
        idx = 0;
        while (idx < siz) {
            temp = Double.Parse(paramlist[2*idx+1])/1000.0;
            templist.Add(temp);
            idx = idx + 1;
        }
        // then add records in growing temperature value
        tempValues.Clear();
        resValues.Clear();
        found = 1;
        prev = -999999.0;
        while (found > 0) {
            found = 0;
            curr = 999999.0;
            currRes = -999999.0;
            idx = 0;
            while (idx < siz) {
                temp = templist[idx];
                if ((temp > prev) && (temp < curr)) {
                    curr = temp;
                    currRes = Double.Parse(paramlist[2*idx])/1000.0;
                    found = 1;
                }
                idx = idx + 1;
            }
            if (found > 0) {
                tempValues.Add(curr);
                resValues.Add(currRes);
                prev = curr;
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of temperature sensors started using <c>yFirstTemperature()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YTemperature</c> object, corresponding to
     *   a temperature sensor currently online, or a <c>null</c> pointer
     *   if there are no more temperature sensors to enumerate.
     * </returns>
     */
    public YTemperature nextTemperature()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindTemperatureInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of temperature sensors currently accessible.
     * <para>
     *   Use the method <c>YTemperature.nextTemperature()</c> to iterate on
     *   next temperature sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YTemperature</c> object, corresponding to
     *   the first temperature sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YTemperature FirstTemperature()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Temperature");
        if (next_hwid == null)  return null;
        return FindTemperatureInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of temperature sensors currently accessible.
     * <para>
     *   Use the method <c>YTemperature.nextTemperature()</c> to iterate on
     *   next temperature sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YTemperature</c> object, corresponding to
     *   the first temperature sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YTemperature FirstTemperatureInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Temperature");
        if (next_hwid == null)  return null;
        return FindTemperatureInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YTemperature implementation)
}
}

