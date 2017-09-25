using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    internal class TCPNotificationHandler : NotificationHandler
    {
        protected internal volatile bool _sendPingNotification = false;
        protected internal volatile bool _connected = false;
        private Dictionary<YDevice, YHTTPRequest> _httpReqByDev = new Dictionary<YDevice, YHTTPRequest>();
        private bool _mustRun =false;
        private Task _runTask = null;

        internal TCPNotificationHandler(YHTTPHub hub) : base(hub)
        {
        }


        public override Task Start()
        {
            _mustRun = true;
            _runTask = Run();
            return Task.FromResult(0);
        }

        private async Task Run()
        {
            YHTTPRequest yreq = new YHTTPRequest((YHTTPHub)_hub, "Notification of " + _hub.RootUrl);
            try {
                String notUrl;
                if (_notifyPos < 0) {
                    notUrl = "GET /not.byn";
                } else {
                    notUrl = string.Format("GET /not.byn?abs=%d", _notifyPos);
                }
                await yreq._requestStart(notUrl, null, 0, null, null);
                _connected = true;
                String fifo = "";
                do {
                    byte[] partial;
                    bool isdone = await yreq._requestProcesss();
                    if (isdone) {
                        //disconnected
                        _connected = false;
                        break;
                    }
                    partial = yreq.imm_getPartialResult();
                    if (partial != null) {
                        //todo: replace string by something more efficient
                        fifo += YAPI.DefaultEncoding.GetString(partial);
                    }
                    int pos;
                    do {
                        pos = fifo.IndexOf("\n");
                        if (pos < 0)
                            break;
                        if (pos == 0 && !_sendPingNotification) {
                            _sendPingNotification = true;
                        } else {
                            string line = fifo.Substring(0, pos + 1);
                            if (line.IndexOf((char)27) == -1) {
                                // drop notification that contain esc char
                                await handleNetNotification(line);
                            }
                        }
                        fifo = fifo.Substring(pos + 1);
                    } while (pos >= 0);
                    _error_delay = 0;
                } while (_mustRun);
                yreq.imm_requestStop();
            } catch (YAPI_Exception) {
                _connected = false;
                yreq.imm_requestStop();
                _notifRetryCount++;
                _hub._devListValidity = 500;
                _error_delay = 100 << (_notifRetryCount > 4 ? 4 : _notifRetryCount);
            }
            yreq.imm_requestStop();
        }


        internal override async Task<byte[]> hubRequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout)
        {
            YHTTPRequest req = new YHTTPRequest(_hub, "request to " + _hub.Host);
            return await req.RequestSync(req_first_line, req_head_and_body, mstimeout);
        }

        internal override async Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, uint mstimeout, YGenericHub.RequestProgress progress, object context)
        {
            ulong start = YAPI.GetTickCount();
            if (!_httpReqByDev.ContainsKey(device)) {
                _httpReqByDev[device] = new YHTTPRequest(_hub, "Device " + device.SerialNumber);
            }
            YHTTPRequest req = _httpReqByDev[device];
            byte[] result = await req.RequestSync(req_first_line, req_head_and_body, mstimeout);
            ulong stop = YAPI.GetTickCount();
            Debug.WriteLine(string.Format("SyncRes on {0} took {1}ms",device.SerialNumber,stop-start));

            return result;
        }

        internal override async Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult asyncResult, object asyncContext)
        {
            ulong start = YAPI.GetTickCount();
            if (!_httpReqByDev.ContainsKey(device)) {
                _httpReqByDev[device] = new YHTTPRequest(_hub, "Device " + device.SerialNumber);
            }
            YHTTPRequest req = _httpReqByDev[device];
            await req.RequestAsync(req_first_line, req_head_and_body, asyncResult, asyncContext);
            ulong stop = YAPI.GetTickCount();
            Debug.WriteLine(string.Format("ASyncRes on {0} took {1}ms", device.SerialNumber, stop - start));
        }

        internal override async Task<bool> Stop(ulong timeout)
        {
            _mustRun = false;
            foreach (YHTTPRequest req in _httpReqByDev.Values) {
                req.imm_EnsureIsEnded();
            }
            if (_runTask != null) {
                await _runTask;
                _runTask = null;
            }
            return false;
        }


        public override bool Connected {
            get {
                return !_sendPingNotification || _connected;
            }
        }

        public override bool hasRwAccess()
        {
            return _hub._http_params.User.Equals("admin");
        }
    }

}