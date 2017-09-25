/*********************************************************************
 *
 * $Id: YCarbonDioxide.cs 28559 2017-09-15 15:01:38Z seb $
 *
 * Implements FindCarbonDioxide(), the high-level API for CarbonDioxide functions
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

//--- (YCarbonDioxide return codes)
//--- (end of YCarbonDioxide return codes)
//--- (YCarbonDioxide class start)
/**
 * <summary>
 *   YCarbonDioxide Class: CarbonDioxide function interface
 * <para>
 *   The Yoctopuce class YCarbonDioxide allows you to read and configure Yoctopuce CO2
 *   sensors. It inherits from YSensor class the core functions to read measurements,
 *   to register callback functions,  to access the autonomous datalogger.
 *   This class adds the ability to perform manual calibration if reuired.
 * </para>
 * </summary>
 */
public class YCarbonDioxide : YSensor
{
//--- (end of YCarbonDioxide class start)
//--- (YCarbonDioxide definitions)
    /**
     * <summary>
     *   invalid abcPeriod value
     * </summary>
     */
    public const  int ABCPERIOD_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _abcPeriod = ABCPERIOD_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackCarbonDioxide = null;
    protected TimedReportCallback _timedReportCallbackCarbonDioxide = null;

    public new delegate Task ValueCallback(YCarbonDioxide func, string value);
    public new delegate Task TimedReportCallback(YCarbonDioxide func, YMeasure measure);
    //--- (end of YCarbonDioxide definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCarbonDioxide(YAPIContext ctx, string func)
        : base(ctx, func, "CarbonDioxide")
    {
        //--- (YCarbonDioxide attributes initialization)
        //--- (end of YCarbonDioxide attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCarbonDioxide(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YCarbonDioxide implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("abcPeriod")) {
            _abcPeriod = json_val.GetInt("abcPeriod");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the Automatic Baseline Calibration period, in hours.
     * <para>
     *   A negative value
     *   means that automatic baseline calibration is disabled.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Automatic Baseline Calibration period, in hours
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCarbonDioxide.ABCPERIOD_INVALID</c>.
     * </para>
     */
    public async Task<int> get_abcPeriod()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ABCPERIOD_INVALID;
            }
        }
        res = _abcPeriod;
        return res;
    }


    /**
     * <summary>
     *   Changes Automatic Baseline Calibration period, in hours.
     * <para>
     *   If you need
     *   to disable automatic baseline calibration (for instance when using the
     *   sensor in an environment that is constantly above 400ppm CO2), set the
     *   period to -1. Remember to call the <c>saveToFlash()</c> method of the
     *   module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to Automatic Baseline Calibration period, in hours
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
    public async Task<int> set_abcPeriod(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("abcPeriod",rest_val);
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
     *   Retrieves a CO2 sensor for a given identifier.
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
     *   This function does not require that the CO2 sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCarbonDioxide.isOnline()</c> to test if the CO2 sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a CO2 sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the CO2 sensor
     * </param>
     * <returns>
     *   a <c>YCarbonDioxide</c> object allowing you to drive the CO2 sensor.
     * </returns>
     */
    public static YCarbonDioxide FindCarbonDioxide(string func)
    {
        YCarbonDioxide obj;
        obj = (YCarbonDioxide) YFunction._FindFromCache("CarbonDioxide", func);
        if (obj == null) {
            obj = new YCarbonDioxide(func);
            YFunction._AddToCache("CarbonDioxide",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a CO2 sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the CO2 sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCarbonDioxide.isOnline()</c> to test if the CO2 sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a CO2 sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the CO2 sensor
     * </param>
     * <returns>
     *   a <c>YCarbonDioxide</c> object allowing you to drive the CO2 sensor.
     * </returns>
     */
    public static YCarbonDioxide FindCarbonDioxideInContext(YAPIContext yctx,string func)
    {
        YCarbonDioxide obj;
        obj = (YCarbonDioxide) YFunction._FindFromCacheInContext(yctx,  "CarbonDioxide", func);
        if (obj == null) {
            obj = new YCarbonDioxide(yctx, func);
            YFunction._AddToCache("CarbonDioxide",  func, obj);
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
        _valueCallbackCarbonDioxide = callback;
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
        if (_valueCallbackCarbonDioxide != null) {
            await _valueCallbackCarbonDioxide(this, value);
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
        _timedReportCallbackCarbonDioxide = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackCarbonDioxide != null) {
            await _timedReportCallbackCarbonDioxide(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Triggers a baseline calibration at standard CO2 ambiant level (400ppm).
     * <para>
     *   It is normally not necessary to manually calibrate the sensor, because
     *   the built-in automatic baseline calibration procedure will automatically
     *   fix any long-term drift based on the lowest level of CO2 observed over the
     *   automatic calibration period. However, if you disable automatic baseline
     *   calibration, you may want to manually trigger a calibration from time to
     *   time. Before starting a baseline calibration, make sure to put the sensor
     *   in a standard environment (e.g. outside in fresh air) at around 400ppm.
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
    public virtual async Task<int> triggerBaselineCalibration()
    {
        return await this.set_command("BC");
    }

    public virtual async Task<int> triggetBaselineCalibration()
    {
        return await this.triggerBaselineCalibration();
    }

    /**
     * <summary>
     *   Triggers a zero calibration of the sensor on carbon dioxide-free air.
     * <para>
     *   It is normally not necessary to manually calibrate the sensor, because
     *   the built-in automatic baseline calibration procedure will automatically
     *   fix any long-term drift based on the lowest level of CO2 observed over the
     *   automatic calibration period. However, if you disable automatic baseline
     *   calibration, you may want to manually trigger a calibration from time to
     *   time. Before starting a zero calibration, you should circulate carbon
     *   dioxide-free air within the sensor for a minute or two, using a small pipe
     *   connected to the sensor. Please contact support@yoctopuce.com for more details
     *   on the zero calibration procedure.
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
    public virtual async Task<int> triggerZeroCalibration()
    {
        return await this.set_command("ZC");
    }

    public virtual async Task<int> triggetZeroCalibration()
    {
        return await this.triggerZeroCalibration();
    }

    /**
     * <summary>
     *   Continues the enumeration of CO2 sensors started using <c>yFirstCarbonDioxide()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCarbonDioxide</c> object, corresponding to
     *   a CO2 sensor currently online, or a <c>null</c> pointer
     *   if there are no more CO2 sensors to enumerate.
     * </returns>
     */
    public YCarbonDioxide nextCarbonDioxide()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindCarbonDioxideInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of CO2 sensors currently accessible.
     * <para>
     *   Use the method <c>YCarbonDioxide.nextCarbonDioxide()</c> to iterate on
     *   next CO2 sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCarbonDioxide</c> object, corresponding to
     *   the first CO2 sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCarbonDioxide FirstCarbonDioxide()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("CarbonDioxide");
        if (next_hwid == null)  return null;
        return FindCarbonDioxideInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of CO2 sensors currently accessible.
     * <para>
     *   Use the method <c>YCarbonDioxide.nextCarbonDioxide()</c> to iterate on
     *   next CO2 sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YCarbonDioxide</c> object, corresponding to
     *   the first CO2 sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCarbonDioxide FirstCarbonDioxideInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("CarbonDioxide");
        if (next_hwid == null)  return null;
        return FindCarbonDioxideInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YCarbonDioxide implementation)
}
}

