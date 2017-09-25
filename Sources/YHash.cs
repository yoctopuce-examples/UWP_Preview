using System.Collections.Generic;

namespace com.yoctopuce.YoctoAPI
{


    public class YHash
    {

        private readonly Dictionary<string, YDevice> _devs = new Dictionary<string, YDevice>(); // hash table of devices, by serial number
        private readonly Dictionary<string, string> _snByName = new Dictionary<string, string>(); // serial number for each device, by name
        private readonly Dictionary<string, YFunctionType> _fnByType = new Dictionary<string, YFunctionType>(2); // functions by type
        private readonly YAPIContext _yctx;

        public YHash(YAPIContext yctx)
        {
            _yctx = yctx;
        }

        internal virtual void imm_reset()
        {
            _fnByType["Module"] = new YFunctionType("Module", _yctx);
        }

        // Reindex a device in YAPI after a name change detected by device refresh
        internal virtual void imm_reindexDevice(YDevice dev)
        {
            string serial = dev.SerialNumber;
            string lname = dev.LogicalName;
            _devs[serial] = dev;

            if (!lname.Equals("")) {
                _snByName[lname] = serial;
            }

            YFunctionType module = _fnByType["Module"];
            YPEntry moduleYPEntry = dev.ModuleYPEntry;
            module.imm_reindexFunction(moduleYPEntry);
            int count = dev.imm_functionCount();
            for (int i = 0; i < count; i++) {
                YPEntry yp = dev.imm_getYPEntry(i);
                string classname = yp.Classname;
                YFunctionType functionType;
                if (_fnByType.ContainsKey(classname)) {
                    functionType = _fnByType[classname];
                } else {
                    functionType = new YFunctionType(classname, _yctx);
                    _fnByType[classname] = functionType;
                }
                functionType.imm_reindexFunction(yp);
            }
        }

        // Return a Device object for a specified URL, serial number or logical
        // device name
        // This function will not cause any network access
        internal virtual YDevice imm_getDevice(string device)
        {
            YDevice dev = null;
            // lookup by serial
            if (_devs.ContainsKey(device)) {
                dev = _devs[device];
            } else {
                // fallback to lookup by logical name
                if (_snByName.ContainsKey(device)) {
                    string serial = _snByName[device];
                    dev = _devs[serial];
                }
            }
            return dev;
        }


        // Remove a device from YAPI after an unplug detected by device refresh
        internal virtual void imm_forgetDevice(string serial)
        {
            YDevice dev = _devs[serial];
            if (dev == null) {
                return;
            }
            string lname = dev.LogicalName;
            _devs.Remove(serial);
            if (_snByName.ContainsKey(lname) && _snByName[lname].Equals(serial)) {
                _snByName.Remove(lname);
            }
            YFunctionType module = _fnByType["Module"];
            module.imm_forgetFunction(serial + ".module");
            int count = dev.imm_functionCount();
            for (int i = 0; i < count; i++) {
                YPEntry yp = dev.imm_getYPEntry(i);
                if (_fnByType.ContainsKey(yp.Classname)) {
                    YFunctionType functionType = _fnByType[yp.Classname];
                    functionType.imm_forgetFunction(yp.HardwareId);
                }
            }
        }

        private YFunctionType imm_getFnByType(string className)
        {
            if (!_fnByType.ContainsKey(className)) {
                _fnByType[className] = new YFunctionType(className, _yctx);
            }
            return _fnByType[className];
        }

        // Find the best known identifier (hardware Id) for a given function
        internal virtual YPEntry imm_resolveFunction(string className, string func)
        {
            if (!YAPI._BaseType.ContainsKey(className)) {
                return imm_getFnByType(className).imm_getYPEntry(func);
            } else {
                // using an abstract baseType
                YPEntry.BaseClass baseType = YAPI._BaseType[className];
                foreach (YFunctionType subClassType in _fnByType.Values) {
                    try {
                        YPEntry yp = subClassType.imm_getYPEntry(func);
                        if (yp.getBaseClass().Equals(baseType)) {
                            return yp;
                        }
                    } catch (YAPI_Exception) {
                    }
                }
            }
            throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "No function of type " + className + " found");
        }


        internal virtual string imm_resolveHwID(string className, string func)
        {
            return imm_resolveFunction(className, func).HardwareId;
        }


        internal virtual string imm_resolveFuncId(string className, string func)
        {
            return imm_resolveFunction(className, func).FuncId;
        }

        internal virtual string imm_resolveSerial(string className, string func)
        {
            return imm_resolveFunction(className, func).Serial;
        }


        // Retrieve a function object by hardware id, updating the indexes on the
        // fly if needed
        internal virtual void imm_setFunction(string className, string func, YFunction yfunc)
        {
            imm_getFnByType(className).imm_setFunction(func, yfunc);
        }

        // Retrieve a function object by hardware id, logicalname, updating the indexes on the
        // fly if needed
        internal virtual YFunction imm_getFunction(string className, string func)
        {
            return imm_getFnByType(className).imm_getFunction(func);
        }

        // Set a function advertised value by hardware id
        internal virtual void imm_setFunctionValue(string hwid, string pubval)
        {
            string classname = YAPIContext.imm_functionClass(hwid);
            YFunctionType fnByType = imm_getFnByType(classname);
            fnByType.imm_setFunctionValue(hwid, pubval);            
        }


        // Find the hardwareId for the first instance of a given function class
        internal virtual string imm_getFirstHardwareId(string className)
        {

            if (!YAPI._BaseType.ContainsKey(className)) {
                YFunctionType ft = imm_getFnByType(className);
                YPEntry yp = ft.imm_getFirstYPEntry();
                if (yp == null) {
                    return null;
                }
                return yp.HardwareId;
            } else {
                // using an abstract baseType
                YPEntry.BaseClass baseType = YAPI._BaseType[className];
                foreach (YFunctionType subClassType in _fnByType.Values) {
                    YPEntry yp = subClassType.imm_getFirstYPEntry();
                    if (yp != null && yp.getBaseClass().Equals(baseType)) {
                        return yp.HardwareId;
                    }
                }
                return null;
            }
        }

        // Find the hardwareId for the next instance of a given function class
        internal virtual string imm_getNextHardwareId(string className, string hwid)
        {
            if (!YAPI._BaseType.ContainsKey(className)) {
                YFunctionType ft = imm_getFnByType(className);
                YPEntry yp = ft.imm_getNextYPEntry(hwid);
                if (yp == null) {
                    return null;
                }
                return yp.HardwareId;
            } else {
                // enumeration of an abstract class
                YPEntry.BaseClass baseType = YAPI._BaseType[className];
                string prevclass = YAPIContext.imm_functionClass(hwid);
                YPEntry res = imm_getFnByType(prevclass).imm_getNextYPEntry(hwid);
                if (res != null) {
                    return res.HardwareId;
                }
                foreach (string altClassName in _fnByType.Keys) {
                    if (!prevclass.Equals("")) {
                        if (!altClassName.Equals(prevclass)) {
                            continue;
                        }
                        prevclass = "";
                        continue;
                    }
                    YFunctionType functionType = _fnByType[altClassName];
                    res = functionType.imm_getFirstYPEntry();
                    if (res != null && res.getBaseClass().Equals(baseType)) {
                        return res.HardwareId;
                    }
                }
                return null;
            }
        }

        internal virtual void imm_reindexYellowPages(Dictionary<string, List<YPEntry>> yellowPages)
        {
            // reindex all Yellow pages
            foreach (string classname in yellowPages.Keys) {
                YFunctionType ftype = imm_getFnByType(classname);
                List<YPEntry> ypEntries = yellowPages[classname];
                foreach (YPEntry yprec in ypEntries) {
                    ftype.imm_reindexFunction(yprec);
                }
            }
        }


        public virtual void imm_clear()
        {
            _devs.Clear();
            _snByName.Clear();
            _fnByType.Clear();
        }
    }

}