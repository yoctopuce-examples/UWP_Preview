/*********************************************************************
 *
 * $Id: YAPI.cs 29463 2017-12-20 07:40:43Z mvuilleu $
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using com.yoctopuce.YoctoAPI;

namespace com.yoctopuce.YoctoAPI
{

    public sealed class YRefParam
    {
        public string Value { get; set; }
    }


    /// 
    public class YAPI
    {
        // Default cache validity (in [ms]) before reloading data from device. This
        // saves a lots of traffic.
        // Note that a value under 2 ms makes little sense since a USB bus itself
        // has a 2ms round trip period
        //todo: generated code must use defautl cachevalidity form YAPIContext
        public static ulong DefaultCacheValidity = 5;

        // Return value for invalid strings
        public const string INVALID_STRING = "!INVALID!";
        public const double INVALID_DOUBLE = -1.79769313486231E+308;
        public const int INVALID_INT = -2147483648;
        public const long INVALID_LONG = -9223372036854775807L;
        public const int INVALID_UINT = -1;
        public const string YOCTO_API_VERSION_STR = "1.10";
        public const string YOCTO_API_BUILD_STR = "29715";
        public const int YOCTO_API_VERSION_BCD = 0x0110;
        public const int YOCTO_VENDORID = 0x24e0;
        public const int YOCTO_DEVID_FACTORYBOOT = 1;
        public const int YOCTO_DEVID_BOOTLOADER = 2;
        // --- (generated code: YFunction return codes)
    // Yoctopuce error codes, used by default as function return value
        public const int SUCCESS = 0;                   // everything worked all right
        public const int NOT_INITIALIZED = -1;          // call yInitAPI() first !
        public const int INVALID_ARGUMENT = -2;         // one of the arguments passed to the function is invalid
        public const int NOT_SUPPORTED = -3;            // the operation attempted is (currently) not supported
        public const int DEVICE_NOT_FOUND = -4;         // the requested device is not reachable
        public const int VERSION_MISMATCH = -5;         // the device firmware is incompatible with this API version
        public const int DEVICE_BUSY = -6;              // the device is busy with another task and cannot answer
        public const int TIMEOUT = -7;                  // the device took too long to provide an answer
        public const int IO_ERROR = -8;                 // there was an I/O problem while talking to the device
        public const int NO_MORE_DATA = -9;             // there is no more data to read from
        public const int EXHAUSTED = -10;               // you have run out of a limited resource, check the documentation
        public const int DOUBLE_ACCES = -11;            // you have two process that try to access to the same device
        public const int UNAUTHORIZED = -12;            // unauthorized access to password-protected device
        public const int RTC_NOT_READY = -13;           // real-time clock has not been initialized (or time was lost)
        public const int FILE_NOT_FOUND = -14;          // the file is not found

//--- (end of generated code: YFunction return codes)
        internal static Encoding DefaultEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

        // Encoding types
        internal const int YOCTO_CALIB_TYPE_OFS = 30;

        // Yoctopuce generic constant
        internal const int YOCTO_MANUFACTURER_LEN = 20;
        internal const int YOCTO_SERIAL_LEN = 20;
        internal const int YOCTO_BASE_SERIAL_LEN = 8;
        internal const int YOCTO_PRODUCTNAME_LEN = 28;
        internal const int YOCTO_FIRMWARE_LEN = 22;
        internal const int YOCTO_LOGICAL_LEN = 20;
        internal const int YOCTO_FUNCTION_LEN = 20;
        internal const int YOCTO_PUBVAL_SIZE = 6; // Size of the data (can be non null
        internal const int YOCTO_PUBVAL_LEN = 16; // Temporary storage, >=
        internal const int YOCTO_PASS_LEN = 20;
        internal const int YOCTO_REALM_LEN = 20;
        internal const int HASH_BUF_SIZE = 28;


        // yInitAPI argument

        public const int DETECT_NONE = 0;
        public const int DETECT_USB = 1;
        public const int DETECT_NET = 2;
        public const int RESEND_MISSING_PKT = 4;
        public static readonly int DETECT_ALL = DETECT_USB | DETECT_NET;
        public const int DEFAULT_PKT_RESEND_DELAY = 50;
        //todo: move to YAPIContex
        internal static int pktAckDelay = DEFAULT_PKT_RESEND_DELAY;


        // - Types used for public yocto_api callbacks
        public delegate Task LogHandler(string log);
        public delegate Task DeviceUpdateHandler(YModule m);
        public delegate double CalibrationHandler(double rawValue, int calibType, List<int> parameters, List<double> rawValues, List<double> refValues);
        public delegate Task HubDiscoveryHandler(string serial, string url);


        private static YAPIContext _SingleYAPI = null;
            


        //todo: Look how to impement YAPIContext strategy
        internal static YAPIContext imm_GetYCtx()
        {
            if (_SingleYAPI == null) {
                _SingleYAPI = new YAPIContext();
            }
            return _SingleYAPI;
        }


        //PUBLIC STATIC METHOD:

        /**
         * <summary>
         *   Returns the version identifier for the Yoctopuce library in use.
         * <para>
         *   The version is a string in the form <c>"Major.Minor.Build"</c>,
         *   for instance <c>"1.01.5535"</c>. For languages using an external
         *   DLL (for instance C#, VisualBasic or Delphi), the character string
         *   includes as well the DLL version, for instance
         *   <c>"1.01.5535 (1.01.5439)"</c>.
         * </para>
         * <para>
         *   If you want to verify in your code that the library version is
         *   compatible with the version that you have used during development,
         *   verify that the major number is strictly equal and that the minor
         *   number is greater or equal. The build number is not relevant
         *   with respect to the library compatibility.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <returns>
         *   a character string describing the library version.
         * </returns>
         */
        public static string GetAPIVersion()
        {
            return YOCTO_API_VERSION_STR + ".29715" + YUSBHub.imm_getAPIVersion();
        }

        /**
         * <summary>
         *   Initializes the Yoctopuce programming library explicitly.
         * <para>
         *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
         *   automatically  initialized when calling <c>yRegisterHub()</c> for the
         *   first time.
         * </para>
         * <para>
         *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
         *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
         *   VirtualHub on which your devices are connected before trying to access them.
         * </para>
         * </summary>
         * <param name="mode">
         *   an integer corresponding to the type of automatic
         *   device detection to use. Possible values are
         *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
         *   and <c>YAPI.DETECT_ALL</c>.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> InitAPI(int mode, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await yctx.InitAPI(mode);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Initializes the Yoctopuce programming library explicitly.
         * <para>
         *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
         *   automatically  initialized when calling <c>yRegisterHub()</c> for the
         *   first time.
         * </para>
         * <para>
         *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
         *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
         *   VirtualHub on which your devices are connected before trying to access them.
         * </para>
         * </summary>
         * <param name="mode">
         *   an integer corresponding to the type of automatic
         *   device detection to use. Possible values are
         *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
         *   and <c>YAPI.DETECT_ALL</c>.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> InitAPI(int mode)
        {
            YAPIContext yctx = imm_GetYCtx();
            return await yctx.InitAPI(mode);
        }



        /**
         * <summary>
         *   Frees dynamically allocated memory blocks used by the Yoctopuce library.
         * <para>
         *   It is generally not required to call this function, unless you
         *   want to free all dynamically allocated memory blocks in order to
         *   track a memory leak for instance.
         *   You should not call any other library function after calling
         *   <c>yFreeAPI()</c>, or your program will crash.
         * </para>
         * </summary>
         */
        public static void FreeAPI()
        {
            YAPIContext yctx;
            yctx = _SingleYAPI;
            if (yctx != null) {
                yctx.FreeAPI();
            }
            _SingleYAPI = null;
        }


        /**
         * <summary>
         *   Setup the Yoctopuce library to use modules connected on a given machine.
         * <para>
         *   The
         *   parameter will determine how the API will work. Use the following values:
         * </para>
         * <para>
         *   <b>usb</b>: When the <c>usb</c> keyword is used, the API will work with
         *   devices connected directly to the USB bus. Some programming languages such a Javascript,
         *   PHP, and Java don't provide direct access to USB hardware, so <c>usb</c> will
         *   not work with these. In this case, use a VirtualHub or a networked YoctoHub (see below).
         * </para>
         * <para>
         *   <b><i>x.x.x.x</i></b> or <b><i>hostname</i></b>: The API will use the devices connected to the
         *   host with the given IP address or hostname. That host can be a regular computer
         *   running a VirtualHub, or a networked YoctoHub such as YoctoHub-Ethernet or
         *   YoctoHub-Wireless. If you want to use the VirtualHub running on you local
         *   computer, use the IP address 127.0.0.1.
         * </para>
         * <para>
         *   <b>callback</b>: that keyword make the API run in "<i>HTTP Callback</i>" mode.
         *   This a special mode allowing to take control of Yoctopuce devices
         *   through a NAT filter when using a VirtualHub or a networked YoctoHub. You only
         *   need to configure your hub to call your server script on a regular basis.
         *   This mode is currently available for PHP and Node.JS only.
         * </para>
         * <para>
         *   Be aware that only one application can use direct USB access at a
         *   given time on a machine. Multiple access would cause conflicts
         *   while trying to access the USB modules. In particular, this means
         *   that you must stop the VirtualHub software before starting
         *   an application that uses direct USB access. The workaround
         *   for this limitation is to setup the library to use the VirtualHub
         *   rather than direct USB access.
         * </para>
         * <para>
         *   If access control has been activated on the hub, virtual or not, you want to
         *   reach, the URL parameter should look like:
         * </para>
         * <para>
         *   <c>http://username:password@address:port</c>
         * </para>
         * <para>
         *   You can call <i>RegisterHub</i> several times to connect to several machines.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> RegisterHub(string url, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().RegisterHub(url);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }



        /**
         * <summary>
         *   Setup the Yoctopuce library to use modules connected on a given machine.
         * <para>
         *   The
         *   parameter will determine how the API will work. Use the following values:
         * </para>
         * <para>
         *   <b>usb</b>: When the <c>usb</c> keyword is used, the API will work with
         *   devices connected directly to the USB bus. Some programming languages such a Javascript,
         *   PHP, and Java don't provide direct access to USB hardware, so <c>usb</c> will
         *   not work with these. In this case, use a VirtualHub or a networked YoctoHub (see below).
         * </para>
         * <para>
         *   <b><i>x.x.x.x</i></b> or <b><i>hostname</i></b>: The API will use the devices connected to the
         *   host with the given IP address or hostname. That host can be a regular computer
         *   running a VirtualHub, or a networked YoctoHub such as YoctoHub-Ethernet or
         *   YoctoHub-Wireless. If you want to use the VirtualHub running on you local
         *   computer, use the IP address 127.0.0.1.
         * </para>
         * <para>
         *   <b>callback</b>: that keyword make the API run in "<i>HTTP Callback</i>" mode.
         *   This a special mode allowing to take control of Yoctopuce devices
         *   through a NAT filter when using a VirtualHub or a networked YoctoHub. You only
         *   need to configure your hub to call your server script on a regular basis.
         *   This mode is currently available for PHP and Node.JS only.
         * </para>
         * <para>
         *   Be aware that only one application can use direct USB access at a
         *   given time on a machine. Multiple access would cause conflicts
         *   while trying to access the USB modules. In particular, this means
         *   that you must stop the VirtualHub software before starting
         *   an application that uses direct USB access. The workaround
         *   for this limitation is to setup the library to use the VirtualHub
         *   rather than direct USB access.
         * </para>
         * <para>
         *   If access control has been activated on the hub, virtual or not, you want to
         *   reach, the URL parameter should look like:
         * </para>
         * <para>
         *   <c>http://username:password@address:port</c>
         * </para>
         * <para>
         *   You can call <i>RegisterHub</i> several times to connect to several machines.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> RegisterHub(string url)
        {
            return await imm_GetYCtx().RegisterHub(url);
        }

        /**
         * <summary>
         *   Fault-tolerant alternative to <c>RegisterHub()</c>.
         * <para>
         *   This function has the same
         *   purpose and same arguments as <c>RegisterHub()</c>, but does not trigger
         *   an error when the selected hub is not available at the time of the function call.
         *   This makes it possible to register a network hub independently of the current
         *   connectivity, and to try to contact it only when a device is actively needed.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> PreregisterHub(string url, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().PreregisterHub(url);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Fault-tolerant alternative to <c>RegisterHub()</c>.
         * <para>
         *   This function has the same
         *   purpose and same arguments as <c>RegisterHub()</c>, but does not trigger
         *   an error when the selected hub is not available at the time of the function call.
         *   This makes it possible to register a network hub independently of the current
         *   connectivity, and to try to contact it only when a device is actively needed.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> PreregisterHub(string url)
        {
            return await imm_GetYCtx().PreregisterHub(url);
        }


        /**
         * <summary>
         *   Setup the Yoctopuce library to no more use modules connected on a previously
         *   registered machine with RegisterHub.
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c> or the
         *   root URL of the hub to monitor
         * </param>
         */
        public static async Task UnregisterHub(string url)
        {
            await imm_GetYCtx().UnregisterHub(url);
        }


        /**
         * <summary>
         *   Test if the hub is reachable.
         * <para>
         *   This method do not register the hub, it only test if the
         *   hub is usable. The url parameter follow the same convention as the <c>RegisterHub</c>
         *   method. This method is useful to verify the authentication parameters for a hub. It
         *   is possible to force this method to return after mstimeout milliseconds.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="mstimeout">
         *   the number of millisecond available to test the connection.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> TestHub(string url, uint mstimeout, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().TestHub(url, mstimeout);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }


        /**
         * <summary>
         *   Test if the hub is reachable.
         * <para>
         *   This method do not register the hub, it only test if the
         *   hub is usable. The url parameter follow the same convention as the <c>RegisterHub</c>
         *   method. This method is useful to verify the authentication parameters for a hub. It
         *   is possible to force this method to return after mstimeout milliseconds.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="mstimeout">
         *   the number of millisecond available to test the connection.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> TestHub(string url, uint mstimeout)
        {
            return await imm_GetYCtx().TestHub(url, mstimeout);
        }

        /**
         * <summary>
         *   Triggers a (re)detection of connected Yoctopuce modules.
         * <para>
         *   The library searches the machines or USB ports previously registered using
         *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
         *   in case a change in the list of connected devices is detected.
         * </para>
         * <para>
         *   This function can be called as frequently as desired to refresh the device list
         *   and to make the application aware of hot-plug events.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> UpdateDeviceList(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().UpdateDeviceList();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }



        /**
         * <summary>
         *   Triggers a (re)detection of connected Yoctopuce modules.
         * <para>
         *   The library searches the machines or USB ports previously registered using
         *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
         *   in case a change in the list of connected devices is detected.
         * </para>
         * <para>
         *   This function can be called as frequently as desired to refresh the device list
         *   and to make the application aware of hot-plug events.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> UpdateDeviceList()
        {
            return await imm_GetYCtx().UpdateDeviceList();
        }

        /**
         * <summary>
         *   Maintains the device-to-library communication channel.
         * <para>
         *   If your program includes significant loops, you may want to include
         *   a call to this function to make sure that the library takes care of
         *   the information pushed by the modules on the communication channels.
         *   This is not strictly necessary, but it may improve the reactivity
         *   of the library for the following commands.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> HandleEvents(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().HandleEvents();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }

        }



        /**
         * <summary>
         *   Maintains the device-to-library communication channel.
         * <para>
         *   If your program includes significant loops, you may want to include
         *   a call to this function to make sure that the library takes care of
         *   the information pushed by the modules on the communication channels.
         *   This is not strictly necessary, but it may improve the reactivity
         *   of the library for the following commands.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> HandleEvents()
        {
            return await imm_GetYCtx().HandleEvents();
        }


        /**
         * <summary>
         *   Pauses the execution flow for a specified duration.
         * <para>
         *   This function implements a passive waiting loop, meaning that it does not
         *   consume CPU cycles significantly. The processor is left available for
         *   other threads and processes. During the pause, the library nevertheless
         *   reads from time to time information from the Yoctopuce modules by
         *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="ms_duration">
         *   an integer corresponding to the duration of the pause,
         *   in milliseconds.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> Sleep(ulong ms_duration, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().Sleep(ms_duration);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }


        /**
         * <summary>
         *   Pauses the execution flow for a specified duration.
         * <para>
         *   This function implements a passive waiting loop, meaning that it does not
         *   consume CPU cycles significantly. The processor is left available for
         *   other threads and processes. During the pause, the library nevertheless
         *   reads from time to time information from the Yoctopuce modules by
         *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="ms_duration">
         *   an integer corresponding to the duration of the pause,
         *   in milliseconds.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public static async Task<int> Sleep(ulong ms_duration)
        {
            return await imm_GetYCtx().Sleep(ms_duration);
        }


        /**
         * <summary>
         *   Force a hub discovery, if a callback as been registered with <c>yRegisterHubDiscoveryCallback</c> it
         *   will be called for each net work hub that will respond to the discovery.
         * <para>
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         *   On failure, throws an exception or returns a negative error code.
         * </returns>
         */
        public static async Task<int> TriggerHubDiscovery(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().TriggerHubDiscovery();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Force a hub discovery, if a callback as been registered with <c>yRegisterHubDiscoveryCallback</c> it
         *   will be called for each net work hub that will respond to the discovery.
         * <para>
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         *   On failure, throws an exception or returns a negative error code.
         * </returns>
         */
        public static async Task<int> TriggerHubDiscovery()
        {
            return await imm_GetYCtx().TriggerHubDiscovery();
        }



        /**
         * <summary>
         *   Returns the current value of a monotone millisecond-based time counter.
         * <para>
         *   This counter can be used to compute delays in relation with
         *   Yoctopuce devices, which also uses the millisecond as timebase.
         * </para>
         * </summary>
         * <returns>
         *   a long integer corresponding to the millisecond counter.
         * </returns>
         */
        public static ulong GetTickCount()
        {
            return (ulong)DateTime.Now.Ticks / 10000;
        }

        /**
         * <summary>
         *   Checks if a given string is valid as logical name for a module or a function.
         * <para>
         *   A valid logical name has a maximum of 19 characters, all among
         *   <c>A..Z</c>, <c>a..z</c>, <c>0..9</c>, <c>_</c>, and <c>-</c>.
         *   If you try to configure a logical name with an incorrect string,
         *   the invalid characters are ignored.
         * </para>
         * </summary>
         * <param name="name">
         *   a string containing the name to check.
         * </param>
         * <returns>
         *   <c>true</c> if the name is valid, <c>false</c> otherwise.
         * </returns>
         */
        public static bool CheckLogicalName(string name)
        {
            return name != null && (name != "" || name.Length <= 19 && Regex.IsMatch(name, "^[A-Za-z0-9_-]*$"));
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is plugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="arrivalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterDeviceArrivalCallback(YAPI.DeviceUpdateHandler arrivalCallback)
        {
            imm_GetYCtx().RegisterDeviceArrivalCallback(arrivalCallback);
        }

        public static void RegisterDeviceChangeCallback(YAPI.DeviceUpdateHandler changeCallback)
        {
            imm_GetYCtx().RegisterDeviceChangeCallback(changeCallback);
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is unplugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="removalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterDeviceRemovalCallback(YAPI.DeviceUpdateHandler removalCallback)
        {
            imm_GetYCtx().RegisterDeviceRemovalCallback(removalCallback);
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time an Network Hub send
         *   an SSDP message.
         * <para>
         *   The callback has two string parameter, the first one
         *   contain the serial number of the hub and the second contain the URL of the
         *   network hub (this URL can be passed to RegisterHub). This callback will be invoked
         *   while yUpdateDeviceList is running. You will have to call this function on a regular basis.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="hubDiscoveryCallback">
         *   a procedure taking two string parameter, the serial
         *   number and the hub URL. Use <c>null</c> to unregister a previously registered  callback.
         * </param>
         */
        public static async Task RegisterHubDiscoveryCallback(YAPI.HubDiscoveryHandler hubDiscoveryCallback)
        {
            await imm_GetYCtx().RegisterHubDiscoveryCallback(hubDiscoveryCallback);
        }

        /**
         * <summary>
         *   Registers a log callback function.
         * <para>
         *   This callback will be called each time
         *   the API have something to say. Quite useful to debug the API.
         * </para>
         * </summary>
         * <param name="logfun">
         *   a procedure taking a string parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterLogFunction(YAPI.LogHandler logfun)
        {
            imm_GetYCtx().RegisterLogFunction(logfun);
        }

        public static string get_debugMsg(string serial)
        {
            return imm_GetYCtx().get_debugMsg(serial);
        }
    }
}