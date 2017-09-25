/*********************************************************************
 *
 * $Id: YColorLedCluster.cs 28443 2017-09-01 14:45:46Z mvuilleu $
 *
 * Implements FindColorLedCluster(), the high-level API for ColorLedCluster functions
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

//--- (YColorLedCluster return codes)
//--- (end of YColorLedCluster return codes)
//--- (YColorLedCluster class start)
/**
 * <summary>
 *   YColorLedCluster Class: ColorLedCluster function interface
 * <para>
 *   The Yoctopuce application programming interface
 *   allows you to drive a color LED cluster. Unlike the ColorLed class, the ColorLedCluster
 *   allows to handle several LEDs at one. Color changes can be done   using RGB coordinates as well as
 *   HSL coordinates.
 *   The module performs all conversions form RGB to HSL automatically. It is then
 *   self-evident to turn on a LED with a given hue and to progressively vary its
 *   saturation or lightness. If needed, you can find more information on the
 *   difference between RGB and HSL in the section following this one.
 * </para>
 * </summary>
 */
public class YColorLedCluster : YFunction
{
//--- (end of YColorLedCluster class start)
//--- (YColorLedCluster definitions)
    /**
     * <summary>
     *   invalid activeLedCount value
     * </summary>
     */
    public const  int ACTIVELEDCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid maxLedCount value
     * </summary>
     */
    public const  int MAXLEDCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinkSeqMaxCount value
     * </summary>
     */
    public const  int BLINKSEQMAXCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid blinkSeqMaxSize value
     * </summary>
     */
    public const  int BLINKSEQMAXSIZE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _activeLedCount = ACTIVELEDCOUNT_INVALID;
    protected int _maxLedCount = MAXLEDCOUNT_INVALID;
    protected int _blinkSeqMaxCount = BLINKSEQMAXCOUNT_INVALID;
    protected int _blinkSeqMaxSize = BLINKSEQMAXSIZE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackColorLedCluster = null;

    public new delegate Task ValueCallback(YColorLedCluster func, string value);
    public new delegate Task TimedReportCallback(YColorLedCluster func, YMeasure measure);
    //--- (end of YColorLedCluster definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YColorLedCluster(YAPIContext ctx, string func)
        : base(ctx, func, "ColorLedCluster")
    {
        //--- (YColorLedCluster attributes initialization)
        //--- (end of YColorLedCluster attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YColorLedCluster(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YColorLedCluster implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("activeLedCount")) {
            _activeLedCount = json_val.GetInt("activeLedCount");
        }
        if (json_val.Has("maxLedCount")) {
            _maxLedCount = json_val.GetInt("maxLedCount");
        }
        if (json_val.Has("blinkSeqMaxCount")) {
            _blinkSeqMaxCount = json_val.GetInt("blinkSeqMaxCount");
        }
        if (json_val.Has("blinkSeqMaxSize")) {
            _blinkSeqMaxSize = json_val.GetInt("blinkSeqMaxSize");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of LEDs currently handled by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of LEDs currently handled by the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.ACTIVELEDCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_activeLedCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ACTIVELEDCOUNT_INVALID;
            }
        }
        res = _activeLedCount;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of LEDs currently handled by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of LEDs currently handled by the device
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
    public async Task<int> set_activeLedCount(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("activeLedCount",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximum number of LEDs that the device can handle.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum number of LEDs that the device can handle
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.MAXLEDCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_maxLedCount()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXLEDCOUNT_INVALID;
            }
        }
        res = _maxLedCount;
        return res;
    }


    /**
     * <summary>
     *   Returns the maximum number of sequences that the device can handle.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum number of sequences that the device can handle
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.BLINKSEQMAXCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_blinkSeqMaxCount()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQMAXCOUNT_INVALID;
            }
        }
        res = _blinkSeqMaxCount;
        return res;
    }


    /**
     * <summary>
     *   Returns the maximum length of sequences.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum length of sequences
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.BLINKSEQMAXSIZE_INVALID</c>.
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
     *   Retrieves a RGB LED cluster for a given identifier.
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
     *   This function does not require that the RGB LED cluster is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLedCluster.isOnline()</c> to test if the RGB LED cluster is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a RGB LED cluster by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the RGB LED cluster
     * </param>
     * <returns>
     *   a <c>YColorLedCluster</c> object allowing you to drive the RGB LED cluster.
     * </returns>
     */
    public static YColorLedCluster FindColorLedCluster(string func)
    {
        YColorLedCluster obj;
        obj = (YColorLedCluster) YFunction._FindFromCache("ColorLedCluster", func);
        if (obj == null) {
            obj = new YColorLedCluster(func);
            YFunction._AddToCache("ColorLedCluster",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a RGB LED cluster for a given identifier in a YAPI context.
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
     *   This function does not require that the RGB LED cluster is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLedCluster.isOnline()</c> to test if the RGB LED cluster is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a RGB LED cluster by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the RGB LED cluster
     * </param>
     * <returns>
     *   a <c>YColorLedCluster</c> object allowing you to drive the RGB LED cluster.
     * </returns>
     */
    public static YColorLedCluster FindColorLedClusterInContext(YAPIContext yctx,string func)
    {
        YColorLedCluster obj;
        obj = (YColorLedCluster) YFunction._FindFromCacheInContext(yctx,  "ColorLedCluster", func);
        if (obj == null) {
            obj = new YColorLedCluster(yctx, func);
            YFunction._AddToCache("ColorLedCluster",  func, obj);
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
        _valueCallbackColorLedCluster = callback;
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
        if (_valueCallbackColorLedCluster != null) {
            await _valueCallbackColorLedCluster(this, value);
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
     *   Changes the current color of consecutve LEDs in the cluster, using a RGB color.
     * <para>
     *   Encoding is done as follows: 0xRRGGBB.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="rgbValue">
     *   new color.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_rgbColor(int ledIndex,int count,int rgbValue)
    {
        return await this.sendCommand("SR"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",rgbValue));
    }

    /**
     * <summary>
     *   Changes the  color at device startup of consecutve LEDs in the cluster, using a RGB color.
     * <para>
     *   Encoding is done as follows: 0xRRGGBB.
     *   Don't forget to call <c>saveLedsConfigAtPowerOn()</c> to make sure the modification is saved in the
     *   device flash memory.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="rgbValue">
     *   new color.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_rgbColorAtPowerOn(int ledIndex,int count,int rgbValue)
    {
        return await this.sendCommand("SC"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",rgbValue));
    }

    /**
     * <summary>
     *   Changes the current color of consecutive LEDs in the cluster, using a HSL color.
     * <para>
     *   Encoding is done as follows: 0xHHSSLL.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="hslValue">
     *   new color.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_hslColor(int ledIndex,int count,int hslValue)
    {
        return await this.sendCommand("SH"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",hslValue));
    }

    /**
     * <summary>
     *   Allows you to modify the current color of a group of adjacent LEDs to another color, in a seamless and
     *   autonomous manner.
     * <para>
     *   The transition is performed in the RGB space.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="rgbValue">
     *   new color (0xRRGGBB).
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> rgb_move(int ledIndex,int count,int rgbValue,int delay)
    {
        return await this.sendCommand("MR"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",rgbValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Allows you to modify the current color of a group of adjacent LEDs  to another color, in a seamless and
     *   autonomous manner.
     * <para>
     *   The transition is performed in the HSL space. In HSL, hue is a circular
     *   value (0..360°). There are always two paths to perform the transition: by increasing
     *   or by decreasing the hue. The module selects the shortest transition.
     *   If the difference is exactly 180°, the module selects the transition which increases
     *   the hue.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the fisrt affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="hslValue">
     *   new color (0xHHSSLL).
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> hsl_move(int ledIndex,int count,int hslValue,int delay)
    {
        return await this.sendCommand("MH"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",hslValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds an RGB transition to a sequence.
     * <para>
     *   A sequence is a transition list, which can
     *   be executed in loop by a group of LEDs.  Sequences are persistent and are saved
     *   in the device flash memory as soon as the <c>saveBlinkSeq()</c> method is called.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="rgbValue">
     *   target color (0xRRGGBB)
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> addRgbMoveToBlinkSeq(int seqIndex,int rgbValue,int delay)
    {
        return await this.sendCommand("AR"+Convert.ToString(seqIndex)+","+String.Format("{0:X}",rgbValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds an HSL transition to a sequence.
     * <para>
     *   A sequence is a transition list, which can
     *   be executed in loop by an group of LEDs.  Sequences are persistant and are saved
     *   in the device flash memory as soon as the <c>saveBlinkSeq()</c> method is called.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="hslValue">
     *   target color (0xHHSSLL)
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> addHslMoveToBlinkSeq(int seqIndex,int hslValue,int delay)
    {
        return await this.sendCommand("AH"+Convert.ToString(seqIndex)+","+String.Format("{0:X}",hslValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds a mirror ending to a sequence.
     * <para>
     *   When the sequence will reach the end of the last
     *   transition, its running speed will automatically be reversed so that the sequence plays
     *   in the reverse direction, like in a mirror. After the first transition of the sequence
     *   is played at the end of the reverse execution, the sequence starts again in
     *   the initial direction.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> addMirrorToBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("AC"+Convert.ToString(seqIndex)+",0,0");
    }

    /**
     * <summary>
     *   Adds to a sequence a jump to another sequence.
     * <para>
     *   When a pixel will reach this jump,
     *   it will be automatically relinked to the new sequence, and will run it starting
     *   from the beginning.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="linkSeqIndex">
     *   index of the sequence to chain.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> addJumpToBlinkSeq(int seqIndex,int linkSeqIndex)
    {
        return await this.sendCommand("AC"+Convert.ToString(seqIndex)+",100,"+Convert.ToString(linkSeqIndex)+",1000");
    }

    /**
     * <summary>
     *   Adds a to a sequence a hard stop code.
     * <para>
     *   When a pixel will reach this stop code,
     *   instead of restarting the sequence in a loop it will automatically be unlinked
     *   from the sequence.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> addUnlinkToBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("AC"+Convert.ToString(seqIndex)+",100,-1,1000");
    }

    /**
     * <summary>
     *   Links adjacent LEDs to a specific sequence.
     * <para>
     *   These LEDs start to execute
     *   the sequence as soon as  startBlinkSeq is called. It is possible to add an offset
     *   in the execution: that way we  can have several groups of LED executing the same
     *   sequence, with a  temporal offset. A LED cannot be linked to more than one sequence.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="offset">
     *   execution offset in ms.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> linkLedToBlinkSeq(int ledIndex,int count,int seqIndex,int offset)
    {
        return await this.sendCommand("LS"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+Convert.ToString(seqIndex)+","+Convert.ToString(offset));
    }

    /**
     * <summary>
     *   Links adjacent LEDs to a specific sequence at device poweron.
     * <para>
     *   Don't forget to configure
     *   the sequence auto start flag as well and call <c>saveLedsConfigAtPowerOn()</c>. It is possible to add an offset
     *   in the execution: that way we  can have several groups of LEDs executing the same
     *   sequence, with a  temporal offset. A LED cannot be linked to more than one sequence.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="offset">
     *   execution offset in ms.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> linkLedToBlinkSeqAtPowerOn(int ledIndex,int count,int seqIndex,int offset)
    {
        return await this.sendCommand("LO"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+Convert.ToString(seqIndex)+","+Convert.ToString(offset));
    }

    /**
     * <summary>
     *   Links adjacent LEDs to a specific sequence.
     * <para>
     *   These LED start to execute
     *   the sequence as soon as  startBlinkSeq is called. This function automatically
     *   introduces a shift between LEDs so that the specified number of sequence periods
     *   appears on the group of LEDs (wave effect).
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="periods">
     *   number of periods to show on LEDs.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> linkLedToPeriodicBlinkSeq(int ledIndex,int count,int seqIndex,int periods)
    {
        return await this.sendCommand("LP"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+Convert.ToString(seqIndex)+","+Convert.ToString(periods));
    }

    /**
     * <summary>
     *   Unlinks adjacent LEDs from a  sequence.
     * <para>
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> unlinkLedFromBlinkSeq(int ledIndex,int count)
    {
        return await this.sendCommand("US"+Convert.ToString(ledIndex)+","+Convert.ToString(count));
    }

    /**
     * <summary>
     *   Starts a sequence execution: every LED linked to that sequence starts to
     *   run it in a loop.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to start.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> startBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("SS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Stops a sequence execution.
     * <para>
     *   If started again, the execution
     *   restarts from the beginning.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to stop.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> stopBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("XS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Stops a sequence execution and resets its contents.
     * <para>
     *   Leds linked to this
     *   sequence are not automatically updated anymore.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to reset
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> resetBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("ZS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Configures a sequence to make it start automatically at device
     *   startup.
     * <para>
     *   Don't forget to call <c>saveBlinkSeq()</c> to make sure the
     *   modification is saved in the device flash memory.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to reset.
     * </param>
     * <param name="autostart">
     *   0 to keep the sequence turned off and 1 to start it automatically.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_blinkSeqStateAtPowerOn(int seqIndex,int autostart)
    {
        return await this.sendCommand("AS"+Convert.ToString(seqIndex)+","+Convert.ToString(autostart));
    }

    /**
     * <summary>
     *   Changes the execution speed of a sequence.
     * <para>
     *   The natural execution speed is 1000 per
     *   thousand. If you configure a slower speed, you can play the sequence in slow-motion.
     *   If you set a negative speed, you can play the sequence in reverse direction.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to start.
     * </param>
     * <param name="speed">
     *   sequence running speed (-1000...1000).
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_blinkSeqSpeed(int seqIndex,int speed)
    {
        return await this.sendCommand("CS"+Convert.ToString(seqIndex)+","+Convert.ToString(speed));
    }

    /**
     * <summary>
     *   Saves the LEDs power-on configuration.
     * <para>
     *   This includes the start-up color or
     *   sequence binding for all LEDs. Warning: if some LEDs are linked to a sequence, the
     *   method <c>saveBlinkSeq()</c> must also be called to save the sequence definition.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> saveLedsConfigAtPowerOn()
    {
        return await this.sendCommand("WL");
    }

    public virtual async Task<int> saveLedsState()
    {
        return await this.sendCommand("WL");
    }

    /**
     * <summary>
     *   Saves the definition of a sequence.
     * <para>
     *   Warning: only sequence steps and flags are saved.
     *   to save the LEDs startup bindings, the method <c>saveLedsConfigAtPowerOn()</c>
     *   must be called.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to start.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> saveBlinkSeq(int seqIndex)
    {
        return await this.sendCommand("WS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Sends a binary buffer to the LED RGB buffer, as is.
     * <para>
     *   First three bytes are RGB components for LED specified as parameter, the
     *   next three bytes for the next LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be updated
     * </param>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_rgbColorBuffer(int ledIndex,byte[] buff)
    {
        return await this._upload("rgb:0:"+Convert.ToString(ledIndex), buff);
    }

    /**
     * <summary>
     *   Sends 24bit RGB colors (provided as a list of integers) to the LED RGB buffer, as is.
     * <para>
     *   The first number represents the RGB value of the LED specified as parameter, the second
     *   number represents the RGB value of the next LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be updated
     * </param>
     * <param name="rgbList">
     *   a list of 24bit RGB codes, in the form 0xRRGGBB
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_rgbColorArray(int ledIndex,List<int> rgbList)
    {
        int listlen;
        byte[] buff;
        int idx;
        int rgb;
        int res;
        listlen = rgbList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            rgb = rgbList[idx];
            buff[3*idx] = (byte)(((((rgb) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((rgb) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((rgb) & (255)) & 0xff);
            idx = idx + 1;
        }

        res = await this._upload("rgb:0:"+Convert.ToString(ledIndex), buff);
        return res;
    }

    /**
     * <summary>
     *   Sets up a smooth RGB color transition to the specified pixel-by-pixel list of RGB
     *   color codes.
     * <para>
     *   The first color code represents the target RGB value of the first LED,
     *   the next color code represents the target value of the next LED, etc.
     * </para>
     * </summary>
     * <param name="rgbList">
     *   a list of target 24bit RGB codes, in the form 0xRRGGBB
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> rgbArray_move(List<int> rgbList,int delay)
    {
        int listlen;
        byte[] buff;
        int idx;
        int rgb;
        int res;
        listlen = rgbList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            rgb = rgbList[idx];
            buff[3*idx] = (byte)(((((rgb) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((rgb) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((rgb) & (255)) & 0xff);
            idx = idx + 1;
        }

        res = await this._upload("rgb:"+Convert.ToString(delay), buff);
        return res;
    }

    /**
     * <summary>
     *   Sends a binary buffer to the LED HSL buffer, as is.
     * <para>
     *   First three bytes are HSL components for the LED specified as parameter, the
     *   next three bytes for the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be updated
     * </param>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_hslColorBuffer(int ledIndex,byte[] buff)
    {
        return await this._upload("hsl:0:"+Convert.ToString(ledIndex), buff);
    }

    /**
     * <summary>
     *   Sends 24bit HSL colors (provided as a list of integers) to the LED HSL buffer, as is.
     * <para>
     *   The first number represents the HSL value of the LED specified as parameter, the second number represents
     *   the HSL value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be updated
     * </param>
     * <param name="hslList">
     *   a list of 24bit HSL codes, in the form 0xHHSSLL
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_hslColorArray(int ledIndex,List<int> hslList)
    {
        int listlen;
        byte[] buff;
        int idx;
        int hsl;
        int res;
        listlen = hslList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            hsl = hslList[idx];
            buff[3*idx] = (byte)(((((hsl) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((hsl) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((hsl) & (255)) & 0xff);
            idx = idx + 1;
        }

        res = await this._upload("hsl:0:"+Convert.ToString(ledIndex), buff);
        return res;
    }

    /**
     * <summary>
     *   Sets up a smooth HSL color transition to the specified pixel-by-pixel list of HSL
     *   color codes.
     * <para>
     *   The first color code represents the target HSL value of the first LED,
     *   the second color code represents the target value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="hslList">
     *   a list of target 24bit HSL codes, in the form 0xHHSSLL
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> hslArray_move(List<int> hslList,int delay)
    {
        int listlen;
        byte[] buff;
        int idx;
        int hsl;
        int res;
        listlen = hslList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            hsl = hslList[idx];
            buff[3*idx] = (byte)(((((hsl) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((hsl) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((hsl) & (255)) & 0xff);
            idx = idx + 1;
        }

        res = await this._upload("hsl:"+Convert.ToString(delay), buff);
        return res;
    }

    /**
     * <summary>
     *   Returns a binary buffer with content from the LED RGB buffer, as is.
     * <para>
     *   First three bytes are RGB components for the first LED in the interval,
     *   the next three bytes for the second LED in the interval, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a binary buffer with RGB components of selected LEDs.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty binary buffer.
     * </para>
     */
    public virtual async Task<byte[]> get_rgbColorBuffer(int ledIndex,int count)
    {
        return await this._download("rgb.bin?typ=0&pos="+Convert.ToString(3*ledIndex)+"&len="+Convert.ToString(3*count));
    }

    /**
     * <summary>
     *   Returns a list on 24bit RGB color values with the current colors displayed on
     *   the RGB leds.
     * <para>
     *   The first number represents the RGB value of the first LED,
     *   the second number represents the RGB value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a list of 24bit color codes with RGB components of selected LEDs, as 0xRRGGBB.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_rgbColorArray(int ledIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int r;
        int g;
        int b;

        buff = await this._download("rgb.bin?typ=0&pos="+Convert.ToString(3*ledIndex)+"&len="+Convert.ToString(3*count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            r = buff[3*idx];
            g = buff[3*idx+1];
            b = buff[3*idx+2];
            res.Add(r*65536+g*256+b);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list on 24bit RGB color values with the RGB LEDs startup colors.
     * <para>
     *   The first number represents the startup RGB value of the first LED,
     *   the second number represents the RGB value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED  which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a list of 24bit color codes with RGB components of selected LEDs, as 0xRRGGBB.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_rgbColorArrayAtPowerOn(int ledIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int r;
        int g;
        int b;

        buff = await this._download("rgb.bin?typ=4&pos="+Convert.ToString(3*ledIndex)+"&len="+Convert.ToString(3*count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            r = buff[3*idx];
            g = buff[3*idx+1];
            b = buff[3*idx+2];
            res.Add(r*65536+g*256+b);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list on sequence index for each RGB LED.
     * <para>
     *   The first number represents the
     *   sequence index for the the first LED, the second number represents the sequence
     *   index for the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a list of integers with sequence index
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_linkedSeqArray(int ledIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int seq;

        buff = await this._download("rgb.bin?typ=1&pos="+Convert.ToString(ledIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            seq = buff[idx];
            res.Add(seq);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list on 32 bit signatures for specified blinking sequences.
     * <para>
     *   Since blinking sequences cannot be read from the device, this can be used
     *   to detect if a specific blinking sequence is already programmed.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first blinking sequence which should be returned
     * </param>
     * <param name="count">
     *   number of blinking sequences which should be returned
     * </param>
     * <returns>
     *   a list of 32 bit integer signatures
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_blinkSeqSignatures(int seqIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int hh;
        int hl;
        int lh;
        int ll;

        buff = await this._download("rgb.bin?typ=2&pos="+Convert.ToString(4*seqIndex)+"&len="+Convert.ToString(4*count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            hh = buff[4*idx];
            hl = buff[4*idx+1];
            lh = buff[4*idx+2];
            ll = buff[4*idx+3];
            res.Add(((hh) << (24))+((hl) << (16))+((lh) << (8))+ll);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list of integers with the current speed for specified blinking sequences.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first sequence speed which should be returned
     * </param>
     * <param name="count">
     *   number of sequence speeds which should be returned
     * </param>
     * <returns>
     *   a list of integers, 0 for sequences turned off and 1 for sequences running
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_blinkSeqStateSpeed(int seqIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int lh;
        int ll;

        buff = await this._download("rgb.bin?typ=6&pos="+Convert.ToString(seqIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            lh = buff[2*idx];
            ll = buff[2*idx+1];
            res.Add(((lh) << (8))+ll);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list of integers with the "auto-start at power on" flag state for specified blinking sequences.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first blinking sequence which should be returned
     * </param>
     * <param name="count">
     *   number of blinking sequences which should be returned
     * </param>
     * <returns>
     *   a list of integers, 0 for sequences turned off and 1 for sequences running
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_blinkSeqStateAtPowerOn(int seqIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int started;

        buff = await this._download("rgb.bin?typ=5&pos="+Convert.ToString(seqIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            started = buff[idx];
            res.Add(started);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list of integers with the started state for specified blinking sequences.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first blinking sequence which should be returned
     * </param>
     * <param name="count">
     *   number of blinking sequences which should be returned
     * </param>
     * <returns>
     *   a list of integers, 0 for sequences turned off and 1 for sequences running
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> get_blinkSeqState(int seqIndex,int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int started;

        buff = await this._download("rgb.bin?typ=3&pos="+Convert.ToString(seqIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            started = buff[idx];
            res.Add(started);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Continues the enumeration of RGB LED clusters started using <c>yFirstColorLedCluster()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLedCluster</c> object, corresponding to
     *   a RGB LED cluster currently online, or a <c>null</c> pointer
     *   if there are no more RGB LED clusters to enumerate.
     * </returns>
     */
    public YColorLedCluster nextColorLedCluster()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindColorLedClusterInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RGB LED clusters currently accessible.
     * <para>
     *   Use the method <c>YColorLedCluster.nextColorLedCluster()</c> to iterate on
     *   next RGB LED clusters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLedCluster</c> object, corresponding to
     *   the first RGB LED cluster currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLedCluster FirstColorLedCluster()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ColorLedCluster");
        if (next_hwid == null)  return null;
        return FindColorLedClusterInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RGB LED clusters currently accessible.
     * <para>
     *   Use the method <c>YColorLedCluster.nextColorLedCluster()</c> to iterate on
     *   next RGB LED clusters.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YColorLedCluster</c> object, corresponding to
     *   the first RGB LED cluster currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLedCluster FirstColorLedClusterInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ColorLedCluster");
        if (next_hwid == null)  return null;
        return FindColorLedClusterInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YColorLedCluster implementation)
}
}

