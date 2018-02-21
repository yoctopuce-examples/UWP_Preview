/*********************************************************************
 *
 * $Id: YHTTPRequest.cs 30019 2018-02-21 12:52:03Z seb $
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
 *********************************************************************/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace com.yoctopuce.YoctoAPI
{
    internal class YHTTPRequest
    {
        public const int MAX_REQUEST_MS = 5000;
        private const int YIO_IDLE_TCP_TIMEOUT = 5000;

        internal delegate void HandleIncommingData(byte[] result, int size);

        private HandleIncommingData _progressCallback;
        // ReSharper disable once NotAccessedField.Local
        private object _context;
        // ReSharper disable once NotAccessedField.Local
        private YGenericHub.RequestAsyncResult _resultCallback;

        private readonly YHTTPHub _hub;
        private StreamSocket _socket;
        private bool _reuse_socket;
        private readonly string _dbglabel;
        private readonly StringBuilder _header = new StringBuilder(1024);
        private bool _header_found;
        private readonly MemoryStream _result = new MemoryStream(1024);
        private ulong _startRequestTime;
        private ulong _requestTimeout;


        private static ulong global_reqNum;
        // ReSharper disable once NotAccessedField.Local
        private ulong _reqNum;
        private Stream _out;
        private Stream _in;
        private Task<byte[]> _currentReq;

        internal YHTTPRequest(YHTTPHub hub, string dbglabel)
        {
            _hub = hub;
            _dbglabel = dbglabel;
            _reqNum = global_reqNum++;
        }

        //private StringBuilder debugLogs = new StringBuilder(1024);

        // ReSharper disable once UnusedParameter.Local
        private void log(string msg)
        {
            //debugLogs.Append(string.Format("{0}:{1}:{2}:{3}\n", Environment.CurrentManagedThreadId, _dbglabel, _reqNum, msg));
        }


        internal async Task EnsureLastRequestDone()
        {
            if (_currentReq != null) {
                ulong now = YAPI.GetTickCount();
                ulong timeout = _startRequestTime + _requestTimeout;
                if (timeout <= now) {
                    throw new YAPI_Exception(YAPI.TIMEOUT, "Last Http request did not finished");
                }
                int msTimeout = (int) (timeout - now);
                Task task = await Task.WhenAny(_currentReq, Task.Delay(msTimeout));
                if (task != _currentReq) {
                    throw new YAPI_Exception(YAPI.TIMEOUT, "Last Http request did not finished");
                }
                _currentReq = null;
            }
        }

        private void closeSocket()
        {
            if (_socket != null) {
                _socket.Dispose();
                _socket = null;
                _in = null;
                _out = null;
            }
            _reuse_socket = false;
        }

        internal async Task<byte[]> doRequestTask(string firstLine, byte[] rest_of_request, ulong mstimeout, object context, YGenericHub.RequestAsyncResult resultCallback, HandleIncommingData progressCb)
        {
            byte[] full_request;
            _context = context;
            _resultCallback = resultCallback;
            _requestTimeout = mstimeout;
            _progressCallback = progressCb;
            log(String.Format("Start({0}):{1}", _reuse_socket ? "reuse" : "new", firstLine));
            retry:
            _startRequestTime = YAPI.GetTickCount();
            _header_found = false;
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
            _result.SetLength(0);
            _header.Length = 0;
            byte[] buffer = new byte[1024];
            Task<int> readTask;
            try {
                if (!_reuse_socket) {
                    // Creates an unconnected socket
                    _socket = new StreamSocket();
                    // Connect the socket to the remote endpoint. Catch any errors.
                    HostName serverHost = new HostName(_hub._http_params.Host);
                    int port = _hub._http_params.Port;
                    _socket.Control.NoDelay = true;
                    string port_str = port.ToString();
                    await _socket.ConnectAsync(serverHost, port_str);
                    _out = _socket.OutputStream.AsStreamForWrite();
                    _in = _socket.InputStream.AsStreamForRead();
                    log(String.Format(" - new socket ({0} / {1})", _socket.ToString(), _in.ToString()));
                }
                readTask = _in.ReadAsync(buffer, 0, buffer.Length);
                if (_reuse_socket) {
                    try {
                        Task task = await Task.WhenAny(readTask, Task.Delay(0));
                        if (task == readTask) {
                            int read = readTask.Result;
                            if (read == 0) {
                                //socket has been reseted
                                log(String.Format(" - reset socket {0} {1})", _socket.ToString(), read));
                                closeSocket();
                                goto retry;
                            }

                            string msg = "suspect data received before request. Reset the socket";
                            log(msg);
                            throw new Exception(msg);
                        }
                    } catch (Exception e) {
                        log("Reset socket connection:" + e.Message);
                        closeSocket();
                        goto retry;
                    }
                }
            } catch (Exception e) {
                log("Exception on socket connection:" + e.Message);
                closeSocket();
                throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
            }
            // write request
            try {
                await _out.WriteAsync(full_request, 0, full_request.Length);
                await _out.FlushAsync();
            } catch (Exception e) {
                closeSocket();
                throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
            }
            _reuse_socket = false;
            bool eof = false;
            do {
                int read;
                try {
                    int waitms;
                    ulong now;
                    if (_requestTimeout > 0) {
                        now = YAPI.GetTickCount();
                        ulong read_timeout = _startRequestTime + _requestTimeout;
                        if (read_timeout < now) {
                            throw new YAPI_Exception(YAPI.TIMEOUT, string.Format("Request took too long {0:D}ms", now - _startRequestTime));
                        }
                        read_timeout -= now;
                        if (read_timeout > YIO_IDLE_TCP_TIMEOUT) {
                            read_timeout = YIO_IDLE_TCP_TIMEOUT;
                        }
                        waitms = (int) read_timeout;
                    } else {
                        waitms = YIO_IDLE_TCP_TIMEOUT;
                    }
                    Task task = await Task.WhenAny(readTask, Task.Delay(waitms));
                    now = YAPI.GetTickCount();
                    if (task != readTask) {
                        string msg = string.Format("Hub did not send data during {0:D}ms", waitms);
                        throw new YAPI_Exception(YAPI.TIMEOUT, msg);
                    }
                    read = readTask.Result;
                    log(String.Format("_requestProcesss read ={0:d} after{1}", read, now - _startRequestTime));
                } catch (IOException e) {
                    closeSocket();
                    throw new YAPI_Exception(YAPI.IO_ERROR, e.Message);
                }
                if (read <= 0) {
                    // end of connection
                    closeSocket();
                    log("end of connection " + _dbglabel);
                    eof = true;
                } else if (read > 0) {
                    if (imm_HandleIncommingData(buffer, read)) {
                        closeSocket();
                        goto retry;
                    }
                    if (_reuse_socket) {
                        byte[] tmp = _result.ToArray();
                        if (tmp[tmp.Length - 1] == 10 && tmp[tmp.Length - 2] == 13) {
                            log("end of request " + _dbglabel + " let the socket open for later");
                            eof = true;
                        }
                    }
                }
                if (!eof) {
                    readTask = _in.ReadAsync(buffer, 0, buffer.Length);
                }
            } while (!eof);

            if (_header.Length==0 && _result.Length == 0) {
                //String full_log = debugLogs.ToString();
                // todo: remove debug logs
                Debug.WriteLine("Short request detected");
            }
            return _result.ToArray();
        }


        // return true if we need to retry the same request with authenctication
        private bool imm_HandleIncommingData(byte[] buffer, int read)
        {
            if (!_header_found) {
                string partial_head = YAPI.DefaultEncoding.GetString(buffer, 0, read);
                _header.Append(partial_head);
                string full_header_str = _header.ToString();
                int pos = full_header_str.IndexOf("\r\n\r\n");
                //todo: implement a indexof methode for stringbuilder in order to be more efficient
                if (pos > 0) {
                    pos += 4;
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
                            }
                            _hub.imm_parseWWWAuthenticate(_header.ToString());
                            return true;
                        }
                        if (!parts[0].Equals("200") && !parts[0].Equals("304")) {
                            throw new YAPI_Exception(YAPI.IO_ERROR, "Received HTTP status " + parts[0] + " (" + parts[1] + ")");
                        }
                    }
                    _hub.imm_authSucceded();
                    byte[] data = YAPI.DefaultEncoding.GetBytes(full_header_str.Substring(pos));
                    if (_progressCallback != null) {
                        _progressCallback(data, data.Length);
                    } else {
                        _result.Write(data, 0, data.Length);
                    }
                }
            } else {
                if (_progressCallback != null) {
                    _progressCallback(buffer, read);
                } else {
                    _result.Write(buffer, 0, read);
                }
            }
            return false;
        }


        internal async Task<byte[]> RequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout)
        {
            //log("STA_Sync:"+req_first_line.TrimEnd());
            await EnsureLastRequestDone();
            byte[] res = await doRequestTask(req_first_line, req_head_and_body, mstimeout, null, null, null);
            //log("END_Sync:" + req_first_line.TrimEnd());
            return res;
        }


        internal async Task RequestAsync(string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult callback, object context)
        {
            //log("STA_Async:" + req_first_line.TrimEnd());
            await EnsureLastRequestDone();
            _currentReq = doRequestTask(req_first_line, req_head_and_body, YHTTPHub.YIO_DEFAULT_TCP_TIMEOUT, context, callback, null);
            //log("END_Aync:" + req_first_line.TrimEnd());
        }

        internal async Task RequestProgress(string req_first_line, HandleIncommingData progressCb)
        {
            //log("STA_Progress:" + req_first_line.TrimEnd());
            await EnsureLastRequestDone();
            await doRequestTask(req_first_line, null, 0, null, null, progressCb);
            //log("END_Progress:" + req_first_line.TrimEnd());
        }
    }
}