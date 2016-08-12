/*********************************************************************
 *
 * $Id: YFileRecord.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements FindFileRecord(), the high-level API for FileRecord functions
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

using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{


    //--- (generated code: YFileRecord return codes)
//--- (end of generated code: YFileRecord return codes)
    //--- (generated code: YFileRecord class start)
/**
 * <summary>
 *   YFileRecord Class: Description of a file on the device filesystem
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YFileRecord
{
//--- (end of generated code: YFileRecord class start)
//--- (generated code: YFileRecord definitions)
    protected string _name;
    protected int _size = 0;
    protected int _crc = 0;

    //--- (end of generated code: YFileRecord definitions)

        public YFileRecord(string json_str)
        {
            YJSONObject json;
            json = new YJSONObject(json_str);
            json.Parse();
            _name = json.GetString("name");
            _crc = json.GetInt("crc");
            _size = json.GetInt("size");
        }

        //--- (generated code: YFileRecord implementation)
#pragma warning disable 1998

    public virtual async Task<string> get_name()
    {
        return _name;
    }

    public virtual async Task<int> get_size()
    {
        return _size;
    }

    public virtual async Task<int> get_crc()
    {
        return _crc;
    }

#pragma warning restore 1998
    //--- (end of generated code: YFileRecord implementation)
    }
}

