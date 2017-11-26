/*********************************************************************
 *
 * $Id: YColorLed.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindColorLed(), the high-level API for ColorLed functions
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

//--- (YColorLed return codes)
//--- (end of YColorLed return codes)
//--- (YColorLed class start)
/**
 * <summary>
 *   YColorLed Class: ColorLed function interface
 * <para>
 *   The Yoctopuce application programming interface
 *   allows you to drive a color LED using RGB coordinates as well as HSL coordinates.
 *   The module performs all conversions form RGB to HSL automatically. It is then
 *   self-evident to turn on a LED with a given hue and to progressively vary its
 *   saturation or lightness. If needed, you can find more information on the
 *   difference between RGB and HSL in the section following this one.
 * </para>
 * </summary>
 */
public class YColorLed : YFunction
{
//--- (end of YColorLed class start)
//--- (YColorLed definitions)
    public class YMove
    {
        public int target = YAPI.INVALID_INT;
        public int ms = YAPI.INVALID_INT;
        public int moving = YAPI.INVALID_UINT;
        public YMove(){}
    }

    /**
     * <summary>
     *   invalid rgbColor value
     * </summary>
     */
    public const  int RGBCOLOR_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid hslColor value
     * </summary>
     */
    public const  int HSLCOLOR_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid rgbColorAtPowerOn value
     * </summary>
     */
    public const  int RGBCOLORATPOWERON_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinkSeqSize value
     * </summary>
     */
    public const  int BLINKSEQSIZE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinkSeqMaxSize value
     * </summary>
     */
    public const  int BLINKSEQMAXSIZE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinkSeqSignature value
     * </summary>
     */
    public const  int BLINKSEQSIGNATURE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    public static readonly YMove RGBMOVE_INVALID = null;
    public static readonly YMove HSLMOVE_INVALID = null;
    protected int _rgbColor = RGBCOLOR_INVALID;
    protected int _hslColor = HSLCOLOR_INVALID;
    protected YMove _rgbMove = new YMove();
    protected YMove _hslMove = new YMove();
    protected int _rgbColorAtPowerOn = RGBCOLORATPOWERON_INVALID;
    protected int _blinkSeqSize = BLINKSEQSIZE_INVALID;
    protected int _blinkSeqMaxSize = BLINKSEQMAXSIZE_INVALID;
    protected int _blinkSeqSignature = BLINKSEQSIGNATURE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackColorLed = null;

    public new delegate Task ValueCallback(YColorLed func, string value);
    public new delegate Task TimedReportCallback(YColorLed func, YMeasure measure);
    //--- (end of YColorLed definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YColorLed(YAPIContext ctx, string func)
        : base(ctx, func, "ColorLed")
    {
        //--- (YColorLed attributes initialization)
        //--- (end of YColorLed attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YColorLed(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YColorLed implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("rgbColor")) {
            _rgbColor = json_val.getInt("rgbColor");
        }
        if (json_val.has("hslColor")) {
            _hslColor = json_val.getInt("hslColor");
        }
        if (json_val.has("rgbColorAtPowerOn")) {
            _rgbColorAtPowerOn = json_val.getInt("rgbColorAtPowerOn");
        }
        if (json_val.has("blinkSeqSize")) {
            _blinkSeqSize = json_val.getInt("blinkSeqSize");
        }
        if (json_val.has("blinkSeqMaxSize")) {
            _blinkSeqMaxSize = json_val.getInt("blinkSeqMaxSize");
        }
        if (json_val.has("blinkSeqSignature")) {
            _blinkSeqSignature = json_val.getInt("blinkSeqSignature");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   invalid rgbMove
     * </summary>
     */
    /**
     * <summary>
     *   invalid hslMove
     * </summary>
     */
    /**
     * <summary>
     *   Returns the current RGB color of the LED.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current RGB color of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.RGBCOLOR_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rgbColor()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RGBCOLOR_INVALID;
            }
        }
        res = _rgbColor;
        return res;
    }


    /**
     * <summary>
     *   Changes the current color of the LED, using an RGB color.
     * <para>
     *   Encoding is done as follows: 0xRRGGBB.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current color of the LED, using an RGB color
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
    public async Task<int> set_rgbColor(int  newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        await _setAttr("rgbColor",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current HSL color of the LED.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current HSL color of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.HSLCOLOR_INVALID</c>.
     * </para>
     */
    public async Task<int> get_hslColor()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HSLCOLOR_INVALID;
            }
        }
        res = _hslColor;
        return res;
    }


    /**
     * <summary>
     *   Changes the current color of the LED, using a color HSL.
     * <para>
     *   Encoding is done as follows: 0xHHSSLL.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current color of the LED, using a color HSL
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
    public async Task<int> set_hslColor(int  newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        await _setAttr("hslColor",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the configured color to be displayed when the module is turned on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the configured color to be displayed when the module is turned on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.RGBCOLORATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rgbColorAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RGBCOLORATPOWERON_INVALID;
            }
        }
        res = _rgbColorAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Changes the color that the LED will display by default when the module is turned on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the color that the LED will display by default when the module is turned on
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
    public async Task<int> set_rgbColorAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        await _setAttr("rgbColorAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current length of the blinking sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current length of the blinking sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQSIZE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_blinkSeqSize()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQSIZE_INVALID;
            }
        }
        res = _blinkSeqSize;
        return res;
    }


    /**
     * <summary>
     *   Returns the maximum length of the blinking sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum length of the blinking sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQMAXSIZE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_blinkSeqMaxSize()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQMAXSIZE_INVALID;
            }
        }
        res = _blinkSeqMaxSize;
        return res;
    }


    /**
     * <summary>
     *   Return the blinking sequence signature.
     * <para>
     *   Since blinking
     *   sequences cannot be read from the device, this can be used
     *   to detect if a specific blinking sequence is already
     *   programmed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQSIGNATURE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_blinkSeqSignature()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQSIGNATURE_INVALID;
            }
        }
        res = _blinkSeqSignature;
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
     *   Retrieves an RGB LED for a given identifier.
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
     *   This function does not require that the RGB LED is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLed.isOnline()</c> to test if the RGB LED is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an RGB LED by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the RGB LED
     * </param>
     * <returns>
     *   a <c>YColorLed</c> object allowing you to drive the RGB LED.
     * </returns>
     */
    public static YColorLed FindColorLed(string func)
    {
        YColorLed obj;
        obj = (YColorLed) YFunction._FindFromCache("ColorLed", func);
        if (obj == null) {
            obj = new YColorLed(func);
            YFunction._AddToCache("ColorLed",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an RGB LED for a given identifier in a YAPI context.
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
     *   This function does not require that the RGB LED is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLed.isOnline()</c> to test if the RGB LED is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an RGB LED by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the RGB LED
     * </param>
     * <returns>
     *   a <c>YColorLed</c> object allowing you to drive the RGB LED.
     * </returns>
     */
    public static YColorLed FindColorLedInContext(YAPIContext yctx,string func)
    {
        YColorLed obj;
        obj = (YColorLed) YFunction._FindFromCacheInContext(yctx,  "ColorLed", func);
        if (obj == null) {
            obj = new YColorLed(yctx, func);
            YFunction._AddToCache("ColorLed",  func, obj);
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
        _valueCallbackColorLed = callback;
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
        if (_valueCallbackColorLed != null) {
            await _valueCallbackColorLed(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> sendCommand(string command)
    {
        return await this.set_command(command);
    }

    /**
     * <summary>
     *   Add a new transition to the blinking sequence, the move will
     *   be performed in the HSL space.
     * <para>
     * </para>
     * </summary>
     * <param name="HSLcolor">
     *   desired HSL color when the traisntion is completed
     * </param>
     * <param name="msDelay">
     *   duration of the color transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addHslMoveToBlinkSeq(int HSLcolor,int msDelay)
    {
        return await this.sendCommand("H"+Convert.ToString(HSLcolor)+","+Convert.ToString(msDelay));
    }

    /**
     * <summary>
     *   Adds a new transition to the blinking sequence, the move is
     *   performed in the RGB space.
     * <para>
     * </para>
     * </summary>
     * <param name="RGBcolor">
     *   desired RGB color when the transition is completed
     * </param>
     * <param name="msDelay">
     *   duration of the color transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addRgbMoveToBlinkSeq(int RGBcolor,int msDelay)
    {
        return await this.sendCommand("R"+Convert.ToString(RGBcolor)+","+Convert.ToString(msDelay));
    }

    /**
     * <summary>
     *   Starts the preprogrammed blinking sequence.
     * <para>
     *   The sequence is
     *   run in a loop until it is stopped by stopBlinkSeq or an explicit
     *   change.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> startBlinkSeq()
    {
        return await this.sendCommand("S");
    }

    /**
     * <summary>
     *   Stops the preprogrammed blinking sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> stopBlinkSeq()
    {
        return await this.sendCommand("X");
    }

    /**
     * <summary>
     *   Resets the preprogrammed blinking sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> resetBlinkSeq()
    {
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Continues the enumeration of RGB LEDs started using <c>yFirstColorLed()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLed</c> object, corresponding to
     *   an RGB LED currently online, or a <c>null</c> pointer
     *   if there are no more RGB LEDs to enumerate.
     * </returns>
     */
    public YColorLed nextColorLed()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindColorLedInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RGB LEDs currently accessible.
     * <para>
     *   Use the method <c>YColorLed.nextColorLed()</c> to iterate on
     *   next RGB LEDs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLed</c> object, corresponding to
     *   the first RGB LED currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLed FirstColorLed()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ColorLed");
        if (next_hwid == null)  return null;
        return FindColorLedInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RGB LEDs currently accessible.
     * <para>
     *   Use the method <c>YColorLed.nextColorLed()</c> to iterate on
     *   next RGB LEDs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YColorLed</c> object, corresponding to
     *   the first RGB LED currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLed FirstColorLedInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ColorLed");
        if (next_hwid == null)  return null;
        return FindColorLedInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YColorLed implementation)
}
}

