/*********************************************************************
 *
 * $Id: YOsControl.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindOsControl(), the high-level API for OsControl functions
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

//--- (YOsControl return codes)
//--- (end of YOsControl return codes)
//--- (YOsControl class start)
/**
 * <summary>
 *   YOsControl Class: OS control
 * <para>
 *   The OScontrol object allows some control over the operating system running a VirtualHub.
 *   OsControl is available on the VirtualHub software only. This feature must be activated at the VirtualHub
 *   start up with -o option.
 * </para>
 * </summary>
 */
public class YOsControl : YFunction
{
//--- (end of YOsControl class start)
//--- (YOsControl definitions)
    /**
     * <summary>
     *   invalid shutdownCountdown value
     * </summary>
     */
    public const  int SHUTDOWNCOUNTDOWN_INVALID = YAPI.INVALID_UINT;
    protected int _shutdownCountdown = SHUTDOWNCOUNTDOWN_INVALID;
    protected ValueCallback _valueCallbackOsControl = null;

    public new delegate Task ValueCallback(YOsControl func, string value);
    public new delegate Task TimedReportCallback(YOsControl func, YMeasure measure);
    //--- (end of YOsControl definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YOsControl(YAPIContext ctx, string func)
        : base(ctx, func, "OsControl")
    {
        //--- (YOsControl attributes initialization)
        //--- (end of YOsControl attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YOsControl(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YOsControl implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("shutdownCountdown")) {
            _shutdownCountdown = json_val.GetInt("shutdownCountdown");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the remaining number of seconds before the OS shutdown, or zero when no
     *   shutdown has been scheduled.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the remaining number of seconds before the OS shutdown, or zero when no
     *   shutdown has been scheduled
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YOsControl.SHUTDOWNCOUNTDOWN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_shutdownCountdown()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SHUTDOWNCOUNTDOWN_INVALID;
            }
        }
        res = _shutdownCountdown;
        return res;
    }


    public async Task<int> set_shutdownCountdown(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("shutdownCountdown",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves OS control for a given identifier.
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
     *   This function does not require that the OS control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YOsControl.isOnline()</c> to test if the OS control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   OS control by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the OS control
     * </param>
     * <returns>
     *   a <c>YOsControl</c> object allowing you to drive the OS control.
     * </returns>
     */
    public static YOsControl FindOsControl(string func)
    {
        YOsControl obj;
        obj = (YOsControl) YFunction._FindFromCache("OsControl", func);
        if (obj == null) {
            obj = new YOsControl(func);
            YFunction._AddToCache("OsControl",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves OS control for a given identifier in a YAPI context.
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
     *   This function does not require that the OS control is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YOsControl.isOnline()</c> to test if the OS control is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   OS control by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the OS control
     * </param>
     * <returns>
     *   a <c>YOsControl</c> object allowing you to drive the OS control.
     * </returns>
     */
    public static YOsControl FindOsControlInContext(YAPIContext yctx,string func)
    {
        YOsControl obj;
        obj = (YOsControl) YFunction._FindFromCacheInContext(yctx,  "OsControl", func);
        if (obj == null) {
            obj = new YOsControl(yctx, func);
            YFunction._AddToCache("OsControl",  func, obj);
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
        _valueCallbackOsControl = callback;
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
        if (_valueCallbackOsControl != null) {
            await _valueCallbackOsControl(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Schedules an OS shutdown after a given number of seconds.
     * <para>
     * </para>
     * </summary>
     * <param name="secBeforeShutDown">
     *   number of seconds before shutdown
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> shutdown(int secBeforeShutDown)
    {
        return await this.set_shutdownCountdown(secBeforeShutDown);
    }

    /**
     * <summary>
     *   Continues the enumeration of OS control started using <c>yFirstOsControl()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOsControl</c> object, corresponding to
     *   OS control currently online, or a <c>null</c> pointer
     *   if there are no more OS control to enumerate.
     * </returns>
     */
    public YOsControl nextOsControl()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindOsControlInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of OS control currently accessible.
     * <para>
     *   Use the method <c>YOsControl.nextOsControl()</c> to iterate on
     *   next OS control.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOsControl</c> object, corresponding to
     *   the first OS control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YOsControl FirstOsControl()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("OsControl");
        if (next_hwid == null)  return null;
        return FindOsControlInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of OS control currently accessible.
     * <para>
     *   Use the method <c>YOsControl.nextOsControl()</c> to iterate on
     *   next OS control.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YOsControl</c> object, corresponding to
     *   the first OS control currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YOsControl FirstOsControlInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("OsControl");
        if (next_hwid == null)  return null;
        return FindOsControlInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YOsControl implementation)
}
}

