/*********************************************************************
 *
 * $Id: YFirmwareUpdate.cs 25204 2016-08-17 13:52:16Z seb $
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
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace com.yoctopuce.YoctoAPI
{

    //--- (generated code: YFirmwareUpdate return codes)
//--- (end of generated code: YFirmwareUpdate return codes)
    //--- (generated code: YFirmwareUpdate class start)
/**
 * <summary>
 *   YFirmwareUpdate Class: Control interface for the firmware update process
 * <para>
 *   The YFirmwareUpdate class let you control the firmware update of a Yoctopuce
 *   module. This class should not be instantiate directly, instead the method
 *   <c>updateFirmware</c> should be called to get an instance of YFirmwareUpdate.
 * </para>
 * </summary>
 */
public class YFirmwareUpdate
{
//--- (end of generated code: YFirmwareUpdate class start)
        //--- (generated code: YFirmwareUpdate definitions)
    protected string _serial;
    protected byte[] _settings;
    protected string _firmwarepath;
    protected string _progress_msg;
    protected int _progress_c = 0;
    protected int _progress = 0;
    protected int _restore_step = 0;
    protected bool _force;

    //--- (end of generated code: YFirmwareUpdate definitions)
        private readonly YAPIContext _yctx;
        //fixme :implement firwmare Update

        internal static async Task<byte[]> _downloadfile(string url)
        {
            HttpClient client = new HttpClient();
            try {
                //Send the GET request
                HttpResponseMessage httpResponse = await client.GetAsync(new Uri(url));
                httpResponse.EnsureSuccessStatusCode();
                IBuffer buffer = await httpResponse.Content.ReadAsBufferAsync();
                return buffer.ToArray();
            } catch (Exception ex) {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                throw new YAPI_Exception(YAPI.IO_ERROR, "unable to contact www.yoctopuce.com :" + ex.Message);
            }
        }

        //fixme: use UWP file access
        internal static async Task<YFirmwareFile> checkFirmware_r(string folder, string serial_base)
        {
            YFirmwareFile bestFirmware = null;
            if (File.Exists(folder)) {
                bestFirmware = _loadFirmwareFile(folder);
            } else {
                string[] listOfFiles = Directory.GetFileSystemEntries(folder);
                foreach (string subfile in listOfFiles) {
                    if (!subfile.StartsWith(serial_base)) {
                        continue;
                    }
                    YFirmwareFile firmware = null;
                    string fullpath = Path.Combine(folder, subfile);
                    if (File.Exists(fullpath)) {
                        try {
                            firmware = _loadFirmwareFile(fullpath);
                        } catch (YAPI_Exception) {
                            continue;
                        }
                    } else if (Directory.Exists(fullpath)) {
                        firmware = await checkFirmware_r(fullpath, serial_base);
                    }
                    if (firmware == null || !firmware.Serial.StartsWith(serial_base, StringComparison.Ordinal)) {
                        continue;
                    }
                    if (bestFirmware == null || bestFirmware.FirmwareReleaseAsInt < firmware.FirmwareReleaseAsInt) {
                        bestFirmware = firmware;
                    }
                }
            }
            return bestFirmware;
        }

        //fixme: use UWP file access
        private static YFirmwareFile _loadFirmwareFile(string file)
        {
            byte[] data = File.ReadAllBytes(file);
            return YFirmwareFile.imm_Parse(file, data);
        }



        public YFirmwareUpdate(YAPIContext yctx, string serial, string path, byte[] settings, bool force)
        {
            _serial = serial;
            _firmwarepath = path;
            _settings = settings;
            _yctx = yctx;
            _force = force;
            //--- (generated code: YFirmwareUpdate attributes initialization)
        //--- (end of generated code: YFirmwareUpdate attributes initialization)
        }


        public YFirmwareUpdate(YAPIContext yctx, string serial, string path, byte[] settings) : this(yctx, serial, path, settings, false)
        {
        }

        public YFirmwareUpdate(string serial, string path, byte[] settings) : this(YAPI.imm_GetYCtx(), serial, path, settings, false)
        {
        }

        private void imm_reportprogress(int progress, string msg)
        {
            _progress = progress;
            _progress_msg = msg;
        }

        private async Task _processMore(int start)
        {
            if (start == 0)
                return;
            imm_reportprogress(0, "Firmware update started");
            YFirmwareFile firmware;
            try {
                //1% -> 5%
                if (_firmwarepath.StartsWith("www.yoctopuce.com") || _firmwarepath.StartsWith("http://www.yoctopuce.com")) {
                    this.imm_reportprogress(1, "Downloading firmware");
                    byte[] bytes = await YFirmwareUpdate._downloadfile(_firmwarepath);
                    firmware = YFirmwareFile.imm_Parse(_firmwarepath, bytes);
                } else {
                    imm_reportprogress(1, "Loading firmware");
                    firmware = YFirmwareUpdate._loadFirmwareFile(_firmwarepath);
                }

                //5% -> 10%
                imm_reportprogress(5, "check if module is already in bootloader");
                YGenericHub hub = null;
                YModule module = YModule.FindModuleInContext(_yctx, _serial + ".module");
                if (await module.isOnline()) {
                    YDevice yDevice = await module.getYDevice();
                    hub = yDevice.Hub;
                } else {
                    // test if already in bootloader
                    foreach (YGenericHub h in _yctx._hubs) {
                        List<string> bootloaders = await h.getBootloaders();
                        if (bootloaders.Contains(_serial)) {
                            hub = h;
                            break;
                        }
                    }
                }
                if (hub == null) {
                    imm_reportprogress(-1, "Device " + _serial + " is not detected");
                    return;
                }

                await hub.firmwareUpdate(_serial, firmware, _settings, imm_firmware_progress);
                //80%-> 98%
                imm_reportprogress(80, "wait to the device restart");
                ulong timeout = YAPI.GetTickCount() + 60000;
                await module.clearCache();
                while (!await module.isOnline() && timeout > YAPI.GetTickCount()) {
                    await Task.Delay(5000);
                    try {
                        await _yctx.UpdateDeviceList();
                    } catch (YAPI_Exception) {
                    }
                }
                if (await module.isOnline()) {
                    if (_settings != null) {
                        await module.set_allSettingsAndFiles(_settings);
                        await module.saveToFlash();
                    }
                    imm_reportprogress(100, "Success");
                } else {
                    imm_reportprogress(-1, "Device did not reboot correctly");
                }
            } catch (YAPI_Exception e) {
                imm_reportprogress(e.errorType, e.Message);
                Debug.WriteLine(e.ToString());
                Debug.Write(e.StackTrace);
            }
        }

#pragma warning disable 1998
        private async Task imm_firmware_progress(int percent, string message)
        {
            imm_reportprogress(5 + percent * 80 / 100, message);
        }
#pragma warning restore 1998        

        /// <summary>
        /// Test if the byn file is valid for this module. It is possible to pass a directory instead of a file.
        /// In that case, this method returns the path of the most recent appropriate byn file. This method will
        /// ignore any firmware older than minrelease.
        /// </summary>
        /// <param name="serial"> : the serial number of the module to update </param>
        /// <param name="path"> : the path of a byn file or a directory that contains byn files </param>
        /// <param name="minrelease"> : a positive integer
        /// </param>
        /// <returns> : the path of the byn file to use, or an empty string if no byn files matches the requirement
        /// 
        /// On failure, returns a string that starts with "error:". </returns>
        public static async Task<string> CheckFirmwareEx(string serial, string path, int minrelease, bool force)
        {
            string link = "";
            int best_rev = 0;
            if (path.StartsWith("www.yoctopuce.com", StringComparison.Ordinal) || path.StartsWith("http://www.yoctopuce.com", StringComparison.Ordinal)) {
                byte[] json = await YFirmwareUpdate._downloadfile("http://www.yoctopuce.com//FR/common/getLastFirmwareLink.php?serial=" + serial);
                YJSONObject obj;
                obj = new YJSONObject(YAPI.DefaultEncoding.GetString(json));
                obj.Parse();
                link = obj.GetString("link");
                best_rev = obj.GetInt("version");

            } else {
                YFirmwareFile firmware = await YFirmwareUpdate.checkFirmware_r(path, serial.Substring(0, YAPI.YOCTO_BASE_SERIAL_LEN));
                if (firmware != null) {
                    best_rev = firmware.FirmwareReleaseAsInt;
                    link = firmware.Path;
                }
            }
            if (minrelease != 0) {
                if (minrelease < best_rev) {
                    return link;
                } else {
                    return "";
                }
            }
            return link;
        }

        /// <summary>
        /// Test if the byn file is valid for this module. It is possible to pass a directory instead of a file.
        /// In that case, this method returns the path of the most recent appropriate byn file. This method will
        /// ignore any firmware older than minrelease.
        /// </summary>
        /// <param name="serial"> : the serial number of the module to update </param>
        /// <param name="path"> : the path of a byn file or a directory that contains byn files </param>
        /// <param name="minrelease"> : a positive integer
        /// </param>
        /// <returns> : the path of the byn file to use, or an empty string if no byn files matches the requirement
        /// 
        /// On failure, returns a string that starts with "error:". </returns>
        public static async Task<string> CheckFirmware(string serial, string path, int minrelease)
        {
            return await CheckFirmwareEx(serial, path, minrelease, false);
        }

        /// <summary>
        /// Returns a list of all the modules in "firmware update" mode. Only devices
        /// connected over USB are listed. For devices connected to a YoctoHub, you
        /// must connect to the YoctoHub web interface.
        /// </summary>
        /// <param name="yctx"> : a YAPI context.
        /// </param>
        /// <returns> an array of strings containing the serial numbers of devices in "firmware update" mode. </returns>
        public static async Task<List<string>> GetAllBootLoadersInContext(YAPIContext yctx)
        {
            List<string> res = new List<string>();
            foreach (YGenericHub h in yctx._hubs) {
                try {
                    List<string> bootloaders;
                    bootloaders = await h.getBootloaders();
                    if (bootloaders != null) {
                        res.AddRange(bootloaders);
                    }
                } catch (YAPI_Exception e) {
                    yctx._Log(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// Returns a list of all the modules in "firmware update" mode. Only devices
        /// connected over USB are listed. For devices connected to a YoctoHub, you
        /// must connect yourself to the YoctoHub web interface.
        /// </summary>
        /// <returns> an array of strings containing the serial numbers of devices in "firmware update" mode. </returns>
        public static async Task<List<string>> GetAllBootLoaders()
        {
            return await GetAllBootLoadersInContext(YAPI.imm_GetYCtx());
        }


        //--- (generated code: YFirmwareUpdate implementation)
#pragma warning disable 1998

    //cannot be generated for Java:
    //public virtual async Task<int> _processMore(int newupdate)

    //cannot be generated for Java:
    //public static List<string> GetAllBootLoaders()

    //cannot be generated for Java:
    //public static List<string> GetAllBootLoadersInContext(YAPIContext yctx)

    //cannot be generated for Java:
    //public static string CheckFirmware(string serial,string path,int minrelease)

    /**
     * <summary>
     *   Returns the progress of the firmware update, on a scale from 0 to 100.
     * <para>
     *   When the object is
     *   instantiated, the progress is zero. The value is updated during the firmware update process until
     *   the value of 100 is reached. The 100 value means that the firmware update was completed
     *   successfully. If an error occurs during the firmware update, a negative value is returned, and the
     *   error message can be retrieved with <c>get_progressMessage</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion)
     *   or a negative error code in case of failure.
     * </returns>
     */
    public virtual async Task<int> get_progress()
    {
        if (_progress >= 0) {
            await this._processMore(0);
        }
        return _progress;
    }

    /**
     * <summary>
     *   Returns the last progress message of the firmware update process.
     * <para>
     *   If an error occurs during the
     *   firmware update process, the error message is returned
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string  with the latest progress message, or the error message.
     * </returns>
     */
    public virtual async Task<string> get_progressMessage()
    {
        return _progress_msg;
    }

    /**
     * <summary>
     *   Starts the firmware update process.
     * <para>
     *   This method starts the firmware update process in background. This method
     *   returns immediately. You can monitor the progress of the firmware update with the <c>get_progress()</c>
     *   and <c>get_progressMessage()</c> methods.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion),
     *   or a negative error code in case of failure.
     * </returns>
     * <para>
     *   On failure returns a negative error code.
     * </para>
     */
    public virtual async Task<int> startUpdate()
    {
        string err;
        int leng;
        err = YAPI.DefaultEncoding.GetString(_settings);
        leng = (err).Length;
        if (( leng >= 6) && ("error:" == (err).Substring(0, 6))) {
            _progress = -1;
            _progress_msg = (err).Substring( 6, leng - 6);
        } else {
            _progress = 0;
            _progress_c = 0;
            await this._processMore(1);
        }
        return _progress;
    }

#pragma warning restore 1998
    //--- (end of generated code: YFirmwareUpdate implementation)
    }


}