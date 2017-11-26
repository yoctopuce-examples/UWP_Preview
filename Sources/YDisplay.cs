/*********************************************************************
 *
 * $Id: YDisplay.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindDisplay(), the high-level API for Display functions
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
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{


    //--- (generated code: YDisplay return codes)
//--- (end of generated code: YDisplay return codes)
    //--- (generated code: YDisplay class start)
/**
 * <summary>
 *   YDisplay Class: Display function interface
 * <para>
 *   Yoctopuce display interface has been designed to easily
 *   show information and images. The device provides built-in
 *   multi-layer rendering. Layers can be drawn offline, individually,
 *   and freely moved on the display. It can also replay recorded
 *   sequences (animations).
 * </para>
 * </summary>
 */
public class YDisplay : YFunction
{
//--- (end of generated code: YDisplay class start)
        //--- (generated code: YDisplay definitions)
    /**
     * <summary>
     *   invalid enabled value
     * </summary>
     */
    public const int ENABLED_FALSE = 0;
    public const int ENABLED_TRUE = 1;
    public const int ENABLED_INVALID = -1;
    /**
     * <summary>
     *   invalid startupSeq value
     * </summary>
     */
    public const  string STARTUPSEQ_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid brightness value
     * </summary>
     */
    public const  int BRIGHTNESS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid orientation value
     * </summary>
     */
    public const int ORIENTATION_LEFT = 0;
    public const int ORIENTATION_UP = 1;
    public const int ORIENTATION_RIGHT = 2;
    public const int ORIENTATION_DOWN = 3;
    public const int ORIENTATION_INVALID = -1;
    /**
     * <summary>
     *   invalid displayWidth value
     * </summary>
     */
    public const  int DISPLAYWIDTH_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid displayHeight value
     * </summary>
     */
    public const  int DISPLAYHEIGHT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid displayType value
     * </summary>
     */
    public const int DISPLAYTYPE_MONO = 0;
    public const int DISPLAYTYPE_GRAY = 1;
    public const int DISPLAYTYPE_RGB = 2;
    public const int DISPLAYTYPE_INVALID = -1;
    /**
     * <summary>
     *   invalid layerWidth value
     * </summary>
     */
    public const  int LAYERWIDTH_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid layerHeight value
     * </summary>
     */
    public const  int LAYERHEIGHT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid layerCount value
     * </summary>
     */
    public const  int LAYERCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _enabled = ENABLED_INVALID;
    protected string _startupSeq = STARTUPSEQ_INVALID;
    protected int _brightness = BRIGHTNESS_INVALID;
    protected int _orientation = ORIENTATION_INVALID;
    protected int _displayWidth = DISPLAYWIDTH_INVALID;
    protected int _displayHeight = DISPLAYHEIGHT_INVALID;
    protected int _displayType = DISPLAYTYPE_INVALID;
    protected int _layerWidth = LAYERWIDTH_INVALID;
    protected int _layerHeight = LAYERHEIGHT_INVALID;
    protected int _layerCount = LAYERCOUNT_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackDisplay = null;

    public new delegate Task ValueCallback(YDisplay func, string value);
    public new delegate Task TimedReportCallback(YDisplay func, YMeasure measure);
    //--- (end of generated code: YDisplay definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YDisplay(YAPIContext ctx, string func)
            : base(ctx, func, "Display")
        {
            //--- (generated code: YDisplay attributes initialization)
        //--- (end of generated code: YDisplay attributes initialization)
        }

        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YDisplay(string func) : this(YAPI.imm_GetYCtx(), func)
        { }

        //--- (generated code: YDisplay implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("enabled")) {
            _enabled = json_val.getInt("enabled") > 0 ? 1 : 0;
        }
        if (json_val.has("startupSeq")) {
            _startupSeq = json_val.getString("startupSeq");
        }
        if (json_val.has("brightness")) {
            _brightness = json_val.getInt("brightness");
        }
        if (json_val.has("orientation")) {
            _orientation = json_val.getInt("orientation");
        }
        if (json_val.has("displayWidth")) {
            _displayWidth = json_val.getInt("displayWidth");
        }
        if (json_val.has("displayHeight")) {
            _displayHeight = json_val.getInt("displayHeight");
        }
        if (json_val.has("displayType")) {
            _displayType = json_val.getInt("displayType");
        }
        if (json_val.has("layerWidth")) {
            _layerWidth = json_val.getInt("layerWidth");
        }
        if (json_val.has("layerHeight")) {
            _layerHeight = json_val.getInt("layerHeight");
        }
        if (json_val.has("layerCount")) {
            _layerCount = json_val.getInt("layerCount");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns true if the screen is powered, false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YDisplay.ENABLED_FALSE</c> or <c>YDisplay.ENABLED_TRUE</c>, according to true if the
     *   screen is powered, false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.ENABLED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_enabled()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ENABLED_INVALID;
            }
        }
        res = _enabled;
        return res;
    }


    /**
     * <summary>
     *   Changes the power state of the display.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YDisplay.ENABLED_FALSE</c> or <c>YDisplay.ENABLED_TRUE</c>, according to the power state
     *   of the display
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
    public async Task<int> set_enabled(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("enabled",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the name of the sequence to play when the displayed is powered on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the sequence to play when the displayed is powered on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.STARTUPSEQ_INVALID</c>.
     * </para>
     */
    public async Task<string> get_startupSeq()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STARTUPSEQ_INVALID;
            }
        }
        res = _startupSeq;
        return res;
    }


    /**
     * <summary>
     *   Changes the name of the sequence to play when the displayed is powered on.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the name of the sequence to play when the displayed is powered on
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
    public async Task<int> set_startupSeq(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("startupSeq",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the luminosity of the  module informative leds (from 0 to 100).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the luminosity of the  module informative leds (from 0 to 100)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.BRIGHTNESS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_brightness()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BRIGHTNESS_INVALID;
            }
        }
        res = _brightness;
        return res;
    }


    /**
     * <summary>
     *   Changes the brightness of the display.
     * <para>
     *   The parameter is a value between 0 and
     *   100. Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the brightness of the display
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
    public async Task<int> set_brightness(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("brightness",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the currently selected display orientation.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDisplay.ORIENTATION_LEFT</c>, <c>YDisplay.ORIENTATION_UP</c>,
     *   <c>YDisplay.ORIENTATION_RIGHT</c> and <c>YDisplay.ORIENTATION_DOWN</c> corresponding to the
     *   currently selected display orientation
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.ORIENTATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_orientation()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ORIENTATION_INVALID;
            }
        }
        res = _orientation;
        return res;
    }


    /**
     * <summary>
     *   Changes the display orientation.
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YDisplay.ORIENTATION_LEFT</c>, <c>YDisplay.ORIENTATION_UP</c>,
     *   <c>YDisplay.ORIENTATION_RIGHT</c> and <c>YDisplay.ORIENTATION_DOWN</c> corresponding to the display orientation
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
    public async Task<int> set_orientation(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("orientation",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the display width, in pixels.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the display width, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.DISPLAYWIDTH_INVALID</c>.
     * </para>
     */
    public async Task<int> get_displayWidth()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISPLAYWIDTH_INVALID;
            }
        }
        res = _displayWidth;
        return res;
    }


    /**
     * <summary>
     *   Returns the display height, in pixels.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the display height, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.DISPLAYHEIGHT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_displayHeight()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISPLAYHEIGHT_INVALID;
            }
        }
        res = _displayHeight;
        return res;
    }


    /**
     * <summary>
     *   Returns the display type: monochrome, gray levels or full color.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDisplay.DISPLAYTYPE_MONO</c>, <c>YDisplay.DISPLAYTYPE_GRAY</c> and
     *   <c>YDisplay.DISPLAYTYPE_RGB</c> corresponding to the display type: monochrome, gray levels or full color
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.DISPLAYTYPE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_displayType()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISPLAYTYPE_INVALID;
            }
        }
        res = _displayType;
        return res;
    }


    /**
     * <summary>
     *   Returns the width of the layers to draw on, in pixels.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the width of the layers to draw on, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.LAYERWIDTH_INVALID</c>.
     * </para>
     */
    public async Task<int> get_layerWidth()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LAYERWIDTH_INVALID;
            }
        }
        res = _layerWidth;
        return res;
    }


    /**
     * <summary>
     *   Returns the height of the layers to draw on, in pixels.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the height of the layers to draw on, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.LAYERHEIGHT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_layerHeight()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LAYERHEIGHT_INVALID;
            }
        }
        res = _layerHeight;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of available layers to draw on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of available layers to draw on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDisplay.LAYERCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_layerCount()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LAYERCOUNT_INVALID;
            }
        }
        res = _layerCount;
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
     *   Retrieves a display for a given identifier.
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
     *   This function does not require that the display is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDisplay.isOnline()</c> to test if the display is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a display by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the display
     * </param>
     * <returns>
     *   a <c>YDisplay</c> object allowing you to drive the display.
     * </returns>
     */
    public static YDisplay FindDisplay(string func)
    {
        YDisplay obj;
        obj = (YDisplay) YFunction._FindFromCache("Display", func);
        if (obj == null) {
            obj = new YDisplay(func);
            YFunction._AddToCache("Display",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a display for a given identifier in a YAPI context.
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
     *   This function does not require that the display is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDisplay.isOnline()</c> to test if the display is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a display by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the display
     * </param>
     * <returns>
     *   a <c>YDisplay</c> object allowing you to drive the display.
     * </returns>
     */
    public static YDisplay FindDisplayInContext(YAPIContext yctx,string func)
    {
        YDisplay obj;
        obj = (YDisplay) YFunction._FindFromCacheInContext(yctx,  "Display", func);
        if (obj == null) {
            obj = new YDisplay(yctx, func);
            YFunction._AddToCache("Display",  func, obj);
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
        _valueCallbackDisplay = callback;
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
        if (_valueCallbackDisplay != null) {
            await _valueCallbackDisplay(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Clears the display screen and resets all display layers to their default state.
     * <para>
     *   Using this function in a sequence will kill the sequence play-back. Don't use that
     *   function to reset the display at sequence start-up.
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
    public virtual async Task<int> resetAll()
    {
        await this.flushLayers();
        this.imm_resetHiddenLayerFlags();
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Smoothly changes the brightness of the screen to produce a fade-in or fade-out
     *   effect.
     * <para>
     * </para>
     * </summary>
     * <param name="brightness">
     *   the new screen brightness
     * </param>
     * <param name="duration">
     *   duration of the brightness transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> fade(int brightness,int duration)
    {
        await this.flushLayers();
        return await this.sendCommand("+"+Convert.ToString(brightness)+","+Convert.ToString(duration));
    }

    /**
     * <summary>
     *   Starts to record all display commands into a sequence, for later replay.
     * <para>
     *   The name used to store the sequence is specified when calling
     *   <c>saveSequence()</c>, once the recording is complete.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> newSequence()
    {
        await this.flushLayers();
        _sequence = "";
        _recording = true;
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Stops recording display commands and saves the sequence into the specified
     *   file on the display internal memory.
     * <para>
     *   The sequence can be later replayed
     *   using <c>playSequence()</c>.
     * </para>
     * </summary>
     * <param name="sequenceName">
     *   the name of the newly created sequence
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> saveSequence(string sequenceName)
    {
        await this.flushLayers();
        _recording = false;
        await this._upload(sequenceName, YAPI.DefaultEncoding.GetBytes(_sequence));
        //We need to use YPRINTF("") for Objective-C
        _sequence = "";
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Replays a display sequence previously recorded using
     *   <c>newSequence()</c> and <c>saveSequence()</c>.
     * <para>
     * </para>
     * </summary>
     * <param name="sequenceName">
     *   the name of the newly created sequence
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> playSequence(string sequenceName)
    {
        await this.flushLayers();
        return await this.sendCommand("S"+sequenceName);
    }

    /**
     * <summary>
     *   Waits for a specified delay (in milliseconds) before playing next
     *   commands in current sequence.
     * <para>
     *   This method can be used while
     *   recording a display sequence, to insert a timed wait in the sequence
     *   (without any immediate effect). It can also be used dynamically while
     *   playing a pre-recorded sequence, to suspend or resume the execution of
     *   the sequence. To cancel a delay, call the same method with a zero delay.
     * </para>
     * </summary>
     * <param name="delay_ms">
     *   the duration to wait, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> pauseSequence(int delay_ms)
    {
        await this.flushLayers();
        return await this.sendCommand("W"+Convert.ToString(delay_ms));
    }

    /**
     * <summary>
     *   Stops immediately any ongoing sequence replay.
     * <para>
     *   The display is left as is.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> stopSequence()
    {
        await this.flushLayers();
        return await this.sendCommand("S");
    }

    /**
     * <summary>
     *   Uploads an arbitrary file (for instance a GIF file) to the display, to the
     *   specified full path name.
     * <para>
     *   If a file already exists with the same path name,
     *   its content is overwritten.
     * </para>
     * </summary>
     * <param name="pathname">
     *   path and name of the new file to create
     * </param>
     * <param name="content">
     *   binary buffer with the content to set
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> upload(string pathname,byte[] content)
    {
        return await this._upload(pathname, content);
    }

    /**
     * <summary>
     *   Copies the whole content of a layer to another layer.
     * <para>
     *   The color and transparency
     *   of all the pixels from the destination layer are set to match the source pixels.
     *   This method only affects the displayed content, but does not change any
     *   property of the layer object.
     *   Note that layer 0 has no transparency support (it is always completely opaque).
     * </para>
     * </summary>
     * <param name="srcLayerId">
     *   the identifier of the source layer (a number in range 0..layerCount-1)
     * </param>
     * <param name="dstLayerId">
     *   the identifier of the destination layer (a number in range 0..layerCount-1)
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> copyLayerContent(int srcLayerId,int dstLayerId)
    {
        await this.flushLayers();
        return await this.sendCommand("o"+Convert.ToString(srcLayerId)+","+Convert.ToString(dstLayerId));
    }

    /**
     * <summary>
     *   Swaps the whole content of two layers.
     * <para>
     *   The color and transparency of all the pixels from
     *   the two layers are swapped. This method only affects the displayed content, but does
     *   not change any property of the layer objects. In particular, the visibility of each
     *   layer stays unchanged. When used between onae hidden layer and a visible layer,
     *   this method makes it possible to easily implement double-buffering.
     *   Note that layer 0 has no transparency support (it is always completely opaque).
     * </para>
     * </summary>
     * <param name="layerIdA">
     *   the first layer (a number in range 0..layerCount-1)
     * </param>
     * <param name="layerIdB">
     *   the second layer (a number in range 0..layerCount-1)
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> swapLayerContent(int layerIdA,int layerIdB)
    {
        await this.flushLayers();
        return await this.sendCommand("E"+Convert.ToString(layerIdA)+","+Convert.ToString(layerIdB));
    }

    /**
     * <summary>
     *   Continues the enumeration of displays started using <c>yFirstDisplay()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDisplay</c> object, corresponding to
     *   a display currently online, or a <c>null</c> pointer
     *   if there are no more displays to enumerate.
     * </returns>
     */
    public YDisplay nextDisplay()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindDisplayInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of displays currently accessible.
     * <para>
     *   Use the method <c>YDisplay.nextDisplay()</c> to iterate on
     *   next displays.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDisplay</c> object, corresponding to
     *   the first display currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDisplay FirstDisplay()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Display");
        if (next_hwid == null)  return null;
        return FindDisplayInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of displays currently accessible.
     * <para>
     *   Use the method <c>YDisplay.nextDisplay()</c> to iterate on
     *   next displays.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YDisplay</c> object, corresponding to
     *   the first display currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDisplay FirstDisplayInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Display");
        if (next_hwid == null)  return null;
        return FindDisplayInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YDisplay implementation)
        private YDisplayLayer[] _allDisplayLayers = null;
        private bool _recording = false;
        private string _sequence;

        /// <summary>
        /// Returns a YDisplayLayer object that can be used to draw on the specified
        /// layer. The content is displayed only when the layer is active on the
        /// screen (and not masked by other overlapping layers).
        /// </summary>
        /// <param name="layerId"> : the identifier of the layer (a number in range 0..layerCount-1)
        /// </param>
        /// <returns> an YDisplayLayer object
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<YDisplayLayer> get_displayLayer(int layerId)
        {
            if (_allDisplayLayers == null) {
                int nb_display_layer = await this.get_layerCount();
                _allDisplayLayers = new YDisplayLayer[nb_display_layer];
                for (int i = 0; i < nb_display_layer; i++) {
                    _allDisplayLayers[i] = new YDisplayLayer(this, i);
                }
            }
            if (layerId < 0 || layerId >= _allDisplayLayers.Length) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Invalid layerId");
            }
            return _allDisplayLayers[layerId];

        }

        public virtual async Task<int> flushLayers()
        {
            if (_allDisplayLayers != null) {
                for (int i = 0; i < _allDisplayLayers.Length; i++) {
                    await _allDisplayLayers[i].flush_now();
                }
            }
            return YAPI.SUCCESS;

        }

        public virtual async Task resetHiddenLayerFlags()
        {
            if (_allDisplayLayers != null) {
                for (int i = 0; i < _allDisplayLayers.Length; i++) {
                    await _allDisplayLayers[i].resetHiddenFlag();
                }
            }
        }

        internal virtual void imm_resetHiddenLayerFlags()
        {
            if (_allDisplayLayers != null) {
                for (int i = 0; i < _allDisplayLayers.Length; i++) {
                    _allDisplayLayers[i].imm_resetHiddenFlag();
                }
            }
        }


        public virtual async Task<int> sendCommand(string cmd)
        {
            if (!_recording) {
                return await this.set_command(cmd);
            }
            this._sequence += cmd + "\n";
            return YAPI.SUCCESS;

        }
    }


}
