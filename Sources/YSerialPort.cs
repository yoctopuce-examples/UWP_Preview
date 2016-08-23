/*********************************************************************
 *
 * $Id: YSerialPort.cs 25248 2016-08-22 15:51:04Z seb $
 *
 * Implements FindSerialPort(), the high-level API for SerialPort functions
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
using System.Collections.Generic;
using System.Threading.Tasks;
namespace com.yoctopuce.YoctoAPI
{

//--- (YSerialPort return codes)
//--- (end of YSerialPort return codes)
//--- (YSerialPort class start)
/**
 * <summary>
 *   YSerialPort Class: SerialPort function interface
 * <para>
 *   The SerialPort function interface allows you to fully drive a Yoctopuce
 *   serial port, to send and receive data, and to configure communication
 *   parameters (baud rate, bit count, parity, flow control and protocol).
 *   Note that Yoctopuce serial ports are not exposed as virtual COM ports.
 *   They are meant to be used in the same way as all Yoctopuce devices.
 * </para>
 * </summary>
 */
public class YSerialPort : YFunction
{
//--- (end of YSerialPort class start)
//--- (YSerialPort definitions)
    /**
     * <summary>
     *   invalid rxCount value
     * </summary>
     */
    public const  int RXCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid txCount value
     * </summary>
     */
    public const  int TXCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid errCount value
     * </summary>
     */
    public const  int ERRCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid rxMsgCount value
     * </summary>
     */
    public const  int RXMSGCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid txMsgCount value
     * </summary>
     */
    public const  int TXMSGCOUNT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid lastMsg value
     * </summary>
     */
    public const  string LASTMSG_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid currentJob value
     * </summary>
     */
    public const  string CURRENTJOB_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid startupJob value
     * </summary>
     */
    public const  string STARTUPJOB_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid voltageLevel value
     * </summary>
     */
    public const int VOLTAGELEVEL_OFF = 0;
    public const int VOLTAGELEVEL_TTL3V = 1;
    public const int VOLTAGELEVEL_TTL3VR = 2;
    public const int VOLTAGELEVEL_TTL5V = 3;
    public const int VOLTAGELEVEL_TTL5VR = 4;
    public const int VOLTAGELEVEL_RS232 = 5;
    public const int VOLTAGELEVEL_RS485 = 6;
    public const int VOLTAGELEVEL_INVALID = -1;
    /**
     * <summary>
     *   invalid protocol value
     * </summary>
     */
    public const  string PROTOCOL_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid serialMode value
     * </summary>
     */
    public const  string SERIALMODE_INVALID = YAPI.INVALID_STRING;
    protected int _rxCount = RXCOUNT_INVALID;
    protected int _txCount = TXCOUNT_INVALID;
    protected int _errCount = ERRCOUNT_INVALID;
    protected int _rxMsgCount = RXMSGCOUNT_INVALID;
    protected int _txMsgCount = TXMSGCOUNT_INVALID;
    protected string _lastMsg = LASTMSG_INVALID;
    protected string _currentJob = CURRENTJOB_INVALID;
    protected string _startupJob = STARTUPJOB_INVALID;
    protected string _command = COMMAND_INVALID;
    protected int _voltageLevel = VOLTAGELEVEL_INVALID;
    protected string _protocol = PROTOCOL_INVALID;
    protected string _serialMode = SERIALMODE_INVALID;
    protected ValueCallback _valueCallbackSerialPort = null;
    protected int _rxptr = 0;
    protected byte[] _rxbuff;
    protected int _rxbuffptr = 0;

    public new delegate Task ValueCallback(YSerialPort func, string value);
    public new delegate Task TimedReportCallback(YSerialPort func, YMeasure measure);
    //--- (end of YSerialPort definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSerialPort(YAPIContext ctx, string func)
        : base(ctx, func, "SerialPort")
    {
        //--- (YSerialPort attributes initialization)
        //--- (end of YSerialPort attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSerialPort(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSerialPort implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("rxCount")) {
            _rxCount = json_val.GetInt("rxCount");
        }
        if (json_val.Has("txCount")) {
            _txCount = json_val.GetInt("txCount");
        }
        if (json_val.Has("errCount")) {
            _errCount = json_val.GetInt("errCount");
        }
        if (json_val.Has("rxMsgCount")) {
            _rxMsgCount = json_val.GetInt("rxMsgCount");
        }
        if (json_val.Has("txMsgCount")) {
            _txMsgCount = json_val.GetInt("txMsgCount");
        }
        if (json_val.Has("lastMsg")) {
            _lastMsg = json_val.GetString("lastMsg");
        }
        if (json_val.Has("currentJob")) {
            _currentJob = json_val.GetString("currentJob");
        }
        if (json_val.Has("startupJob")) {
            _startupJob = json_val.GetString("startupJob");
        }
        if (json_val.Has("command")) {
            _command = json_val.GetString("command");
        }
        if (json_val.Has("voltageLevel")) {
            _voltageLevel = json_val.GetInt("voltageLevel");
        }
        if (json_val.Has("protocol")) {
            _protocol = json_val.GetString("protocol");
        }
        if (json_val.Has("serialMode")) {
            _serialMode = json_val.GetString("serialMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the total number of bytes received since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of bytes received since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.RXCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rxCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RXCOUNT_INVALID;
            }
        }
        return _rxCount;
    }


    /**
     * <summary>
     *   Returns the total number of bytes transmitted since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of bytes transmitted since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.TXCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_txCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TXCOUNT_INVALID;
            }
        }
        return _txCount;
    }


    /**
     * <summary>
     *   Returns the total number of communication errors detected since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of communication errors detected since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.ERRCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_errCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ERRCOUNT_INVALID;
            }
        }
        return _errCount;
    }


    /**
     * <summary>
     *   Returns the total number of messages received since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of messages received since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.RXMSGCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rxMsgCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RXMSGCOUNT_INVALID;
            }
        }
        return _rxMsgCount;
    }


    /**
     * <summary>
     *   Returns the total number of messages send since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of messages send since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.TXMSGCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_txMsgCount()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TXMSGCOUNT_INVALID;
            }
        }
        return _txMsgCount;
    }


    /**
     * <summary>
     *   Returns the latest message fully received (for Line, Frame and Modbus protocols).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest message fully received (for Line, Frame and Modbus protocols)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.LASTMSG_INVALID</c>.
     * </para>
     */
    public async Task<string> get_lastMsg()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LASTMSG_INVALID;
            }
        }
        return _lastMsg;
    }


    /**
     * <summary>
     *   Returns the name of the job file currently in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the job file currently in use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.CURRENTJOB_INVALID</c>.
     * </para>
     */
    public async Task<string> get_currentJob()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTJOB_INVALID;
            }
        }
        return _currentJob;
    }


    /**
     * <summary>
     *   Changes the job to use when the device is powered on.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the job to use when the device is powered on
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_currentJob(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("currentJob",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the job file to use when the device is powered on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the job file to use when the device is powered on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.STARTUPJOB_INVALID</c>.
     * </para>
     */
    public async Task<string> get_startupJob()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STARTUPJOB_INVALID;
            }
        }
        return _startupJob;
    }


    /**
     * <summary>
     *   Changes the job to use when the device is powered on.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the job to use when the device is powered on
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_startupJob(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("startupJob",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        return _command;
    }


    public async Task<int> set_command(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("command",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the voltage level used on the serial line.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YSerialPort.VOLTAGELEVEL_OFF</c>, <c>YSerialPort.VOLTAGELEVEL_TTL3V</c>,
     *   <c>YSerialPort.VOLTAGELEVEL_TTL3VR</c>, <c>YSerialPort.VOLTAGELEVEL_TTL5V</c>,
     *   <c>YSerialPort.VOLTAGELEVEL_TTL5VR</c>, <c>YSerialPort.VOLTAGELEVEL_RS232</c> and
     *   <c>YSerialPort.VOLTAGELEVEL_RS485</c> corresponding to the voltage level used on the serial line
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.VOLTAGELEVEL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_voltageLevel()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return VOLTAGELEVEL_INVALID;
            }
        }
        return _voltageLevel;
    }


    /**
     * <summary>
     *   Changes the voltage type used on the serial line.
     * <para>
     *   Valid
     *   values  will depend on the Yoctopuce device model featuring
     *   the serial port feature.  Check your device documentation
     *   to find out which values are valid for that specific model.
     *   Trying to set an invalid value will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YSerialPort.VOLTAGELEVEL_OFF</c>, <c>YSerialPort.VOLTAGELEVEL_TTL3V</c>,
     *   <c>YSerialPort.VOLTAGELEVEL_TTL3VR</c>, <c>YSerialPort.VOLTAGELEVEL_TTL5V</c>,
     *   <c>YSerialPort.VOLTAGELEVEL_TTL5VR</c>, <c>YSerialPort.VOLTAGELEVEL_RS232</c> and
     *   <c>YSerialPort.VOLTAGELEVEL_RS485</c> corresponding to the voltage type used on the serial line
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_voltageLevel(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("voltageLevel",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the type of protocol used over the serial line, as a string.
     * <para>
     *   Possible values are "Line" for ASCII messages separated by CR and/or LF,
     *   "Frame:[timeout]ms" for binary messages separated by a delay time,
     *   "Modbus-ASCII" for MODBUS messages in ASCII mode,
     *   "Modbus-RTU" for MODBUS messages in RTU mode,
     *   "Char" for a continuous ASCII stream or
     *   "Byte" for a continuous binary stream.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the type of protocol used over the serial line, as a string
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.PROTOCOL_INVALID</c>.
     * </para>
     */
    public async Task<string> get_protocol()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PROTOCOL_INVALID;
            }
        }
        return _protocol;
    }


    /**
     * <summary>
     *   Changes the type of protocol used over the serial line.
     * <para>
     *   Possible values are "Line" for ASCII messages separated by CR and/or LF,
     *   "Frame:[timeout]ms" for binary messages separated by a delay time,
     *   "Modbus-ASCII" for MODBUS messages in ASCII mode,
     *   "Modbus-RTU" for MODBUS messages in RTU mode,
     *   "Char" for a continuous ASCII stream or
     *   "Byte" for a continuous binary stream.
     *   The suffix "/[wait]ms" can be added to reduce the transmit rate so that there
     *   is always at lest the specified number of milliseconds between each bytes sent.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the type of protocol used over the serial line
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_protocol(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("protocol",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the serial port communication parameters, as a string such as
     *   "9600,8N1".
     * <para>
     *   The string includes the baud rate, the number of data bits,
     *   the parity, and the number of stop bits. An optional suffix is included
     *   if flow control is active: "CtsRts" for hardware handshake, "XOnXOff"
     *   for logical flow control and "Simplex" for acquiring a shared bus using
     *   the RTS line (as used by some RS485 adapters for instance).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the serial port communication parameters, as a string such as
     *   "9600,8N1"
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSerialPort.SERIALMODE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_serialMode()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SERIALMODE_INVALID;
            }
        }
        return _serialMode;
    }


    /**
     * <summary>
     *   Changes the serial port communication parameters, with a string such as
     *   "9600,8N1".
     * <para>
     *   The string includes the baud rate, the number of data bits,
     *   the parity, and the number of stop bits. An optional suffix can be added
     *   to enable flow control: "CtsRts" for hardware handshake, "XOnXOff"
     *   for logical flow control and "Simplex" for acquiring a shared bus using
     *   the RTS line (as used by some RS485 adapters for instance).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the serial port communication parameters, with a string such as
     *   "9600,8N1"
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_serialMode(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("serialMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a serial port for a given identifier.
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
     *   This function does not require that the serial port is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSerialPort.isOnline()</c> to test if the serial port is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a serial port by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the serial port
     * </param>
     * <returns>
     *   a <c>YSerialPort</c> object allowing you to drive the serial port.
     * </returns>
     */
    public static YSerialPort FindSerialPort(string func)
    {
        YSerialPort obj;
        obj = (YSerialPort) YFunction._FindFromCache("SerialPort", func);
        if (obj == null) {
            obj = new YSerialPort(func);
            YFunction._AddToCache("SerialPort",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a serial port for a given identifier in a YAPI context.
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
     *   This function does not require that the serial port is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSerialPort.isOnline()</c> to test if the serial port is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a serial port by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the serial port
     * </param>
     * <returns>
     *   a <c>YSerialPort</c> object allowing you to drive the serial port.
     * </returns>
     */
    public static YSerialPort FindSerialPortInContext(YAPIContext yctx,string func)
    {
        YSerialPort obj;
        obj = (YSerialPort) YFunction._FindFromCacheInContext(yctx,  "SerialPort", func);
        if (obj == null) {
            obj = new YSerialPort(yctx, func);
            YFunction._AddToCache("SerialPort",  func, obj);
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
        _valueCallbackSerialPort = callback;
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
        if (_valueCallbackSerialPort != null) {
            await _valueCallbackSerialPort(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> sendCommand(string text)
    {
        return await this.set_command(text);
    }

    /**
     * <summary>
     *   Clears the serial port buffer and resets counters to zero.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> reset()
    {
        _rxptr = 0;
        _rxbuffptr = 0;
        _rxbuff = new byte[0];
        // may throw an exception
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Sends a single byte to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="code">
     *   the byte to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeByte(int code)
    {
        return await this.sendCommand("$"+String.Format("{0:X02}",code));
    }

    /**
     * <summary>
     *   Sends an ASCII string to the serial port, as is.
     * <para>
     * </para>
     * </summary>
     * <param name="text">
     *   the text string to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeStr(string text)
    {
        byte[] buff;
        int bufflen;
        int idx;
        int ch;
        buff = YAPI.DefaultEncoding.GetBytes(text);
        bufflen = (buff).Length;
        if (bufflen < 100) {
            ch = 0x20;
            idx = 0;
            while ((idx < bufflen) && (ch != 0)) {
                ch = buff[idx];
                if ((ch >= 0x20) && (ch < 0x7f)) {
                    idx = idx + 1;
                } else {
                    ch = 0;
                }
            }
            if (idx >= bufflen) {
                return await this.sendCommand("+"+text);
            }
        }
        // send string using file upload
        return await this._upload("txdata", buff);
    }

    /**
     * <summary>
     *   Sends a binary buffer to the serial port, as is.
     * <para>
     * </para>
     * </summary>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeBin(byte[] buff)
    {
        return await this._upload("txdata", buff);
    }

    /**
     * <summary>
     *   Sends a byte sequence (provided as a list of bytes) to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="byteList">
     *   a list of byte codes
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeArray(List<int> byteList)
    {
        byte[] buff;
        int bufflen;
        int idx;
        int hexb;
        int res;
        bufflen = byteList.Count;
        buff = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            hexb = byteList[idx];
            buff[idx] = (byte)(hexb & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = await this._upload("txdata", buff);
        return res;
    }

    /**
     * <summary>
     *   Sends a byte sequence (provided as a hexadecimal string) to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="hexString">
     *   a string of hexadecimal byte codes
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeHex(string hexString)
    {
        byte[] buff;
        int bufflen;
        int idx;
        int hexb;
        int res;
        bufflen = (hexString).Length;
        if (bufflen < 100) {
            return await this.sendCommand("$"+hexString);
        }
        bufflen = ((bufflen) >> (1));
        buff = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            hexb = Convert.ToInt32((hexString).Substring( 2 * idx, 2), 16);
            buff[idx] = (byte)(hexb & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = await this._upload("txdata", buff);
        return res;
    }

    /**
     * <summary>
     *   Sends an ASCII string to the serial port, followed by a line break (CR LF).
     * <para>
     * </para>
     * </summary>
     * <param name="text">
     *   the text string to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeLine(string text)
    {
        byte[] buff;
        int bufflen;
        int idx;
        int ch;
        buff = YAPI.DefaultEncoding.GetBytes(""+text+"\r\n");
        bufflen = (buff).Length-2;
        if (bufflen < 100) {
            ch = 0x20;
            idx = 0;
            while ((idx < bufflen) && (ch != 0)) {
                ch = buff[idx];
                if ((ch >= 0x20) && (ch < 0x7f)) {
                    idx = idx + 1;
                } else {
                    ch = 0;
                }
            }
            if (idx >= bufflen) {
                return await this.sendCommand("!"+text);
            }
        }
        // send string using file upload
        return await this._upload("txdata", buff);
    }

    /**
     * <summary>
     *   Reads one byte from the receive buffer, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer,
     *   or if there is no data available yet, the function returns YAPI.NO_MORE_DATA.
     * </para>
     * </summary>
     * <returns>
     *   the next byte
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> readByte()
    {
        int currpos;
        int reqlen;
        byte[] buff;
        int bufflen;
        int mult;
        int endpos;
        int res;
        
        // first check if we have the requested character in the look-ahead buffer
        bufflen = (_rxbuff).Length;
        if ((_rxptr >= _rxbuffptr) && (_rxptr < _rxbuffptr+bufflen)) {
            res = _rxbuff[_rxptr-_rxbuffptr];
            _rxptr = _rxptr + 1;
            return res;
        }
        
        // try to preload more than one byte to speed-up byte-per-byte access
        currpos = _rxptr;
        reqlen = 1024;
        buff = await this.readBin(reqlen);
        bufflen = (buff).Length;
        if (_rxptr == currpos+bufflen) {
            res = buff[0];
            _rxptr = currpos+1;
            _rxbuffptr = currpos;
            _rxbuff = buff;
            return res;
        }
        // mixed bidirectional data, retry with a smaller block
        _rxptr = currpos;
        reqlen = 16;
        buff = await this.readBin(reqlen);
        bufflen = (buff).Length;
        if (_rxptr == currpos+bufflen) {
            res = buff[0];
            _rxptr = currpos+1;
            _rxbuffptr = currpos;
            _rxbuff = buff;
            return res;
        }
        // still mixed, need to process character by character
        _rxptr = currpos;
        
        // may throw an exception
        buff = await this._download("rxdata.bin?pos="+Convert.ToString(_rxptr)+"&len=1");
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        _rxptr = endpos;
        if (bufflen == 0) {
            return YAPI.NO_MORE_DATA;
        }
        res = buff[0];
        return res;
    }

    /**
     * <summary>
     *   Reads data from the receive buffer as a string, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of characters to read
     * </param>
     * <returns>
     *   a string with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<string> readStr(int nChars)
    {
        byte[] buff;
        int bufflen;
        int mult;
        int endpos;
        string res;
        if (nChars > 65535) {
            nChars = 65535;
        }
        // may throw an exception
        buff = await this._download("rxdata.bin?pos="+Convert.ToString( _rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        _rxptr = endpos;
        res = (YAPI.DefaultEncoding.GetString(buff)).Substring( 0, bufflen);
        return res;
    }

    /**
     * <summary>
     *   Reads data from the receive buffer as a binary buffer, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a binary object with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<byte[]> readBin(int nChars)
    {
        byte[] buff;
        int bufflen;
        int mult;
        int endpos;
        int idx;
        byte[] res;
        if (nChars > 65535) {
            nChars = 65535;
        }
        // may throw an exception
        buff = await this._download("rxdata.bin?pos="+Convert.ToString( _rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        _rxptr = endpos;
        res = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            res[idx] = (byte)(buff[idx] & 0xff);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads data from the receive buffer as a list of bytes, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a sequence of bytes with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<List<int>> readArray(int nChars)
    {
        byte[] buff;
        int bufflen;
        int mult;
        int endpos;
        int idx;
        int b;
        List<int> res = new List<int>();
        if (nChars > 65535) {
            nChars = 65535;
        }
        // may throw an exception
        buff = await this._download("rxdata.bin?pos="+Convert.ToString( _rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        _rxptr = endpos;
        res.Clear();
        idx = 0;
        while (idx < bufflen) {
            b = buff[idx];
            res.Add(b);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads data from the receive buffer as a hexadecimal string, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nBytes">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a string with receive buffer contents, encoded in hexadecimal
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<string> readHex(int nBytes)
    {
        byte[] buff;
        int bufflen;
        int mult;
        int endpos;
        int ofs;
        string res;
        if (nBytes > 65535) {
            nBytes = 65535;
        }
        // may throw an exception
        buff = await this._download("rxdata.bin?pos="+Convert.ToString( _rxptr)+"&len="+Convert.ToString(nBytes));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        _rxptr = endpos;
        res = "";
        ofs = 0;
        while (ofs + 3 < bufflen) {
            res = ""+ res+""+String.Format("{0:X02}", buff[ofs])+""+String.Format("{0:X02}", buff[ofs + 1])+""+String.Format("{0:X02}", buff[ofs + 2])+""+String.Format("{0:X02}",buff[ofs + 3]);
            ofs = ofs + 4;
        }
        while (ofs < bufflen) {
            res = ""+ res+""+String.Format("{0:X02}",buff[ofs]);
            ofs = ofs + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads a single line (or message) from the receive buffer, starting at current stream position.
     * <para>
     *   This function is intended to be used when the serial port is configured for a message protocol,
     *   such as 'Line' mode or frame protocols.
     * </para>
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer,
     *   the function returns the oldest available line and moves the stream position just after.
     *   If no new full line is received, the function returns an empty line.
     * </para>
     * </summary>
     * <returns>
     *   a string with a single line of text
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<string> readLine()
    {
        string url;
        byte[] msgbin;
        List<string> msgarr = new List<string>();
        int msglen;
        string res;
        // may throw an exception
        url = "rxmsg.json?pos="+Convert.ToString(_rxptr)+"&len=1&maxw=1";
        msgbin = await this._download(url);
        msgarr = this.imm_json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        _rxptr = YAPIContext.imm_atoi(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(msgarr[0]));
        return res;
    }

    /**
     * <summary>
     *   Searches for incoming messages in the serial port receive buffer matching a given pattern,
     *   starting at current position.
     * <para>
     *   This function will only compare and return printable characters
     *   in the message strings. Binary protocols are handled as hexadecimal strings.
     * </para>
     * <para>
     *   The search returns all messages matching the expression provided as argument in the buffer.
     *   If no matching message is found, the search waits for one up to the specified maximum timeout
     *   (in milliseconds).
     * </para>
     * </summary>
     * <param name="pattern">
     *   a limited regular expression describing the expected message format,
     *   or an empty string if all messages should be returned (no filtering).
     *   When using binary protocols, the format applies to the hexadecimal
     *   representation of the message.
     * </param>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a message if none is found
     *   in the receive buffer.
     * </param>
     * <returns>
     *   an array of strings containing the messages found, if any.
     *   Binary messages are converted to hexadecimal representation.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<string>> readMessages(string pattern,int maxWait)
    {
        string url;
        byte[] msgbin;
        List<string> msgarr = new List<string>();
        int msglen;
        List<string> res = new List<string>();
        int idx;
        // may throw an exception
        url = "rxmsg.json?pos="+Convert.ToString( _rxptr)+"&maxw="+Convert.ToString( maxWait)+"&pat="+pattern;
        msgbin = await this._download(url);
        msgarr = this.imm_json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return res;
        }
        // last element of array is the new position
        msglen = msglen - 1;
        _rxptr = YAPIContext.imm_atoi(msgarr[msglen]);
        idx = 0;
        while (idx < msglen) {
            res.Add(this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(msgarr[idx])));
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the current internal stream position to the specified value.
     * <para>
     *   This function
     *   does not affect the device, it only changes the value stored in the API object
     *   for the next read operations.
     * </para>
     * </summary>
     * <param name="absPos">
     *   the absolute position index for next read operations.
     * </param>
     * <returns>
     *   nothing.
     * </returns>
     */
    public virtual async Task<int> read_seek(int absPos)
    {
        _rxptr = absPos;
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current absolute stream position pointer of the API object.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the absolute position index for next read operations.
     * </returns>
     */
    public virtual async Task<int> read_tell()
    {
        return _rxptr;
    }

    /**
     * <summary>
     *   Returns the number of bytes available to read in the input buffer starting from the
     *   current absolute stream position pointer of the API object.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the number of bytes available to read
     * </returns>
     */
    public virtual async Task<int> read_avail()
    {
        byte[] buff;
        int bufflen;
        int res;
        // may throw an exception
        buff = await this._download("rxcnt.bin?pos="+Convert.ToString(_rxptr));
        bufflen = (buff).Length - 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            bufflen = bufflen - 1;
        }
        res = YAPIContext.imm_atoi((YAPI.DefaultEncoding.GetString(buff)).Substring( 0, bufflen));
        return res;
    }

    /**
     * <summary>
     *   Sends a text line query to the serial port, and reads the reply, if any.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'Line' protocol.
     * </para>
     * </summary>
     * <param name="query">
     *   the line query to send (without CR/LF)
     * </param>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a reply.
     * </param>
     * <returns>
     *   the next text line received after sending the text query, as a string.
     *   Additional lines can be obtained by calling readLine or readMessages.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<string> queryLine(string query,int maxWait)
    {
        string url;
        byte[] msgbin;
        List<string> msgarr = new List<string>();
        int msglen;
        string res;
        // may throw an exception
        url = "rxmsg.json?len=1&maxw="+Convert.ToString( maxWait)+"&cmd=!"+query;
        msgbin = await this._download(url);
        msgarr = this.imm_json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        _rxptr = YAPIContext.imm_atoi(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(msgarr[0]));
        return res;
    }

    /**
     * <summary>
     *   Saves the job definition string (JSON data) into a job file.
     * <para>
     *   The job file can be later enabled using <c>selectJob()</c>.
     * </para>
     * </summary>
     * <param name="jobfile">
     *   name of the job file to save on the device filesystem
     * </param>
     * <param name="jsonDef">
     *   a string containing a JSON definition of the job
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> uploadJob(string jobfile,string jsonDef)
    {
        await this._upload(jobfile, YAPI.DefaultEncoding.GetBytes(jsonDef));
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Load and start processing the specified job file.
     * <para>
     *   The file must have
     *   been previously created using the user interface or uploaded on the
     *   device filesystem using the <c>uploadJob()</c> function.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="jobfile">
     *   name of the job file (on the device filesystem)
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectJob(string jobfile)
    {
        return await this.set_currentJob(jobfile);
    }

    /**
     * <summary>
     *   Manually sets the state of the RTS line.
     * <para>
     *   This function has no effect when
     *   hardware handshake is enabled, as the RTS line is driven automatically.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="val">
     *   1 to turn RTS on, 0 to turn RTS off
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_RTS(int val)
    {
        return await this.sendCommand("R"+Convert.ToString(val));
    }

    /**
     * <summary>
     *   Reads the level of the CTS line.
     * <para>
     *   The CTS line is usually driven by
     *   the RTS signal of the connected serial device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   1 if the CTS line is high, 0 if the CTS line is low.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> get_CTS()
    {
        byte[] buff;
        int res;
        // may throw an exception
        buff = await this._download("cts.txt");
        if (!((buff).Length == 1)) { this._throw( YAPI.IO_ERROR, "invalid CTS reply"); return YAPI.IO_ERROR; }
        res = buff[0] - 48;
        return res;
    }

    /**
     * <summary>
     *   Sends a MODBUS message (provided as a hexadecimal string) to the serial port.
     * <para>
     *   The message must start with the slave address. The MODBUS CRC/LRC is
     *   automatically added by the function. This function does not wait for a reply.
     * </para>
     * </summary>
     * <param name="hexString">
     *   a hexadecimal message string, including device address but no CRC/LRC
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> writeMODBUS(string hexString)
    {
        return await this.sendCommand(":"+hexString);
    }

    /**
     * <summary>
     *   Sends a message to a specified MODBUS slave connected to the serial port, and reads the
     *   reply, if any.
     * <para>
     *   The message is the PDU, provided as a vector of bytes.
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to query
     * </param>
     * <param name="pduBytes">
     *   the message to send (PDU), as a vector of bytes. The first byte of the
     *   PDU is the MODBUS function code.
     * </param>
     * <returns>
     *   the received reply, as a vector of bytes.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array (or a MODBUS error reply).
     * </para>
     */
    public virtual async Task<List<int>> queryMODBUS(int slaveNo,List<int> pduBytes)
    {
        int funCode;
        int nib;
        int i;
        string cmd;
        string url;
        string pat;
        byte[] msgs;
        List<string> reps = new List<string>();
        string rep;
        List<int> res = new List<int>();
        int replen;
        int hexb;
        funCode = pduBytes[0];
        nib = ((funCode) >> (4));
        pat = ""+String.Format("{0:X02}", slaveNo)+"["+String.Format("{0:X}", nib)+""+String.Format("{0:X}", (nib+8))+"]"+String.Format("{0:X}",((funCode) & (15)))+".*";
        cmd = ""+String.Format("{0:X02}", slaveNo)+""+String.Format("{0:X02}",funCode);
        i = 1;
        while (i < pduBytes.Count) {
            cmd = ""+ cmd+""+String.Format("{0:X02}",((pduBytes[i]) & (0xff)));
            i = i + 1;
        }
        // may throw an exception
        url = "rxmsg.json?cmd=:"+ cmd+"&pat=:"+pat;
        msgs = await this._download(url);
        reps = this.imm_json_get_array(msgs);
        if (!(reps.Count > 1)) { this._throw( YAPI.IO_ERROR, "no reply from slave"); return res; }
        if (reps.Count > 1) {
            rep = this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(reps[0]));
            replen = (((rep).Length - 3) >> (1));
            i = 0;
            while (i < replen) {
                hexb = Convert.ToInt32((rep).Substring(2 * i + 3, 2), 16);
                res.Add(hexb);
                i = i + 1;
            }
            if (res[0] != funCode) {
                i = res[1];
                if (!(i > 1)) { this._throw( YAPI.NOT_SUPPORTED, "MODBUS error: unsupported function code"); return res; }
                if (!(i > 2)) { this._throw( YAPI.INVALID_ARGUMENT, "MODBUS error: illegal data address"); return res; }
                if (!(i > 3)) { this._throw( YAPI.INVALID_ARGUMENT, "MODBUS error: illegal data value"); return res; }
                if (!(i > 4)) { this._throw( YAPI.INVALID_ARGUMENT, "MODBUS error: failed to execute function"); return res; }
            }
        }
        return res;
    }

    /**
     * <summary>
     *   Reads one or more contiguous internal bits (or coil status) from a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x01 (Read Coils).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to query
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first bit/coil to read (zero-based)
     * </param>
     * <param name="nBits">
     *   the number of bits/coils to read
     * </param>
     * <returns>
     *   a vector of integers, each corresponding to one bit.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> modbusReadBits(int slaveNo,int pduAddr,int nBits)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        List<int> res = new List<int>();
        int bitpos;
        int idx;
        int val;
        int mask;
        pdu.Add(0x01);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nBits) >> (8)));
        pdu.Add(((nBits) & (0xff)));
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        bitpos = 0;
        idx = 2;
        val = reply[idx];
        mask = 1;
        while (bitpos < nBits) {
            if (((val) & (mask)) == 0) {
                res.Add(0);
            } else {
                res.Add(1);
            }
            bitpos = bitpos + 1;
            if (mask == 0x80) {
                idx = idx + 1;
                val = reply[idx];
                mask = 1;
            } else {
                mask = ((mask) << (1));
            }
        }
        return res;
    }

    /**
     * <summary>
     *   Reads one or more contiguous input bits (or discrete inputs) from a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x02 (Read Discrete Inputs).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to query
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first bit/input to read (zero-based)
     * </param>
     * <param name="nBits">
     *   the number of bits/inputs to read
     * </param>
     * <returns>
     *   a vector of integers, each corresponding to one bit.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> modbusReadInputBits(int slaveNo,int pduAddr,int nBits)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        List<int> res = new List<int>();
        int bitpos;
        int idx;
        int val;
        int mask;
        pdu.Add(0x02);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nBits) >> (8)));
        pdu.Add(((nBits) & (0xff)));
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        bitpos = 0;
        idx = 2;
        val = reply[idx];
        mask = 1;
        while (bitpos < nBits) {
            if (((val) & (mask)) == 0) {
                res.Add(0);
            } else {
                res.Add(1);
            }
            bitpos = bitpos + 1;
            if (mask == 0x80) {
                idx = idx + 1;
                val = reply[idx];
                mask = 1;
            } else {
                mask = ((mask) << (1));
            }
        }
        return res;
    }

    /**
     * <summary>
     *   Reads one or more contiguous internal registers (holding registers) from a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x03 (Read Holding Registers).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to query
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first holding register to read (zero-based)
     * </param>
     * <param name="nWords">
     *   the number of holding registers to read
     * </param>
     * <returns>
     *   a vector of integers, each corresponding to one 16-bit register value.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> modbusReadRegisters(int slaveNo,int pduAddr,int nWords)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        List<int> res = new List<int>();
        int regpos;
        int idx;
        int val;
        pdu.Add(0x03);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nWords) >> (8)));
        pdu.Add(((nWords) & (0xff)));
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        regpos = 0;
        idx = 2;
        while (regpos < nWords) {
            val = ((reply[idx]) << (8));
            idx = idx + 1;
            val = val + reply[idx];
            idx = idx + 1;
            res.Add(val);
            regpos = regpos + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads one or more contiguous input registers (read-only registers) from a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x04 (Read Input Registers).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to query
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first input register to read (zero-based)
     * </param>
     * <param name="nWords">
     *   the number of input registers to read
     * </param>
     * <returns>
     *   a vector of integers, each corresponding to one 16-bit input value.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> modbusReadInputRegisters(int slaveNo,int pduAddr,int nWords)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        List<int> res = new List<int>();
        int regpos;
        int idx;
        int val;
        pdu.Add(0x04);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nWords) >> (8)));
        pdu.Add(((nWords) & (0xff)));
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        regpos = 0;
        idx = 2;
        while (regpos < nWords) {
            val = ((reply[idx]) << (8));
            idx = idx + 1;
            val = val + reply[idx];
            idx = idx + 1;
            res.Add(val);
            regpos = regpos + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets a single internal bit (or coil) on a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x05 (Write Single Coil).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to drive
     * </param>
     * <param name="pduAddr">
     *   the relative address of the bit/coil to set (zero-based)
     * </param>
     * <param name="value">
     *   the value to set (0 for OFF state, non-zero for ON state)
     * </param>
     * <returns>
     *   the number of bits/coils affected on the device (1)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> modbusWriteBit(int slaveNo,int pduAddr,int value)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        int res;
        res = 0;
        if (value != 0) {
            value = 0xff;
        }
        pdu.Add(0x05);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(value);
        pdu.Add(0x00);
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        res = 1;
        return res;
    }

    /**
     * <summary>
     *   Sets several contiguous internal bits (or coils) on a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x0f (Write Multiple Coils).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to drive
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first bit/coil to set (zero-based)
     * </param>
     * <param name="bits">
     *   the vector of bits to be set (one integer per bit)
     * </param>
     * <returns>
     *   the number of bits/coils affected on the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> modbusWriteBits(int slaveNo,int pduAddr,List<int> bits)
    {
        int nBits;
        int nBytes;
        int bitpos;
        int val;
        int mask;
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        int res;
        res = 0;
        nBits = bits.Count;
        nBytes = (((nBits + 7)) >> (3));
        pdu.Add(0x0f);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nBits) >> (8)));
        pdu.Add(((nBits) & (0xff)));
        pdu.Add(nBytes);
        bitpos = 0;
        val = 0;
        mask = 1;
        while (bitpos < nBits) {
            if (bits[bitpos] != 0) {
                val = ((val) | (mask));
            }
            bitpos = bitpos + 1;
            if (mask == 0x80) {
                pdu.Add(val);
                val = 0;
                mask = 1;
            } else {
                mask = ((mask) << (1));
            }
        }
        if (mask != 1) {
            pdu.Add(val);
        }
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        res = ((reply[3]) << (8));
        res = res + reply[4];
        return res;
    }

    /**
     * <summary>
     *   Sets a single internal register (or holding register) on a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x06 (Write Single Register).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to drive
     * </param>
     * <param name="pduAddr">
     *   the relative address of the register to set (zero-based)
     * </param>
     * <param name="value">
     *   the 16 bit value to set
     * </param>
     * <returns>
     *   the number of registers affected on the device (1)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> modbusWriteRegister(int slaveNo,int pduAddr,int value)
    {
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        int res;
        res = 0;
        pdu.Add(0x06);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((value) >> (8)));
        pdu.Add(((value) & (0xff)));
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        res = 1;
        return res;
    }

    /**
     * <summary>
     *   Sets several contiguous internal registers (or holding registers) on a MODBUS serial device.
     * <para>
     *   This method uses the MODBUS function code 0x10 (Write Multiple Registers).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to drive
     * </param>
     * <param name="pduAddr">
     *   the relative address of the first internal register to set (zero-based)
     * </param>
     * <param name="values">
     *   the vector of 16 bit values to set
     * </param>
     * <returns>
     *   the number of registers affected on the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> modbusWriteRegisters(int slaveNo,int pduAddr,List<int> values)
    {
        int nWords;
        int nBytes;
        int regpos;
        int val;
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        int res;
        res = 0;
        nWords = values.Count;
        nBytes = 2 * nWords;
        pdu.Add(0x10);
        pdu.Add(((pduAddr) >> (8)));
        pdu.Add(((pduAddr) & (0xff)));
        pdu.Add(((nWords) >> (8)));
        pdu.Add(((nWords) & (0xff)));
        pdu.Add(nBytes);
        regpos = 0;
        while (regpos < nWords) {
            val = values[regpos];
            pdu.Add(((val) >> (8)));
            pdu.Add(((val) & (0xff)));
            regpos = regpos + 1;
        }
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        res = ((reply[3]) << (8));
        res = res + reply[4];
        return res;
    }

    /**
     * <summary>
     *   Sets several contiguous internal registers (holding registers) on a MODBUS serial device,
     *   then performs a contiguous read of a set of (possibly different) internal registers.
     * <para>
     *   This method uses the MODBUS function code 0x17 (Read/Write Multiple Registers).
     * </para>
     * </summary>
     * <param name="slaveNo">
     *   the address of the slave MODBUS device to drive
     * </param>
     * <param name="pduWriteAddr">
     *   the relative address of the first internal register to set (zero-based)
     * </param>
     * <param name="values">
     *   the vector of 16 bit values to set
     * </param>
     * <param name="pduReadAddr">
     *   the relative address of the first internal register to read (zero-based)
     * </param>
     * <param name="nReadWords">
     *   the number of 16 bit values to read
     * </param>
     * <returns>
     *   a vector of integers, each corresponding to one 16-bit register value read.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<int>> modbusWriteAndReadRegisters(int slaveNo,int pduWriteAddr,List<int> values,int pduReadAddr,int nReadWords)
    {
        int nWriteWords;
        int nBytes;
        int regpos;
        int val;
        int idx;
        List<int> pdu = new List<int>();
        List<int> reply = new List<int>();
        List<int> res = new List<int>();
        nWriteWords = values.Count;
        nBytes = 2 * nWriteWords;
        pdu.Add(0x17);
        pdu.Add(((pduReadAddr) >> (8)));
        pdu.Add(((pduReadAddr) & (0xff)));
        pdu.Add(((nReadWords) >> (8)));
        pdu.Add(((nReadWords) & (0xff)));
        pdu.Add(((pduWriteAddr) >> (8)));
        pdu.Add(((pduWriteAddr) & (0xff)));
        pdu.Add(((nWriteWords) >> (8)));
        pdu.Add(((nWriteWords) & (0xff)));
        pdu.Add(nBytes);
        regpos = 0;
        while (regpos < nWriteWords) {
            val = values[regpos];
            pdu.Add(((val) >> (8)));
            pdu.Add(((val) & (0xff)));
            regpos = regpos + 1;
        }
        // may throw an exception
        reply = await this.queryMODBUS(slaveNo, pdu);
        if (reply.Count == 0) {
            return res;
        }
        if (reply[0] != pdu[0]) {
            return res;
        }
        regpos = 0;
        idx = 2;
        while (regpos < nReadWords) {
            val = ((reply[idx]) << (8));
            idx = idx + 1;
            val = val + reply[idx];
            idx = idx + 1;
            res.Add(val);
            regpos = regpos + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Continues the enumeration of serial ports started using <c>yFirstSerialPort()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSerialPort</c> object, corresponding to
     *   a serial port currently online, or a <c>null</c> pointer
     *   if there are no more serial ports to enumerate.
     * </returns>
     */
    public YSerialPort nextSerialPort()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSerialPortInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of serial ports currently accessible.
     * <para>
     *   Use the method <c>YSerialPort.nextSerialPort()</c> to iterate on
     *   next serial ports.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSerialPort</c> object, corresponding to
     *   the first serial port currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSerialPort FirstSerialPort()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SerialPort");
        if (next_hwid == null)  return null;
        return FindSerialPortInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of serial ports currently accessible.
     * <para>
     *   Use the method <c>YSerialPort.nextSerialPort()</c> to iterate on
     *   next serial ports.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSerialPort</c> object, corresponding to
     *   the first serial port currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSerialPort FirstSerialPortInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SerialPort");
        if (next_hwid == null)  return null;
        return FindSerialPortInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSerialPort implementation)
}
}

