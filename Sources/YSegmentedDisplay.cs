/*********************************************************************
 *
 * $Id: YSegmentedDisplay.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindSegmentedDisplay(), the high-level API for SegmentedDisplay functions
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

//--- (YSegmentedDisplay return codes)
//--- (end of YSegmentedDisplay return codes)
//--- (YSegmentedDisplay class start)
/**
 * <summary>
 *   YSegmentedDisplay Class: SegmentedDisplay function interface
 * <para>
 *   The SegmentedDisplay class allows you to drive segmented displays.
 * </para>
 * </summary>
 */
public class YSegmentedDisplay : YFunction
{
//--- (end of YSegmentedDisplay class start)
//--- (YSegmentedDisplay definitions)
    /**
     * <summary>
     *   invalid displayedText value
     * </summary>
     */
    public const  string DISPLAYEDTEXT_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid displayMode value
     * </summary>
     */
    public const int DISPLAYMODE_DISCONNECTED = 0;
    public const int DISPLAYMODE_MANUAL = 1;
    public const int DISPLAYMODE_AUTO1 = 2;
    public const int DISPLAYMODE_AUTO60 = 3;
    public const int DISPLAYMODE_INVALID = -1;
    protected string _displayedText = DISPLAYEDTEXT_INVALID;
    protected int _displayMode = DISPLAYMODE_INVALID;
    protected ValueCallback _valueCallbackSegmentedDisplay = null;

    public new delegate Task ValueCallback(YSegmentedDisplay func, string value);
    public new delegate Task TimedReportCallback(YSegmentedDisplay func, YMeasure measure);
    //--- (end of YSegmentedDisplay definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSegmentedDisplay(YAPIContext ctx, string func)
        : base(ctx, func, "SegmentedDisplay")
    {
        //--- (YSegmentedDisplay attributes initialization)
        //--- (end of YSegmentedDisplay attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSegmentedDisplay(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSegmentedDisplay implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("displayedText")) {
            _displayedText = json_val.GetString("displayedText");
        }
        if (json_val.Has("displayMode")) {
            _displayMode = json_val.GetInt("displayMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the text currently displayed on the screen.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the text currently displayed on the screen
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSegmentedDisplay.DISPLAYEDTEXT_INVALID</c>.
     * </para>
     */
    public async Task<string> get_displayedText()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISPLAYEDTEXT_INVALID;
            }
        }
        res = _displayedText;
        return res;
    }


    /**
     * <summary>
     *   Changes the text currently displayed on the screen.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the text currently displayed on the screen
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
    public async Task<int> set_displayedText(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("displayedText",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_displayMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISPLAYMODE_INVALID;
            }
        }
        res = _displayMode;
        return res;
    }


    public async Task<int> set_displayMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("displayMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a segmented display for a given identifier.
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
     *   This function does not require that the segmented displays is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSegmentedDisplay.isOnline()</c> to test if the segmented displays is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a segmented display by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the segmented displays
     * </param>
     * <returns>
     *   a <c>YSegmentedDisplay</c> object allowing you to drive the segmented displays.
     * </returns>
     */
    public static YSegmentedDisplay FindSegmentedDisplay(string func)
    {
        YSegmentedDisplay obj;
        obj = (YSegmentedDisplay) YFunction._FindFromCache("SegmentedDisplay", func);
        if (obj == null) {
            obj = new YSegmentedDisplay(func);
            YFunction._AddToCache("SegmentedDisplay",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a segmented display for a given identifier in a YAPI context.
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
     *   This function does not require that the segmented displays is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSegmentedDisplay.isOnline()</c> to test if the segmented displays is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a segmented display by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the segmented displays
     * </param>
     * <returns>
     *   a <c>YSegmentedDisplay</c> object allowing you to drive the segmented displays.
     * </returns>
     */
    public static YSegmentedDisplay FindSegmentedDisplayInContext(YAPIContext yctx,string func)
    {
        YSegmentedDisplay obj;
        obj = (YSegmentedDisplay) YFunction._FindFromCacheInContext(yctx,  "SegmentedDisplay", func);
        if (obj == null) {
            obj = new YSegmentedDisplay(yctx, func);
            YFunction._AddToCache("SegmentedDisplay",  func, obj);
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
        _valueCallbackSegmentedDisplay = callback;
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
        if (_valueCallbackSegmentedDisplay != null) {
            await _valueCallbackSegmentedDisplay(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of segmented displays started using <c>yFirstSegmentedDisplay()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSegmentedDisplay</c> object, corresponding to
     *   a segmented display currently online, or a <c>null</c> pointer
     *   if there are no more segmented displays to enumerate.
     * </returns>
     */
    public YSegmentedDisplay nextSegmentedDisplay()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSegmentedDisplayInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of segmented displays currently accessible.
     * <para>
     *   Use the method <c>YSegmentedDisplay.nextSegmentedDisplay()</c> to iterate on
     *   next segmented displays.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSegmentedDisplay</c> object, corresponding to
     *   the first segmented displays currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSegmentedDisplay FirstSegmentedDisplay()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SegmentedDisplay");
        if (next_hwid == null)  return null;
        return FindSegmentedDisplayInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of segmented displays currently accessible.
     * <para>
     *   Use the method <c>YSegmentedDisplay.nextSegmentedDisplay()</c> to iterate on
     *   next segmented displays.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSegmentedDisplay</c> object, corresponding to
     *   the first segmented displays currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSegmentedDisplay FirstSegmentedDisplayInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SegmentedDisplay");
        if (next_hwid == null)  return null;
        return FindSegmentedDisplayInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSegmentedDisplay implementation)
}
}

