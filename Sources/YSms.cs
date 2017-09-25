/*********************************************************************
 *
 * $Id: YSms.cs 27422 2017-05-11 10:01:51Z seb $
 *
 * Implements FindSms(), the high-level API for Sms functions
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
    //--- (generated code: YSms return codes)
//--- (end of generated code: YSms return codes)
    //--- (generated code: YSms class start)
/**
 * <summary>
 *   YSms Class: SMS message sent or received
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSms
{
//--- (end of generated code: YSms class start)
//--- (generated code: YSms definitions)
    protected YMessageBox _mbox;
    protected int _slot = 0;
    protected bool _deliv;
    protected string _smsc;
    protected int _mref = 0;
    protected string _orig;
    protected string _dest;
    protected int _pid = 0;
    protected int _alphab = 0;
    protected int _mclass = 0;
    protected string _stamp;
    protected byte[] _udh;
    protected byte[] _udata;
    protected int _npdu = 0;
    protected byte[] _pdu;
    protected List<YSms> _parts = new List<YSms>();
    protected string _aggSig;
    protected int _aggIdx = 0;
    protected int _aggCnt = 0;

    //--- (end of generated code: YSms definitions)


        /// <param name="mbox"> : message box </param>
        protected internal YSms(YMessageBox mbox) {
            _mbox = mbox;
            //--- (generated code: YSms attributes initialization)
        //--- (end of generated code: YSms attributes initialization)
        }


        //--- (generated code: YSms implementation)
#pragma warning disable 1998

    public virtual async Task<int> get_slot()
    {
        return _slot;
    }

    public virtual async Task<string> get_smsc()
    {
        return _smsc;
    }

    public virtual async Task<int> get_msgRef()
    {
        return _mref;
    }

    public virtual async Task<string> get_sender()
    {
        return _orig;
    }

    public virtual async Task<string> get_recipient()
    {
        return _dest;
    }

    public virtual async Task<int> get_protocolId()
    {
        return _pid;
    }

    public virtual async Task<bool> isReceived()
    {
        return _deliv;
    }

    public virtual async Task<int> get_alphabet()
    {
        return _alphab;
    }

    public virtual async Task<int> get_msgClass()
    {
        if (((_mclass) & (16)) == 0) {
            return -1;
        }
        return ((_mclass) & (3));
    }

    public virtual async Task<int> get_dcs()
    {
        return ((_mclass) | ((((_alphab) << (2)))));
    }

    public virtual async Task<string> get_timestamp()
    {
        return _stamp;
    }

    public virtual async Task<byte[]> get_userDataHeader()
    {
        return _udh;
    }

    public virtual async Task<byte[]> get_userData()
    {
        return _udata;
    }

    public virtual async Task<string> get_textData()
    {
        byte[] isolatin;
        int isosize;
        int i;
        if (_alphab == 0) {
            // using GSM standard 7-bit alphabet
            return await _mbox.gsm2str(_udata);
        }
        if (_alphab == 2) {
            // using UCS-2 alphabet
            isosize = (((_udata).Length) >> (1));
            isolatin = new byte[isosize];
            i = 0;
            while (i < isosize) {
                isolatin[i] = (byte)(_udata[2*i+1] & 0xff);
                i = i + 1;
            }
            return YAPI.DefaultEncoding.GetString(isolatin);
        }
        // default: convert 8 bit to string as-is
        return YAPI.DefaultEncoding.GetString(_udata);
    }

    public virtual async Task<List<int>> get_unicodeData()
    {
        List<int> res = new List<int>();
        int unisize;
        int unival;
        int i;
        if (_alphab == 0) {
            // using GSM standard 7-bit alphabet
            return await _mbox.gsm2unicode(_udata);
        }
        if (_alphab == 2) {
            // using UCS-2 alphabet
            unisize = (((_udata).Length) >> (1));
            res.Clear();
            i = 0;
            while (i < unisize) {
                unival = 256*_udata[2*i]+_udata[2*i+1];
                res.Add(unival);
                i = i + 1;
            }
        } else {
            // return straight 8-bit values
            unisize = (_udata).Length;
            res.Clear();
            i = 0;
            while (i < unisize) {
                res.Add(_udata[i]+0);
                i = i + 1;
            }
        }
        return res;
    }

    public virtual async Task<int> get_partCount()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _npdu;
    }

    public virtual async Task<byte[]> get_pdu()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _pdu;
    }

    public virtual async Task<List<YSms>> get_parts()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _parts;
    }

    public virtual async Task<string> get_concatSignature()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _aggSig;
    }

    public virtual async Task<int> get_concatIndex()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _aggIdx;
    }

    public virtual async Task<int> get_concatCount()
    {
        if (_npdu == 0) {
            await this.generatePdu();
        }
        return _aggCnt;
    }

    public virtual async Task<int> set_slot(int val)
    {
        _slot = val;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_received(bool val)
    {
        _deliv = val;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_smsc(string val)
    {
        _smsc = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_msgRef(int val)
    {
        _mref = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_sender(string val)
    {
        _orig = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_recipient(string val)
    {
        _dest = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_protocolId(int val)
    {
        _pid = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_alphabet(int val)
    {
        _alphab = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_msgClass(int val)
    {
        if (val == -1) {
            _mclass = 0;
        } else {
            _mclass = 16+val;
        }
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_dcs(int val)
    {
        _alphab = (((((val) >> (2)))) & (3));
        _mclass = ((val) & (16+3));
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_timestamp(string val)
    {
        _stamp = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_userDataHeader(byte[] val)
    {
        _udh = val;
        _npdu = 0;
        await this.parseUserDataHeader();
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> set_userData(byte[] val)
    {
        _udata = val;
        _npdu = 0;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> convertToUnicode()
    {
        List<int> ucs2 = new List<int>();
        int udatalen;
        int i;
        int uni;
        if (_alphab == 2) {
            return YAPI.SUCCESS;
        }
        if (_alphab == 0) {
            ucs2 = await _mbox.gsm2unicode(_udata);
        } else {
            udatalen = (_udata).Length;
            ucs2.Clear();
            i = 0;
            while (i < udatalen) {
                uni = _udata[i];
                ucs2.Add(uni);
                i = i + 1;
            }
        }
        _alphab = 2;
        _udata = new byte[0];
        await this.addUnicodeData(ucs2);
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> addText(string val)
    {
        byte[] udata;
        int udatalen;
        byte[] newdata;
        int newdatalen;
        int i;
        if ((val).Length == 0) {
            return YAPI.SUCCESS;
        }
        if (_alphab == 0) {
            // Try to append using GSM 7-bit alphabet
            newdata = await _mbox.str2gsm(val);
            newdatalen = (newdata).Length;
            if (newdatalen == 0) {
                // 7-bit not possible, switch to unicode
                await this.convertToUnicode();
                newdata = YAPI.DefaultEncoding.GetBytes(val);
                newdatalen = (newdata).Length;
            }
        } else {
            newdata = YAPI.DefaultEncoding.GetBytes(val);
            newdatalen = (newdata).Length;
        }
        udatalen = (_udata).Length;
        if (_alphab == 2) {
            // Append in unicode directly
            udata = new byte[udatalen + 2*newdatalen];
            i = 0;
            while (i < udatalen) {
                udata[i] = (byte)(_udata[i] & 0xff);
                i = i + 1;
            }
            i = 0;
            while (i < newdatalen) {
                udata[udatalen+1] = (byte)(newdata[i] & 0xff);
                udatalen = udatalen + 2;
                i = i + 1;
            }
        } else {
            // Append binary buffers
            udata = new byte[udatalen+newdatalen];
            i = 0;
            while (i < udatalen) {
                udata[i] = (byte)(_udata[i] & 0xff);
                i = i + 1;
            }
            i = 0;
            while (i < newdatalen) {
                udata[udatalen] = (byte)(newdata[i] & 0xff);
                udatalen = udatalen + 1;
                i = i + 1;
            }
        }
        return await this.set_userData(udata);
    }

    public virtual async Task<int> addUnicodeData(List<int> val)
    {
        int arrlen;
        int newdatalen;
        int i;
        int uni;
        byte[] udata;
        int udatalen;
        int surrogate;
        if (_alphab != 2) {
            await this.convertToUnicode();
        }
        // compute number of 16-bit code units
        arrlen = val.Count;
        newdatalen = arrlen;
        i = 0;
        while (i < arrlen) {
            uni = val[i];
            if (uni > 65535) {
                newdatalen = newdatalen + 1;
            }
            i = i + 1;
        }
        // now build utf-16 buffer
        udatalen = (_udata).Length;
        udata = new byte[udatalen+2*newdatalen];
        i = 0;
        while (i < udatalen) {
            udata[i] = (byte)(_udata[i] & 0xff);
            i = i + 1;
        }
        i = 0;
        while (i < arrlen) {
            uni = val[i];
            if (uni >= 65536) {
                surrogate = uni - 65536;
                uni = (((((surrogate) >> (10))) & (1023))) + 55296;
                udata[udatalen] = (byte)(((uni) >> (8)) & 0xff);
                udata[udatalen+1] = (byte)(((uni) & (255)) & 0xff);
                udatalen = udatalen + 2;
                uni = (((surrogate) & (1023))) + 56320;
            }
            udata[udatalen] = (byte)(((uni) >> (8)) & 0xff);
            udata[udatalen+1] = (byte)(((uni) & (255)) & 0xff);
            udatalen = udatalen + 2;
            i = i + 1;
        }
        return await this.set_userData(udata);
    }

    public virtual async Task<int> set_pdu(byte[] pdu)
    {
        _pdu = pdu;
        _npdu = 1;
        return await this.parsePdu(pdu);
    }

    public virtual async Task<int> set_parts(List<YSms> parts)
    {
        List<YSms> sorted = new List<YSms>();
        int partno;
        int initpartno;
        int i;
        int retcode;
        int totsize;
        YSms subsms;
        byte[] subdata;
        byte[] res;
        _npdu = parts.Count;
        if (_npdu == 0) {
            return YAPI.INVALID_ARGUMENT;
        }
        sorted.Clear();
        partno = 0;
        while (partno < _npdu) {
            initpartno = partno;
            i = 0;
            while (i < _npdu) {
                subsms = parts[i];
                if (await subsms.get_concatIndex() == partno) {
                    sorted.Add(subsms);
                    partno = partno + 1;
                }
                i = i + 1;
            }
            if (initpartno == partno) {
                partno = partno + 1;
            }
        }
        _parts = sorted;
        _npdu = sorted.Count;
        // inherit header fields from first part
        subsms = _parts[0];
        retcode = await this.parsePdu(await subsms.get_pdu());
        if (retcode != YAPI.SUCCESS) {
            return retcode;
        }
        // concatenate user data from all parts
        totsize = 0;
        partno = 0;
        while (partno < _parts.Count) {
            subsms = _parts[partno];
            subdata = await subsms.get_userData();
            totsize = totsize + (subdata).Length;
            partno = partno + 1;
        }
        res = new byte[totsize];
        totsize = 0;
        partno = 0;
        while (partno < _parts.Count) {
            subsms = _parts[partno];
            subdata = await subsms.get_userData();
            i = 0;
            while (i < (subdata).Length) {
                res[totsize] = (byte)(subdata[i] & 0xff);
                totsize = totsize + 1;
                i = i + 1;
            }
            partno = partno + 1;
        }
        _udata = res;
        return YAPI.SUCCESS;
    }

    public virtual async Task<byte[]> encodeAddress(string addr)
    {
        byte[] bytes;
        int srclen;
        int numlen;
        int i;
        int val;
        int digit;
        byte[] res;
        bytes = YAPI.DefaultEncoding.GetBytes(addr);
        srclen = (bytes).Length;
        numlen = 0;
        i = 0;
        while (i < srclen) {
            val = bytes[i];
            if ((val >= 48) && (val < 58)) {
                numlen = numlen + 1;
            }
            i = i + 1;
        }
        if (numlen == 0) {
            res = new byte[1];
            res[0] = (byte)(0 & 0xff);
            return res;
        }
        res = new byte[2+((numlen+1) >> (1))];
        res[0] = (byte)(numlen & 0xff);
        if (bytes[0] == 43) {
            res[1] = (byte)(145 & 0xff);
        } else {
            res[1] = (byte)(129 & 0xff);
        }
        numlen = 4;
        digit = 0;
        i = 0;
        while (i < srclen) {
            val = bytes[i];
            if ((val >= 48) && (val < 58)) {
                if (((numlen) & (1)) == 0) {
                    digit = val - 48;
                } else {
                    res[((numlen) >> (1))] = (byte)(digit + 16*(val-48) & 0xff);
                }
                numlen = numlen + 1;
            }
            i = i + 1;
        }
        // pad with F if needed
        if (((numlen) & (1)) != 0) {
            res[((numlen) >> (1))] = (byte)(digit + 240 & 0xff);
        }
        return res;
    }

    public virtual async Task<string> decodeAddress(byte[] addr,int ofs,int siz)
    {
        int addrType;
        byte[] gsm7;
        string res;
        int i;
        int rpos;
        int carry;
        int nbits;
        int byt;
        if (siz == 0) {
            return "";
        }
        res = "";
        addrType = ((addr[ofs]) & (112));
        if (addrType == 80) {
            // alphanumeric number
            siz = ((4*siz) / (7));
            gsm7 = new byte[siz];
            rpos = 1;
            carry = 0;
            nbits = 0;
            i = 0;
            while (i < siz) {
                if (nbits == 7) {
                    gsm7[i] = (byte)(carry & 0xff);
                    carry = 0;
                    nbits = 0;
                } else {
                    byt = addr[ofs+rpos];
                    rpos = rpos + 1;
                    gsm7[i] = (byte)(((carry) | ((((((byt) << (nbits)))) & (127)))) & 0xff);
                    carry = ((byt) >> ((7 - nbits)));
                    nbits = nbits + 1;
                }
                i = i + 1;
            }
            return await _mbox.gsm2str(gsm7);
        } else {
            // standard phone number
            if (addrType == 16) {
                res = "+";
            }
            siz = (((siz+1)) >> (1));
            i = 0;
            while (i < siz) {
                byt = addr[ofs+i+1];
                res = ""+ res+""+String.Format("{0:X}", ((byt) & (15)))+""+String.Format("{0:X}",((byt) >> (4)));
                i = i + 1;
            }
            // remove padding digit if needed
            if (((addr[ofs+siz]) >> (4)) == 15) {
                res = (res).Substring( 0, (res).Length-1);
            }
            return res;
        }
    }

    public virtual async Task<byte[]> encodeTimeStamp(string exp)
    {
        int explen;
        int i;
        byte[] res;
        int n;
        byte[] expasc;
        int v1;
        int v2;
        explen = (exp).Length;
        if (explen == 0) {
            res = new byte[0];
            return res;
        }
        if ((exp).Substring(0, 1) == "+") {
            n = YAPIContext.imm_atoi((exp).Substring(1, explen-1));
            res = new byte[1];
            if (n > 30*86400) {
                n = 192+(((n+6*86400)) / ((7*86400)));
            } else {
                if (n > 86400) {
                    n = 166+(((n+86399)) / (86400));
                } else {
                    if (n > 43200) {
                        n = 143+(((n-43200+1799)) / (1800));
                    } else {
                        n = -1+(((n+299)) / (300));
                    }
                }
            }
            if (n < 0) {
                n = 0;
            }
            res[0] = (byte)(n & 0xff);
            return res;
        }
        if ((exp).Substring(4, 1) == "-" || (exp).Substring(4, 1) == "/") {
            // ignore century
            exp = (exp).Substring( 2, explen-2);
            explen = (exp).Length;
        }
        expasc = YAPI.DefaultEncoding.GetBytes(exp);
        res = new byte[7];
        n = 0;
        i = 0;
        while ((i+1 < explen) && (n < 7)) {
            v1 = expasc[i];
            if ((v1 >= 48) && (v1 < 58)) {
                v2 = expasc[i+1];
                if ((v2 >= 48) && (v2 < 58)) {
                    v1 = v1 - 48;
                    v2 = v2 - 48;
                    res[n] = (byte)((((v2) << (4))) + v1 & 0xff);
                    n = n + 1;
                    i = i + 1;
                }
            }
            i = i + 1;
        }
        while (n < 7) {
            res[n] = (byte)(0 & 0xff);
            n = n + 1;
        }
        if (i+2 < explen) {
            // convert for timezone in cleartext ISO format +/-nn:nn
            v1 = expasc[i-3];
            v2 = expasc[i];
            if (((v1 == 43) || (v1 == 45)) && (v2 == 58)) {
                v1 = expasc[i+1];
                v2 = expasc[i+2];
                if ((v1 >= 48) && (v1 < 58) && (v1 >= 48) && (v1 < 58)) {
                    v1 = (((10*(v1 - 48)+(v2 - 48))) / (15));
                    n = n - 1;
                    v2 = 4 * res[n] + v1;
                    if (expasc[i-3] == 45) {
                        v2 += 128;
                    }
                    res[n] = (byte)(v2 & 0xff);
                }
            }
        }
        return res;
    }

    public virtual async Task<string> decodeTimeStamp(byte[] exp,int ofs,int siz)
    {
        int n;
        string res;
        int i;
        int byt;
        string sign;
        string hh;
        string ss;
        if (siz < 1) {
            return "";
        }
        if (siz == 1) {
            n = exp[ofs];
            if (n < 144) {
                n = n * 300;
            } else {
                if (n < 168) {
                    n = (n-143) * 1800;
                } else {
                    if (n < 197) {
                        n = (n-166) * 86400;
                    } else {
                        n = (n-192) * 7 * 86400;
                    }
                }
            }
            return "+"+Convert.ToString(n);
        }
        res = "20";
        i = 0;
        while ((i < siz) && (i < 6)) {
            byt = exp[ofs+i];
            res = ""+ res+""+String.Format("{0:X}", ((byt) & (15)))+""+String.Format("{0:X}",((byt) >> (4)));
            if (i < 3) {
                if (i < 2) {
                    res = ""+res+"-";
                } else {
                    res = ""+res+" ";
                }
            } else {
                if (i < 5) {
                    res = ""+res+":";
                }
            }
            i = i + 1;
        }
        if (siz == 7) {
            byt = exp[ofs+i];
            sign = "+";
            if (((byt) & (8)) != 0) {
                byt = byt - 8;
                sign = "-";
            }
            byt = (10*(((byt) & (15)))) + (((byt) >> (4)));
            hh = ""+Convert.ToString(((byt) >> (2)));
            ss = ""+Convert.ToString(15*(((byt) & (3))));
            if ((hh).Length<2) {
                hh = "0"+hh;
            }
            if ((ss).Length<2) {
                ss = "0"+ss;
            }
            res = ""+ res+""+ sign+""+ hh+":"+ss;
        }
        return res;
    }

    public virtual async Task<int> udataSize()
    {
        int res;
        int udhsize;
        udhsize = (_udh).Length;
        res = (_udata).Length;
        if (_alphab == 0) {
            if (udhsize > 0) {
                res = res + (((8 + 8*udhsize + 6)) / (7));
            }
            res = (((res * 7 + 7)) / (8));
        } else {
            if (udhsize > 0) {
                res = res + 1 + udhsize;
            }
        }
        return res;
    }

    public virtual async Task<byte[]> encodeUserData()
    {
        int udsize;
        int udlen;
        int udhsize;
        int udhlen;
        byte[] res;
        int i;
        int wpos;
        int carry;
        int nbits;
        int thisb;
        // nbits = number of bits in carry
        udsize = await this.udataSize();
        udhsize = (_udh).Length;
        udlen = (_udata).Length;
        res = new byte[1+udsize];
        udhlen = 0;
        nbits = 0;
        carry = 0;
        // 1. Encode UDL
        if (_alphab == 0) {
            // 7-bit encoding
            if (udhsize > 0) {
                udhlen = (((8 + 8*udhsize + 6)) / (7));
                nbits = 7*udhlen - 8 - 8*udhsize;
            }
            res[0] = (byte)(udhlen+udlen & 0xff);
        } else {
            // 8-bit encoding
            res[0] = (byte)(udsize & 0xff);
        }
        // 2. Encode UDHL and UDL
        wpos = 1;
        if (udhsize > 0) {
            res[wpos] = (byte)(udhsize & 0xff);
            wpos = wpos + 1;
            i = 0;
            while (i < udhsize) {
                res[wpos] = (byte)(_udh[i] & 0xff);
                wpos = wpos + 1;
                i = i + 1;
            }
        }
        // 3. Encode UD
        if (_alphab == 0) {
            // 7-bit encoding
            i = 0;
            while (i < udlen) {
                if (nbits == 0) {
                    carry = _udata[i];
                    nbits = 7;
                } else {
                    thisb = _udata[i];
                    res[wpos] = (byte)(((carry) | ((((((thisb) << (nbits)))) & (255)))) & 0xff);
                    wpos = wpos + 1;
                    nbits = nbits - 1;
                    carry = ((thisb) >> ((7 - nbits)));
                }
                i = i + 1;
            }
            if (nbits > 0) {
                res[wpos] = (byte)(carry & 0xff);
            }
        } else {
            // 8-bit encoding
            i = 0;
            while (i < udlen) {
                res[wpos] = (byte)(_udata[i] & 0xff);
                wpos = wpos + 1;
                i = i + 1;
            }
        }
        return res;
    }

    public virtual async Task<int> generateParts()
    {
        int udhsize;
        int udlen;
        int mss;
        int partno;
        int partlen;
        byte[] newud;
        byte[] newudh;
        YSms newpdu;
        int i;
        int wpos;
        udhsize = (_udh).Length;
        udlen = (_udata).Length;
        mss = 140 - 1 - 5 - udhsize;
        if (_alphab == 0) {
            mss = (((mss * 8 - 6)) / (7));
        }
        _npdu = (((udlen+mss-1)) / (mss));
        _parts.Clear();
        partno = 0;
        wpos = 0;
        while (wpos < udlen) {
            partno = partno + 1;
            newudh = new byte[5+udhsize];
            newudh[0] = (byte)(0 & 0xff);           // IEI: concatenated message
            newudh[1] = (byte)(3 & 0xff);           // IEDL: 3 bytes
            newudh[2] = (byte)(_mref & 0xff);
            newudh[3] = (byte)(_npdu & 0xff);
            newudh[4] = (byte)(partno & 0xff);
            i = 0;
            while (i < udhsize) {
                newudh[5+i] = (byte)(_udh[i] & 0xff);
                i = i + 1;
            }
            if (wpos+mss < udlen) {
                partlen = mss;
            } else {
                partlen = udlen-wpos;
            }
            newud = new byte[partlen];
            i = 0;
            while (i < partlen) {
                newud[i] = (byte)(_udata[wpos] & 0xff);
                wpos = wpos + 1;
                i = i + 1;
            }
            newpdu = new YSms(_mbox);
            await newpdu.set_received(await this.isReceived());
            await newpdu.set_smsc(await this.get_smsc());
            await newpdu.set_msgRef(await this.get_msgRef());
            await newpdu.set_sender(await this.get_sender());
            await newpdu.set_recipient(await this.get_recipient());
            await newpdu.set_protocolId(await this.get_protocolId());
            await newpdu.set_dcs(await this.get_dcs());
            await newpdu.set_timestamp(await this.get_timestamp());
            await newpdu.set_userDataHeader(newudh);
            await newpdu.set_userData(newud);
            _parts.Add(newpdu);
        }
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> generatePdu()
    {
        byte[] sca;
        byte[] hdr;
        byte[] addr;
        byte[] stamp;
        byte[] udata;
        int pdutyp;
        int pdulen;
        int i;
        // Determine if the message can fit within a single PDU
        _parts.Clear();
        if (await this.udataSize() > 140) {
            // multiple PDU are needed
            _pdu = new byte[0];
            return await this.generateParts();
        }
        sca = await this.encodeAddress(_smsc);
        if ((sca).Length > 0) {
            sca[0] = (byte)((sca).Length-1 & 0xff);
        }
        stamp = await this.encodeTimeStamp(_stamp);
        udata = await this.encodeUserData();
        if (_deliv) {
            addr = await this.encodeAddress(_orig);
            hdr = new byte[1];
            pdutyp = 0;
        } else {
            addr = await this.encodeAddress(_dest);
            _mref = await _mbox.nextMsgRef();
            hdr = new byte[2];
            hdr[1] = (byte)(_mref & 0xff);
            pdutyp = 1;
            if ((stamp).Length > 0) {
                pdutyp = pdutyp + 16;
            }
            if ((stamp).Length == 7) {
                pdutyp = pdutyp + 8;
            }
        }
        if ((_udh).Length > 0) {
            pdutyp = pdutyp + 64;
        }
        hdr[0] = (byte)(pdutyp & 0xff);
        pdulen = (sca).Length+(hdr).Length+(addr).Length+2+(stamp).Length+(udata).Length;
        _pdu = new byte[pdulen];
        pdulen = 0;
        i = 0;
        while (i < (sca).Length) {
            _pdu[pdulen] = (byte)(sca[i] & 0xff);
            pdulen = pdulen + 1;
            i = i + 1;
        }
        i = 0;
        while (i < (hdr).Length) {
            _pdu[pdulen] = (byte)(hdr[i] & 0xff);
            pdulen = pdulen + 1;
            i = i + 1;
        }
        i = 0;
        while (i < (addr).Length) {
            _pdu[pdulen] = (byte)(addr[i] & 0xff);
            pdulen = pdulen + 1;
            i = i + 1;
        }
        _pdu[pdulen] = (byte)(_pid & 0xff);
        pdulen = pdulen + 1;
        _pdu[pdulen] = (byte)(await this.get_dcs() & 0xff);
        pdulen = pdulen + 1;
        i = 0;
        while (i < (stamp).Length) {
            _pdu[pdulen] = (byte)(stamp[i] & 0xff);
            pdulen = pdulen + 1;
            i = i + 1;
        }
        i = 0;
        while (i < (udata).Length) {
            _pdu[pdulen] = (byte)(udata[i] & 0xff);
            pdulen = pdulen + 1;
            i = i + 1;
        }
        _npdu = 1;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> parseUserDataHeader()
    {
        int udhlen;
        int i;
        int iei;
        int ielen;
        string sig;
        _aggSig = "";
        _aggIdx = 0;
        _aggCnt = 0;
        udhlen = (_udh).Length;
        i = 0;
        while (i+1 < udhlen) {
            iei = _udh[i];
            ielen = _udh[i+1];
            i = i + 2;
            if (i + ielen <= udhlen) {
                if ((iei == 0) && (ielen == 3)) {
                    // concatenated SMS, 8-bit ref
                    sig = ""+ _orig+"-"+ _dest+"-"+String.Format("{0:X02}",
                    _mref)+"-"+String.Format("{0:X02}",_udh[i]);
                    _aggSig = sig;
                    _aggCnt = _udh[i+1];
                    _aggIdx = _udh[i+2];
                }
                if ((iei == 8) && (ielen == 4)) {
                    // concatenated SMS, 16-bit ref
                    sig = ""+ _orig+"-"+ _dest+"-"+String.Format("{0:X02}",
                    _mref)+"-"+String.Format("{0:X02}", _udh[i])+""+String.Format("{0:X02}",_udh[i+1]);
                    _aggSig = sig;
                    _aggCnt = _udh[i+2];
                    _aggIdx = _udh[i+3];
                }
            }
            i = i + ielen;
        }
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> parsePdu(byte[] pdu)
    {
        int rpos;
        int addrlen;
        int pdutyp;
        int tslen;
        int dcs;
        int udlen;
        int udhsize;
        int udhlen;
        int i;
        int carry;
        int nbits;
        int thisb;
        _pdu = pdu;
        _npdu = 1;
        // parse meta-data
        _smsc = await this.decodeAddress(pdu,  1, 2*(pdu[0]-1));
        rpos = 1+pdu[0];
        pdutyp = pdu[rpos];
        rpos = rpos + 1;
        _deliv = (((pdutyp) & (3)) == 0);
        if (_deliv) {
            addrlen = pdu[rpos];
            rpos = rpos + 1;
            _orig = await this.decodeAddress(pdu,  rpos, addrlen);
            _dest = "";
            tslen = 7;
        } else {
            _mref = pdu[rpos];
            rpos = rpos + 1;
            addrlen = pdu[rpos];
            rpos = rpos + 1;
            _dest = await this.decodeAddress(pdu,  rpos, addrlen);
            _orig = "";
            if ((((pdutyp) & (16))) != 0) {
                if ((((pdutyp) & (8))) != 0) {
                    tslen = 7;
                } else {
                    tslen= 1;
                }
            } else {
                tslen = 0;
            }
        }
        rpos = rpos + ((((addrlen+3)) >> (1)));
        _pid = pdu[rpos];
        rpos = rpos + 1;
        dcs = pdu[rpos];
        rpos = rpos + 1;
        _alphab = (((((dcs) >> (2)))) & (3));
        _mclass = ((dcs) & (16+3));
        _stamp = await this.decodeTimeStamp(pdu,  rpos, tslen);
        rpos = rpos + tslen;
        // parse user data (including udh)
        nbits = 0;
        carry = 0;
        udlen = pdu[rpos];
        rpos = rpos + 1;
        if (((pdutyp) & (64)) != 0) {
            udhsize = pdu[rpos];
            rpos = rpos + 1;
            _udh = new byte[udhsize];
            i = 0;
            while (i < udhsize) {
                _udh[i] = (byte)(pdu[rpos] & 0xff);
                rpos = rpos + 1;
                i = i + 1;
            }
            if (_alphab == 0) {
                // 7-bit encoding
                udhlen = (((8 + 8*udhsize + 6)) / (7));
                nbits = 7*udhlen - 8 - 8*udhsize;
                if (nbits > 0) {
                    thisb = pdu[rpos];
                    rpos = rpos + 1;
                    carry = ((thisb) >> (nbits));
                    nbits = 8 - nbits;
                }
            } else {
                // byte encoding
                udhlen = 1+udhsize;
            }
            udlen = udlen - udhlen;
        } else {
            udhsize = 0;
            _udh = new byte[0];
        }
        _udata = new byte[udlen];
        if (_alphab == 0) {
            // 7-bit encoding
            i = 0;
            while (i < udlen) {
                if (nbits == 7) {
                    _udata[i] = (byte)(carry & 0xff);
                    carry = 0;
                    nbits = 0;
                } else {
                    thisb = pdu[rpos];
                    rpos = rpos + 1;
                    _udata[i] = (byte)(((carry) | ((((((thisb) << (nbits)))) & (127)))) & 0xff);
                    carry = ((thisb) >> ((7 - nbits)));
                    nbits = nbits + 1;
                }
                i = i + 1;
            }
        } else {
            // 8-bit encoding
            i = 0;
            while (i < udlen) {
                _udata[i] = (byte)(pdu[rpos] & 0xff);
                rpos = rpos + 1;
                i = i + 1;
            }
        }
        await this.parseUserDataHeader();
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> send()
    {
        int i;
        int retcode;
        YSms pdu;

        if (_npdu == 0) {
            await this.generatePdu();
        }
        if (_npdu == 1) {
            return await _mbox._upload("sendSMS", _pdu);
        }
        retcode = YAPI.SUCCESS;
        i = 0;
        while ((i < _npdu) && (retcode == YAPI.SUCCESS)) {
            pdu = _parts[i];
            retcode= await pdu.send();
            i = i + 1;
        }
        return retcode;
    }

    public virtual async Task<int> deleteFromSIM()
    {
        int i;
        int retcode;
        YSms pdu;

        if (_slot > 0) {
            return await _mbox.clearSIMSlot(_slot);
        }
        retcode = YAPI.SUCCESS;
        i = 0;
        while ((i < _npdu) && (retcode == YAPI.SUCCESS)) {
            pdu = _parts[i];
            retcode= await pdu.deleteFromSIM();
            i = i + 1;
        }
        return retcode;
    }

#pragma warning restore 1998
    //--- (end of generated code: YSms implementation)
    }

}