/*********************************************************************
 *
 * $Id: YSnoopingRecord.cs 29015 2017-10-24 16:29:41Z seb $
 *
 * Implements FindSnoopingRecord(), the high-level API for SnoopingRecord functions
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

using System;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    //--- (generated code: YSnoopingRecord return codes)
//--- (end of generated code: YSnoopingRecord return codes)
    //--- (generated code: YSnoopingRecord class start)
/**
 * <summary>
 *   YSnoopingRecord Class: Description of a message intercepted
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSnoopingRecord
{
//--- (end of generated code: YSnoopingRecord class start)
//--- (generated code: YSnoopingRecord definitions)
    protected int _tim = 0;
    protected int _dir = 0;
    protected string _msg;

    //--- (end of generated code: YSnoopingRecord definitions)

    internal YSnoopingRecord(string json_str)
    {
        YJSONObject json = new YJSONObject(json_str);
        json.parse();
        _tim = json.getInt("t");
        string m = json.getString("m");
        _dir = (m[0] == '<' ? 1 : 0);
        _msg = m.Substring(1);
    }

    //--- (generated code: YSnoopingRecord implementation)
#pragma warning disable 1998

    public virtual async Task<int> get_time()
    {
        return _tim;
    }

    public virtual async Task<int> get_direction()
    {
        return _dir;
    }

    public virtual async Task<string> get_message()
    {
        return _msg;
    }

#pragma warning restore 1998
    //--- (end of generated code: YSnoopingRecord implementation)
    }

}