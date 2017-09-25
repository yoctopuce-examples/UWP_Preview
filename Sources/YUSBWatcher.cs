/*********************************************************************
 *
 * $Id: yocto_api.cs 22010 2015-11-16 15:55:26Z seb $
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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace com.yoctopuce.YoctoAPI
{

    public class YUSBWatcher
    {
        private const ushort usagePage = 0xFF00;
        private const ushort usageId = 0x0001;


        private YUSBHub _hub;
        internal readonly List<YUSBDevice> _allDevice = new List<YUSBDevice>(4);
        internal readonly List<YUSBDevice> _usableDevices = new List<YUSBDevice>(4);

        internal YUSBWatcher(YUSBHub hub)
        {
            _hub = hub;


            // Create a selector that gets a HID device using VID/PID and a
            // VendorDefined usage.
            var selector = HidDevice.GetDeviceSelector(usagePage, usageId);
        }

    

        internal async Task<int> ImediateEnum()
        {
            // Create a selector that gets a HID device using VID/PID and a
            // VendorDefined usage.
            var selector = HidDevice.GetDeviceSelector(usagePage, usageId);
            // Enumerate devices using the selector.
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
            foreach (YUSBDevice yusbDevice in _usableDevices) {
                yusbDevice.MarkForUnplug = true;
            }

            if (devices.Count > 0) {
                for (var i = 0; i < devices.Count; i++) {
                    var devinfo = devices.ElementAt(i);
                    if (!devinfo.IsEnabled) {
                        _hub._yctx._Log(devinfo.Name +" is disabled (skip)\n");
                        continue;
                    }
                    bool found = false;
                    foreach (YUSBDevice yusbDevice in _usableDevices) {
                        if (yusbDevice.Info.Id == devinfo.Id) {
                            yusbDevice.MarkForUnplug = false;
                            found = true;
                            break;
                        }
                    }
                    if (found) {
                        continue;
                    }

                    // Open the target HID device at index 0.
                    var device = await HidDevice.FromIdAsync(devinfo.Id,
                        FileAccessMode.ReadWrite);
                    if (device == null) {
                        // allready opened
                        continue;
                    }
                    if (device.VendorId == 0x24e0) {
                        YUSBDevice yusbDevice = new YUSBDevice(this, _hub, device, devinfo);
                        //Debug.WriteLine("setup yusbDevice=s " + yusbDevice.GetHashCode() + " device=" + device.GetHashCode());
                        await yusbDevice.Setup(YUSBPkt.YPKT_USB_VERSION_BCD);
                        _allDevice.Add(yusbDevice);
                    } else {
                        Debug.WriteLine("drop" + device.VendorId + ":" + device.ProductId);
                        device.Dispose();
                    }
                    
                }
            }
            _usableDevices.RemoveAll(item => item.MarkForUnplug);
            return _usableDevices.Count;
        }


        internal List<YUSBDevice> imm_GetUsableDevices()
        {
            return _usableDevices;
        }

        internal void imm_newUsableDevice(YUSBDevice yusbDevice)
        {
            if (!_usableDevices.Contains(yusbDevice)) {
                _usableDevices.Add(yusbDevice);
            }
        }

        internal void imm_removeUsableDevice(YUSBDevice yusbDevice)
        {
            if (_usableDevices.Contains(yusbDevice)) {
                _usableDevices.Remove(yusbDevice);
            }
        }

        private YUSBDevice imm_devFromSerial(String serial)
        {
            //todo: test if we spent too much time here (alternatively use hashmap)
            foreach (YUSBDevice device in _usableDevices) {
                if (device.SerialNumber == serial) {
                    return device;
                }
            }
            return null;
        }


        internal async Task<byte[]> DevRequestSync(string serial, byte[] request, YGenericHub.RequestProgress progress, object context)
        {
            YUSBDevice d = imm_devFromSerial(serial);
            return await d.DevRequestSync(serial, request, progress, context);
        }

        internal async Task DevRequestAsync(string serial, byte[] request, YGenericHub.RequestAsyncResult asyncResult, object asyncContext)
        {
            YUSBDevice d = imm_devFromSerial(serial);
            await d.DevRequestAsync(serial, request, asyncResult, asyncContext);
        }

        public void Stop()
        {
            foreach (YUSBDevice yusbDevice in _allDevice) {
                yusbDevice.imm_Stop();
            }
            _allDevice.Clear();
        }
    }
}