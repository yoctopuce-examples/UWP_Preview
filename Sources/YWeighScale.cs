/*********************************************************************
 *
 * $Id: YWeighScale.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindWeighScale(), the high-level API for WeighScale functions
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

//--- (YWeighScale return codes)
//--- (end of YWeighScale return codes)
//--- (YWeighScale class start)
/**
 * <summary>
 *   YWeighScale Class: WeighScale function interface
 * <para>
 *   The YWeighScale class provides a weight measurement from a ratiometric load cell
 *   sensor. It can be used to control the bridge excitation parameters, in order to avoid
 *   measure shifts caused by temperature variation in the electronics, and can also
 *   automatically apply an additional correction factor based on temperature to
 *   compensate for offsets in the load cell itself.
 * </para>
 * </summary>
 */
public class YWeighScale : YSensor
{
//--- (end of YWeighScale class start)
//--- (YWeighScale definitions)
    /**
     * <summary>
     *   invalid excitation value
     * </summary>
     */
    public const int EXCITATION_OFF = 0;
    public const int EXCITATION_DC = 1;
    public const int EXCITATION_AC = 2;
    public const int EXCITATION_INVALID = -1;
    /**
     * <summary>
     *   invalid adaptRatio value
     * </summary>
     */
    public const  double ADAPTRATIO_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid compTemperature value
     * </summary>
     */
    public const  double COMPTEMPERATURE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid compensation value
     * </summary>
     */
    public const  double COMPENSATION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid zeroTracking value
     * </summary>
     */
    public const  double ZEROTRACKING_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _excitation = EXCITATION_INVALID;
    protected double _adaptRatio = ADAPTRATIO_INVALID;
    protected double _compTemperature = COMPTEMPERATURE_INVALID;
    protected double _compensation = COMPENSATION_INVALID;
    protected double _zeroTracking = ZEROTRACKING_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackWeighScale = null;
    protected TimedReportCallback _timedReportCallbackWeighScale = null;

    public new delegate Task ValueCallback(YWeighScale func, string value);
    public new delegate Task TimedReportCallback(YWeighScale func, YMeasure measure);
    //--- (end of YWeighScale definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWeighScale(YAPIContext ctx, string func)
        : base(ctx, func, "WeighScale")
    {
        //--- (YWeighScale attributes initialization)
        //--- (end of YWeighScale attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YWeighScale(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YWeighScale implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("excitation")) {
            _excitation = json_val.getInt("excitation");
        }
        if (json_val.has("adaptRatio")) {
            _adaptRatio = Math.Round(json_val.getDouble("adaptRatio") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("compTemperature")) {
            _compTemperature = Math.Round(json_val.getDouble("compTemperature") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("compensation")) {
            _compensation = Math.Round(json_val.getDouble("compensation") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("zeroTracking")) {
            _zeroTracking = Math.Round(json_val.getDouble("zeroTracking") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current load cell bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YWeighScale.EXCITATION_OFF</c>, <c>YWeighScale.EXCITATION_DC</c> and
     *   <c>YWeighScale.EXCITATION_AC</c> corresponding to the current load cell bridge excitation method
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWeighScale.EXCITATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_excitation()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return EXCITATION_INVALID;
            }
        }
        res = _excitation;
        return res;
    }


    /**
     * <summary>
     *   Changes the current load cell bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YWeighScale.EXCITATION_OFF</c>, <c>YWeighScale.EXCITATION_DC</c> and
     *   <c>YWeighScale.EXCITATION_AC</c> corresponding to the current load cell bridge excitation method
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
    public async Task<int> set_excitation(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("excitation",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the compensation temperature update rate, in percents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the compensation temperature update rate, in percents
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
    public async Task<int> set_adaptRatio(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("adaptRatio",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the compensation temperature update rate, in percents.
     * <para>
     *   the maximal value is 65 percents.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the compensation temperature update rate, in percents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWeighScale.ADAPTRATIO_INVALID</c>.
     * </para>
     */
    public async Task<double> get_adaptRatio()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ADAPTRATIO_INVALID;
            }
        }
        res = _adaptRatio;
        return res;
    }


    /**
     * <summary>
     *   Returns the current compensation temperature.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current compensation temperature
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWeighScale.COMPTEMPERATURE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_compTemperature()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMPTEMPERATURE_INVALID;
            }
        }
        res = _compTemperature;
        return res;
    }


    /**
     * <summary>
     *   Returns the current current thermal compensation value.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current current thermal compensation value
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWeighScale.COMPENSATION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_compensation()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMPENSATION_INVALID;
            }
        }
        res = _compensation;
        return res;
    }


    /**
     * <summary>
     *   Changes the compensation temperature update rate, in percents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the compensation temperature update rate, in percents
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
    public async Task<int> set_zeroTracking(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("zeroTracking",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the zero tracking threshold value.
     * <para>
     *   When this threshold is larger than
     *   zero, any measure under the threshold will automatically be ignored and the
     *   zero compensation will be updated.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the zero tracking threshold value
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YWeighScale.ZEROTRACKING_INVALID</c>.
     * </para>
     */
    public async Task<double> get_zeroTracking()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ZEROTRACKING_INVALID;
            }
        }
        res = _zeroTracking;
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
     *   Retrieves a weighing scale sensor for a given identifier.
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
     *   This function does not require that the weighing scale sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWeighScale.isOnline()</c> to test if the weighing scale sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a weighing scale sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the weighing scale sensor
     * </param>
     * <returns>
     *   a <c>YWeighScale</c> object allowing you to drive the weighing scale sensor.
     * </returns>
     */
    public static YWeighScale FindWeighScale(string func)
    {
        YWeighScale obj;
        obj = (YWeighScale) YFunction._FindFromCache("WeighScale", func);
        if (obj == null) {
            obj = new YWeighScale(func);
            YFunction._AddToCache("WeighScale",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a weighing scale sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the weighing scale sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YWeighScale.isOnline()</c> to test if the weighing scale sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a weighing scale sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the weighing scale sensor
     * </param>
     * <returns>
     *   a <c>YWeighScale</c> object allowing you to drive the weighing scale sensor.
     * </returns>
     */
    public static YWeighScale FindWeighScaleInContext(YAPIContext yctx,string func)
    {
        YWeighScale obj;
        obj = (YWeighScale) YFunction._FindFromCacheInContext(yctx,  "WeighScale", func);
        if (obj == null) {
            obj = new YWeighScale(yctx, func);
            YFunction._AddToCache("WeighScale",  func, obj);
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
        _valueCallbackWeighScale = callback;
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
        if (_valueCallbackWeighScale != null) {
            await _valueCallbackWeighScale(this, value);
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
        _timedReportCallbackWeighScale = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackWeighScale != null) {
            await _timedReportCallbackWeighScale(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Adapts the load cell signal bias (stored in the corresponding genericSensor)
     *   so that the current signal corresponds to a zero weight.
     * <para>
     * </para>
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
    public virtual async Task<int> tare()
    {
        return await this.set_command("T");
    }

    /**
     * <summary>
     *   Configures the load cell span parameters (stored in the corresponding genericSensor)
     *   so that the current signal corresponds to the specified reference weight.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="currWeight">
     *   reference weight presently on the load cell.
     * </param>
     * <param name="maxWeight">
     *   maximum weight to be expectect on the load cell.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> setupSpan(double currWeight,double maxWeight)
    {
        return await this.set_command("S"+Convert.ToString( (int) Math.Round(1000*currWeight))+":"+Convert.ToString((int) Math.Round(1000*maxWeight)));
    }

    /**
     * <summary>
     *   Records a weight offset thermal compensation table, in order to automatically correct the
     *   measured weight based on the compensation temperature.
     * <para>
     *   The weight correction will be applied by linear interpolation between specified points.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, corresponding to all
     *   temperatures for which an offset correction is specified.
     * </param>
     * <param name="compValues">
     *   array of floating point numbers, corresponding to the offset correction
     *   to apply for each of the temperature included in the first
     *   argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_offsetCompensationTable(List<double> tempValues,List<double> compValues)
    {
        int siz;
        int res;
        int idx;
        int found;
        double prev;
        double curr;
        double currComp;
        double idxTemp;
        siz = tempValues.Count;
        if (!(siz != 1)) { this._throw( YAPI.INVALID_ARGUMENT, "thermal compensation table must have at least two points"); return YAPI.INVALID_ARGUMENT; }
        if (!(siz == compValues.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "table sizes mismatch"); return YAPI.INVALID_ARGUMENT; }

        res = await this.set_command("2Z");
        if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to reset thermal compensation table"); return YAPI.IO_ERROR; }
        // add records in growing temperature value
        found = 1;
        prev = -999999.0;
        while (found > 0) {
            found = 0;
            curr = 99999999.0;
            currComp = -999999.0;
            idx = 0;
            while (idx < siz) {
                idxTemp = tempValues[idx];
                if ((idxTemp > prev) && (idxTemp < curr)) {
                    curr = idxTemp;
                    currComp = compValues[idx];
                    found = 1;
                }
                idx = idx + 1;
            }
            if (found > 0) {
                res = await this.set_command("2m"+Convert.ToString( (int) Math.Round(1000*curr))+":"+Convert.ToString((int) Math.Round(1000*currComp)));
                if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to set thermal compensation table"); return YAPI.IO_ERROR; }
                prev = curr;
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves the weight offset thermal compensation table previously configured using the
     *   <c>set_offsetCompensationTable</c> function.
     * <para>
     *   The weight correction is applied by linear interpolation between specified points.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, that is filled by the function
     *   with all temperatures for which an offset correction is specified.
     * </param>
     * <param name="compValues">
     *   array of floating point numbers, that is filled by the function
     *   with the offset correction applied for each of the temperature
     *   included in the first argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadOffsetCompensationTable(List<double> tempValues,List<double> compValues)
    {
        string id;
        byte[] bin_json;
        List<string> paramlist = new List<string>();
        int siz;
        int idx;
        double temp;
        double comp;

        id = await this.get_functionId();
        id = (id).Substring( 11, (id).Length - 11);
        bin_json = await this._download("extra.json?page=2");
        paramlist = this.imm_json_get_array(bin_json);
        // convert all values to float and append records
        siz = ((paramlist.Count) >> (1));
        tempValues.Clear();
        compValues.Clear();
        idx = 0;
        while (idx < siz) {
            temp = Double.Parse(paramlist[2*idx])/1000.0;
            comp = Double.Parse(paramlist[2*idx+1])/1000.0;
            tempValues.Add(temp);
            compValues.Add(comp);
            idx = idx + 1;
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Records a weight span thermal compensation table, in order to automatically correct the
     *   measured weight based on the compensation temperature.
     * <para>
     *   The weight correction will be applied by linear interpolation between specified points.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, corresponding to all
     *   temperatures for which a span correction is specified.
     * </param>
     * <param name="compValues">
     *   array of floating point numbers, corresponding to the span correction
     *   (in percents) to apply for each of the temperature included in the first
     *   argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_spanCompensationTable(List<double> tempValues,List<double> compValues)
    {
        int siz;
        int res;
        int idx;
        int found;
        double prev;
        double curr;
        double currComp;
        double idxTemp;
        siz = tempValues.Count;
        if (!(siz != 1)) { this._throw( YAPI.INVALID_ARGUMENT, "thermal compensation table must have at least two points"); return YAPI.INVALID_ARGUMENT; }
        if (!(siz == compValues.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "table sizes mismatch"); return YAPI.INVALID_ARGUMENT; }

        res = await this.set_command("3Z");
        if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to reset thermal compensation table"); return YAPI.IO_ERROR; }
        // add records in growing temperature value
        found = 1;
        prev = -999999.0;
        while (found > 0) {
            found = 0;
            curr = 99999999.0;
            currComp = -999999.0;
            idx = 0;
            while (idx < siz) {
                idxTemp = tempValues[idx];
                if ((idxTemp > prev) && (idxTemp < curr)) {
                    curr = idxTemp;
                    currComp = compValues[idx];
                    found = 1;
                }
                idx = idx + 1;
            }
            if (found > 0) {
                res = await this.set_command("3m"+Convert.ToString( (int) Math.Round(1000*curr))+":"+Convert.ToString((int) Math.Round(1000*currComp)));
                if (!(res==YAPI.SUCCESS)) { this._throw( YAPI.IO_ERROR, "unable to set thermal compensation table"); return YAPI.IO_ERROR; }
                prev = curr;
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves the weight span thermal compensation table previously configured using the
     *   <c>set_spanCompensationTable</c> function.
     * <para>
     *   The weight correction is applied by linear interpolation between specified points.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tempValues">
     *   array of floating point numbers, that is filled by the function
     *   with all temperatures for which an span correction is specified.
     * </param>
     * <param name="compValues">
     *   array of floating point numbers, that is filled by the function
     *   with the span correction applied for each of the temperature
     *   included in the first argument, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadSpanCompensationTable(List<double> tempValues,List<double> compValues)
    {
        string id;
        byte[] bin_json;
        List<string> paramlist = new List<string>();
        int siz;
        int idx;
        double temp;
        double comp;

        id = await this.get_functionId();
        id = (id).Substring( 11, (id).Length - 11);
        bin_json = await this._download("extra.json?page=3");
        paramlist = this.imm_json_get_array(bin_json);
        // convert all values to float and append records
        siz = ((paramlist.Count) >> (1));
        tempValues.Clear();
        compValues.Clear();
        idx = 0;
        while (idx < siz) {
            temp = Double.Parse(paramlist[2*idx])/1000.0;
            comp = Double.Parse(paramlist[2*idx+1])/1000.0;
            tempValues.Add(temp);
            compValues.Add(comp);
            idx = idx + 1;
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of weighing scale sensors started using <c>yFirstWeighScale()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWeighScale</c> object, corresponding to
     *   a weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are no more weighing scale sensors to enumerate.
     * </returns>
     */
    public YWeighScale nextWeighScale()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindWeighScaleInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of weighing scale sensors currently accessible.
     * <para>
     *   Use the method <c>YWeighScale.nextWeighScale()</c> to iterate on
     *   next weighing scale sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YWeighScale</c> object, corresponding to
     *   the first weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWeighScale FirstWeighScale()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WeighScale");
        if (next_hwid == null)  return null;
        return FindWeighScaleInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of weighing scale sensors currently accessible.
     * <para>
     *   Use the method <c>YWeighScale.nextWeighScale()</c> to iterate on
     *   next weighing scale sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YWeighScale</c> object, corresponding to
     *   the first weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YWeighScale FirstWeighScaleInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("WeighScale");
        if (next_hwid == null)  return null;
        return FindWeighScaleInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YWeighScale implementation)
}
}

