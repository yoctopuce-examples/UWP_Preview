﻿/*********************************************************************
 *
 * $Id: YHTTPRequest.cs 25187 2016-08-12 17:18:51Z seb $
 *
 * internal yHTTPRequest object
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
 *********************************************************************/using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace com.yoctopuce.YoctoAPI
{
    internal class YHTTPRequest
    {
        public const int MAX_REQUEST_MS = 5000;
        private const int YIO_IDLE_TCP_TIMEOUT = 5000;
        private object _context;
        private YGenericHub.RequestAsyncResult _resultCallback;
        private Task<byte[]> _pendingRequest = null;
        private readonly YHTTPHub _hub;
        private StreamSocket _socket = null;
        private bool _reuse_socket = false;
        private bool _eof = true;
        private string _firstLine;
        private byte[] _rest_of_request;
        private readonly string _dbglabel;
        private readonly StringBuilder _header = new StringBuilder(1024);
        private bool? _header_found;
        private readonly MemoryStream _result = new MemoryStream(1024);
        private ulong _startRequestTime;
        private ulong _lastReceiveTime;
        private ulong _requestTimeout;


        private static ulong global_reqNum = 0;
        private ulong _reqNum;
        private Stream _out;
        private Stream _in;

        internal YHTTPRequest(YHTTPHub hub, string dbglabel)
        {
            _hub = hub;
            _dbglabel = dbglabel;
            _reqNum = global_reqNum++;
        }

        private void log(string msg)
        {
            //Debug.WriteLine(string.Format("{0}:{1}:{2}", _dbglabel, _reqNum, msg));
        }

        internal async Task WaitRequestEnd(ulong mstimeout)
        {
            ulong timeout = YAPI.GetTickCount() + mstimeout;
            while (timeout > YAPI.GetTickCount() && !_eof) {
                //todo: add timeout if this function take too long
                await _requestProcesss();
            }
            if (!_eof) {
                _hub._yctx._Log("WARNING: Last Http request did not finished");
            }
            // ensure that we close all socket
            _reuse_socket = false;
            imm_requestStop();
        }


        internal async Task _requestReserve()
        {

            if (_pendingRequest != null) {
                await _pendingRequest;
            }
            if (!_eof) {
                throw new YAPI_Exception(YAPI.TIMEOUT, "Last Http request did not finished");
            }
        }


        internal async Task _requestStart(string firstLine, byte[] rest_of_request, ulong mstimeout, object context, YGenericHub.RequestAsyncResult resultCallback)
        {
            byte[] full_request;
            _firstLine = firstLine;
            _rest_of_request = rest_of_request;
            _context = context;

            log(String.Format("Start({0}):{1}", _reuse_socket ? "reuse" : "new", firstLine));
            _startRequestTime = YAPI.GetTickCount();
            _requestTimeout = mstimeout;
            _resultCallback = resultCallback;
            string persistent_tag = firstLine.Substring(firstLine.Length - 2);
            if (persistent_tag.Equals("&.")) {
                firstLine += " \r\n";
            } else {
                firstLine += " \r\nConnection: close\r\n";
            }
            if (rest_of_request == null) {
                string str_request = firstLine + _hub.imm_getAuthorization(firstLine) + "\r\n";
                full_request = YAPI.DefaultEncoding.GetBytes(str_request);
            } else {
                string str_request = firstLine + _hub.imm_getAuthorization(firstLine);
                int len = str_request.Length;
                full_request = new byte[len + rest_of_request.Length];
                Array.Copy(YAPI.DefaultEncoding.GetBytes(str_request), 0, full_request, 0, len);
                Array.Copy(rest_of_request, 0, full_request, len, rest_of_request.Length);
            }
            bool retry;
            do {
                retry = false;
                try {
                    if (!_reuse_socket) {
                        // Creates an unconnected socket
                        _socket = new StreamSocket();
                        // Connect the socket to the remote endpoint. Catch any errors.
                        HostName serverHost = new HostName(_hub._http_params.Host);
                        int port = _hub._http_params.Port;
                        _socket.Control.NoDelay = true;
                        await _socket.ConnectAsync(serverHost, port.ToString());
                        _out = _socket.OutputStream.AsStreamForWrite();
                        _in = _socket.InputStream.AsStreamForRead();
                    }
                    _result.SetLength(0);
                    _header.Length = 0;
                    _header_found = false;
                    _eof = false;
                } catch (Exception e) {
                    log("Exception on socket connection:" + e.Message);
                    imm_requestStop();
                    throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
                }

                // write request
                try {
                    await _out.WriteAsync(full_request, 0, full_request.Length);
                    await _out.FlushAsync();
                    _lastReceiveTime = 0;
                    if (_reuse_socket) {
                        // ensure socket is still valid
                        bool canTimeout = _in.CanTimeout;
                        bool canRead = _in.CanRead;
                        //Debug.WriteLine("Reuse socket :{0} /{1}", canTimeout, canRead);
                        if (canRead) {
                            byte[] buffer = new byte[1024];
                            int read = await _in.ReadAsync(buffer, 0, buffer.Length);
                            if (read <= 0) {
                                // end of connection
                                //Debug.WriteLine("invalid Reuse socket :{0} restart", read);
                                _reuse_socket = false;
                                retry = true;
                            } else {
                                //Debug.WriteLine("Reuse socket :{0} readed", read);
                                string partial_head = YAPI.DefaultEncoding.GetString(buffer, 0, read);
                                _header.Append(partial_head);
                            }
                        }
                    }
                } catch (Exception e) {
                    log(e.ToString());
                    if (_reuse_socket) {
                        retry = true;
                    } else {
                        imm_requestStop();
                        throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
                    }
                }
                _reuse_socket = false;
            } while (retry);

        }


        internal void imm_requestStop()
        {
            log(String.Format("Stop({0})", _reuse_socket ? "reuse" : "new"));
            if (!_reuse_socket) {
                if (_socket != null) {
                    try {
                        _socket.Dispose();
                    } catch (Exception ex) {
                        log("Exception on close:" + ex.Message);

                    }
                    _socket = null;
                }
            }
        }

        internal async Task _requestReset()
        {
            imm_requestStop();
            await _requestStart(_firstLine, _rest_of_request, _requestTimeout, _context, _resultCallback);
        }


        internal async Task<int> _requestProcesss()
        {
            bool retry;
            int read = 0;

            if (_eof) {
                log("_requestProcesss early return");
                return -1;
            }

            do {
                retry = false;
                byte[] buffer = new byte[1024];
                try {
                    int waitms;
                    if (_requestTimeout > 0) {
                        ulong read_timeout = _startRequestTime + _requestTimeout - YAPI.GetTickCount();
                        if (read_timeout < 0) {
                            throw new YAPI_Exception(YAPI.TIMEOUT, string.Format("Hub did not send data during {0:D}ms", YAPI.GetTickCount() - _lastReceiveTime));
                        }
                        if (read_timeout > YIO_IDLE_TCP_TIMEOUT) {
                            read_timeout = YIO_IDLE_TCP_TIMEOUT;
                        }
                        waitms = (int)read_timeout;
                    } else {
                        waitms = YIO_IDLE_TCP_TIMEOUT;
                    }
                    //todo: find a way to quit this function before timeout when the main thread call stop notification
                    // ex: main thread is calling YHTTPHub.stopNotifications when the TCPNotification thread is calling _requestProcess
                    read = await _in.ReadAsync(buffer, 0, buffer.Length);
                    ulong now = YAPI.GetTickCount();
                    log(String.Format("_requestProcesss read ={0:d} after{1}", read, now - _startRequestTime));
                } catch (IOException e) {
                    throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
                }
                if (read <= 0) {
                    // end of connection
                    _reuse_socket = false;
                    _eof = true;
                } else if (read > 0) {
                    _lastReceiveTime = YAPI.GetTickCount();
                    if (!_header_found.Value) {
                        string partial_head = YAPI.DefaultEncoding.GetString(buffer, 0, read);
                        _header.Append(partial_head);
                        string full_header_str = _header.ToString();
                        int pos = full_header_str.IndexOf("\r\n\r\n"); //todo: implement a indexof methode for stringbuilder in order to be more efficient
                        if (pos > 0) {
                            pos += 4;
                            byte[] data = YAPI.DefaultEncoding.GetBytes(full_header_str.Substring(pos));
                            _result.Write(data, 0, data.Length);
                            _header_found = true;
                            _header.Length = pos;
                            if (full_header_str.IndexOf("0K\r\n") == 0) {
                                _reuse_socket = true;
                            } else if (full_header_str.IndexOf("OK\r\n") != 0) {
                                int lpos = full_header_str.IndexOf("\r\n");
                                if (full_header_str.IndexOf("HTTP/1.1 ") != 0) {
                                    throw new YAPI_Exception(YAPI.IO_ERROR, "Invalid HTTP response header");
                                }
                                string[] parts = full_header_str.Substring(9, lpos - 9).Split(' ');
                                if (parts[0].Equals("401")) {
                                    if (!_hub.imm_needRetryWithAuth()) {
                                        // No credential provided, give up immediately
                                        throw new YAPI_Exception(YAPI.UNAUTHORIZED, "Authentication required");
                                    } else {
                                        _hub.imm_parseWWWAuthenticate(_header.ToString());
                                        await _requestReset();
                                        break;
                                    }
                                }
                                if (!parts[0].Equals("200") && !parts[0].Equals("304")) {
                                    throw new YAPI_Exception(YAPI.IO_ERROR, "Received HTTP status " + parts[0] + " (" + parts[1] + ")");
                                }
                            }
                            _hub.imm_authSucceded();
                        }
                    } else {
                        _result.Write(buffer, 0, read);
                    }
                    if (_reuse_socket) {
                        byte[] tmp = _result.ToArray();
                        if (tmp[tmp.Length - 1] == 10 && tmp[tmp.Length - 2] == 13) {
                            _eof = true;
                        }
                    }

                }
            } while (retry);
            return read;
        }


        internal byte[] imm_getPartialResult()
        {
            byte[] res;
            if (!_header_found.Value) {
                return null;
            }
            if (_result.Length == 0) {
                if (_eof) {
                    throw new YAPI_Exception(YAPI.NO_MORE_DATA, "end of file reached");
                }
                return null;
            }
            res = _result.ToArray();
            _result.SetLength(0);
            return res;
        }


        internal async Task<byte[]> RequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout)
        {
            byte[] res;
            await _requestReserve();
            await _requestStart(req_first_line, req_head_and_body, mstimeout, null, null);
            res = await finishRequest();
            return res;
        }

        private async Task<byte[]> finishRequest()
        {
            byte[] res;
            try {
                int handled;
                do {
                    handled = await _requestProcesss();
                } while (handled > 0);
                res = _result.ToArray();
                if (res.Length == 0) {
                    log("suspicious request");
                }
                _result.SetLength(0);
            } catch (YAPI_Exception ex) {
                imm_requestStop();
                log("Sync request failed" + ex.Message);
                throw ex;
            }
            if (res.Length == 0) {
                log("suspicious request");
            }
            imm_requestStop();
            _pendingRequest = null;
            return res;
        }


        internal async Task RequestAsync(string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult callback, object context)
        {
            await _requestReserve();
            await _requestStart(req_first_line, req_head_and_body, YHTTPHub.YIO_DEFAULT_TCP_TIMEOUT, context, callback);
            _pendingRequest = finishRequest();
        }






    }

}