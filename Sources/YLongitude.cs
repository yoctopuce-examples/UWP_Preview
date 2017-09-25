/*********************************************************************
 *
 * $Id: YLongitude.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindLongitude(), the high-level API for Longitude functions
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

//--- (YLongitude return codes)
//--- (end of YLongitude return codes)
//--- (YLongitude class start)
/**
 * <summary>
 *   YLongitude Class: Longitude function interface
 * <para>
 *   The Yoctopuce class YLongitude allows you to read the longitude from Yoctopuce
 *   geolocalization sensors. It inherits from the YSensor class the core functions to
 *   read measurements, register callback functions, access the autonomous
 *   datalogger.
 * </para>
 * </summary>
 */
public class YLongitude : YSensor
{
//--- (end of YLongitude class start)
//--- (YLongitude definitions)
    protected ValueCallback _valueCallbackLongitude = null;
    protected TimedReportCallback _timedReportCallbackLongitude = null;

    public new delegate Task ValueCallback(YLongitude func, string value);
    public new delegate Task TimedReportCallback(YLongitude func, YMeasure measure);
    //--- (end of YLongitude definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLongitude(YAPIContext ctx, string func)
        : base(ctx, func, "Longitude")
    {
        //--- (YLongitude attributes initialization)
        //--- (end of YLongitude attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLongitude(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YLongitude implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves a longitude sensor for a given identifier.
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
     *   This function does not require that the longitude sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLongitude.isOnline()</c> to test if the longitude sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a longitude sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the longitude sensor
     * </param>
     * <returns>
     *   a <c>YLongitude</c> object allowing you to drive the longitude sensor.
     * </returns>
     */
    public static YLongitude FindLongitude(string func)
    {
        YLongitude obj;
        obj = (YLongitude) YFunction._FindFromCache("Longitude", func);
        if (obj == null) {
            obj = new YLongitude(func);
            YFunction._AddToCache("Longitude",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a longitude sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the longitude sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLongitude.isOnline()</c> to test if the longitude sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a longitude sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the longitude sensor
     * </param>
     * <returns>
     *   a <c>YLongitude</c> object allowing you to drive the longitude sensor.
     * </returns>
     */
    public static YLongitude FindLongitudeInContext(YAPIContext yctx,string func)
    {
        YLongitude obj;
        obj = (YLongitude) YFunction._FindFromCacheInContext(yctx,  "Longitude", func);
        if (obj == null) {
            obj = new YLongitude(yctx, func);
            YFunction._AddToCache("Longitude",  func, obj);
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
        _valueCallbackLongitude = callback;
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
        if (_valueCallbackLongitude != null) {
            await _valueCallbackLongitude(this, value);
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
        _timedReportCallbackLongitude = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackLongitude != null) {
            await _timedReportCallbackLongitude(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of longitude sensors started using <c>yFirstLongitude()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLongitude</c> object, corresponding to
     *   a longitude sensor currently online, or a <c>null</c> pointer
     *   if there are no more longitude sensors to enumerate.
     * </returns>
     */
    public YLongitude nextLongitude()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindLongitudeInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of longitude sensors currently accessible.
     * <para>
     *   Use the method <c>YLongitude.nextLongitude()</c> to iterate on
     *   next longitude sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLongitude</c> object, corresponding to
     *   the first longitude sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLongitude FirstLongitude()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Longitude");
        if (next_hwid == null)  return null;
        return FindLongitudeInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of longitude sensors currently accessible.
     * <para>
     *   Use the method <c>YLongitude.nextLongitude()</c> to iterate on
     *   next longitude sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YLongitude</c> object, corresponding to
     *   the first longitude sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLongitude FirstLongitudeInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Longitude");
        if (next_hwid == null)  return null;
        return FindLongitudeInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YLongitude implementation)
}
}

