using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{


    internal abstract class NotificationHandler
    {
        protected internal const char NOTIFY_NETPKT_NAME = '0';
        protected internal const char NOTIFY_NETPKT_CHILD = '2';
        protected internal const char NOTIFY_NETPKT_FUNCNAME = '4';
        protected internal const char NOTIFY_NETPKT_FUNCVAL = '5';
        protected internal const char NOTIFY_NETPKT_FUNCNAMEYDX = '8';
        protected internal const char NOTIFY_NETPKT_FLUSHV2YDX = 't';
        protected internal const char NOTIFY_NETPKT_FUNCV2YDX = 'u';
        protected internal const char NOTIFY_NETPKT_TIMEV2YDX = 'v';
        protected internal const char NOTIFY_NETPKT_DEVLOGYDX = 'w';
        protected internal const char NOTIFY_NETPKT_TIMEVALYDX = 'x';
        protected internal const char NOTIFY_NETPKT_FUNCVALYDX = 'y';
        protected internal const char NOTIFY_NETPKT_TIMEAVGYDX = 'z';
        protected internal const char NOTIFY_NETPKT_NOT_SYNC = '@';
        private const char NOTIFY_NETPKT_LOG = '7';
        private const int NOTIFY_NETPKT_STOP = 10;
        private const int NET_HUB_NOT_CONNECTION_TIMEOUT = 6000;


        protected internal long _notifyPos = -1;
        protected internal int _notifRetryCount = 0;
        protected internal int _error_delay = 0;


        protected internal readonly YHTTPHub _hub;


        internal NotificationHandler(YHTTPHub hub)
        {
            _hub = hub;
        }

        // Network notification format: 7x7bit (mapped to 7 chars in range 32..159)
        //                              used to represent 1 flag (RAW6BYTES) + 6 bytes
        // INPUT:  [R765432][1076543][2107654][3210765][4321076][5432107][6543210]
        // OUTPUT: 7 bytes array (1 byte for the funcTypeV2 and 6 bytes of USB like data
        //                     funcTypeV2 + [R][-byte 0][-byte 1-][-byte 2-][-byte 3-][-byte 4-][-byte 5-]
        //
        // return null on error
        //
        protected internal virtual byte[] decodeNetFuncValV2(byte[] p)
        {
            int p_ofs = 0;
            int ch = p[p_ofs] & 0xff;
            int len = 0;
            byte[] funcVal = new byte[7];
            Array.Clear(funcVal, 0, 7);
            if (ch < 32 || ch > 32 + 127) {
                return null;
            }
            // get the 7 first bits
            ch -= 32;
            funcVal[0] = (byte)((ch & 0x40) != 0 ? YGenericHub.NOTIFY_V2_6RAWBYTES : YGenericHub.NOTIFY_V2_TYPEDDATA);
            // clear flag
            ch &= 0x3f;
            while (len < YAPI.YOCTO_PUBVAL_SIZE) {
                p_ofs++;
                if (p_ofs >= p.Length) {
                    break;
                }
                int newCh = p[p_ofs] & 0xff;
                if (newCh == NOTIFY_NETPKT_STOP) {
                    break;
                }
                if (newCh < 32 || newCh > 32 + 127) {
                    return null;
                }
                newCh -= 32;
                ch = (ch << 7) + newCh;
                funcVal[len + 1] = (byte)(ch >> (5 - len));
                len++;
            }
            return funcVal;
        }

        protected internal virtual async Task handleNetNotification(string notification_line)
        {
            string ev = notification_line.Trim();

            if (ev.Length >= 3 && ev[0] >= NOTIFY_NETPKT_FLUSHV2YDX && ev[0] <= NOTIFY_NETPKT_TIMEAVGYDX) {
                // function value ydx (tiny notification)
                _hub._devListValidity = 10000;
                _notifRetryCount = 0;
                if (_notifyPos >= 0) {
                    _notifyPos += ev.Length + 1;
                }
                int devydx = ev[1] - 65; // from 'A'
                int funydx = ev[2] - 48; // from '0'

                if ((funydx & 64) != 0) { // high bit of devydx is on second character
                    funydx -= 64;
                    devydx += 128;
                }
                string value = ev.Substring(3);
                if (_hub._serialByYdx.ContainsKey(devydx)) {
                    string serial = _hub._serialByYdx[devydx];
                    string funcid;
                    YDevice ydev = _hub._yctx._yHash.imm_getDevice(serial);
                    if (ydev != null) {
                        switch (ev[0]) {
                            case NOTIFY_NETPKT_FUNCVALYDX:
                                funcid = ydev.imm_getYPEntry(funydx).FuncId;
                                if (!funcid.Equals("")) {
                                    // function value ydx (tiny notification)
                                    _hub.handleValueNotification(serial, funcid, value);
                                }
                                break;
                            case NOTIFY_NETPKT_DEVLOGYDX:
                                await ydev.triggerLogPull();
                                break;
                            case NOTIFY_NETPKT_TIMEVALYDX:
                            case NOTIFY_NETPKT_TIMEAVGYDX:
                            case NOTIFY_NETPKT_TIMEV2YDX:
                                if (funydx == 0xf) {
                                    int[] data = new int[5];
                                    for (int i = 0; i < 5; i++) {
                                        string part = value.Substring(i * 2, 2);
                                        data[i] = Convert.ToInt32(part, 16);
                                    }
                                    ydev.imm_setDeviceTime(data);
                                } else {
                                    funcid = ydev.imm_getYPEntry(funydx).FuncId;
                                    if (!funcid.Equals("")) {
                                        // timed value report
                                        List<int> report = new List<int>(1 + value.Length / 2);
                                        report.Add((ev[0] == NOTIFY_NETPKT_TIMEVALYDX ? 0 : (ev[0] == NOTIFY_NETPKT_TIMEAVGYDX ? 1 : 2)));
                                        for (int pos = 0; pos < value.Length; pos += 2) {
                                            int intval = Convert.ToInt32(value.Substring(pos, 2), 16);
                                            report.Add(intval);
                                        }
                                        _hub.imm_handleTimedNotification(serial, funcid, ydev.imm_getDeviceTime(), report);
                                    }
                                }
                                break;
                            case NOTIFY_NETPKT_FUNCV2YDX:
                                funcid = ydev.imm_getYPEntry(funydx).FuncId;
                                if (!funcid.Equals("")) {
                                    byte[] rawval = decodeNetFuncValV2(YAPI.DefaultEncoding.GetBytes(value));
                                    if (rawval != null) {
                                        string decodedval = YGenericHub.imm_decodePubVal(rawval[0], rawval, 1, 6);
                                        // function value ydx (tiny notification)
                                        _hub.handleValueNotification(serial, funcid, decodedval);
                                    }
                                }
                                break;
                            case NOTIFY_NETPKT_FLUSHV2YDX:
                                // To be implemented later
                                break;
                        }
                    }
                }
            } else if (ev.Length >= 5 && ev.StartsWith("YN01", StringComparison.Ordinal)) {
                _hub._devListValidity = 10000;
                _notifRetryCount = 0;
                if (_notifyPos >= 0) {
                    _notifyPos += ev.Length + 1;
                }
                char notype = ev[4];
                if (notype == NOTIFY_NETPKT_NOT_SYNC) {
                    _notifyPos = Convert.ToInt32(ev.Substring(5));
                } else {
                    switch (notype) {
                        case NOTIFY_NETPKT_NAME: // device name change, or arrival
                        case NOTIFY_NETPKT_CHILD: // device plug/unplug
                        case NOTIFY_NETPKT_FUNCNAME: // function name change
                        case NOTIFY_NETPKT_FUNCNAMEYDX: // function name change (ydx)
                            _hub._devListExpires = 0;
                            break;
                        case NOTIFY_NETPKT_FUNCVAL: // function value (long notification)
                            string[] parts = ev.Substring(5).Split(',');
                            _hub.handleValueNotification(parts[0], parts[1], parts[2]);
                            break;
                    }
                }
            } else {
                // oops, bad notification ? be safe until a good one comes
                _hub._devListValidity = 500;
                _notifyPos = -1;
            }
        }


        internal abstract Task<byte[]> hubRequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout);

        /// <param name="device">            the device that will receive the request</param>
        /// <param name="req_first_line">    first line of request without space, HTTP1.1 or \r\n </param>
        /// <param name="req_head_and_body"> http headers with double \r\n followed by potential body </param>
        /// <param name="mstimeout">         number of milisecond allowed to the request to finish </param>
        /// <param name="progress">          progress callback (usefull for file upload)</param>
        /// <param name="context">           context for progress callback</param>
        /// <returns> return the raw response without the http header </returns>
        internal abstract Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, uint mstimeout, YGenericHub.RequestProgress progress, object context);

        /// <param name="device">            the device that will receive the request</param>
        /// <param name="req_first_line">    first line of request without space, HTTP1.1 or \r\n </param>
        /// <param name="req_head_and_body"> http headers with double \r\n followed by potential body </param>
        /// <param name="asyncResult">       the callback to call when the request is done </param>
        /// <param name="asyncContext">      a pointer to a context to pass for the asyncResutl </param>
        internal abstract Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult asyncResult, object asyncContext);

        /// <summary>
        /// Wait until all pending async request have finished
        /// </summary>
        /// <param name="timeout"> the nubmer of milisecond before going into timeout </param>
        /// <returns> true if some request are still pendin </returns>
        internal abstract Task<bool> waitAndFreeAsyncTasks(ulong timeout);

        public abstract bool Connected { get; }

        public abstract bool hasRwAccess();

        public abstract Task Start();
    }

}