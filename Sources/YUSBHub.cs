/*********************************************************************
 *
 * $Id: YUSBHub.cs 25191 2016-08-15 12:43:02Z seb $
 *
 * High-level programming interface, common to all modules
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
using System.Diagnostics;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{



    internal class YUSBHub : YGenericHub
    {
        private YUSBWatcher _ywatcher;
        private new const long YPROG_BOOTLOADER_TIMEOUT = 3600000; // 1 hour

        internal override string SerialNumber {
            get {
                return "";
            }
        }

        public override string imm_get_urlOf(string serialNumber)
        {
            return "usb";
        }

        public override List<string> imm_get_subDeviceOf(string serialNumber)
        {
            return new List<string>();
        }

        internal YUSBHub(YAPIContext yctx, int idx, bool requestPermission) : base(yctx, new HTTPParams("usb://"), idx, true)
        {
            Debug.WriteLine("alloc YUSBHub");
            _ywatcher = new YUSBWatcher(this);
        }
#pragma warning disable 1998
        internal override async Task startNotifications()
        {
            return;
        }

        internal override async Task stopNotifications()
        {
            return;
        }
#pragma warning restore 1998

        internal override void imm_release()
        {
            _ywatcher.Stop();
            Debug.WriteLine("release YUSBHub");
        }




        internal override async Task updateDeviceListAsync(bool forceupdate)
        {
            ulong now = YAPI.GetTickCount();
            if (forceupdate) {
                _devListExpires = 0;
            }
            if (_devListExpires > now) {
                return;
            }

            await _ywatcher.ImediateEnum();
            List<YUSBDevice> devices = _ywatcher.imm_GetUsableDevices();
            //_yctx._Log(string.Format("{0} devices usable\n",devices.Count));
            Dictionary<string, List<YPEntry>> yellowPages = new Dictionary<string, List<YPEntry>>();
            List<WPEntry> whitePages = new List<WPEntry>();
            foreach (YUSBDevice d in devices) {
                whitePages.Add(d.imm_GetWhitesPagesEntry());
                d.imm_UpdateYellowPages(yellowPages);
            }
            await updateFromWpAndYp(whitePages, yellowPages);
            // reset device list cache timeout for this hub
            now = YAPI.GetTickCount();
            _devListExpires = now + _devListValidity;
        }

        public override  Task<List<string>> getBootloaders()
        {
            //todo: implement get bootloaders
            throw new NotImplementedException();
        }

        internal override Task<int> ping(uint mstimeout)
        {
            return Task.FromResult(YAPI.SUCCESS);
        }

        internal override Task<List<string>> firmwareUpdate(string serial, YFirmwareFile firmware, byte[] settings, UpdateProgress progress)
        {
            //todo: implement firmware upate
            throw new NotImplementedException();
        }

        private byte[] imm_prepareRequest(string firstLine, byte[] rest_of_request)
        {
            byte[] currentRequest;
            if (rest_of_request == null) {
                currentRequest = YAPI.DefaultEncoding.GetBytes(firstLine + "\r\n\r\n");
            } else {
                firstLine += "\r\n";
                int len = firstLine.Length + rest_of_request.Length;
                currentRequest = new byte[len];
                Buffer.BlockCopy(YAPI.DefaultEncoding.GetBytes(firstLine), 0, currentRequest, 0, firstLine.Length);
                Buffer.BlockCopy(rest_of_request, 0, currentRequest, firstLine.Length, rest_of_request.Length);
            }
            return currentRequest;
        }

        internal override async Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, RequestAsyncResult asyncResult, object asyncContext)
        {
            String serial = device.SerialNumber;
            byte[] req = imm_prepareRequest(req_first_line, req_head_and_body);
            await _ywatcher.DevRequestAsync(serial, req, asyncResult, asyncContext);
        }

        internal override async Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, RequestProgress progress, object context)
        {
            String serial = device.SerialNumber;
            byte[] req = imm_prepareRequest(req_first_line, req_head_and_body);
            return await _ywatcher.DevRequestSync(serial, req, progress, context);
        }


        internal override string RootUrl {
            get {
                return "usb";
            }
        }

        internal override bool imm_isSameHub(string url, object request, object response, object session)
        {
            return url.StartsWith("usb", StringComparison.Ordinal);
        }
        
    }

}