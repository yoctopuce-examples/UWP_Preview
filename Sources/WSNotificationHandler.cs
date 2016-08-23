using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Threading;

namespace com.yoctopuce.YoctoAPI
{


    internal class WSRequest
    {


        private readonly int _channel;
        private readonly byte[] _requestData;
        private byte[] _responseData = new byte[512];
        private int _responseLen;
        private bool _finished = false;
        private readonly bool _async;
        private readonly byte _asyncId;
        private readonly Object _progressCtx;
        private readonly YGenericHub.RequestProgress _progressCb;
        private int _errorCode = YAPI.SUCCESS;
        private string _errorMsg = null;
        private readonly ulong _tmOpen = YAPI.GetTickCount();
        private ulong _tmIn;
        private ulong _tmOut;
        private ulong _tmClose;
        private TaskCompletionSource<byte[]> _tcs = new TaskCompletionSource<byte[]>();
        private string _debug;


        internal int ReqPosition { get; set; }
        internal int ReqSize { get { return _requestData.Length; } }
        internal byte AsyncId { get { return _asyncId; } }
        internal int Channel { get { return _channel; } }
        internal int RespPosition { get; set; }
        internal int RespSize { get { return _responseData.Length; } }
        internal int ErrorCode {
            get { return _errorCode; }
        }

        internal string ErrorMsg {
            get { return _errorMsg; }
        }

        internal WSRequest(string debug, int tcpchanel, byte asyncid, byte[] full_request)
        {
            _async = true;
            _asyncId = asyncid;
            _channel = tcpchanel;
            _requestData = full_request;
            _debug = debug;
            _progressCb = null;
            _progressCtx = null;

        }

        internal WSRequest(string debug, int tcpchanel, byte[] full_request, YGenericHub.RequestProgress progress, Object context)
        {
            _async = false;
            _asyncId = 0;
            _channel = tcpchanel;
            _requestData = full_request;
            _debug = debug;
            _progressCb = progress;
            _progressCtx = context;
        }

        internal IBuffer imm_GetRequestData(int ofs, int length)
        {
            return _requestData.AsBuffer(ofs, length);
        }


        internal void imm_close(int res, string reasonPhrase)
        {
            _errorCode = res;
            _errorMsg = reasonPhrase;
            _finished = true;
            _tmClose = YAPI.GetTickCount();
            //imm_logReq("close:" + reasonPhrase);
            _tcs.SetResult(_responseData);
        }

        internal async Task<byte[]> getResponseBytes()
        {
            return await _tcs.Task;
        }

        internal void imm_logReq(String msg)
        {
            if (_finished) {
                ulong write = _tmOut - _tmOpen;
                ulong read = _tmIn - _tmOut;
                ulong end = _tmClose - _tmOpen;
                Debug.WriteLine($"{_debug}:{msg} in {write:d}+{read:d}={end:d} ms");
            } else {
                ulong opened = YAPI.GetTickCount() - _tmOpen;
                Debug.WriteLine($"{_debug}:{msg} opened since {opened:d} ms");
            }
        }

        internal bool imm_isAsync()
        {
            return _async;
        }

        internal void imm_reportDataSent()
        {
            _tmOut = YAPI.GetTickCount();
        }

        internal void imm_AppendResponseData(byte[] data)
        {
            int length = _responseData.Length;
            if (_responseLen + data.Length > length) {
                byte[] tmp = new byte[length * 2];
                Array.Copy(_responseData, 0, tmp, 0, _responseLen);
                Array.Copy(data, 0, tmp, _responseLen, data.Length);
                _responseLen += data.Length;
                _responseData = tmp;
            } else {
                Array.Copy(data, 0, _responseData, _responseLen, data.Length);
                _responseLen += data.Length;
            }
            _tmIn = YAPI.GetTickCount();
        }


    }



    internal class WSNotificationHandler : NotificationHandler
    {
        private enum ConnectionState
        {
            DEAD, DISCONNECTED, CONNECTING, AUTHENTICATING, CONNECTED
        }

        private const int NB_TCP_CHANNEL = 4;
        private const int HUB_TCP_CHANNEL = 0;
        private const int DEVICE_TCP_CHANNEL = 0;
        private const int WS_REQUEST_MAX_DURATION = 50000;
        private const int MAX_DATA_LEN = 124;


        // default transport layer parameters
        private const int DEFAULT_TCP_ROUND_TRIP_TIME = 30;
        private const int DEFAULT_TCP_MAX_WINDOW_SIZE = 4 * 65536;
        private readonly HashAlgorithmProvider _md5Alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
        private readonly HashAlgorithmProvider _sha1Alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);



        private String _notificationsFifo = "";
        private bool _connected;
        private readonly Queue<WSRequest>[] _workingRequests;

        private bool _firstNotif;
        private ulong _connectionTime;
        private ConnectionState _connectionState = ConnectionState.CONNECTING;
        private int _remoteVersion;
        private string _remoteSerial;
        private long _remoteNouce;
        private UInt32 _nounce;
        private int _session_errno;
        private string _session_error;
        private bool _rwAccess;
        private ulong _tcpRoundTripTime = DEFAULT_TCP_ROUND_TRIP_TIME;
        private int _tcpMaxWindowSize = DEFAULT_TCP_MAX_WINDOW_SIZE;
        private readonly ulong[] _lastUploadAckTime = new ulong[NB_TCP_CHANNEL];
        private readonly long[] _lastUploadAckBytes = new long[NB_TCP_CHANNEL];
        private readonly int[] _lastUploadRateBytes = new int[NB_TCP_CHANNEL];
        private readonly ulong[] _lastUploadRateTime = new ulong[NB_TCP_CHANNEL];
        private int _uploadRate = 0;
        private byte _nextAsyncId = 48;
        private MessageWebSocket _webSock;

        private static readonly byte[] rnrn = new byte[] { 13, 10, 13, 10 };
        private ThreadPoolTimer _reconnectionTimer;
        private bool _mustStop;


        public WSNotificationHandler(YHTTPHub hub) : base(hub)
        {
            _workingRequests = new Queue<WSRequest>[NB_TCP_CHANNEL];
            for (int i = 0; i < NB_TCP_CHANNEL; i++) {
                _workingRequests[i] = new Queue<WSRequest>();
            }
        }

        public override async Task Start()
        {
            await ConnectToServer();
        }

        private async Task ConnectToServer()
        {
            _webSock = new MessageWebSocket();
            _webSock.Control.MessageType = SocketMessageType.Binary;
            _webSock.MessageReceived += WebSock_MessageReceived;
            _webSock.Closed += WebSock_Closed;
            string url = "ws://" + _hub._http_params.imm_getUrl(false, false) + "/not.byn";
            Uri serverUri = new Uri(url);

            try {
                //Connect to the server.
                await _webSock.ConnectAsync(serverUri);
                WSLOG("WS:Connected to " + url);
                _connected = true;
                _notifRetryCount = 0;
                _reconnectionTimer = null;
            } catch (Exception ex) {
                //Add code here to handle any exceptions
                throw new YAPI_Exception(YAPI.IO_ERROR, ex.Message);
            }
        }

        //The Closed event handler
        private async void WebSock_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            WSLOG("WS: disconnenction detected");
            _connected = false;
            foreach (Queue<WSRequest> queue in _workingRequests) {
                foreach (WSRequest wsRequest in queue) {
                    wsRequest.imm_close(YAPI.IO_ERROR, "Websocket connection lost with " + _hub.Host);
                }
                queue.Clear();
            }
            if (!_mustStop) {
                _hub._devListValidity = 500;
                int error_delay = 100 << (_notifRetryCount > 4 ? 4 : _notifRetryCount);
                _notifRetryCount++;
                TimeSpan delay = TimeSpan.FromMilliseconds(error_delay);
                _reconnectionTimer = ThreadPoolTimer.CreateTimer(
                    expirationHandler, delay);
            }
            WSLOG("WS: disconnenction detected done");
        }

        private async void expirationHandler(ThreadPoolTimer timer)
        {
            WSLOG("WS: recconnection");
            await ConnectToServer();
        }


        private async Task<WSRequest> sendRequest(string req_first_line, byte[] req_head_and_body, int tcpchanel, bool async, YGenericHub.RequestProgress progress, Object context)
        {
            WSRequest request;
            string debug = req_first_line.Trim();
            byte[] full_request;
            byte[] req_first_lineBytes;
            if (req_head_and_body == null) {
                req_first_line += "\r\n\r\n";
                req_first_lineBytes = YAPI.DefaultEncoding.GetBytes(req_first_line);
                full_request = req_first_lineBytes;
            } else {
                req_first_line += "\r\n";
                req_first_lineBytes = YAPI.DefaultEncoding.GetBytes(req_first_line);
                full_request = new byte[req_first_lineBytes.Length + req_head_and_body.Length];
                Array.Copy(req_first_lineBytes, 0, full_request, 0, req_first_lineBytes.Length);
                Array.Copy(req_head_and_body, 0, full_request, req_first_lineBytes.Length, req_head_and_body.Length);
            }

            ulong timeout = YAPI.GetTickCount() + WS_REQUEST_MAX_DURATION;
            while ((_connectionState != ConnectionState.CONNECTED && _connectionState != ConnectionState.DEAD)) {
                if (timeout < YAPI.GetTickCount()) {
                    if (_connectionState != ConnectionState.CONNECTED && _connectionState != ConnectionState.CONNECTING) {
                        throw new YAPI_Exception(YAPI.IO_ERROR, "IO error with hub");
                    } else {
                        throw new YAPI_Exception(YAPI.TIMEOUT, "Last request did not finished correctly");
                    }
                }
                if (_connectionState == ConnectionState.DEAD) {
                    throw new YAPI_Exception(_session_errno, _session_error);
                }

            }
            if (async) {
                request = new WSRequest(debug, tcpchanel, _nextAsyncId++, full_request);
                if (_nextAsyncId >= 127) {
                    _nextAsyncId = 48;
                }
            } else {
                request = new WSRequest(debug, tcpchanel, full_request, progress, context);
            }

            _workingRequests[tcpchanel].Enqueue(request);

            //todo: handle timeout
            await processRequests(request);
            if (request.ErrorCode != YAPI.SUCCESS) {
                throw new YAPI_Exception(request.ErrorCode, request.ErrorMsg);
            }
            return request;
        }



        /*
    *   look through all pending request if there is some data that we can send
    *
    */
        private async Task processRequests(WSRequest request)
        {
            int tcpchan = request.Channel;

            int throttle_start = request.ReqPosition;
            int throttle_end = request.ReqSize;
            if (throttle_end > 2108 && _remoteVersion >= YGenericHub.USB_META_WS_PROTO_V2 && tcpchan == 0) {
                // Perform throttling on large uploads
                if (request.ReqPosition == 0) {
                    // First chunk is always first multiple of full window (124 bytes) above 2KB
                    throttle_end = 2108;
                    // Prepare to compute effective transfer rate
                    _lastUploadAckBytes[tcpchan] = 0;
                    _lastUploadAckTime[tcpchan] = 0;
                    // Start with initial RTT based estimate
                    _uploadRate = (int)(_tcpMaxWindowSize * 1000 / (int)_tcpRoundTripTime);
                } else if (_lastUploadAckTime[tcpchan] == 0) {
                    // first block not yet acked, wait more
                    throttle_end = 0;
                } else {
                    // adapt window frame to available bandwidth
                    long bytesOnTheAir = request.ReqPosition - _lastUploadAckBytes[tcpchan];
                    ulong timeOnTheAir = YAPI.GetTickCount() - _lastUploadAckTime[tcpchan];
                    int uploadRate = _uploadRate;
                    int toBeSent = (int)(2 * uploadRate + 1024 - bytesOnTheAir + (uploadRate * (int)timeOnTheAir / 1000));
                    if (toBeSent + bytesOnTheAir > DEFAULT_TCP_MAX_WINDOW_SIZE) {
                        toBeSent = (int)(DEFAULT_TCP_MAX_WINDOW_SIZE - bytesOnTheAir);
                    }
                    WSLOG(string.Format("throttling: {0} bytes/s ({1} + {2} = {3})", _uploadRate, toBeSent, bytesOnTheAir, bytesOnTheAir + toBeSent));
                    if (toBeSent < 64) {
                        ulong waitTime = (ulong)(1000 * (128 - toBeSent) / _uploadRate);
                        if (waitTime < 2)
                            waitTime = 2;
                        //_next_transmit_tm = YAPI.GetTickCount() + waitTime;
                        WSLOG(string.Format("WS: {0} sent {1}ms ago, waiting {2}ms...", bytesOnTheAir, timeOnTheAir, waitTime));
                        throttle_end = 0;
                    }
                    if (throttle_end > request.ReqPosition + toBeSent) {
                        // when sending partial content, round up to full frames
                        if (toBeSent > 124) {
                            toBeSent = (toBeSent / 124) * 124;
                        }
                        throttle_end = request.ReqPosition + toBeSent;
                    }
                }
            }
            while (request.ReqPosition < throttle_end) {
                IBuffer data;
                int datalen = throttle_end - request.ReqPosition;
                if (datalen > MAX_DATA_LEN) {
                    datalen = MAX_DATA_LEN;
                }
                if (request.imm_isAsync() && (request.ReqPosition + datalen == request.ReqSize)) {
                    if (datalen == MAX_DATA_LEN) {
                        // last frame is already full we must send the async close in another one
                        data = request.imm_GetRequestData(request.ReqPosition, datalen);
                        await Send_WSStream(_webSock, YGenericHub.YSTREAM_TCP, tcpchan, data, 0);
                        //WSLOG(string.Format("ws_req:{0}: sent {1} bytes on chan{2} ({3}/{4})", request, datalen, tcpchan, request.ReqPosition, request.ReqSize));
                        request.imm_reportDataSent();
                        request.ReqPosition += datalen;
                        datalen = 0;
                    }
                    data = request.imm_GetRequestData(request.ReqPosition, datalen);
                    await Send_WSStream(_webSock, YGenericHub.YSTREAM_TCP_ASYNCCLOSE, tcpchan, data, request.AsyncId);
                    //WSLOG(string.Format("ws_req:{0}: sent async close {1}", request, request.AsyncId));
                    request.ReqPosition += datalen;
                } else {
                    data = request.imm_GetRequestData(request.ReqPosition, datalen);
                    await Send_WSStream(_webSock, YGenericHub.YSTREAM_TCP, tcpchan, data, 0);
                    request.imm_reportDataSent();
                    //WSLOG(string.Format("ws_req:{0}: sent {1} bytes on chan{2} ({3}/{4})", request, datalen, tcpchan, request.ReqPosition, request.ReqSize));
                    request.ReqPosition += datalen;
                }
            }
            if (request.ReqPosition < request.ReqSize) {
                int sent = request.ReqPosition - throttle_start;
                // not completely sent, cannot do more for now
                if (sent > 0 && _uploadRate > 0) {
                    ulong waitTime = (ulong)(1000 * sent / _uploadRate);
                    if (waitTime < 2)
                        waitTime = 2;
                    //_next_transmit_tm = YAPI.GetTickCount() + waitTime;
                    WSLOG(string.Format("Sent {0}bytes, waiting {1}ms...", sent, waitTime));
                } else {
                    //_next_transmit_tm = YAPI.GetTickCount() + 100;
                }
            }
        }



        private static byte[] imm_verifyHTTPheader(byte[] full_result)
        {
            int hpos = YAPIContext.imm_find_in_bytes(full_result, rnrn);
            int ofs = 0;
            int size = full_result.Length;
            if (hpos >= 0) {
                ofs += hpos + 4;
                size -= hpos + 4;
            }
            byte[] res = new byte[size];
            Array.Copy(full_result, ofs, res, 0, size);
            return res;
        }




        internal override async Task<byte[]> hubRequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout)
        {
            WSRequest wsRequest = await sendRequest(req_first_line, req_head_and_body, HUB_TCP_CHANNEL, false, null, null);
            byte[] full_result = await wsRequest.getResponseBytes();
            byte[] immVerifyHttPheader = imm_verifyHTTPheader(full_result);
            return immVerifyHttPheader;
        }



        internal override async Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, uint mstimeout, YGenericHub.RequestProgress progress, object context)
        {
            if (mstimeout == 0) {
                // simulate a wait indefinitely
                mstimeout = 86400000; //24h
            }
            WSRequest wsRequest = await sendRequest(req_first_line, req_head_and_body, DEVICE_TCP_CHANNEL, false, progress, context);
            byte[] full_result = await wsRequest.getResponseBytes();
            return imm_verifyHTTPheader(full_result);
        }


        internal override async Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult asyncResult,
        object asyncContext)
        {
            WSRequest wsRequest = await sendRequest(req_first_line, req_head_and_body, DEVICE_TCP_CHANNEL, true, null, null);

        }

        internal override async Task<bool> Stop(ulong timeout)
        {
            WSLOG("Stop");
            _mustStop = true;
            if (_reconnectionTimer != null) {
                _reconnectionTimer.Cancel();
            }
            foreach (Queue<WSRequest> queue in _workingRequests) {
                while (queue.Count > 0) {
                    WSRequest wsRequest = queue.Peek();
                    await wsRequest.getResponseBytes();
                    queue.Dequeue();
                }
            }
            _webSock.Close(1000, "");
            //_outDataWriter = null;
            WSLOG("Stop end");
            return false;
        }


        public override bool Connected {
            get {
                return _connected;
            }
        }
        public override bool hasRwAccess()
        {
            return _rwAccess;
        }


        private async Task sendAuthenticationMeta(MessageWebSocket webSock)
        {
            byte header = (byte)(((YGenericHub.YSTREAM_META << 3) + 0) & 0xff);

            DataWriter dataWriter = new DataWriter(webSock.OutputStream);
            dataWriter.WriteByte(header);
            dataWriter.WriteByte(YGenericHub.USB_META_WS_AUTHENTICATION);
            dataWriter.WriteByte(YGenericHub.USB_META_WS_PROTO_V2);
            if (_hub._http_params.imm_hasAuthParam()) {
                dataWriter.WriteUInt16(YGenericHub.USB_META_WS_VALID_SHA1);
                dataWriter.WriteUInt32(_nounce);
                byte[] sha1 = imm_computeAUTH(_hub._http_params.User, _hub._http_params.Pass, _remoteSerial, _remoteNouce);
                dataWriter.WriteBytes(sha1);
            } else {
                dataWriter.WriteUInt16(0);
                dataWriter.WriteUInt32(0);
                for (int i = 0; i < 5; i++) {
                    dataWriter.WriteUInt32(0);
                }
            }
            await dataWriter.StoreAsync();
            dataWriter.DetachStream();
        }


        private byte[] imm_computeAUTH(String user, String pass, String serial, long noune)
        {
            String ha1_str = user + ":" + serial + ":" + pass;
            byte[] bytes_to_hash = YAPI.DefaultEncoding.GetBytes(ha1_str);
            IBuffer md5digest = _md5Alg.HashData(bytes_to_hash.AsBuffer());
            byte[] digest = md5digest.ToArray();
            String ha1 = YAPIContext.imm_bytesToHexStr(digest, 0, digest.Length).ToLower();
            String sha1_raw = ha1 + string.Format("{0:x2}{1:x2}{2:x2}{3:x2}",
                    noune & 0xff, (noune >> 8) & 0xff, (noune >> 16) & 0xff, (noune >> 24) & 0xff);
            byte[] bytes_to_sha1 = YAPI.DefaultEncoding.GetBytes(ha1_str);
            IBuffer sha1digest = _sha1Alg.HashData(bytes_to_sha1.AsBuffer());
            return sha1digest.ToArray();
        }


        //todo: use custom StringBuilder for better perfomance

        private async Task decodeTCPNotif(String tcpNofif)
        {
            _notificationsFifo += tcpNofif;
            int pos;
            do {
                pos = _notificationsFifo.IndexOf("\n");
                if (pos < 0)
                    break;
                // discard ping notification (pos==0)
                if (pos > 0) {
                    String line = _notificationsFifo.Substring(0, pos + 1);
                    if (line.IndexOf((char)27) == -1) {
                        // drop notification that contain esc char
                        await handleNetNotification(line);
                    }
                }
                _notificationsFifo = _notificationsFifo.Substring(pos + 1);
            }

            while (pos >= 0);
        }

        //The MessageReceived event handler.
        private async void WebSock_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            DataReader messageReader = args.GetDataReader();
            messageReader.ByteOrder = ByteOrder.LittleEndian;
            WSRequest workingRequest;
            uint messageSize = messageReader.UnconsumedBufferLength;
            byte first_byte = messageReader.ReadByte();
            int tcpChanel = first_byte & 0x7;
            int ystream = (first_byte & 0xff) >> 3;

            switch (ystream) {
                case YGenericHub.YSTREAM_TCP_NOTIF:
                    if (_firstNotif) {
                        if (!_hub._http_params.imm_hasAuthParam()) {
                            _connectionState = ConnectionState.CONNECTED;
                            _firstNotif = false;
                        } else {
                            return;
                        }
                    }
                    byte[] chars = new byte[messageReader.UnconsumedBufferLength];
                    messageReader.ReadBytes(chars);
                    String tcpNotif = YAPI.DefaultEncoding.GetString(chars);
                    await decodeTCPNotif(tcpNotif);
                    break;
                case YGenericHub.YSTREAM_EMPTY:
                    return;
                case YGenericHub.YSTREAM_TCP_ASYNCCLOSE:
                    workingRequest = _workingRequests[tcpChanel].Peek();
                    if (workingRequest != null && messageReader.UnconsumedBufferLength >= 1) {
                        uint contentSize = messageReader.UnconsumedBufferLength - 1;
                        //todo: try to copy data more efficently
                        byte[] data = new byte[contentSize];
                        if (contentSize > 0) {
                            messageReader.ReadBytes(data);
                        }
                        int asyncId = messageReader.ReadByte();
                        if (workingRequest.AsyncId != asyncId) {
                            _hub._yctx._Log("WS: Incorrect async-close signature on tcpChan " + tcpChanel + "\n");
                            return;
                        }
                        workingRequest.imm_AppendResponseData(data);
                        workingRequest.imm_close(YAPI.SUCCESS, "");
                        _workingRequests[tcpChanel].Dequeue();
                    }
                    break;
                case YGenericHub.YSTREAM_TCP:
                case YGenericHub.YSTREAM_TCP_CLOSE:
                    workingRequest = _workingRequests[tcpChanel].Peek();
                    if (workingRequest != null) {
                        uint contentSize = messageReader.UnconsumedBufferLength;
                        //todo: try to copy data more efficently
                        byte[] data = new byte[contentSize];
                        if (contentSize > 0) {
                            messageReader.ReadBytes(data);
                        }
                        workingRequest.imm_AppendResponseData(data);
                        if (ystream == YGenericHub.YSTREAM_TCP_CLOSE) {
                            await Send_WSStream(sender, YGenericHub.YSTREAM_TCP_CLOSE, tcpChanel, null, 0);
                            _workingRequests[tcpChanel].Dequeue();
                            workingRequest.imm_close(YAPI.SUCCESS, "");
                        }
                    }
                    break;
                case YGenericHub.YSTREAM_META:
                    int metatype = messageReader.ReadByte();
                    long nounce;
                    int version;
                    switch (metatype) {
                        case YGenericHub.USB_META_WS_ANNOUNCE:
                            version = messageReader.ReadByte();
                            if (version < YGenericHub.USB_META_WS_PROTO_V1 || messageSize < YGenericHub.USB_META_WS_ANNOUNCE_SIZE) {
                                return;
                            }
                            _remoteVersion = version;
                            int maxtcpws = messageReader.ReadUInt16(); // ignore reserved word
                            if (maxtcpws > 0) {
                                _tcpMaxWindowSize = maxtcpws;
                            }
                            nounce = messageReader.ReadUInt32();
                            byte[] serial_char = new byte[messageReader.UnconsumedBufferLength];
                            messageReader.ReadBytes(serial_char);
                            int len;
                            for (len = YAPI.YOCTO_BASE_SERIAL_LEN; len < serial_char.Length; len++) {
                                if (serial_char[len] == 0) {
                                    break;
                                }
                            }
                            _remoteSerial = YAPI.DefaultEncoding.GetString(serial_char, 0, len);
                            _remoteNouce = nounce;
                            _connectionTime = YAPI.GetTickCount();
                            Random randomGenerator = new Random();
                            _nounce = (uint)randomGenerator.Next();
                            _connectionState = ConnectionState.AUTHENTICATING;
                            await sendAuthenticationMeta(_webSock);
                            break;
                        case YGenericHub.USB_META_WS_AUTHENTICATION:
                            if (_connectionState != ConnectionState.AUTHENTICATING)
                                return;
                            version = messageReader.ReadByte();
                            if (version < YGenericHub.USB_META_WS_PROTO_V1 || messageSize < YGenericHub.USB_META_WS_AUTHENTICATION_SIZE) {
                                return;
                            }
                            _tcpRoundTripTime = YAPI.GetTickCount() - _connectionTime + 1;
                            long uploadRate = _tcpMaxWindowSize * 1000 / (int)_tcpRoundTripTime;
                            _hub._yctx._Log(string.Format("WS:RTT={0}ms, WS={1}, uploadRate={2} KB/s\n", _tcpRoundTripTime, _tcpMaxWindowSize, uploadRate / 1000.0));
                            int flags = messageReader.ReadInt16();
                            messageReader.ReadUInt32(); // drop nounce
                            if ((flags & YGenericHub.USB_META_WS_AUTH_FLAGS_RW) != 0)
                                _rwAccess = true;
                            if ((flags & YGenericHub.USB_META_WS_VALID_SHA1) != 0) {
                                byte[] remote_sha1 = new byte[20];
                                messageReader.ReadBytes(remote_sha1);
                                byte[] sha1 = imm_computeAUTH(_hub._http_params.User, _hub._http_params.Pass, _remoteSerial, _nounce);
                                if (remote_sha1.SequenceEqual(sha1)) {
                                    _connectionState = ConnectionState.CONNECTED;
                                } else {
                                    errorOnSession(YAPI.UNAUTHORIZED, string.Format("Authentication as {0} failed", _hub._http_params.User));
                                }
                            } else {
                                if (!_hub._http_params.imm_hasAuthParam()) {
                                    _connectionState = ConnectionState.CONNECTED;
                                } else {
                                    if (_hub._http_params.User == "admin" && !_rwAccess) {
                                        errorOnSession(YAPI.UNAUTHORIZED, string.Format("Authentication as {0} failed", _hub._http_params.User));
                                    } else {
                                        errorOnSession(YAPI.UNAUTHORIZED, string.Format("Authentication error : hub has no password for {0}", _hub._http_params.User));
                                    }
                                }
                            }
                            break;
                        case YGenericHub.USB_META_WS_ERROR:
                            // drop reserved byte
                            messageReader.ReadByte();
                            int html_error = messageReader.ReadUInt16();
                            if (html_error == 401) {
                                errorOnSession(YAPI.UNAUTHORIZED, "Authentication failed");
                            } else {
                                errorOnSession(YAPI.IO_ERROR, string.Format("Remote hub closed connection with error %d", html_error));
                            }
                            break;
                        case YGenericHub.USB_META_ACK_UPLOAD:
                            int tcpchan = messageReader.ReadByte();
                            workingRequest = _workingRequests[tcpchan].Peek();
                            if (workingRequest != null) {
                                int b0 = messageReader.ReadByte();
                                int b1 = messageReader.ReadByte();
                                int b2 = messageReader.ReadByte();
                                int b3 = messageReader.ReadByte();
                                int ackBytes = b0 + (b1 << 8) + (b2 << 16) + (b3 << 24);
                                ulong ackTime = YAPI.GetTickCount();
                                if (_lastUploadAckTime[tcpchan] != 0 && ackBytes > _lastUploadAckBytes[tcpchan]) {
                                    _lastUploadAckBytes[tcpchan] = ackBytes;
                                    _lastUploadAckTime[tcpchan] = ackTime;

                                    int deltaBytes = ackBytes - _lastUploadRateBytes[tcpchan];
                                    ulong deltaTime = ackTime - _lastUploadRateTime[tcpchan];
                                    if (deltaTime < 500)
                                        break; // wait more
                                    if (deltaTime < 1000 && deltaBytes < 65536)
                                        break; // wait more
                                    _lastUploadRateBytes[tcpchan] = ackBytes;
                                    _lastUploadRateTime[tcpchan] = ackTime;
                                    //fixme: workingRequest.reportProgress(ackBytes);
                                    double newRate = deltaBytes * 1000.0 / deltaTime;
                                    _uploadRate = (int)(0.8 * _uploadRate + 0.3 * newRate);// +10% intentionally
                                    _hub._yctx._Log(string.Format("Upload rate: {0:F2} KB/s (based on {1:F2} KB in {2:F}s)\n", newRate / 1000.0, deltaBytes / 1000.0, deltaTime / 1000.0));
                                } else {
                                    _hub._yctx._Log("First Ack received\n");
                                    _lastUploadAckBytes[tcpchan] = ackBytes;
                                    _lastUploadAckTime[tcpchan] = ackTime;
                                    _lastUploadRateBytes[tcpchan] = ackBytes;
                                    _lastUploadRateTime[tcpchan] = ackTime;
                                    //fixme: workingRequest.reportProgress(ackBytes);
                                }
                            }
                            break;
                        default:
                            WSLOG(string.Format("unhandled Meta pkt {0}", ystream));
                            break;
                    }
                    break;
                default:
                    _hub._yctx._Log(string.Format("Invalid WS stream type ({0})\n", ystream));
                    break;
            }
        }

        private void errorOnSession(int errno, string err_msg)
        {
            if (_connectionState == ConnectionState.DEAD) {
                // already dead
                return;
            }
            _connectionState = ConnectionState.DEAD;
            if (errno != YAPI.SUCCESS) {
                _session_errno = errno;
                _session_error = err_msg;
            }
        }


        private void WSLOG(string msg)
        {
            int thid = Environment.CurrentManagedThreadId;
            Debug.WriteLine("[" + thid + "]:" + msg);
        }




        private async Task Send_WSStream(MessageWebSocket webSock, int ystreamTcp, int tcpchan, IBuffer data, byte asyncId)
        {

            //WSLOG(string.Format("WS: Send stream type:{0}  chan:{1} len:{2} asyncid:{3}", ystreamTcp, tcpchan, data != null ? data.Length : 0, asyncId));
            byte header = (byte)(((ystreamTcp << 3) + tcpchan) & 0xff);
            DataWriter outDataWriter = new DataWriter(webSock.OutputStream);
            outDataWriter.WriteByte(header);
            if (data != null) {
                outDataWriter.WriteBuffer(data);
            }
            if (ystreamTcp == YGenericHub.YSTREAM_TCP_ASYNCCLOSE) {
                outDataWriter.WriteByte(asyncId);
            }
            await outDataWriter.StoreAsync();
            outDataWriter.DetachStream();
        }

    }

}