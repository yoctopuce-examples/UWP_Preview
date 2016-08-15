/*********************************************************************
 *
 * $Id: YFunctionType.cs 25191 2016-08-15 12:43:02Z seb $
 *
 * Internal YFunctionType object
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
using System.Collections.Generic;

namespace com.yoctopuce.YoctoAPI
{


    // YFunctionType Class (used internally)
    //
    // Instances of this class stores everything we know about a given type of function:
    // Mapping between function logical names and Hardware ID as discovered on hubs,
    // and existing instances of YFunction (either already connected or simply requested).
    // To keep it simple, this implementation separates completely the name resolution
    // mechanism, implemented using the yellow pages, and the storage and retrieval of
    // existing YFunction instances.
    //
    internal class YFunctionType
    {

        // private attributes, to be used within yocto_api only
        private string _className;
        private readonly YAPIContext _yctx;
        private readonly Dictionary<string, YPEntry> _ypEntries = new Dictionary<string, YPEntry>(); // Yellow page by Hardware Id
        private readonly Dictionary<string, YFunction> _connectedFns = new Dictionary<string, YFunction>(); // functions requested and available, by Hardware Id
        private readonly Dictionary<string, YFunction> _requestedFns = new Dictionary<string, YFunction>(); // functions requested but not yet known, by any type of name
        private readonly Dictionary<string, string> _hwIdByName = new Dictionary<string, string>(); // hash table of function Hardware Id by logical name


        // class used to store module.function (can be serial or logical name)
        public class HWID
        {

            internal readonly string module;
            internal readonly string function;

            public virtual string Module {
                get {
                    return module;
                }
            }

            public virtual string Function {
                get {
                    return function;
                }
            }

            public HWID(string module, string function)
            {
                this.module = module;
                this.function = function;
            }

            public HWID(string full_hwid)
            {
                int pos = full_hwid.IndexOf('.');
                this.module = full_hwid.Substring(0, pos);
                this.function = full_hwid.Substring(pos + 1);
            }


            public override string ToString()
            {
                return module + "." + function;
            }


        }

        public YFunctionType(string classname, YAPIContext yctx)
        {
            _className = classname;
            _yctx = yctx;
        }


        public virtual void imm_reindexFunction(YPEntry yp)
        {
            string oldLogicalName = "";

            string hardwareId = yp.HardwareId;

            if (_ypEntries.ContainsKey(hardwareId)) {
                oldLogicalName = _ypEntries[hardwareId].LogicalName;
            }

            if (!oldLogicalName.Equals("") && !oldLogicalName.Equals(yp.LogicalName)) {
                if (_hwIdByName[oldLogicalName].Equals(hardwareId)) {
                    _hwIdByName.Remove(oldLogicalName);
                }
            }
            if (!yp.LogicalName.Equals("")) {
                _hwIdByName[yp.LogicalName] = hardwareId;
            }
            _ypEntries[yp.HardwareId] = yp;
        }


        // Forget a disconnected function given by HardwareId
        public virtual void imm_forgetFunction(string hwid)
        {
            if (_ypEntries.ContainsKey(hwid)) {
                string currname = _ypEntries[hwid].LogicalName;
                if (!currname.Equals("") && _hwIdByName[currname].Equals(hwid)) {
                    _hwIdByName.Remove(currname);
                }
                _ypEntries.Remove(hwid);
            }
        }

        // Find the exact Hardware Id of the specified function, if currently connected
        // If device is not known as connected, return a clean error
        // This function will not cause any network access
        public virtual HWID imm_resolve(string func)
        {
            // Try to resolve str_func to a known Function instance, if possible, without any device access
            int dotpos = func.IndexOf('.');
            if (dotpos < 0) {
                // First case: func is the logical name of a function
                if (_hwIdByName.ContainsKey(func)) {
                    string hwid_str = _hwIdByName[func];
                    return new HWID(hwid_str);
                }

                // fallback to assuming that func is a logical name or serial number of a module
                // with an implicit function name (like serial.module for instance)
                func += string.Format(".{0}{1}", char.ToLower(_className[0]), _className.Substring(1));
            }

            // Second case: func is in the form: device_id.function_id
            HWID hwid = new HWID(func);
            // quick lookup for a known pure hardware id
            if (_ypEntries.ContainsKey(hwid.ToString())) {
                return hwid;
            }

            if (hwid.module.Length > 0) {

                // either the device id is a logical name, or the function is unknown
                YDevice dev = _yctx._yHash.imm_getDevice(hwid.module);
                if (dev == null) {
                    throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "Device [" + hwid.module + "] not online");
                }
                string serial = dev.SerialNumber;
                hwid = new HWID(serial, hwid.Function);
                if (_ypEntries.ContainsKey(hwid.ToString())) {
                    return hwid;
                }
                // not found neither, may be funcid is a function logicalname
                int nfun = dev.imm_functionCount();
                for (int i = 0; i < nfun; i++) {
                    YPEntry yp = dev.imm_getYPEntry(i);
                    if (yp.LogicalName.Equals(hwid.Function) && _ypEntries.ContainsValue(yp)) {
                        return new HWID(serial, yp.FuncId);
                    }
                }
            } else {
                // only functionId  (ie ".temperature")
                foreach (string hwid_str in _connectedFns.Keys) {
                    HWID tmpid = new HWID(hwid_str);
                    if (tmpid.Function.Equals(hwid.Function)) {
                        return tmpid;
                    }
                }
            }
            throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "No function [" + hwid.function + "] found on device [" + hwid.module + "]");
        }


        // Retrieve a function object by hardware id, updating the indexes on the fly if needed
        public virtual void imm_setFunction(string func, YFunction yfunc)
        {
            HWID hwid;
            try {
                hwid = imm_resolve(func);
                _connectedFns[hwid.ToString()] = yfunc;
            } catch (YAPI_Exception) {
                _requestedFns[func] = yfunc;
            }
        }

        // Retrieve a function object by hardware id, updating the indexes on the fly if needed
        public virtual YFunction imm_getFunction(string func)
        {
            try {
                HWID hwid = imm_resolve(func);
                // the function has been located on a device
                if (_connectedFns.ContainsKey(hwid.ToString())) {
                    return _connectedFns[hwid.ToString()];
                }

                if (_requestedFns.ContainsKey(func)) {
                    YFunction req_fn = _requestedFns[func];
                    _connectedFns[hwid.ToString()] = req_fn;
                    _requestedFns.Remove(func);
                    return req_fn;
                }

            } catch (YAPI_Exception) {
                // the function is still abstract
                if (_requestedFns.ContainsKey(func)) {
                    return _requestedFns[func];
                }
            }
            return null;
        }

        public virtual YPEntry imm_getYPEntry(string func)
        {
            HWID hwid = imm_resolve(func);
            return _ypEntries[hwid.ToString()];
        }


        // Retrieve a function advertised value by hardware id
        public virtual void imm_setFunctionValue(string hwid, string pubval)
        {

            if (!_ypEntries.ContainsKey(hwid)) {
                // device has not been correctly registered
                return;
            }
            YPEntry yp = _ypEntries[hwid];
            if (yp.AdvertisedValue.Equals(pubval)) {
                return;
            }
            yp.AdvertisedValue = pubval;
        }

        // Find the the hardwareId of the first instance of a given function class
        public virtual YPEntry imm_getFirstYPEntry()
        {
            foreach (string key in _ypEntries.Keys) {
                return _ypEntries[key];
            }
            return null;
        }

        // Find the hardwareId for the next instance of a given function class
        public virtual YPEntry imm_getNextYPEntry(string hwid)
        {
            bool found = false;
            foreach (string iter_hwid in _ypEntries.Keys) {
                if (found) {
                    return _ypEntries[iter_hwid];
                }
                if (hwid.Equals(iter_hwid)) {
                    found = true;
                }
            }
            return null; // no more instance found
        }

    }

}