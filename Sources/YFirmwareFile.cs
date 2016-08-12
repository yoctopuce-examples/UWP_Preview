using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace com.yoctopuce.YoctoAPI
{

    public class YFirmwareFile
    {
        private const int BYN_REV_V4 = 4;
        private const int BYN_REV_V5 = 5;
        private const int BYN_REV_V6 = 6;
        private const int MAX_ROM_ZONES_PER_FILES = 16;
        private const int MAX_FLASH_ZONES_PER_FILES = 4;
        private const int BYN_HEAD_SIZE_V4 = (96 + 8);
        private const int BYN_HEAD_SIZE_V5 = (96 + 32);
        private const int BYN_HEAD_SIZE_V6 = (96 + 48);
        private const int BYN_MD5_OFS_V6 = (96 + 16);

        private readonly string _path;
        private readonly string _serial;
        private readonly string _pictype;
        private readonly string _product;
        private readonly string _firmware;
        private readonly string _prog_version;
        private readonly int _ROM_nb_zone;
        private readonly int _FLA_nb_zone;
        private readonly int _ROM_total_size;
        private readonly int _FLA_total_size;
        private readonly byte[] _data;
        private int _zone_ofs;

        public virtual byn_zone getBynZone(int zOfs)
        {
            return new byn_zone(this, _data, zOfs);
        }

        public virtual int FirstZoneOfs
        {
            get
            {
                return _zone_ofs;
            }
        }

        public class byn_zone
        {
            private readonly YFirmwareFile outerInstance;


            public const int SIZE = 8;
            public readonly int addr_page;
            public readonly int len;

            public byn_zone(YFirmwareFile outerInstance, byte[] data, int zOfs)
            {
                this.outerInstance = outerInstance;
                addr_page = (data[zOfs + 3] << 24) | (data[zOfs + 3] << 16) | (data[zOfs + 1] << 8) | data[zOfs];
                len = (data[zOfs + 7] << 24) | (data[zOfs + 6] << 16) | (data[zOfs + 5] << 8) | data[zOfs + 4];
            }
        }


        private YFirmwareFile(string path, string serial, string pictype, string product, string firmware, string prog_version, int ROM_nb_zone, int FLA_nb_zone, int ROM_total_size, int FLA_total_size, byte[] data, int zone_ofs)
        {
            _path = path;
            _serial = serial;
            _pictype = pictype;
            _product = product;
            _firmware = firmware;
            _prog_version = prog_version;
            _ROM_nb_zone = ROM_nb_zone;
            _FLA_nb_zone = FLA_nb_zone;
            _ROM_total_size = ROM_total_size;
            _FLA_total_size = FLA_total_size;
            _data = data;
            _zone_ofs = zone_ofs;
        }

        public static YFirmwareFile imm_Parse(string path, byte[] data)
        {

            int ofs;

            if (data[0] != 'B' || data[1] != 'Y' || data[2] != 'N' || data[3] != 0) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Not a firmware file");
            }
            ofs = 4;
            int rev = data[ofs] + (data[ofs + 1] << 8);
            ofs += 2;
            string serial = imm_getString(data, ofs, YAPI.YOCTO_SERIAL_LEN);
            ofs += YAPI.YOCTO_SERIAL_LEN;
            string pictype = imm_getString(data, ofs, 20);
            ofs += 20;
            string product = imm_getString(data, ofs, YAPI.YOCTO_PRODUCTNAME_LEN);
            ofs += YAPI.YOCTO_PRODUCTNAME_LEN;
            string firmware = imm_getString(data, ofs, YAPI.YOCTO_FIRMWARE_LEN);
            ofs += YAPI.YOCTO_FIRMWARE_LEN;
            if (serial.Length >= YAPI.YOCTO_SERIAL_LEN) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Bad serial_buf");
            }
            if (product.Length >= YAPI.YOCTO_PRODUCTNAME_LEN) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Bad product name");
            }
            if (firmware.Length >= YAPI.YOCTO_FIRMWARE_LEN) {
                throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Bad firmware revision");
            }

            int ROM_nb_zone = 0;
            int FLA_nb_zone = 0;
            int ROM_total_size = 0;
            int FLA_total_size = 0;
            string prog_version = "";
            int zone_ofs;
            switch (rev) {
                case BYN_REV_V4:
                    zone_ofs = BYN_HEAD_SIZE_V4;
                    ROM_nb_zone = imm_getInt(data, ofs);
                    ofs += 4;
                    int datasize = imm_getInt(data, ofs);
                    ofs += 4;
                    if (ROM_nb_zone > MAX_ROM_ZONES_PER_FILES) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Too many zones");
                    }
                    if (datasize != data.Length - BYN_HEAD_SIZE_V4) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Incorrect file size");
                    }
                    break;
                case BYN_REV_V5:
                    zone_ofs = BYN_HEAD_SIZE_V5;
                    prog_version = imm_checkProgField(data, ofs, YAPI.YOCTO_FIRMWARE_LEN);
                    ofs += YAPI.YOCTO_FIRMWARE_LEN;
                    ofs += 2; //skip pad
                    ROM_nb_zone = imm_getInt(data, ofs);
                    ofs += 4;
                    datasize = imm_getInt(data, ofs);
                    ofs += 4;
                    if (ROM_nb_zone > MAX_ROM_ZONES_PER_FILES) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Too many zones");
                    }
                    if (datasize != data.Length - BYN_HEAD_SIZE_V5) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Incorrect file size");
                    }
                    break;
                case BYN_REV_V6:
                    zone_ofs = BYN_HEAD_SIZE_V6;
                    int size = data.Length - BYN_MD5_OFS_V6;
                    var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                    IBuffer buff = data.AsBuffer(BYN_MD5_OFS_V6, size);
                    IBuffer messageDigest = alg.HashData(buff);
                    for (uint i = 0; i < 16; i++) {
                        if (data[ofs + i] != messageDigest.GetByte(i)) {
                            throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Invalid checksum");
                        }
                    }

                    ofs += 16;
                    prog_version = imm_checkProgField(data, ofs, YAPI.YOCTO_FIRMWARE_LEN);
                    ofs += YAPI.YOCTO_FIRMWARE_LEN;
                    ROM_nb_zone = data[ofs++];
                    FLA_nb_zone = data[ofs++];
                    ROM_total_size = imm_getInt(data, ofs);
                    ofs += 4;
                    FLA_total_size = imm_getInt(data, ofs);
                    ofs += 4;
                    if (ROM_nb_zone > MAX_ROM_ZONES_PER_FILES) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Too many ROM zones");
                    }
                    if (FLA_nb_zone > MAX_FLASH_ZONES_PER_FILES) {
                        throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "Too many FLASH zones");
                    }
                    break;
                default:
                    throw new YAPI_Exception(YAPI.INVALID_ARGUMENT, "unknown BYN file revision");
            }
            return new YFirmwareFile(path, serial, pictype, product, firmware, prog_version, ROM_nb_zone, FLA_nb_zone, ROM_total_size, FLA_total_size, data, zone_ofs);
        }

        private static string imm_getString(byte[] serial_buf, int ofs, int maxlen)
        {
            int i;
            for (i = 0; i < maxlen && serial_buf[ofs + i] != 0;) {
                i++;
            }
            return YAPI.DefaultEncoding.GetString(serial_buf, ofs, i);
        }

        private static int imm_getInt(byte[] data, int ofs)
        {
            return (data[ofs + 3] << 24) | (data[ofs + 3] << 16) | (data[ofs + 1] << 8) | data[ofs];
        }

        private static string imm_checkProgField(byte[] prog_buf, int ofs, int maxlen)
        {
            string prog_version = imm_getString(prog_buf, ofs, maxlen);
            if (!prog_version.Equals("")) {
                int byn = Convert.ToInt32(prog_version);
                try {
                    int tools = int.Parse(YAPI.YOCTO_API_BUILD_STR);
                    if (byn > tools) {
                        throw new YAPI_Exception(YAPI.VERSION_MISMATCH, "Too recent firmware. Please update the yoctopuce library");
                    }
                } catch (System.FormatException) {
                }
            }
            return prog_version;
        }

        public virtual string Serial
        {
            get
            {
                return _serial;
            }
        }

        public virtual string Pictype
        {
            get
            {
                return _pictype;
            }
        }

        public virtual string Product
        {
            get
            {
                return _product;
            }
        }

        public virtual string FirmwareRelease
        {
            get
            {
                return _firmware;
            }
        }

        public virtual int FirmwareReleaseAsInt
        {
            get
            {
                try {
                    return int.Parse(_firmware);
                } catch (System.FormatException) {
                    return 0;
                }
            }
        }

        public virtual string Prog_version
        {
            get
            {
                return _prog_version;
            }
        }

        public virtual int ROM_nb_zone
        {
            get
            {
                return _ROM_nb_zone;
            }
        }

        public virtual int FLA_nb_zone
        {
            get
            {
                return _FLA_nb_zone;
            }
        }

        public virtual int ROM_total_size
        {
            get
            {
                return _ROM_total_size;
            }
        }

        public virtual int FLA_total_size
        {
            get
            {
                return _FLA_total_size;
            }
        }

        public virtual byte[] Data
        {
            get
            {
                return _data;
            }
        }

        public virtual string Path
        {
            get
            {
                return _path;
            }
        }


    }

}