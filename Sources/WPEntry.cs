/*********************************************************************
 *
 * $Id: WPEntry.cs 24941 2016-07-01 08:53:57Z seb $
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
namespace com.yoctopuce.YoctoAPI {


    internal class WPEntry {

        private string _logicalName = "";
        private readonly string _productName;
        private readonly int _productId;
        private readonly string _networkUrl;
        private int _beacon;
        private readonly string _serialNumber;

        public WPEntry(YJSONObject json) : base() {
            _serialNumber = json.GetString("serialNumber");
            _logicalName = json.GetString("logicalName");
            _productName = json.GetString("productName");
            _productId = json.GetInt("productId");
            string networkUrl = json.GetString("networkUrl");
            //Remove the /api of the network URL
            _networkUrl = networkUrl.Substring(0, networkUrl.Length - 4);
            _beacon = json.GetInt("beacon");
        }

        public override string ToString() {
            return "WPEntry [serialNumber=" + _serialNumber + ", logicalName=" + _logicalName + ", productName=" + _productName + ", productId=" + _productId + ", networkUrl=" + _networkUrl + ", beacon=" + _beacon + "]";
        }

        // called for Jni
        public WPEntry(string logicalName, string productName, int productId, string networkUrl, int beacon, string serialNumber) {
            _logicalName = logicalName;
            _productName = productName;
            _productId = productId;
            _networkUrl = networkUrl;
            _beacon = beacon;
            _serialNumber = serialNumber;
        }

        public virtual int Beacon {
            get {
                return _beacon;
            }
            set {
                this._beacon = value;
            }
        }


        public virtual string LogicalName {
            set {
                this._logicalName = value;
            }
            get {
                return _logicalName;
            }
        }


        public virtual string NetworkUrl {
            get {
                return _networkUrl;
            }
        }

        public virtual int ProductId {
            get {
                return _productId;
            }
        }

        public virtual string ProductName {
            get {
                return _productName;
            }
        }

        public virtual string SerialNumber {
            get {
                return _serialNumber;
            }
        }
    }

}