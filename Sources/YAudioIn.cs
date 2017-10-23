/*********************************************************************
 *
 * $Id: YAudioIn.cs 28741 2017-10-03 08:10:04Z seb $
 *
 * Implements FindAudioIn(), the high-level API for AudioIn functions
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

//--- (YAudioIn return codes)
//--- (end of YAudioIn return codes)
//--- (YAudioIn class start)
/**
 * <summary>
 *   YAudioIn Class: AudioIn function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to configure the volume of the input channel.
 * </para>
 * </summary>
 */
public class YAudioIn : YFunction
{
//--- (end of YAudioIn class start)
//--- (YAudioIn definitions)
    /**
     * <summary>
     *   invalid volume value
     * </summary>
     */
    public const  int VOLUME_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid mute value
     * </summary>
     */
    public const int MUTE_FALSE = 0;
    public const int MUTE_TRUE = 1;
    public const int MUTE_INVALID = -1;
    /**
     * <summary>
     *   invalid volumeRange value
     * </summary>
     */
    public const  string VOLUMERANGE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid signal value
     * </summary>
     */
    public const  int SIGNAL_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid noSignalFor value
     * </summary>
     */
    public const  int NOSIGNALFOR_INVALID = YAPI.INVALID_INT;
    protected int _volume = VOLUME_INVALID;
    protected int _mute = MUTE_INVALID;
    protected string _volumeRange = VOLUMERANGE_INVALID;
    protected int _signal = SIGNAL_INVALID;
    protected int _noSignalFor = NOSIGNALFOR_INVALID;
    protected ValueCallback _valueCallbackAudioIn = null;

    public new delegate Task ValueCallback(YAudioIn func, string value);
    public new delegate Task TimedReportCallback(YAudioIn func, YMeasure measure);
    //--- (end of YAudioIn definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAudioIn(YAPIContext ctx, string func)
        : base(ctx, func, "AudioIn")
    {
        //--- (YAudioIn attributes initialization)
        //--- (end of YAudioIn attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAudioIn(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAudioIn implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("volume")) {
            _volume = json_val.GetInt("volume");
        }
        if (json_val.Has("mute")) {
            _mute = json_val.GetInt("mute") > 0 ? 1 : 0;
        }
        if (json_val.Has("volumeRange")) {
            _volumeRange = json_val.GetString("volumeRange");
        }
        if (json_val.Has("signal")) {
            _signal = json_val.GetInt("signal");
        }
        if (json_val.Has("noSignalFor")) {
            _noSignalFor = json_val.GetInt("noSignalFor");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns audio input gain, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to audio input gain, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAudioIn.VOLUME_INVALID</c>.
     * </para>
     */
    public async Task<int> get_volume()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLUME_INVALID;
            }
        }
        res = _volume;
        return res;
    }


    /**
     * <summary>
     *   Changes audio input gain, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to audio input gain, in per cents
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
    public async Task<int> set_volume(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("volume",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the mute function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YAudioIn.MUTE_FALSE</c> or <c>YAudioIn.MUTE_TRUE</c>, according to the state of the mute function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAudioIn.MUTE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_mute()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MUTE_INVALID;
            }
        }
        res = _mute;
        return res;
    }


    /**
     * <summary>
     *   Changes the state of the mute function.
     * <para>
     *   Remember to call the matching module
     *   <c>saveToFlash()</c> method to save the setting permanently.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YAudioIn.MUTE_FALSE</c> or <c>YAudioIn.MUTE_TRUE</c>, according to the state of the mute function
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
    public async Task<int> set_mute(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("mute",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the supported volume range.
     * <para>
     *   The low value of the
     *   range corresponds to the minimal audible value. To
     *   completely mute the sound, use <c>set_mute()</c>
     *   instead of the <c>set_volume()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the supported volume range
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAudioIn.VOLUMERANGE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_volumeRange()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLUMERANGE_INVALID;
            }
        }
        res = _volumeRange;
        return res;
    }


    /**
     * <summary>
     *   Returns the detected input signal level.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the detected input signal level
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAudioIn.SIGNAL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_signal()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SIGNAL_INVALID;
            }
        }
        res = _signal;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of seconds elapsed without detecting a signal.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of seconds elapsed without detecting a signal
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAudioIn.NOSIGNALFOR_INVALID</c>.
     * </para>
     */
    public async Task<int> get_noSignalFor()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return NOSIGNALFOR_INVALID;
            }
        }
        res = _noSignalFor;
        return res;
    }


    /**
     * <summary>
     *   Retrieves an audio input for a given identifier.
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
     *   This function does not require that the audio input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAudioIn.isOnline()</c> to test if the audio input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an audio input by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the audio input
     * </param>
     * <returns>
     *   a <c>YAudioIn</c> object allowing you to drive the audio input.
     * </returns>
     */
    public static YAudioIn FindAudioIn(string func)
    {
        YAudioIn obj;
        obj = (YAudioIn) YFunction._FindFromCache("AudioIn", func);
        if (obj == null) {
            obj = new YAudioIn(func);
            YFunction._AddToCache("AudioIn",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an audio input for a given identifier in a YAPI context.
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
     *   This function does not require that the audio input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAudioIn.isOnline()</c> to test if the audio input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an audio input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the audio input
     * </param>
     * <returns>
     *   a <c>YAudioIn</c> object allowing you to drive the audio input.
     * </returns>
     */
    public static YAudioIn FindAudioInInContext(YAPIContext yctx,string func)
    {
        YAudioIn obj;
        obj = (YAudioIn) YFunction._FindFromCacheInContext(yctx,  "AudioIn", func);
        if (obj == null) {
            obj = new YAudioIn(yctx, func);
            YFunction._AddToCache("AudioIn",  func, obj);
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
        _valueCallbackAudioIn = callback;
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
        if (_valueCallbackAudioIn != null) {
            await _valueCallbackAudioIn(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of audio inputs started using <c>yFirstAudioIn()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAudioIn</c> object, corresponding to
     *   an audio input currently online, or a <c>null</c> pointer
     *   if there are no more audio inputs to enumerate.
     * </returns>
     */
    public YAudioIn nextAudioIn()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAudioInInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of audio inputs currently accessible.
     * <para>
     *   Use the method <c>YAudioIn.nextAudioIn()</c> to iterate on
     *   next audio inputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAudioIn</c> object, corresponding to
     *   the first audio input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAudioIn FirstAudioIn()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AudioIn");
        if (next_hwid == null)  return null;
        return FindAudioInInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of audio inputs currently accessible.
     * <para>
     *   Use the method <c>YAudioIn.nextAudioIn()</c> to iterate on
     *   next audio inputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAudioIn</c> object, corresponding to
     *   the first audio input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAudioIn FirstAudioInInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AudioIn");
        if (next_hwid == null)  return null;
        return FindAudioInInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAudioIn implementation)
}
}

