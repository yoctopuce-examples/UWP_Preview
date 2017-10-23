/*********************************************************************
 *
 * $Id: YDaisyChain.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindDaisyChain(), the high-level API for DaisyChain functions
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

//--- (YDaisyChain return codes)
//--- (end of YDaisyChain return codes)
//--- (YDaisyChain class start)
/**
 * <summary>
 *   YDaisyChain Class: DaisyChain function interface
 * <para>
 *   The YDaisyChain interface can be used to verify that devices that
 *   are daisy-chained directly from device to device, without a hub,
 *   are detected properly.
 * </para>
 * </summary>
 */
public class YDaisyChain : YFunction
{
//--- (end of YDaisyChain class start)
//--- (YDaisyChain definitions)
    /**
     * <summary>
     *   invalid daisyState value
     * </summary>
     */
    public const int DAISYSTATE_READY = 0;
    public const int DAISYSTATE_IS_CHILD = 1;
    public const int DAISYSTATE_FIRMWARE_MISMATCH = 2;
    public const int DAISYSTATE_CHILD_MISSING = 3;
    public const int DAISYSTATE_CHILD_LOST = 4;
    public const int DAISYSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid childCount value
     * </summary>
     */
    public const  int CHILDCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid requiredChildCount value
     * </summary>
     */
    public const  int REQUIREDCHILDCOUNT_INVALID = YAPI.INVALID_UINT;
    protected int _daisyState = DAISYSTATE_INVALID;
    protected int _childCount = CHILDCOUNT_INVALID;
    protected int _requiredChildCount = REQUIREDCHILDCOUNT_INVALID;
    protected ValueCallback _valueCallbackDaisyChain = null;

    public new delegate Task ValueCallback(YDaisyChain func, string value);
    public new delegate Task TimedReportCallback(YDaisyChain func, YMeasure measure);
    //--- (end of YDaisyChain definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YDaisyChain(YAPIContext ctx, string func)
        : base(ctx, func, "DaisyChain")
    {
        //--- (YDaisyChain attributes initialization)
        //--- (end of YDaisyChain attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YDaisyChain(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YDaisyChain implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("daisyState")) {
            _daisyState = json_val.GetInt("daisyState");
        }
        if (json_val.Has("childCount")) {
            _childCount = json_val.GetInt("childCount");
        }
        if (json_val.Has("requiredChildCount")) {
            _requiredChildCount = json_val.GetInt("requiredChildCount");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the state of the daisy-link between modules.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDaisyChain.DAISYSTATE_READY</c>, <c>YDaisyChain.DAISYSTATE_IS_CHILD</c>,
     *   <c>YDaisyChain.DAISYSTATE_FIRMWARE_MISMATCH</c>, <c>YDaisyChain.DAISYSTATE_CHILD_MISSING</c> and
     *   <c>YDaisyChain.DAISYSTATE_CHILD_LOST</c> corresponding to the state of the daisy-link between modules
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDaisyChain.DAISYSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_daisyState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DAISYSTATE_INVALID;
            }
        }
        res = _daisyState;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of child nodes currently detected.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of child nodes currently detected
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDaisyChain.CHILDCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_childCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CHILDCOUNT_INVALID;
            }
        }
        res = _childCount;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of child nodes expected in normal conditions.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of child nodes expected in normal conditions
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDaisyChain.REQUIREDCHILDCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_requiredChildCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REQUIREDCHILDCOUNT_INVALID;
            }
        }
        res = _requiredChildCount;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of child nodes expected in normal conditions.
     * <para>
     *   If the value is zero, no check is performed. If it is non-zero, the number
     *   child nodes is checked on startup and the status will change to error if
     *   the count does not match.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of child nodes expected in normal conditions
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
    public async Task<int> set_requiredChildCount(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("requiredChildCount",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a module chain for a given identifier.
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
     *   This function does not require that the module chain is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDaisyChain.isOnline()</c> to test if the module chain is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a module chain by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the module chain
     * </param>
     * <returns>
     *   a <c>YDaisyChain</c> object allowing you to drive the module chain.
     * </returns>
     */
    public static YDaisyChain FindDaisyChain(string func)
    {
        YDaisyChain obj;
        obj = (YDaisyChain) YFunction._FindFromCache("DaisyChain", func);
        if (obj == null) {
            obj = new YDaisyChain(func);
            YFunction._AddToCache("DaisyChain",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a module chain for a given identifier in a YAPI context.
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
     *   This function does not require that the module chain is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDaisyChain.isOnline()</c> to test if the module chain is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a module chain by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the module chain
     * </param>
     * <returns>
     *   a <c>YDaisyChain</c> object allowing you to drive the module chain.
     * </returns>
     */
    public static YDaisyChain FindDaisyChainInContext(YAPIContext yctx,string func)
    {
        YDaisyChain obj;
        obj = (YDaisyChain) YFunction._FindFromCacheInContext(yctx,  "DaisyChain", func);
        if (obj == null) {
            obj = new YDaisyChain(yctx, func);
            YFunction._AddToCache("DaisyChain",  func, obj);
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
        _valueCallbackDaisyChain = callback;
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
        if (_valueCallbackDaisyChain != null) {
            await _valueCallbackDaisyChain(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of module chains started using <c>yFirstDaisyChain()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDaisyChain</c> object, corresponding to
     *   a module chain currently online, or a <c>null</c> pointer
     *   if there are no more module chains to enumerate.
     * </returns>
     */
    public YDaisyChain nextDaisyChain()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindDaisyChainInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of module chains currently accessible.
     * <para>
     *   Use the method <c>YDaisyChain.nextDaisyChain()</c> to iterate on
     *   next module chains.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDaisyChain</c> object, corresponding to
     *   the first module chain currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDaisyChain FirstDaisyChain()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DaisyChain");
        if (next_hwid == null)  return null;
        return FindDaisyChainInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of module chains currently accessible.
     * <para>
     *   Use the method <c>YDaisyChain.nextDaisyChain()</c> to iterate on
     *   next module chains.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YDaisyChain</c> object, corresponding to
     *   the first module chain currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDaisyChain FirstDaisyChainInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DaisyChain");
        if (next_hwid == null)  return null;
        return FindDaisyChainInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YDaisyChain implementation)
}
}

