/*********************************************************************
 *
 * $Id: YAltitude.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindAltitude(), the high-level API for Altitude functions
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

//--- (YAltitude return codes)
//--- (end of YAltitude return codes)
//--- (YAltitude class start)
/**
 * <summary>
 *   YAltitude Class: Altitude function interface
 * <para>
 *   The Yoctopuce class YAltitude allows you to read and configure Yoctopuce altitude
 *   sensors. It inherits from the YSensor class the core functions to read measurements,
 *   register callback functions, access to the autonomous datalogger.
 *   This class adds the ability to configure the barometric pressure adjusted to
 *   sea level (QNH) for barometric sensors.
 * </para>
 * </summary>
 */
public class YAltitude : YSensor
{
//--- (end of YAltitude class start)
//--- (YAltitude definitions)
    /**
     * <summary>
     *   invalid qnh value
     * </summary>
     */
    public const  double QNH_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid technology value
     * </summary>
     */
    public const  string TECHNOLOGY_INVALID = YAPI.INVALID_STRING;
    protected double _qnh = QNH_INVALID;
    protected string _technology = TECHNOLOGY_INVALID;
    protected ValueCallback _valueCallbackAltitude = null;
    protected TimedReportCallback _timedReportCallbackAltitude = null;

    public new delegate Task ValueCallback(YAltitude func, string value);
    public new delegate Task TimedReportCallback(YAltitude func, YMeasure measure);
    //--- (end of YAltitude definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAltitude(YAPIContext ctx, string func)
        : base(ctx, func, "Altitude")
    {
        //--- (YAltitude attributes initialization)
        //--- (end of YAltitude attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAltitude(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAltitude implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("qnh")) {
            _qnh = Math.Round(json_val.GetDouble("qnh") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("technology")) {
            _technology = json_val.GetString("technology");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the current estimated altitude.
     * <para>
     *   This allows to compensate for
     *   ambient pressure variations and to work in relative mode.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current estimated altitude
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
    public async Task<int> set_currentValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the barometric pressure adjusted to sea level used to compute
     *   the altitude (QNH).
     * <para>
     *   This enables you to compensate for atmospheric pressure
     *   changes due to weather conditions.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the barometric pressure adjusted to sea level used to compute
     *   the altitude (QNH)
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
    public async Task<int> set_qnh(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("qnh",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the barometric pressure adjusted to sea level used to compute
     *   the altitude (QNH).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the barometric pressure adjusted to sea level used to compute
     *   the altitude (QNH)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAltitude.QNH_INVALID</c>.
     * </para>
     */
    public async Task<double> get_qnh()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return QNH_INVALID;
            }
        }
        return _qnh;
    }


    /**
     * <summary>
     *   Returns the technology used by the sesnor to compute
     *   altitude.
     * <para>
     *   Possibles values are  "barometric" and "gps"
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the technology used by the sesnor to compute
     *   altitude
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAltitude.TECHNOLOGY_INVALID</c>.
     * </para>
     */
    public async Task<string> get_technology()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TECHNOLOGY_INVALID;
            }
        }
        return _technology;
    }


    /**
     * <summary>
     *   Retrieves an altimeter for a given identifier.
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
     *   This function does not require that the altimeter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAltitude.isOnline()</c> to test if the altimeter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an altimeter by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the altimeter
     * </param>
     * <returns>
     *   a <c>YAltitude</c> object allowing you to drive the altimeter.
     * </returns>
     */
    public static YAltitude FindAltitude(string func)
    {
        YAltitude obj;
        obj = (YAltitude) YFunction._FindFromCache("Altitude", func);
        if (obj == null) {
            obj = new YAltitude(func);
            YFunction._AddToCache("Altitude",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an altimeter for a given identifier in a YAPI context.
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
     *   This function does not require that the altimeter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAltitude.isOnline()</c> to test if the altimeter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an altimeter by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the altimeter
     * </param>
     * <returns>
     *   a <c>YAltitude</c> object allowing you to drive the altimeter.
     * </returns>
     */
    public static YAltitude FindAltitudeInContext(YAPIContext yctx,string func)
    {
        YAltitude obj;
        obj = (YAltitude) YFunction._FindFromCacheInContext(yctx,  "Altitude", func);
        if (obj == null) {
            obj = new YAltitude(yctx, func);
            YFunction._AddToCache("Altitude",  func, obj);
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
        _valueCallbackAltitude = callback;
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
        if (_valueCallbackAltitude != null) {
            await _valueCallbackAltitude(this, value);
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
        _timedReportCallbackAltitude = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackAltitude != null) {
            await _timedReportCallbackAltitude(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of altimeters started using <c>yFirstAltitude()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAltitude</c> object, corresponding to
     *   an altimeter currently online, or a <c>null</c> pointer
     *   if there are no more altimeters to enumerate.
     * </returns>
     */
    public YAltitude nextAltitude()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAltitudeInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of altimeters currently accessible.
     * <para>
     *   Use the method <c>YAltitude.nextAltitude()</c> to iterate on
     *   next altimeters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAltitude</c> object, corresponding to
     *   the first altimeter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAltitude FirstAltitude()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Altitude");
        if (next_hwid == null)  return null;
        return FindAltitudeInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of altimeters currently accessible.
     * <para>
     *   Use the method <c>YAltitude.nextAltitude()</c> to iterate on
     *   next altimeters.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAltitude</c> object, corresponding to
     *   the first altimeter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAltitude FirstAltitudeInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Altitude");
        if (next_hwid == null)  return null;
        return FindAltitudeInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAltitude implementation)
}
}

