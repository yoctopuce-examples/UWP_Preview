/*********************************************************************
 *
 * $Id: YPEntry.cs 25191 2016-08-15 12:43:02Z seb $
 *
 * Yellow page implementation
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

namespace com.yoctopuce.YoctoAPI {

    internal class YPEntry {
        internal sealed class BaseClass {
            public static readonly BaseClass Function = new BaseClass("Function", InnerEnum.Function, 0);
            public static readonly BaseClass Sensor = new BaseClass("Sensor", InnerEnum.Sensor, 1);

            private static readonly IList<BaseClass> valueList = new List<BaseClass>();

            static BaseClass() {
                valueList.Add(Function);
                valueList.Add(Sensor);
            }

            public enum InnerEnum {
                Function,
                Sensor
            }

            private readonly string nameValue;
            private readonly int ordinalValue;
            private readonly InnerEnum innerEnumValue;
            private static int nextOrdinal = 0;

            internal int _intval = 0;

            internal BaseClass(string name, InnerEnum innerEnum, int intval) {
                _intval = intval;

                nameValue = name;
                ordinalValue = nextOrdinal++;
                innerEnumValue = innerEnum;
            }

            public override string ToString() {
                if (this == Sensor) {
                    return "Sensor";
                }
                else
                    return "Function";
            }

            public static BaseClass forByte(byte bval) {
                return values()[bval];
            }


            public static IList<BaseClass> values() {
                return valueList;
            }

            public InnerEnum InnerEnumValue() {
                return innerEnumValue;
            }

            public int ordinal() {
                return ordinalValue;
            }

            public static BaseClass valueOf(string name) {
                foreach (BaseClass enumInstance in BaseClass.values()) {
                    if (enumInstance.nameValue == name) {
                        return enumInstance;
                    }
                }
                throw new System.ArgumentException(name);
            }
        }

        private readonly string _classname;
        private readonly string _serial;
        private readonly string _funcId;
        private string _logicalName = "";
        private string _advertisedValue = "";
        private int _index = -1;
        private readonly BaseClass _baseclass;

        public YPEntry(YJSONObject json) {
            string hardwareId = json.GetString("hardwareId");
            int pos = hardwareId.IndexOf('.');
            _serial = hardwareId.Substring(0, pos);
            _funcId = hardwareId.Substring(pos + 1);
            _classname = YAPIContext.imm_functionClass(_funcId);
            _logicalName = json.GetString("logicalName");
            _advertisedValue = json.GetString("advertisedValue");
            _index = json.GetInt("index");

            if (json.Has("baseType")) {
                _baseclass = BaseClass.values()[json.GetInt("baseType")];
            }
            else {
                _baseclass = BaseClass.Function;
            }
        }

        public YPEntry(string serial, string functionID, BaseClass baseclass) {
            _serial = serial;
            _funcId = functionID;
            _baseclass = baseclass;
            _classname = YAPIContext.imm_functionClass(_funcId);
        }

        //called from Jni
        public YPEntry(string classname, string serial, string funcId, string logicalName, string advertisedValue, int baseType, int funYdx) {
            _serial = serial;
            _funcId = funcId;
            _logicalName = logicalName;
            _advertisedValue = advertisedValue;
            _baseclass = BaseClass.values()[baseType];
            _index = funYdx;
            _classname = classname;
        }

        public override string ToString() {
            return "YPEntry{" + "_classname='" + _classname + '\'' + ", _serial='" + _serial + '\'' + ", _funcId='" + _funcId + '\'' + ", _logicalName='" + _logicalName + '\'' + ", _advertisedValue='" + _advertisedValue + '\'' + ", _index=" + _index + ", _baseclass=" + _baseclass + '}';
        }

        public virtual string AdvertisedValue {
            get {
                return _advertisedValue;
            }
            set {
                this._advertisedValue = value;
            }
        }


        public virtual string HardwareId {
            get {
                return _serial + "." + _funcId;
            }
        }

        public virtual string Serial {
            get {
                return _serial;
            }
        }

        public virtual string FuncId {
            get {
                return _funcId;
            }
        }

        public virtual int Index {
            get {
                return _index;
            }
            set {
                _index = value;
            }
        }


        public virtual BaseClass getBaseClass() {
            return _baseclass;
        }

        public virtual string BaseType {
            get {
                return _baseclass.ToString();
            }
        }

        public virtual string LogicalName {
            get {
                return _logicalName;
            }
            set {
                this._logicalName = value;
            }
        }


        public virtual string Classname {
            get {
                return _classname;
            }
        }

        // Find the exact Hardware Id of the specified function, if currently connected
        // If device is not known as connected, return a clean error
        // This function will not cause any network access
        public virtual string getFriendlyName(YAPIContext ctx) {
            if (_classname.Equals("Module")) {
                if (_logicalName.Equals("")) {
                    return _serial + ".module";
                }
                else {
                    return _logicalName + ".module";
                }
            }
            else {
                YPEntry moduleYP = ctx._yHash.imm_resolveFunction("Module", _serial);
                string module = moduleYP.getFriendlyName(ctx);
                int pos = module.IndexOf(".", StringComparison.Ordinal);
                module = module.Substring(0, pos);
                if (_logicalName.Equals("")) {
                    return module + "." + _funcId;
                }
                else {
                    return module + "." + _logicalName;
                }
            }
        }

    }

}