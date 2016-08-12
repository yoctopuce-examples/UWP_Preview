/*********************************************************************
 *
 * $Id: YFiles.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindFiles(), the high-level API for Files functions
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    //--- (generated code: YFiles return codes)
//--- (end of generated code: YFiles return codes)
    //--- (generated code: YFiles class start)
/**
 * <summary>
 *   YFiles Class: Files function interface
 * <para>
 *   The filesystem interface makes it possible to store files
 *   on some devices, for instance to design a custom web UI
 *   (for networked devices) or to add fonts (on display
 *   devices).
 * </para>
 * </summary>
 */
public class YFiles : YFunction
{
//--- (end of generated code: YFiles class start)
//--- (generated code: YFiles definitions)
    /**
     * <summary>
     *   invalid filesCount value
     * </summary>
     */
    public const  int FILESCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid freeSpace value
     * </summary>
     */
    public const  int FREESPACE_INVALID = YAPI.INVALID_UINT;
    protected int _filesCount = FILESCOUNT_INVALID;
    protected int _freeSpace = FREESPACE_INVALID;
    protected ValueCallback _valueCallbackFiles = null;

    public new delegate Task ValueCallback(YFiles func, string value);
    public new delegate Task TimedReportCallback(YFiles func, YMeasure measure);
    //--- (end of generated code: YFiles definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YFiles(YAPIContext ctx, string func): base(ctx, func, "Files")
        {
            //--- (generated code: YFiles attributes initialization)
        //--- (end of generated code: YFiles attributes initialization)
        }

        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
        protected YFiles(string func): this(YAPI.imm_GetYCtx(), func)
        {}

        //--- (generated code: YFiles implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("filesCount")) {
            _filesCount = json_val.GetInt("filesCount");
        }
        if (json_val.Has("freeSpace")) {
            _freeSpace = json_val.GetInt("freeSpace");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of files currently loaded in the filesystem.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of files currently loaded in the filesystem
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YFiles.FILESCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_filesCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FILESCOUNT_INVALID;
            }
        }
        return _filesCount;
    }


    /**
     * <summary>
     *   Returns the free space for uploading new files to the filesystem, in bytes.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the free space for uploading new files to the filesystem, in bytes
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YFiles.FREESPACE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_freeSpace()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return FREESPACE_INVALID;
            }
        }
        return _freeSpace;
    }


    /**
     * <summary>
     *   Retrieves a filesystem for a given identifier.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the filesystem is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YFiles.isOnline()</c> to test if the filesystem is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a filesystem by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the filesystem
     * </param>
     * <returns>
     *   a <c>YFiles</c> object allowing you to drive the filesystem.
     * </returns>
     */
    public static YFiles FindFiles(string func)
    {
        YFiles obj;
        obj = (YFiles) YFunction._FindFromCache("Files", func);
        if (obj == null) {
            obj = new YFiles(func);
            YFunction._AddToCache("Files",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a filesystem for a given identifier in a YAPI context.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the filesystem is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YFiles.isOnline()</c> to test if the filesystem is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a filesystem by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the filesystem
     * </param>
     * <returns>
     *   a <c>YFiles</c> object allowing you to drive the filesystem.
     * </returns>
     */
    public static YFiles FindFilesInContext(YAPIContext yctx,string func)
    {
        YFiles obj;
        obj = (YFiles) YFunction._FindFromCacheInContext(yctx,  "Files", func);
        if (obj == null) {
            obj = new YFiles(yctx, func);
            YFunction._AddToCache("Files",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackFiles = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public override async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackFiles != null) {
            await _valueCallbackFiles(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<byte[]> sendCommand(string command)
    {
        string url;
        url = "files.json?a="+command;
        // may throw an exception
        return await this._download(url);
    }

    /**
     * <summary>
     *   Reinitialize the filesystem to its clean, unfragmented, empty state.
     * <para>
     *   All files previously uploaded are permanently lost.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> format_fs()
    {
        byte[] json;
        string res;
        json = await this.sendCommand("format");
        res = this.imm_json_get_key(json, "res");
        if (!(res == "ok")) { this._throw( YAPI.IO_ERROR, "format failed"); return YAPI.IO_ERROR; }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a list of YFileRecord objects that describe files currently loaded
     *   in the filesystem.
     * <para>
     * </para>
     * </summary>
     * <param name="pattern">
     *   an optional filter pattern, using star and question marks
     *   as wildcards. When an empty pattern is provided, all file records
     *   are returned.
     * </param>
     * <returns>
     *   a list of <c>YFileRecord</c> objects, containing the file path
     *   and name, byte size and 32-bit CRC of the file content.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list.
     * </para>
     */
    public virtual async Task<List<YFileRecord>> get_list(string pattern)
    {
        byte[] json;
        List<string> filelist = new List<string>();
        List<YFileRecord> res = new List<YFileRecord>();
        json = await this.sendCommand("dir&f="+pattern);
        filelist = this.imm_json_get_array(json);
        res.Clear();
        for (int ii = 0; ii < filelist.Count; ii++) {
            res.Add(new YFileRecord(filelist[ii]));
        }
        return res;
    }

    /**
     * <summary>
     *   Test if a file exist on the filesystem of the module.
     * <para>
     * </para>
     * </summary>
     * <param name="filename">
     *   the file name to test.
     * </param>
     * <returns>
     *   a true if the file existe, false ortherwise.
     * </returns>
     * <para>
     *   On failure, throws an exception.
     * </para>
     */
    public virtual async Task<bool> fileExist(string filename)
    {
        byte[] json;
        List<string> filelist = new List<string>();
        if ((filename).Length == 0) {
            return false;
        }
        json = await this.sendCommand("dir&f="+filename);
        filelist = this.imm_json_get_array(json);
        if (filelist.Count > 0) {
            return true;
        }
        return false;
    }

    /**
     * <summary>
     *   Downloads the requested file and returns a binary buffer with its content.
     * <para>
     * </para>
     * </summary>
     * <param name="pathname">
     *   path and name of the file to download
     * </param>
     * <returns>
     *   a binary buffer with the file content
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty content.
     * </para>
     */
    public virtual async Task<byte[]> download(string pathname)
    {
        return await this._download(pathname);
    }

    /**
     * <summary>
     *   Uploads a file to the filesystem, to the specified full path name.
     * <para>
     *   If a file already exists with the same path name, its content is overwritten.
     * </para>
     * </summary>
     * <param name="pathname">
     *   path and name of the new file to create
     * </param>
     * <param name="content">
     *   binary buffer with the content to set
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> upload(string pathname,byte[] content)
    {
        return await this._upload(pathname, content);
    }

    /**
     * <summary>
     *   Deletes a file, given by its full path name, from the filesystem.
     * <para>
     *   Because of filesystem fragmentation, deleting a file may not always
     *   free up the whole space used by the file. However, rewriting a file
     *   with the same path name will always reuse any space not freed previously.
     *   If you need to ensure that no space is taken by previously deleted files,
     *   you can use <c>format_fs</c> to fully reinitialize the filesystem.
     * </para>
     * </summary>
     * <param name="pathname">
     *   path and name of the file to remove.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> remove(string pathname)
    {
        byte[] json;
        string res;
        json = await this.sendCommand("del&f="+pathname);
        res  = this.imm_json_get_key(json, "res");
        if (!(res == "ok")) { this._throw( YAPI.IO_ERROR, "unable to remove file"); return YAPI.IO_ERROR; }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of filesystems started using <c>yFirstFiles()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YFiles</c> object, corresponding to
     *   a filesystem currently online, or a <c>null</c> pointer
     *   if there are no more filesystems to enumerate.
     * </returns>
     */
    public YFiles nextFiles()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindFilesInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of filesystems currently accessible.
     * <para>
     *   Use the method <c>YFiles.nextFiles()</c> to iterate on
     *   next filesystems.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YFiles</c> object, corresponding to
     *   the first filesystem currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YFiles FirstFiles()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Files");
        if (next_hwid == null)  return null;
        return FindFilesInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of filesystems currently accessible.
     * <para>
     *   Use the method <c>YFiles.nextFiles()</c> to iterate on
     *   next filesystems.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YFiles</c> object, corresponding to
     *   the first filesystem currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YFiles FirstFilesInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Files");
        if (next_hwid == null)  return null;
        return FindFilesInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YFiles implementation)
    }

}