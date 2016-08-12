using System;
using System.Text;

namespace com.yoctopuce.YoctoAPI {

    internal class YSSDPCacheEntry {
        private string mSerial;
        private string mUUID;
        private string mURL;
        private DateTime mDetectedTime;
        private long mMaxAgeInMS;
        private bool mRegistered;

        internal YSSDPCacheEntry(string uuid, string URL, long maxAgeInMS) {
            mUUID = uuid;
            mURL = URL;
            mMaxAgeInMS = maxAgeInMS;
            mDetectedTime = DateTime.Now;
            mRegistered = false;

            StringBuilder serial = new StringBuilder(YAPI.YOCTO_SERIAL_LEN);
            int i, j;
            for (i = 0, j = 0; i < 4; i++, j += 2) {
                string ch = uuid.Substring(j, 2);
                serial.Append((char)Convert.ToInt32(ch, 16));
            }
            j++;
            for (; i < 6; i++, j += 2) {
                string ch = uuid.Substring(j, 2);
                serial.Append((char)Convert.ToInt32(ch, 16));
            }
            j++;
            for (; i < 8; i++, j += 2) {
                string ch = uuid.Substring(j, 2);
                serial.Append((char)Convert.ToInt32(ch, 16));
            }
            serial.Append('-');
            i = uuid.IndexOf("-COFF-EE", StringComparison.Ordinal);
            i += 8;
            while (uuid[i] == '0') {
                i++;
            }
            string numPart = uuid.Substring(i);
            for (i = numPart.Length; i < 5; i++) {
                serial.Append('0');
            }
            serial.Append(numPart);
            mSerial = serial.ToString();
        }

        internal virtual string Serial {
            get {
                return mSerial;
            }
            set {
                mSerial = value;
            }
        }


        internal virtual string UUID {
            get {
                return mUUID;
            }
        }

        internal virtual string URL {
            get {
                return mURL;
            }
            set {
                mURL = value;
            }
        }


        internal virtual long MaxAgeInMS {
            get {
                return mMaxAgeInMS;
            }
        }

        internal virtual void resetExpiration(int cacheValidity) {
            mMaxAgeInMS = cacheValidity;
            mDetectedTime = DateTime.Now;
        }

        internal virtual bool hasExpired() {
            DateTime now = DateTime.Now;
            if ((now.Ticks - mDetectedTime.Ticks) > mMaxAgeInMS) {
                return true;
            }
            return false; //To change body of created methods use File | Settings | File Templates.
        }

        internal virtual bool Registered {
            get {
                return mRegistered;
            }
            set {
                mRegistered = value;
            }
        }

    }

}