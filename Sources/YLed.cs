/*********************************************************************
 *
 * $Id: YLed.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindLed(), the high-level API for Led functions
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

//--- (YLed return codes)
//--- (end of YLed return codes)
//--- (YLed class start)
/**
 * <summary>
 *   YLed Class: Led function interface
 * <para>
 *   The Yoctopuce application programming interface
 *   allows you not only to drive the intensity of the LED, but also to
 *   have it blink at various preset frequencies.
 * </para>
 * </summary>
 */
public class YLed : YFunction
{
//--- (end of YLed class start)
//--- (YLed definitions)
    /**
     * <summary>
     *   invalid power value
     * </summary>
     */
    public const int POWER_OFF = 0;
    public const int POWER_ON = 1;
    public const int POWER_INVALID = -1;
    /**
     * <summary>
     *   invalid luminosity value
     * </summary>
     */
    public const  int LUMINOSITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinking value
     * </summary>
     */
    public const int BLINKING_STILL = 0;
    public const int BLINKING_RELAX = 1;
    public const int BLINKING_AWARE = 2;
    public const int BLINKING_RUN = 3;
    public const int BLINKING_CALL = 4;
    public const int BLINKING_PANIC = 5;
    public const int BLINKING_INVALID = -1;
    protected int _power = POWER_INVALID;
    protected int _luminosity = LUMINOSITY_INVALID;
    protected int _blinking = BLINKING_INVALID;
    protected ValueCallback _valueCallbackLed = null;

    public new delegate Task ValueCallback(YLed func, string value);
    public new delegate Task TimedReportCallback(YLed func, YMeasure measure);
    //--- (end of YLed definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLed(YAPIContext ctx, string func)
        : base(ctx, func, "Led")
    {
        //--- (YLed attributes initialization)
        //--- (end of YLed attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLed(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YLed implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("power")) {
            _power = json_val.GetInt("power") > 0 ? 1 : 0;
        }
        if (json_val.Has("luminosity")) {
            _luminosity = json_val.GetInt("luminosity");
        }
        if (json_val.Has("blinking")) {
            _blinking = json_val.GetInt("blinking");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current LED state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YLed.POWER_OFF</c> or <c>YLed.POWER_ON</c>, according to the current LED state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YLed.POWER_INVALID</c>.
     * </para>
     */
    public async Task<int> get_power()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POWER_INVALID;
            }
        }
        res = _power;
        return res;
    }


    /**
     * <summary>
     *   Changes the state of the LED.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YLed.POWER_OFF</c> or <c>YLed.POWER_ON</c>, according to the state of the LED
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
    public async Task<int> set_power(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("power",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current LED intensity (in per cent).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current LED intensity (in per cent)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YLed.LUMINOSITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_luminosity()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LUMINOSITY_INVALID;
            }
        }
        res = _luminosity;
        return res;
    }


    /**
     * <summary>
     *   Changes the current LED intensity (in per cent).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current LED intensity (in per cent)
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
    public async Task<int> set_luminosity(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("luminosity",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current LED signaling mode.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YLed.BLINKING_STILL</c>, <c>YLed.BLINKING_RELAX</c>, <c>YLed.BLINKING_AWARE</c>,
     *   <c>YLed.BLINKING_RUN</c>, <c>YLed.BLINKING_CALL</c> and <c>YLed.BLINKING_PANIC</c> corresponding to
     *   the current LED signaling mode
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YLed.BLINKING_INVALID</c>.
     * </para>
     */
    public async Task<int> get_blinking()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKING_INVALID;
            }
        }
        res = _blinking;
        return res;
    }


    /**
     * <summary>
     *   Changes the current LED signaling mode.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YLed.BLINKING_STILL</c>, <c>YLed.BLINKING_RELAX</c>, <c>YLed.BLINKING_AWARE</c>,
     *   <c>YLed.BLINKING_RUN</c>, <c>YLed.BLINKING_CALL</c> and <c>YLed.BLINKING_PANIC</c> corresponding to
     *   the current LED signaling mode
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
    public async Task<int> set_blinking(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("blinking",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a LED for a given identifier.
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
     *   This function does not require that the LED is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLed.isOnline()</c> to test if the LED is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a LED by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the LED
     * </param>
     * <returns>
     *   a <c>YLed</c> object allowing you to drive the LED.
     * </returns>
     */
    public static YLed FindLed(string func)
    {
        YLed obj;
        obj = (YLed) YFunction._FindFromCache("Led", func);
        if (obj == null) {
            obj = new YLed(func);
            YFunction._AddToCache("Led",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a LED for a given identifier in a YAPI context.
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
     *   This function does not require that the LED is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLed.isOnline()</c> to test if the LED is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a LED by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the LED
     * </param>
     * <returns>
     *   a <c>YLed</c> object allowing you to drive the LED.
     * </returns>
     */
    public static YLed FindLedInContext(YAPIContext yctx,string func)
    {
        YLed obj;
        obj = (YLed) YFunction._FindFromCacheInContext(yctx,  "Led", func);
        if (obj == null) {
            obj = new YLed(yctx, func);
            YFunction._AddToCache("Led",  func, obj);
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
        _valueCallbackLed = callback;
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
        if (_valueCallbackLed != null) {
            await _valueCallbackLed(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of LEDs started using <c>yFirstLed()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLed</c> object, corresponding to
     *   a LED currently online, or a <c>null</c> pointer
     *   if there are no more LEDs to enumerate.
     * </returns>
     */
    public YLed nextLed()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindLedInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of LEDs currently accessible.
     * <para>
     *   Use the method <c>YLed.nextLed()</c> to iterate on
     *   next LEDs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLed</c> object, corresponding to
     *   the first LED currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLed FirstLed()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Led");
        if (next_hwid == null)  return null;
        return FindLedInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of LEDs currently accessible.
     * <para>
     *   Use the method <c>YLed.nextLed()</c> to iterate on
     *   next LEDs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YLed</c> object, corresponding to
     *   the first LED currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLed FirstLedInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Led");
        if (next_hwid == null)  return null;
        return FindLedInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YLed implementation)
}
}

