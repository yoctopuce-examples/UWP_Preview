/*********************************************************************
 *
 * $Id: YBuzzer.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindBuzzer(), the high-level API for Buzzer functions
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

//--- (YBuzzer return codes)
//--- (end of YBuzzer return codes)
//--- (YBuzzer class start)
/**
 * <summary>
 *   YBuzzer Class: Buzzer function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to
 *   choose the frequency and volume at which the buzzer must sound.
 *   You can also pre-program a play sequence.
 * </para>
 * </summary>
 */
public class YBuzzer : YFunction
{
//--- (end of YBuzzer class start)
//--- (YBuzzer definitions)
    /**
     * <summary>
     *   invalid frequency value
     * </summary>
     */
    public const  double FREQUENCY_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid volume value
     * </summary>
     */
    public const  int VOLUME_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid playSeqSize value
     * </summary>
     */
    public const  int PLAYSEQSIZE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid playSeqMaxSize value
     * </summary>
     */
    public const  int PLAYSEQMAXSIZE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid playSeqSignature value
     * </summary>
     */
    public const  int PLAYSEQSIGNATURE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected double _frequency = FREQUENCY_INVALID;
    protected int _volume = VOLUME_INVALID;
    protected int _playSeqSize = PLAYSEQSIZE_INVALID;
    protected int _playSeqMaxSize = PLAYSEQMAXSIZE_INVALID;
    protected int _playSeqSignature = PLAYSEQSIGNATURE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackBuzzer = null;

    public new delegate Task ValueCallback(YBuzzer func, string value);
    public new delegate Task TimedReportCallback(YBuzzer func, YMeasure measure);
    //--- (end of YBuzzer definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YBuzzer(YAPIContext ctx, string func)
        : base(ctx, func, "Buzzer")
    {
        //--- (YBuzzer attributes initialization)
        //--- (end of YBuzzer attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YBuzzer(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YBuzzer implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("frequency")) {
            _frequency = Math.Round(json_val.getDouble("frequency") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("volume")) {
            _volume = json_val.getInt("volume");
        }
        if (json_val.has("playSeqSize")) {
            _playSeqSize = json_val.getInt("playSeqSize");
        }
        if (json_val.has("playSeqMaxSize")) {
            _playSeqMaxSize = json_val.getInt("playSeqMaxSize");
        }
        if (json_val.has("playSeqSignature")) {
            _playSeqSignature = json_val.getInt("playSeqSignature");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the frequency of the signal sent to the buzzer.
     * <para>
     *   A zero value stops the buzzer.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the frequency of the signal sent to the buzzer
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
    public async Task<int> set_frequency(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("frequency",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the  frequency of the signal sent to the buzzer/speaker.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the  frequency of the signal sent to the buzzer/speaker
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBuzzer.FREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<double> get_frequency()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FREQUENCY_INVALID;
            }
        }
        res = _frequency;
        return res;
    }


    /**
     * <summary>
     *   Returns the volume of the signal sent to the buzzer/speaker.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the volume of the signal sent to the buzzer/speaker
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBuzzer.VOLUME_INVALID</c>.
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
     *   Changes the volume of the signal sent to the buzzer/speaker.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the volume of the signal sent to the buzzer/speaker
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
     *   Returns the current length of the playing sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current length of the playing sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBuzzer.PLAYSEQSIZE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_playSeqSize()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PLAYSEQSIZE_INVALID;
            }
        }
        res = _playSeqSize;
        return res;
    }


    /**
     * <summary>
     *   Returns the maximum length of the playing sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum length of the playing sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBuzzer.PLAYSEQMAXSIZE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_playSeqMaxSize()
    {
        int res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PLAYSEQMAXSIZE_INVALID;
            }
        }
        res = _playSeqMaxSize;
        return res;
    }


    /**
     * <summary>
     *   Returns the playing sequence signature.
     * <para>
     *   As playing
     *   sequences cannot be read from the device, this can be used
     *   to detect if a specific playing sequence is already
     *   programmed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the playing sequence signature
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBuzzer.PLAYSEQSIGNATURE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_playSeqSignature()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PLAYSEQSIGNATURE_INVALID;
            }
        }
        res = _playSeqSignature;
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
     *   Retrieves a buzzer for a given identifier.
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
     *   This function does not require that the buzzer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBuzzer.isOnline()</c> to test if the buzzer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a buzzer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the buzzer
     * </param>
     * <returns>
     *   a <c>YBuzzer</c> object allowing you to drive the buzzer.
     * </returns>
     */
    public static YBuzzer FindBuzzer(string func)
    {
        YBuzzer obj;
        obj = (YBuzzer) YFunction._FindFromCache("Buzzer", func);
        if (obj == null) {
            obj = new YBuzzer(func);
            YFunction._AddToCache("Buzzer",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a buzzer for a given identifier in a YAPI context.
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
     *   This function does not require that the buzzer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBuzzer.isOnline()</c> to test if the buzzer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a buzzer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the buzzer
     * </param>
     * <returns>
     *   a <c>YBuzzer</c> object allowing you to drive the buzzer.
     * </returns>
     */
    public static YBuzzer FindBuzzerInContext(YAPIContext yctx,string func)
    {
        YBuzzer obj;
        obj = (YBuzzer) YFunction._FindFromCacheInContext(yctx,  "Buzzer", func);
        if (obj == null) {
            obj = new YBuzzer(yctx, func);
            YFunction._AddToCache("Buzzer",  func, obj);
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
        _valueCallbackBuzzer = callback;
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
        if (_valueCallbackBuzzer != null) {
            await _valueCallbackBuzzer(this, value);
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
     *   Adds a new frequency transition to the playing sequence.
     * <para>
     * </para>
     * </summary>
     * <param name="freq">
     *   desired frequency when the transition is completed, in Hz
     * </param>
     * <param name="msDelay">
     *   duration of the frequency transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addFreqMoveToPlaySeq(int freq,int msDelay)
    {
        return await this.sendCommand("A"+Convert.ToString(freq)+","+Convert.ToString(msDelay));
    }

    /**
     * <summary>
     *   Adds a pulse to the playing sequence.
     * <para>
     * </para>
     * </summary>
     * <param name="freq">
     *   pulse frequency, in Hz
     * </param>
     * <param name="msDuration">
     *   pulse duration, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addPulseToPlaySeq(int freq,int msDuration)
    {
        return await this.sendCommand("B"+Convert.ToString(freq)+","+Convert.ToString(msDuration));
    }

    /**
     * <summary>
     *   Adds a new volume transition to the playing sequence.
     * <para>
     *   Frequency stays untouched:
     *   if frequency is at zero, the transition has no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="volume">
     *   desired volume when the transition is completed, as a percentage.
     * </param>
     * <param name="msDuration">
     *   duration of the volume transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addVolMoveToPlaySeq(int volume,int msDuration)
    {
        return await this.sendCommand("C"+Convert.ToString(volume)+","+Convert.ToString(msDuration));
    }

    /**
     * <summary>
     *   Adds notes to the playing sequence.
     * <para>
     *   Notes are provided as text words, separated by
     *   spaces. The pitch is specified using the usual letter from A to G. The duration is
     *   specified as the divisor of a whole note: 4 for a fourth, 8 for an eight note, etc.
     *   Some modifiers are supported: <c>#</c> and <c>b</c> to alter a note pitch,
     *   <c>'</c> and <c>,</c> to move to the upper/lower octave, <c>.</c> to enlarge
     *   the note duration.
     * </para>
     * </summary>
     * <param name="notes">
     *   notes to be played, as a text string.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> addNotesToPlaySeq(string notes)
    {
        int tempo;
        int prevPitch;
        int prevDuration;
        int prevFreq;
        int note;
        int num;
        int typ;
        byte[] ascNotes;
        int notesLen;
        int i;
        int ch;
        int dNote;
        int pitch;
        int freq;
        int ms;
        int ms16;
        int rest;
        tempo = 100;
        prevPitch = 3;
        prevDuration = 4;
        prevFreq = 110;
        note = -99;
        num = 0;
        typ = 3;
        ascNotes = YAPI.DefaultEncoding.GetBytes(notes);
        notesLen = (ascNotes).Length;
        i = 0;
        while (i < notesLen) {
            ch = ascNotes[i];
            // A (note))
            if (ch == 65) {
                note = 0;
            }
            // B (note)
            if (ch == 66) {
                note = 2;
            }
            // C (note)
            if (ch == 67) {
                note = 3;
            }
            // D (note)
            if (ch == 68) {
                note = 5;
            }
            // E (note)
            if (ch == 69) {
                note = 7;
            }
            // F (note)
            if (ch == 70) {
                note = 8;
            }
            // G (note)
            if (ch == 71) {
                note = 10;
            }
            // '#' (sharp modifier)
            if (ch == 35) {
                note = note + 1;
            }
            // 'b' (flat modifier)
            if (ch == 98) {
                note = note - 1;
            }
            // ' (octave up)
            if (ch == 39) {
                prevPitch = prevPitch + 12;
            }
            // , (octave down)
            if (ch == 44) {
                prevPitch = prevPitch - 12;
            }
            // R (rest)
            if (ch == 82) {
                typ = 0;
            }
            // ! (staccato modifier)
            if (ch == 33) {
                typ = 1;
            }
            // ^ (short modifier)
            if (ch == 94) {
                typ = 2;
            }
            // _ (legato modifier)
            if (ch == 95) {
                typ = 4;
            }
            // - (glissando modifier)
            if (ch == 45) {
                typ = 5;
            }
            // % (tempo change)
            if ((ch == 37) && (num > 0)) {
                tempo = num;
                num = 0;
            }
            if ((ch >= 48) && (ch <= 57)) {
                // 0-9 (number)
                num = (num * 10) + (ch - 48);
            }
            if (ch == 46) {
                // . (duration modifier)
                num = ((num * 2) / (3));
            }
            if (((ch == 32) || (i+1 == notesLen)) && ((note > -99) || (typ != 3))) {
                if (num == 0) {
                    num = prevDuration;
                } else {
                    prevDuration = num;
                }
                ms = (int) Math.Round(320000.0 / (tempo * num));
                if (typ == 0) {
                    await this.addPulseToPlaySeq(0, ms);
                } else {
                    dNote = note - (((prevPitch) % (12)));
                    if (dNote > 6) {
                        dNote = dNote - 12;
                    }
                    if (dNote <= -6) {
                        dNote = dNote + 12;
                    }
                    pitch = prevPitch + dNote;
                    freq = (int) Math.Round(440 * Math.Exp(pitch * 0.05776226504666));
                    ms16 = ((ms) >> (4));
                    rest = 0;
                    if (typ == 3) {
                        rest = 2 * ms16;
                    }
                    if (typ == 2) {
                        rest = 8 * ms16;
                    }
                    if (typ == 1) {
                        rest = 12 * ms16;
                    }
                    if (typ == 5) {
                        await this.addPulseToPlaySeq(prevFreq, ms16);
                        await this.addFreqMoveToPlaySeq(freq, 8 * ms16);
                        await this.addPulseToPlaySeq(freq, ms - 9 * ms16);
                    } else {
                        await this.addPulseToPlaySeq(freq, ms - rest);
                        if (rest > 0) {
                            await this.addPulseToPlaySeq(0, rest);
                        }
                    }
                    prevFreq = freq;
                    prevPitch = pitch;
                }
                note = -99;
                num = 0;
                typ = 3;
            }
            i = i + 1;
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Starts the preprogrammed playing sequence.
     * <para>
     *   The sequence
     *   runs in loop until it is stopped by stopPlaySeq or an explicit
     *   change. To play the sequence only once, use <c>oncePlaySeq()</c>.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> startPlaySeq()
    {
        return await this.sendCommand("S");
    }

    /**
     * <summary>
     *   Stops the preprogrammed playing sequence and sets the frequency to zero.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> stopPlaySeq()
    {
        return await this.sendCommand("X");
    }

    /**
     * <summary>
     *   Resets the preprogrammed playing sequence and sets the frequency to zero.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> resetPlaySeq()
    {
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Starts the preprogrammed playing sequence and run it once only.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> oncePlaySeq()
    {
        return await this.sendCommand("s");
    }

    /**
     * <summary>
     *   Activates the buzzer for a short duration.
     * <para>
     * </para>
     * </summary>
     * <param name="frequency">
     *   pulse frequency, in hertz
     * </param>
     * <param name="duration">
     *   pulse duration in millseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> pulse(int frequency,int duration)
    {
        return await this.set_command("P"+Convert.ToString(frequency)+","+Convert.ToString(duration));
    }

    /**
     * <summary>
     *   Makes the buzzer frequency change over a period of time.
     * <para>
     * </para>
     * </summary>
     * <param name="frequency">
     *   frequency to reach, in hertz. A frequency under 25Hz stops the buzzer.
     * </param>
     * <param name="duration">
     *   pulse duration in millseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> freqMove(int frequency,int duration)
    {
        return await this.set_command("F"+Convert.ToString(frequency)+","+Convert.ToString(duration));
    }

    /**
     * <summary>
     *   Makes the buzzer volume change over a period of time, frequency  stays untouched.
     * <para>
     * </para>
     * </summary>
     * <param name="volume">
     *   volume to reach in %
     * </param>
     * <param name="duration">
     *   change duration in millseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> volumeMove(int volume,int duration)
    {
        return await this.set_command("V"+Convert.ToString(volume)+","+Convert.ToString(duration));
    }

    /**
     * <summary>
     *   Immediately play a note sequence.
     * <para>
     *   Notes are provided as text words, separated by
     *   spaces. The pitch is specified using the usual letter from A to G. The duration is
     *   specified as the divisor of a whole note: 4 for a fourth, 8 for an eight note, etc.
     *   Some modifiers are supported: <c>#</c> and <c>b</c> to alter a note pitch,
     *   <c>'</c> and <c>,</c> to move to the upper/lower octave, <c>.</c> to enlarge
     *   the note duration.
     * </para>
     * </summary>
     * <param name="notes">
     *   notes to be played, as a text string.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> playNotes(string notes)
    {
        await this.resetPlaySeq();
        await this.addNotesToPlaySeq(notes);
        return await this.oncePlaySeq();
    }

    /**
     * <summary>
     *   Continues the enumeration of buzzers started using <c>yFirstBuzzer()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBuzzer</c> object, corresponding to
     *   a buzzer currently online, or a <c>null</c> pointer
     *   if there are no more buzzers to enumerate.
     * </returns>
     */
    public YBuzzer nextBuzzer()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindBuzzerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of buzzers currently accessible.
     * <para>
     *   Use the method <c>YBuzzer.nextBuzzer()</c> to iterate on
     *   next buzzers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBuzzer</c> object, corresponding to
     *   the first buzzer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBuzzer FirstBuzzer()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Buzzer");
        if (next_hwid == null)  return null;
        return FindBuzzerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of buzzers currently accessible.
     * <para>
     *   Use the method <c>YBuzzer.nextBuzzer()</c> to iterate on
     *   next buzzers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YBuzzer</c> object, corresponding to
     *   the first buzzer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBuzzer FirstBuzzerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Buzzer");
        if (next_hwid == null)  return null;
        return FindBuzzerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YBuzzer implementation)
}
}

