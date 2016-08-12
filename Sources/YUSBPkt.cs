
/*********************************************************************
 *
 * $Id: YUSBPkt.cs 25186 2016-08-12 17:15:06Z seb $
 *
 * YUSBPkt Class: USB packet definitions
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage.Streams;
using Buffer = System.Buffer;


namespace com.yoctopuce.YoctoAPI
{
    public class YPktStreamHead
    {


        internal uint PktNumber { get; set; }
        internal uint StreamType { get; set; }
        internal uint PktType { get; set; }
        private byte[] Data;
        internal uint Ofs { get; }
        internal uint Len { get; }

        public YPktStreamHead(uint pktNumber, uint pktType, uint streamType, byte[] data, uint ofs, uint len)
        {
            PktNumber = pktNumber;
            PktType = pktType;
            StreamType = streamType;
            Data = data;
            Ofs = ofs;
            Len = len;
        }

        internal byte imm_GetByte(uint ofs)
        {
            return Data[ofs + Ofs];
        }

        public override string ToString()
        {
            string type, stream;
            switch (PktType) {
                case YUSBPkt.YPKT_CONF:
                    type = "CONF";
                    switch (StreamType) {
                        case YUSBPkt.USB_CONF_RESET:
                            stream = "RESET";
                            break;
                        case YUSBPkt.USB_CONF_START:
                            stream = "START";
                            break;
                        default:
                            stream = "INVALID!";
                            break;
                    }
                    break;
                case YUSBPkt.YPKT_STREAM:
                    type = "STREAM";
                    switch (StreamType) {
                        case YGenericHub.YSTREAM_EMPTY:
                            stream = "EMPTY";
                            break;
                        case YGenericHub.YSTREAM_NOTICE:
                            stream = "NOTICE";
                            break;
                        case YGenericHub.YSTREAM_TCP:
                            stream = "TCP";
                            break;
                        case YGenericHub.YSTREAM_TCP_CLOSE:
                            stream = "TCP_CLOSE";
                            break;
                        case YGenericHub.YSTREAM_REPORT:
                            stream = "REPORT";
                            break;
                        case YGenericHub.YSTREAM_META:
                            stream = "META";
                            break;
                        case YGenericHub.YSTREAM_REPORT_V2:
                            stream = "REPORT_V2";
                            break;
                        case YGenericHub.YSTREAM_NOTICE_V2:
                            stream = "NOTICE_v2";
                            break;
                        default:
                            stream = "INVALID!";
                            break;
                    }
                    break;
                default:
                    type = "INVALID!";
                    stream = "INVALID!";
                    break;
            }
            return string.Format("Stream: type={0:D}({1}) stream/cmd={2:D}({3}) size={4:D} (pktno={5:D})", PktType, type, StreamType, stream, Len, PktNumber);
        }

        //decode
        public static YPktStreamHead imm_Decode(long pos, byte[] pkt)
        {
            if (pkt.Length < YUSBPkt.USB_PKT_STREAM_HEAD + pos ) {
                return null;
            }
            uint b = (uint)(pkt[pos++] & 0xff);
            uint pktNumber = b & 7;
            uint streamType = (b >> 3);
            b = (uint)(pkt[pos++] & 0xff);
            uint pktType = b & 3;
            uint dataLen = b >> 2;
            uint remaining = (uint)(pkt.Length - pos);
            if (dataLen > remaining) {
                throw new YAPI_Exception(YAPI.IO_ERROR, string.Format("invalid ystream header (invalid length {0:D}>{1:D})", dataLen, remaining));
            }
            return new YPktStreamHead(pktNumber, pktType, streamType, pkt, (uint)pos, dataLen);
        }


        public string imm_GetString(uint ofs, uint maxlen)
        {
            uint start = Ofs + ofs;
            int len = 0;
            while (len < maxlen && start + len < Data.Length) {
                if (Data[start + len] == 0)
                    break;
                len++;
            }
            return YAPI.DefaultEncoding.GetString(Data, (int)start, len);
        }

        public uint imm_CopyData(byte[] response, uint ofs)
        {
            Buffer.BlockCopy(Data, (int)Ofs, response, (int)ofs, (int)Len);
            return Len;
        }
    }


    public class YUSBPkt
    {

        protected internal const int USB_PKT_STREAM_HEAD = 2;
        // pkt type definitions
        protected internal const int YPKT_STREAM = 0;
        protected internal const int YPKT_CONF = 1;
        // pkt config type
        protected internal const int USB_CONF_RESET = 0;
        protected internal const int USB_CONF_START = 1;

        // generic pkt definitions
        protected internal const int YPKT_USB_LEGACY_VERSION_BCD = 0x0207;
        protected internal const int YPKT_USB_VERSION_BCD = 0x0208;
        public const int USB_PKT_SIZE = 64;
        protected internal const int USB_MAX_PKT_CONTENT_SIZE = 62;
        protected internal int _pktno = 0;
        protected internal List<YPktStreamHead> _streams;


        internal YUSBPkt(int pktno, List<YPktStreamHead> streams)
        {
            _streams = streams;
            _pktno = pktno;
        }


        internal virtual int Pktno {
            get {
                return _pktno;
            }
        }

        public virtual List<YPktStreamHead> Streams {
            get {
                return _streams;
            }
        }

        public override string ToString()
        {
            string dump = string.Format("pktno:{0:D} with {1:D} ystream\n", _pktno, _streams.Count);
            foreach (YPktStreamHead s in _streams) {
                dump += "\n" + s.ToString();
            }
            return dump;
        }

        public virtual string[] toStringARR()
        {
            string[] dump = new string[_streams.Count + 1];
            dump[0] = string.Format("pktno:{0:D} with {1:D} ystream\n", _pktno, _streams.Count);
            int pos = 1;
            foreach (YPktStreamHead s in _streams) {
                dump[pos++] = s.ToString();
            }
            return dump;
        }




        protected internal class ConfPktReset
        {
            internal int _api;
            internal int _ok;
            internal int _ifaceNo;
            internal int _nbIface;

            public ConfPktReset(int api, int ok, int ifaceno, int nbiface)
            {
                this._api = api;
                this._ok = ok;
                this._ifaceNo = ifaceno;
                this._nbIface = nbiface;
            }

            public virtual int Api {
                get {
                    return _api;
                }
            }

            public virtual int Ok {
                get {
                    return _ok;
                }
            }

            public virtual int IfaceNo {
                get {
                    return _ifaceNo;
                }
            }

            public virtual int NbIface {
                get {
                    return _nbIface;
                }
            }

            public static ConfPktReset imm_Decode(sbyte[] data)
            {
                int api = data[0] + ((int)data[1] << 8);
                return new ConfPktReset(api, data[2], data[3], data[4]);
            }

        }

        protected internal class ConfPktStart
        {

            internal readonly int _nbIface;
            internal readonly int _ack_delay;

            public ConfPktStart(int nbiface, int ack_delay)
            {
                _nbIface = nbiface;
                _ack_delay = ack_delay;
            }


            public static ConfPktStart imm_Decode(sbyte[] data)
            {
                int nbiface = data[0] & 0xff;
                int ackDelay;
                if (data.Length >= 2) {
                    ackDelay = data[1] & 0xff;
                } else {
                    ackDelay = 0;
                }
                return new ConfPktStart(nbiface, ackDelay);
            }

            public virtual int AckDelay {
                get {
                    return _ack_delay;
                }
            }
        }

        public static void imm_FormatConfReset(HidOutputReport report, uint api_version)
        {
            byte[] raw = new byte[USB_PKT_SIZE + 1];
            raw[0] = 0;
            raw[1] = 0 + (USB_CONF_RESET << 3);
            raw[2] = YPKT_CONF + (USB_MAX_PKT_CONTENT_SIZE << 2);
            raw[3] = (byte)(api_version & 0xff);
            raw[4] = (byte)(api_version >> 8);
            raw[5] = 1; // nbifac
            raw[6] = 0; // ifaceno
            raw[7] = 1; // nbifac
            IBuffer buf = raw.AsBuffer();
            report.Data = buf;
        }

        public static void imm_FormatConfStart(HidOutputReport report, byte nbinface, byte pktAckDelay)
        {
            byte[] raw = new byte[USB_PKT_SIZE + 1];
            raw[0] = 0;
            raw[1] = 0 + (USB_CONF_START << 3);
            raw[2] = YPKT_CONF + (USB_MAX_PKT_CONTENT_SIZE << 2);
            raw[3] = nbinface; // nbifac
            raw[4] = pktAckDelay; // ifaceno
            report.Data = raw.AsBuffer();
        }


        public static int imm_FormatTCP(HidOutputReport outReport,  byte[] request, int pos, bool padWithEmpty)
        {
            int size, streamType, remaining;

            if (request != null) {
                int toWrite = request.Length - pos;
                streamType = YGenericHub.YSTREAM_TCP;
                if (toWrite < USB_MAX_PKT_CONTENT_SIZE) {
                    size = toWrite;
                    remaining = USB_PKT_SIZE - size - 2;
                } else {
                    size = USB_MAX_PKT_CONTENT_SIZE;
                    remaining = 0;
                }
            } else {
                streamType = YGenericHub.YSTREAM_TCP_CLOSE;
                size = 0;
                remaining = USB_PKT_SIZE - 2;
            }

            byte[] raw = new byte[USB_PKT_SIZE + 1];
            raw[0] = 0;
            raw[1] = (byte)(0 + (streamType << 3));
            raw[2] = (byte)(YPKT_STREAM + (size << 2));
            if (size > 0) {
                Buffer.BlockCopy(request, pos, raw, 3, size);
            }
            if (remaining > 2 && padWithEmpty) {
                raw[3 + size] = 0 + (YGenericHub.YSTREAM_EMPTY << 3);
                raw[4 + size] = (byte)(YPKT_STREAM + ((remaining - 2) << 2));
            }
            outReport.Data = raw.AsBuffer();
            return size;
        }

        public static void imm_FormatMetaUTC(HidOutputReport report, bool padWithEmpty)
        {
            byte[] raw = new byte[USB_PKT_SIZE + 1];
            raw[0] = 0;
            raw[1] = (byte)(0 + (YGenericHub.YSTREAM_META << 3));
            raw[2] = (byte)(YPKT_STREAM + (5 << 2));
            raw[3] = YGenericHub.USB_META_UTCTIME;
            UInt32 currUtcTime = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            raw[4] = (byte)(currUtcTime & 0xff);
            raw[5] = (byte)((currUtcTime >> 8) & 0xff);
            raw[6] = (byte)((currUtcTime >> 16) & 0xff);
            raw[7] = (byte)((currUtcTime >> 24) & 0xff);
            if (padWithEmpty) {
                raw[8] = 0 + (YGenericHub.YSTREAM_EMPTY << 3);
                raw[9] = (byte)(YPKT_STREAM + ((55) << 2));
            }
            report.Data = raw.AsBuffer();
        }
    }

}