using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{
    internal class TCPNotificationHandler : NotificationHandler
    {
        protected internal volatile bool _sendPingNotification = false;
        protected internal volatile bool _connected = false;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Dictionary<YDevice, YHTTPRequest> _httpReqByDev = new Dictionary<YDevice, YHTTPRequest>();
        private Task _runTask = null;
        private string _fifo;
        private CancellationToken _token;

        internal TCPNotificationHandler(YHTTPHub hub) : base(hub)
        { }


        public override Task Start()
        {
            _token = _tokenSource.Token;
            _runTask = Task.Run(Run, _token);
            return Task.FromResult(0);
        }

        internal override async Task<bool> Stop(ulong timeout)
        {
            foreach (YHTTPRequest req in _httpReqByDev.Values) {
                await req.EnsureLastRequestDone();
            }
            _tokenSource.Cancel();
            if (_runTask != null) {
                try {
                    Task task = await Task.WhenAny(_runTask, Task.Delay(10000));
                    if (task != _runTask) {
                        throw new YAPI_Exception(YAPI.IO_ERROR, "Unable to stop notification thread/task after 10s");
                    }
                } catch (AggregateException e) {
                    foreach (var v in e.InnerExceptions)
                        Debug.WriteLine(e.Message + " " + v.Message);
                } finally {
                    _tokenSource.Dispose();
                }
                _tokenSource = null;
                _runTask = null;
            }


            return false;
        }


        private async Task Run()
        {
            _token.ThrowIfCancellationRequested();
            YHTTPRequest yreq = new YHTTPRequest((YHTTPHub) _hub, "Notification of " + _hub.RootUrl);
            try {
                String notUrl;
                if (_notifyPos < 0) {
                    notUrl = "GET /not.byn";
                } else {
                    notUrl = string.Format("GET /not.byn?abs=%d", _notifyPos);
                }
                _fifo = "";
                _connected = true;
                await yreq.RequestProgress(notUrl, ProgressCb);
            } catch (YAPI_Exception ex) {
                Debug.WriteLine(ex.Message);
                _notifRetryCount++;
                _hub._devListValidity = 500;
                _error_delay = 100 << (_notifRetryCount > 4 ? 4 : _notifRetryCount);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            _connected = false;
            //yreq.imm_requestStop();
        }

        private void ProgressCb(byte[] partial, int size)
        {
            _token.ThrowIfCancellationRequested();
            if (partial != null) {
                //todo: replace string by something more efficient
                string s = YAPI.DefaultEncoding.GetString(partial,0,size);
                _fifo += s;
            }
            int pos;
            do {
                pos = _fifo.IndexOf("\n");
                if (pos < 0)
                    break;
                if (pos == 0 && !_sendPingNotification) {
                    _sendPingNotification = true;
                } else {
                    string line = _fifo.Substring(0, pos + 1);
                    if (line.IndexOf((char) 27) == -1) {
                        // drop notification that contain esc char
                        handleNetNotification(line);
                    }
                }
                _fifo = _fifo.Substring(pos + 1);
            } while (pos >= 0);
            _error_delay = 0;
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
            //Debug.WriteLine(string.Format("SyncRes on {0} took {1}ms", device.SerialNumber, stop - start));

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
            //Debug.WriteLine(string.Format("ASyncRes on {0} took {1}ms", device.SerialNumber, stop - start));
        }


        public override bool Connected {
            get { return !_sendPingNotification || _connected; }
        }

        public override bool hasRwAccess()
        {
            return _hub._http_params.User.Equals("admin");
        }
    }
}