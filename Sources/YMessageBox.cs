/*********************************************************************
 *
 * $Id: YMessageBox.cs 27700 2017-06-01 12:27:09Z seb $
 *
 * Implements FindMessageBox(), the high-level API for MessageBox functions
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
    //--- (generated code: YMessageBox return codes)
//--- (end of generated code: YMessageBox return codes)
    //--- (generated code: YMessageBox class start)
/**
 * <summary>
 *   YMessageBox Class: MessageBox function interface
 * <para>
 *   YMessageBox functions provides SMS sending and receiving capability to
 *   GSM-enabled Yoctopuce devices.
 * </para>
 * </summary>
 */
public class YMessageBox : YFunction
{
//--- (end of generated code: YMessageBox class start)
//--- (generated code: YMessageBox definitions)
    /**
     * <summary>
     *   invalid slotsInUse value
     * </summary>
     */
    public const  int SLOTSINUSE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid slotsCount value
     * </summary>
     */
    public const  int SLOTSCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid slotsBitmap value
     * </summary>
     */
    public const  string SLOTSBITMAP_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid pduSent value
     * </summary>
     */
    public const  int PDUSENT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid pduReceived value
     * </summary>
     */
    public const  int PDURECEIVED_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _slotsInUse = SLOTSINUSE_INVALID;
    protected int _slotsCount = SLOTSCOUNT_INVALID;
    protected string _slotsBitmap = SLOTSBITMAP_INVALID;
    protected int _pduSent = PDUSENT_INVALID;
    protected int _pduReceived = PDURECEIVED_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackMessageBox = null;
    protected int _nextMsgRef = 0;
    protected string _prevBitmapStr;
    protected List<YSms> _pdus = new List<YSms>();
    protected List<YSms> _messages = new List<YSms>();
    protected bool _gsm2unicodeReady;
    protected List<int> _gsm2unicode = new List<int>();
    protected byte[] _iso2gsm;

    public new delegate Task ValueCallback(YMessageBox func, string value);
    public new delegate Task TimedReportCallback(YMessageBox func, YMeasure measure);
    //--- (end of generated code: YMessageBox definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YMessageBox(YAPIContext ctx, String func)
            : base(ctx, func, "MessageBox")
        {
            //--- (generated code: YMessageBox attributes initialization)
        //--- (end of generated code: YMessageBox attributes initialization)
        }

        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YMessageBox(String func)
            : this(YAPI.imm_GetYCtx(), func)
        {}

        //--- (generated code: YMessageBox implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("slotsInUse")) {
            _slotsInUse = json_val.GetInt("slotsInUse");
        }
        if (json_val.Has("slotsCount")) {
            _slotsCount = json_val.GetInt("slotsCount");
        }
        if (json_val.Has("slotsBitmap")) {
            _slotsBitmap = json_val.GetString("slotsBitmap");
        }
        if (json_val.Has("pduSent")) {
            _pduSent = json_val.GetInt("pduSent");
        }
        if (json_val.Has("pduReceived")) {
            _pduReceived = json_val.GetInt("pduReceived");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of message storage slots currently in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of message storage slots currently in use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMessageBox.SLOTSINUSE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_slotsInUse()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SLOTSINUSE_INVALID;
            }
        }
        res = _slotsInUse;
        return res;
    }


    /**
     * <summary>
     *   Returns the total number of message storage slots on the SIM card.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of message storage slots on the SIM card
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMessageBox.SLOTSCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_slotsCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SLOTSCOUNT_INVALID;
            }
        }
        res = _slotsCount;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_slotsBitmap()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SLOTSBITMAP_INVALID;
            }
        }
        res = _slotsBitmap;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of SMS units sent so far.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of SMS units sent so far
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMessageBox.PDUSENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_pduSent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PDUSENT_INVALID;
            }
        }
        res = _pduSent;
        return res;
    }


    /**
     * <summary>
     *   Changes the value of the outgoing SMS units counter.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the value of the outgoing SMS units counter
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
    public async Task<int> set_pduSent(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pduSent",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the number of SMS units received so far.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of SMS units received so far
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMessageBox.PDURECEIVED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_pduReceived()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PDURECEIVED_INVALID;
            }
        }
        res = _pduReceived;
        return res;
    }


    /**
     * <summary>
     *   Changes the value of the incoming SMS units counter.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the value of the incoming SMS units counter
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
    public async Task<int> set_pduReceived(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pduReceived",rest_val);
        return YAPI.SUCCESS;
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
     *   Retrieves a MessageBox interface for a given identifier.
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
     *   This function does not require that the MessageBox interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMessageBox.isOnline()</c> to test if the MessageBox interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a MessageBox interface by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the MessageBox interface
     * </param>
     * <returns>
     *   a <c>YMessageBox</c> object allowing you to drive the MessageBox interface.
     * </returns>
     */
    public static YMessageBox FindMessageBox(string func)
    {
        YMessageBox obj;
        obj = (YMessageBox) YFunction._FindFromCache("MessageBox", func);
        if (obj == null) {
            obj = new YMessageBox(func);
            YFunction._AddToCache("MessageBox",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a MessageBox interface for a given identifier in a YAPI context.
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
     *   This function does not require that the MessageBox interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMessageBox.isOnline()</c> to test if the MessageBox interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a MessageBox interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the MessageBox interface
     * </param>
     * <returns>
     *   a <c>YMessageBox</c> object allowing you to drive the MessageBox interface.
     * </returns>
     */
    public static YMessageBox FindMessageBoxInContext(YAPIContext yctx,string func)
    {
        YMessageBox obj;
        obj = (YMessageBox) YFunction._FindFromCacheInContext(yctx,  "MessageBox", func);
        if (obj == null) {
            obj = new YMessageBox(yctx, func);
            YFunction._AddToCache("MessageBox",  func, obj);
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
        _valueCallbackMessageBox = callback;
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
        if (_valueCallbackMessageBox != null) {
            await _valueCallbackMessageBox(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> nextMsgRef()
    {
        _nextMsgRef = _nextMsgRef + 1;
        return _nextMsgRef;
    }

    public virtual async Task<int> clearSIMSlot(int slot)
    {
        _prevBitmapStr = "";
        return await this.set_command("DS"+Convert.ToString(slot));
    }

    public virtual async Task<YSms> fetchPdu(int slot)
    {
        byte[] binPdu;
        List<string> arrPdu = new List<string>();
        string hexPdu;
        YSms sms;

        binPdu = await this._download("sms.json?pos="+Convert.ToString(slot)+"&len=1");
        arrPdu = this.imm_json_get_array(binPdu);
        hexPdu = this.imm_decode_json_string(arrPdu[0]);
        sms = new YSms(this);
        await sms.set_slot(slot);
        await sms.parsePdu(YAPIContext.imm_hexStrToBin(hexPdu));
        return sms;
    }

    public virtual async Task<int> initGsm2Unicode()
    {
        int i;
        int uni;
        _gsm2unicode.Clear();
        // 00-07
        _gsm2unicode.Add(64);
        _gsm2unicode.Add(163);
        _gsm2unicode.Add(36);
        _gsm2unicode.Add(165);
        _gsm2unicode.Add(232);
        _gsm2unicode.Add(233);
        _gsm2unicode.Add(249);
        _gsm2unicode.Add(236);
        // 08-0F
        _gsm2unicode.Add(242);
        _gsm2unicode.Add(199);
        _gsm2unicode.Add(10);
        _gsm2unicode.Add(216);
        _gsm2unicode.Add(248);
        _gsm2unicode.Add(13);
        _gsm2unicode.Add(197);
        _gsm2unicode.Add(229);
        // 10-17
        _gsm2unicode.Add(916);
        _gsm2unicode.Add(95);
        _gsm2unicode.Add(934);
        _gsm2unicode.Add(915);
        _gsm2unicode.Add(923);
        _gsm2unicode.Add(937);
        _gsm2unicode.Add(928);
        _gsm2unicode.Add(936);
        // 18-1F
        _gsm2unicode.Add(931);
        _gsm2unicode.Add(920);
        _gsm2unicode.Add(926);
        _gsm2unicode.Add(27);
        _gsm2unicode.Add(198);
        _gsm2unicode.Add(230);
        _gsm2unicode.Add(223);
        _gsm2unicode.Add(201);
        // 20-7A
        i = 32;
        while (i <= 122) {
            _gsm2unicode.Add(i);
            i = i + 1;
        }
        // exceptions in range 20-7A
        _gsm2unicode[36] = 164;
        _gsm2unicode[64] = 161;
        _gsm2unicode[91] = 196;
        _gsm2unicode[92] = 214;
        _gsm2unicode[93] = 209;
        _gsm2unicode[94] = 220;
        _gsm2unicode[95] = 167;
        _gsm2unicode[96] = 191;
        // 7B-7F
        _gsm2unicode.Add(228);
        _gsm2unicode.Add(246);
        _gsm2unicode.Add(241);
        _gsm2unicode.Add(252);
        _gsm2unicode.Add(224);
        // Invert table as well wherever possible
        _iso2gsm = new byte[256];
        i = 0;
        while (i <= 127) {
            uni = _gsm2unicode[i];
            if (uni <= 255) {
                _iso2gsm[uni] = (byte)(i & 0xff);
            }
            i = i + 1;
        }
        i = 0;
        while (i < 4) {
            // mark escape sequences
            _iso2gsm[91+i] = (byte)(27 & 0xff);
            _iso2gsm[123+i] = (byte)(27 & 0xff);
            i = i + 1;
        }
        // Done
        _gsm2unicodeReady = true;
        return YAPI.SUCCESS;
    }

    public virtual async Task<List<int>> gsm2unicode(byte[] gsm)
    {
        int i;
        int gsmlen;
        int reslen;
        List<int> res = new List<int>();
        int uni;
        if (!(_gsm2unicodeReady)) {
            await this.initGsm2Unicode();
        }
        gsmlen = (gsm).Length;
        reslen = gsmlen;
        i = 0;
        while (i < gsmlen) {
            if (gsm[i] == 27) {
                reslen = reslen - 1;
            }
            i = i + 1;
        }
        res.Clear();
        i = 0;
        while (i < gsmlen) {
            uni = _gsm2unicode[gsm[i]];
            if ((uni == 27) && (i+1 < gsmlen)) {
                i = i + 1;
                uni = gsm[i];
                if (uni < 60) {
                    if (uni < 41) {
                        if (uni==20) {
                            uni=94;
                        } else {
                            if (uni==40) {
                                uni=123;
                            } else {
                                uni=0;
                            }
                        }
                    } else {
                        if (uni==41) {
                            uni=125;
                        } else {
                            if (uni==47) {
                                uni=92;
                            } else {
                                uni=0;
                            }
                        }
                    }
                } else {
                    if (uni < 62) {
                        if (uni==60) {
                            uni=91;
                        } else {
                            if (uni==61) {
                                uni=126;
                            } else {
                                uni=0;
                            }
                        }
                    } else {
                        if (uni==62) {
                            uni=93;
                        } else {
                            if (uni==64) {
                                uni=124;
                            } else {
                                if (uni==101) {
                                    uni=164;
                                } else {
                                    uni=0;
                                }
                            }
                        }
                    }
                }
            }
            if (uni > 0) {
                res.Add(uni);
            }
            i = i + 1;
        }
        return res;
    }

    public virtual async Task<string> gsm2str(byte[] gsm)
    {
        int i;
        int gsmlen;
        int reslen;
        byte[] resbin;
        string resstr;
        int uni;
        if (!(_gsm2unicodeReady)) {
            await this.initGsm2Unicode();
        }
        gsmlen = (gsm).Length;
        reslen = gsmlen;
        i = 0;
        while (i < gsmlen) {
            if (gsm[i] == 27) {
                reslen = reslen - 1;
            }
            i = i + 1;
        }
        resbin = new byte[reslen];
        i = 0;
        reslen = 0;
        while (i < gsmlen) {
            uni = _gsm2unicode[gsm[i]];
            if ((uni == 27) && (i+1 < gsmlen)) {
                i = i + 1;
                uni = gsm[i];
                if (uni < 60) {
                    if (uni < 41) {
                        if (uni==20) {
                            uni=94;
                        } else {
                            if (uni==40) {
                                uni=123;
                            } else {
                                uni=0;
                            }
                        }
                    } else {
                        if (uni==41) {
                            uni=125;
                        } else {
                            if (uni==47) {
                                uni=92;
                            } else {
                                uni=0;
                            }
                        }
                    }
                } else {
                    if (uni < 62) {
                        if (uni==60) {
                            uni=91;
                        } else {
                            if (uni==61) {
                                uni=126;
                            } else {
                                uni=0;
                            }
                        }
                    } else {
                        if (uni==62) {
                            uni=93;
                        } else {
                            if (uni==64) {
                                uni=124;
                            } else {
                                if (uni==101) {
                                    uni=164;
                                } else {
                                    uni=0;
                                }
                            }
                        }
                    }
                }
            }
            if ((uni > 0) && (uni < 256)) {
                resbin[reslen] = (byte)(uni & 0xff);
                reslen = reslen + 1;
            }
            i = i + 1;
        }
        resstr = YAPI.DefaultEncoding.GetString(resbin);
        if ((resstr).Length > reslen) {
            resstr = (resstr).Substring(0, reslen);
        }
        return resstr;
    }

    public virtual async Task<byte[]> str2gsm(string msg)
    {
        byte[] asc;
        int asclen;
        int i;
        int ch;
        int gsm7;
        int extra;
        byte[] res;
        int wpos;
        if (!(_gsm2unicodeReady)) {
            await this.initGsm2Unicode();
        }
        asc = YAPI.DefaultEncoding.GetBytes(msg);
        asclen = (asc).Length;
        extra = 0;
        i = 0;
        while (i < asclen) {
            ch = asc[i];
            gsm7 = _iso2gsm[ch];
            if (gsm7 == 27) {
                extra = extra + 1;
            }
            if (gsm7 == 0) {
                // cannot use standard GSM encoding
                res = new byte[0];
                return res;
            }
            i = i + 1;
        }
        res = new byte[asclen+extra];
        wpos = 0;
        i = 0;
        while (i < asclen) {
            ch = asc[i];
            gsm7 = _iso2gsm[ch];
            res[wpos] = (byte)(gsm7 & 0xff);
            wpos = wpos + 1;
            if (gsm7 == 27) {
                if (ch < 100) {
                    if (ch<93) {
                        if (ch<92) {
                            gsm7=60;
                        } else {
                            gsm7=47;
                        }
                    } else {
                        if (ch<94) {
                            gsm7=62;
                        } else {
                            gsm7=20;
                        }
                    }
                } else {
                    if (ch<125) {
                        if (ch<124) {
                            gsm7=40;
                        } else {
                            gsm7=64;
                        }
                    } else {
                        if (ch<126) {
                            gsm7=41;
                        } else {
                            gsm7=61;
                        }
                    }
                }
                res[wpos] = (byte)(gsm7 & 0xff);
                wpos = wpos + 1;
            }
            i = i + 1;
        }
        return res;
    }

    public virtual async Task<int> checkNewMessages()
    {
        string bitmapStr;
        byte[] prevBitmap;
        byte[] newBitmap;
        int slot;
        int nslots;
        int pduIdx;
        int idx;
        int bitVal;
        int prevBit;
        int i;
        int nsig;
        int cnt;
        string sig;
        List<YSms> newArr = new List<YSms>();
        List<YSms> newMsg = new List<YSms>();
        List<YSms> newAgg = new List<YSms>();
        List<string> signatures = new List<string>();
        YSms sms;

        bitmapStr = await this.get_slotsBitmap();
        if (bitmapStr == _prevBitmapStr) {
            return YAPI.SUCCESS;
        }
        prevBitmap = YAPIContext.imm_hexStrToBin(_prevBitmapStr);
        newBitmap = YAPIContext.imm_hexStrToBin(bitmapStr);
        _prevBitmapStr = bitmapStr;
        nslots = 8*(newBitmap).Length;
        newArr.Clear();
        newMsg.Clear();
        signatures.Clear();
        nsig = 0;
        // copy known messages
        pduIdx = 0;
        while (pduIdx < _pdus.Count) {
            sms = _pdus[pduIdx];
            slot = await sms.get_slot();
            idx = ((slot) >> (3));
            if (idx < (newBitmap).Length) {
                bitVal = ((1) << ((((slot) & (7)))));
                if ((((newBitmap[idx]) & (bitVal))) != 0) {
                    newArr.Add(sms);
                    if (await sms.get_concatCount() == 0) {
                        newMsg.Add(sms);
                    } else {
                        sig = await sms.get_concatSignature();
                        i = 0;
                        while ((i < nsig) && ((sig).Length > 0)) {
                            if (signatures[i] == sig) {
                                sig = "";
                            }
                            i = i + 1;
                        }
                        if ((sig).Length > 0) {
                            signatures.Add(sig);
                            nsig = nsig + 1;
                        }
                    }
                }
            }
            pduIdx = pduIdx + 1;
        }
        // receive new messages
        slot = 0;
        while (slot < nslots) {
            idx = ((slot) >> (3));
            bitVal = ((1) << ((((slot) & (7)))));
            prevBit = 0;
            if (idx < (prevBitmap).Length) {
                prevBit = ((prevBitmap[idx]) & (bitVal));
            }
            if ((((newBitmap[idx]) & (bitVal))) != 0) {
                if (prevBit == 0) {
                    sms = await this.fetchPdu(slot);
                    newArr.Add(sms);
                    if (await sms.get_concatCount() == 0) {
                        newMsg.Add(sms);
                    } else {
                        sig = await sms.get_concatSignature();
                        i = 0;
                        while ((i < nsig) && ((sig).Length > 0)) {
                            if (signatures[i] == sig) {
                                sig = "";
                            }
                            i = i + 1;
                        }
                        if ((sig).Length > 0) {
                            signatures.Add(sig);
                            nsig = nsig + 1;
                        }
                    }
                }
            }
            slot = slot + 1;
        }
        _pdus = newArr;
        // append complete concatenated messages
        i = 0;
        while (i < nsig) {
            sig = signatures[i];
            cnt = 0;
            pduIdx = 0;
            while (pduIdx < _pdus.Count) {
                sms = _pdus[pduIdx];
                if (await sms.get_concatCount() > 0) {
                    if (await sms.get_concatSignature() == sig) {
                        if (cnt == 0) {
                            cnt = await sms.get_concatCount();
                            newAgg.Clear();
                        }
                        newAgg.Add(sms);
                    }
                }
                pduIdx = pduIdx + 1;
            }
            if ((cnt > 0) && (newAgg.Count == cnt)) {
                sms = new YSms(this);
                await sms.set_parts(newAgg);
                newMsg.Add(sms);
            }
            i = i + 1;
        }
        _messages = newMsg;
        return YAPI.SUCCESS;
    }

    public virtual async Task<List<YSms>> get_pdus()
    {
        await this.checkNewMessages();
        return _pdus;
    }

    /**
     * <summary>
     *   Clear the SMS units counters.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> clearPduCounters()
    {
        int retcode;

        retcode = await this.set_pduReceived(0);
        if (retcode != YAPI.SUCCESS) {
            return retcode;
        }
        retcode = await this.set_pduSent(0);
        return retcode;
    }

    /**
     * <summary>
     *   Sends a regular text SMS, with standard parameters.
     * <para>
     *   This function can send messages
     *   of more than 160 characters, using SMS concatenation. ISO-latin accented characters
     *   are supported. For sending messages with special unicode characters such as asian
     *   characters and emoticons, use <c>newMessage</c> to create a new message and define
     *   the content of using methods <c>addText</c> and <c>addUnicodeData</c>.
     * </para>
     * </summary>
     * <param name="recipient">
     *   a text string with the recipient phone number, either as a
     *   national number, or in international format starting with a plus sign
     * </param>
     * <param name="message">
     *   the text to be sent in the message
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> sendTextMessage(string recipient,string message)
    {
        YSms sms;

        sms = new YSms(this);
        await sms.set_recipient(recipient);
        await sms.addText(message);
        return await sms.send();
    }

    /**
     * <summary>
     *   Sends a Flash SMS (class 0 message).
     * <para>
     *   Flash messages are displayed on the handset
     *   immediately and are usually not saved on the SIM card. This function can send messages
     *   of more than 160 characters, using SMS concatenation. ISO-latin accented characters
     *   are supported. For sending messages with special unicode characters such as asian
     *   characters and emoticons, use <c>newMessage</c> to create a new message and define
     *   the content of using methods <c>addText</c> et <c>addUnicodeData</c>.
     * </para>
     * </summary>
     * <param name="recipient">
     *   a text string with the recipient phone number, either as a
     *   national number, or in international format starting with a plus sign
     * </param>
     * <param name="message">
     *   the text to be sent in the message
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> sendFlashMessage(string recipient,string message)
    {
        YSms sms;

        sms = new YSms(this);
        await sms.set_recipient(recipient);
        await sms.set_msgClass(0);
        await sms.addText(message);
        return await sms.send();
    }

    /**
     * <summary>
     *   Creates a new empty SMS message, to be configured and sent later on.
     * <para>
     * </para>
     * </summary>
     * <param name="recipient">
     *   a text string with the recipient phone number, either as a
     *   national number, or in international format starting with a plus sign
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<YSms> newMessage(string recipient)
    {
        YSms sms;
        sms = new YSms(this);
        await sms.set_recipient(recipient);
        return sms;
    }

    /**
     * <summary>
     *   Returns the list of messages received and not deleted.
     * <para>
     *   This function
     *   will automatically decode concatenated SMS.
     * </para>
     * </summary>
     * <returns>
     *   an YSms object list.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list.
     * </para>
     */
    public virtual async Task<List<YSms>> get_messages()
    {
        await this.checkNewMessages();
        return _messages;
    }

    /**
     * <summary>
     *   Continues the enumeration of MessageBox interfaces started using <c>yFirstMessageBox()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMessageBox</c> object, corresponding to
     *   a MessageBox interface currently online, or a <c>null</c> pointer
     *   if there are no more MessageBox interfaces to enumerate.
     * </returns>
     */
    public YMessageBox nextMessageBox()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindMessageBoxInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of MessageBox interfaces currently accessible.
     * <para>
     *   Use the method <c>YMessageBox.nextMessageBox()</c> to iterate on
     *   next MessageBox interfaces.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMessageBox</c> object, corresponding to
     *   the first MessageBox interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMessageBox FirstMessageBox()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MessageBox");
        if (next_hwid == null)  return null;
        return FindMessageBoxInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of MessageBox interfaces currently accessible.
     * <para>
     *   Use the method <c>YMessageBox.nextMessageBox()</c> to iterate on
     *   next MessageBox interfaces.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YMessageBox</c> object, corresponding to
     *   the first MessageBox interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMessageBox FirstMessageBoxInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MessageBox");
        if (next_hwid == null)  return null;
        return FindMessageBoxInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YMessageBox implementation)
    }

}