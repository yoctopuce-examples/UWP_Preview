/*********************************************************************
 *
 * $Id: YMultiCellWeighScale.cs 29478 2017-12-21 08:10:05Z seb $
 *
 * Implements FindMultiCellWeighScale(), the high-level API for MultiCellWeighScale functions
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

//--- (YMultiCellWeighScale return codes)
//--- (end of YMultiCellWeighScale return codes)
//--- (YMultiCellWeighScale class start)
/**
 * <summary>
 *   YMultiCellWeighScale Class: MultiCellWeighScale function interface
 * <para>
 *   The YMultiCellWeighScale class provides a weight measurement from a set of ratiometric load cells
 *   sensor. It can be used to control the bridge excitation parameters, in order to avoid
 *   measure shifts caused by temperature variation in the electronics, and can also
 *   automatically apply an additional correction factor based on temperature to
 *   compensate for offsets in the load cells themselves.
 * </para>
 * </summary>
 */
public class YMultiCellWeighScale : YSensor
{
//--- (end of YMultiCellWeighScale class start)
//--- (YMultiCellWeighScale definitions)
    /**
     * <summary>
     *   invalid cellCount value
     * </summary>
     */
    public const  int CELLCOUNT_INVALID = YAPI.INVALID_UINT;
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
     *   invalid compTempAdaptRatio value
     * </summary>
     */
    public const  double COMPTEMPADAPTRATIO_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid compTempAvg value
     * </summary>
     */
    public const  double COMPTEMPAVG_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid compTempChg value
     * </summary>
     */
    public const  double COMPTEMPCHG_INVALID = YAPI.INVALID_DOUBLE;
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
    protected int _cellCount = CELLCOUNT_INVALID;
    protected int _excitation = EXCITATION_INVALID;
    protected double _compTempAdaptRatio = COMPTEMPADAPTRATIO_INVALID;
    protected double _compTempAvg = COMPTEMPAVG_INVALID;
    protected double _compTempChg = COMPTEMPCHG_INVALID;
    protected double _compensation = COMPENSATION_INVALID;
    protected double _zeroTracking = ZEROTRACKING_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackMultiCellWeighScale = null;
    protected TimedReportCallback _timedReportCallbackMultiCellWeighScale = null;

    public new delegate Task ValueCallback(YMultiCellWeighScale func, string value);
    public new delegate Task TimedReportCallback(YMultiCellWeighScale func, YMeasure measure);
    //--- (end of YMultiCellWeighScale definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMultiCellWeighScale(YAPIContext ctx, string func)
        : base(ctx, func, "MultiCellWeighScale")
    {
        //--- (YMultiCellWeighScale attributes initialization)
        //--- (end of YMultiCellWeighScale attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMultiCellWeighScale(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YMultiCellWeighScale implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("cellCount")) {
            _cellCount = json_val.getInt("cellCount");
        }
        if (json_val.has("excitation")) {
            _excitation = json_val.getInt("excitation");
        }
        if (json_val.has("compTempAdaptRatio")) {
            _compTempAdaptRatio = Math.Round(json_val.getDouble("compTempAdaptRatio") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("compTempAvg")) {
            _compTempAvg = Math.Round(json_val.getDouble("compTempAvg") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("compTempChg")) {
            _compTempChg = Math.Round(json_val.getDouble("compTempChg") * 1000.0 / 65536.0) / 1000.0;
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
     *   Returns the number of load cells in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of load cells in use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.CELLCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_cellCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CELLCOUNT_INVALID;
            }
        }
        res = _cellCount;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of load cells in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of load cells in use
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
    public async Task<int> set_cellCount(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("cellCount",rest_val);
        return YAPI.SUCCESS;
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
     *   a value among <c>YMultiCellWeighScale.EXCITATION_OFF</c>, <c>YMultiCellWeighScale.EXCITATION_DC</c>
     *   and <c>YMultiCellWeighScale.EXCITATION_AC</c> corresponding to the current load cell bridge excitation method
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.EXCITATION_INVALID</c>.
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
     *   a value among <c>YMultiCellWeighScale.EXCITATION_OFF</c>, <c>YMultiCellWeighScale.EXCITATION_DC</c>
     *   and <c>YMultiCellWeighScale.EXCITATION_AC</c> corresponding to the current load cell bridge excitation method
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
     *   Changes the averaged temperature update rate, in percents.
     * <para>
     *   The averaged temperature is updated every 10 seconds, by applying this adaptation rate
     *   to the difference between the measures ambiant temperature and the current compensation
     *   temperature. The standard rate is 0.04 percents, and the maximal rate is 65 percents.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the averaged temperature update rate, in percents
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
    public async Task<int> set_compTempAdaptRatio(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("compTempAdaptRatio",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the averaged temperature update rate, in percents.
     * <para>
     *   The averaged temperature is updated every 10 seconds, by applying this adaptation rate
     *   to the difference between the measures ambiant temperature and the current compensation
     *   temperature. The standard rate is 0.04 percents, and the maximal rate is 65 percents.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the averaged temperature update rate, in percents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.COMPTEMPADAPTRATIO_INVALID</c>.
     * </para>
     */
    public async Task<double> get_compTempAdaptRatio()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMPTEMPADAPTRATIO_INVALID;
            }
        }
        res = _compTempAdaptRatio;
        return res;
    }


    /**
     * <summary>
     *   Returns the current averaged temperature, used for thermal compensation.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current averaged temperature, used for thermal compensation
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.COMPTEMPAVG_INVALID</c>.
     * </para>
     */
    public async Task<double> get_compTempAvg()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMPTEMPAVG_INVALID;
            }
        }
        res = _compTempAvg;
        return res;
    }


    /**
     * <summary>
     *   Returns the current temperature variation, used for thermal compensation.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current temperature variation, used for thermal compensation
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.COMPTEMPCHG_INVALID</c>.
     * </para>
     */
    public async Task<double> get_compTempChg()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMPTEMPCHG_INVALID;
            }
        }
        res = _compTempChg;
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
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.COMPENSATION_INVALID</c>.
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
     *   On failure, throws an exception or returns <c>YMultiCellWeighScale.ZEROTRACKING_INVALID</c>.
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
     *   Retrieves a multi-cell weighing scale sensor for a given identifier.
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
     *   This function does not require that the multi-cell weighing scale sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMultiCellWeighScale.isOnline()</c> to test if the multi-cell weighing scale sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a multi-cell weighing scale sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the multi-cell weighing scale sensor
     * </param>
     * <returns>
     *   a <c>YMultiCellWeighScale</c> object allowing you to drive the multi-cell weighing scale sensor.
     * </returns>
     */
    public static YMultiCellWeighScale FindMultiCellWeighScale(string func)
    {
        YMultiCellWeighScale obj;
        obj = (YMultiCellWeighScale) YFunction._FindFromCache("MultiCellWeighScale", func);
        if (obj == null) {
            obj = new YMultiCellWeighScale(func);
            YFunction._AddToCache("MultiCellWeighScale",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a multi-cell weighing scale sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the multi-cell weighing scale sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMultiCellWeighScale.isOnline()</c> to test if the multi-cell weighing scale sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a multi-cell weighing scale sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the multi-cell weighing scale sensor
     * </param>
     * <returns>
     *   a <c>YMultiCellWeighScale</c> object allowing you to drive the multi-cell weighing scale sensor.
     * </returns>
     */
    public static YMultiCellWeighScale FindMultiCellWeighScaleInContext(YAPIContext yctx,string func)
    {
        YMultiCellWeighScale obj;
        obj = (YMultiCellWeighScale) YFunction._FindFromCacheInContext(yctx,  "MultiCellWeighScale", func);
        if (obj == null) {
            obj = new YMultiCellWeighScale(yctx, func);
            YFunction._AddToCache("MultiCellWeighScale",  func, obj);
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
        _valueCallbackMultiCellWeighScale = callback;
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
        if (_valueCallbackMultiCellWeighScale != null) {
            await _valueCallbackMultiCellWeighScale(this, value);
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
        _timedReportCallbackMultiCellWeighScale = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackMultiCellWeighScale != null) {
            await _timedReportCallbackMultiCellWeighScale(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Adapts the load cells signal bias (stored in the corresponding genericSensor)
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
     *   Configures the load cells span parameters (stored in the corresponding genericSensors)
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
     *   Continues the enumeration of multi-cell weighing scale sensors started using <c>yFirstMultiCellWeighScale()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMultiCellWeighScale</c> object, corresponding to
     *   a multi-cell weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are no more multi-cell weighing scale sensors to enumerate.
     * </returns>
     */
    public YMultiCellWeighScale nextMultiCellWeighScale()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindMultiCellWeighScaleInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of multi-cell weighing scale sensors currently accessible.
     * <para>
     *   Use the method <c>YMultiCellWeighScale.nextMultiCellWeighScale()</c> to iterate on
     *   next multi-cell weighing scale sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMultiCellWeighScale</c> object, corresponding to
     *   the first multi-cell weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMultiCellWeighScale FirstMultiCellWeighScale()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MultiCellWeighScale");
        if (next_hwid == null)  return null;
        return FindMultiCellWeighScaleInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of multi-cell weighing scale sensors currently accessible.
     * <para>
     *   Use the method <c>YMultiCellWeighScale.nextMultiCellWeighScale()</c> to iterate on
     *   next multi-cell weighing scale sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YMultiCellWeighScale</c> object, corresponding to
     *   the first multi-cell weighing scale sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMultiCellWeighScale FirstMultiCellWeighScaleInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MultiCellWeighScale");
        if (next_hwid == null)  return null;
        return FindMultiCellWeighScaleInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YMultiCellWeighScale implementation)
}
}

