/*********************************************************************
 *
 * $Id: YSensor.cs 29466 2017-12-20 08:11:49Z mvuilleu $
 *
 * Implements yFindSensor(), the high-level API for Sensor functions
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
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI {


    //--- (generated code: YSensor class start)
/**
 * <summary>
 *   YSensor Class: Sensor function interface
 * <para>
 *   The YSensor class is the parent class for all Yoctopuce sensors. It can be
 *   used to read the current value and unit of any sensor, read the min/max
 *   value, configure autonomous recording frequency and access recorded data.
 *   It also provide a function to register a callback invoked each time the
 *   observed value changes, or at a predefined interval. Using this class rather
 *   than a specific subclass makes it possible to create generic applications
 *   that work with any Yoctopuce sensor, even those that do not yet exist.
 *   Note: The YAnButton class is the only analog input which does not inherit
 *   from YSensor.
 * </para>
 * </summary>
 */
public class YSensor : YFunction
{
//--- (end of generated code: YSensor class start)

        //--- (generated code: YSensor definitions)
    /**
     * <summary>
     *   invalid unit value
     * </summary>
     */
    public const  string UNIT_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid currentValue value
     * </summary>
     */
    public const  double CURRENTVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid lowestValue value
     * </summary>
     */
    public const  double LOWESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid highestValue value
     * </summary>
     */
    public const  double HIGHESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid currentRawValue value
     * </summary>
     */
    public const  double CURRENTRAWVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid logFrequency value
     * </summary>
     */
    public const  string LOGFREQUENCY_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid reportFrequency value
     * </summary>
     */
    public const  string REPORTFREQUENCY_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid advMode value
     * </summary>
     */
    public const int ADVMODE_IMMEDIATE = 0;
    public const int ADVMODE_PERIOD_AVG = 1;
    public const int ADVMODE_PERIOD_MIN = 2;
    public const int ADVMODE_PERIOD_MAX = 3;
    public const int ADVMODE_INVALID = -1;
    /**
     * <summary>
     *   invalid calibrationParam value
     * </summary>
     */
    public const  string CALIBRATIONPARAM_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid resolution value
     * </summary>
     */
    public const  double RESOLUTION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid sensorState value
     * </summary>
     */
    public const  int SENSORSTATE_INVALID = YAPI.INVALID_INT;
    protected string _unit = UNIT_INVALID;
    protected double _currentValue = CURRENTVALUE_INVALID;
    protected double _lowestValue = LOWESTVALUE_INVALID;
    protected double _highestValue = HIGHESTVALUE_INVALID;
    protected double _currentRawValue = CURRENTRAWVALUE_INVALID;
    protected string _logFrequency = LOGFREQUENCY_INVALID;
    protected string _reportFrequency = REPORTFREQUENCY_INVALID;
    protected int _advMode = ADVMODE_INVALID;
    protected string _calibrationParam = CALIBRATIONPARAM_INVALID;
    protected double _resolution = RESOLUTION_INVALID;
    protected int _sensorState = SENSORSTATE_INVALID;
    protected ValueCallback _valueCallbackSensor = null;
    protected TimedReportCallback _timedReportCallbackSensor = null;
    protected double _prevTimedReport = 0;
    protected double _iresol = 0;
    protected double _offset = 0;
    protected double _scale = 0;
    protected double _decexp = 0;
    protected bool _isScal;
    protected bool _isScal32;
    protected int _caltyp = 0;
    protected List<int> _calpar = new List<int>();
    protected List<double> _calraw = new List<double>();
    protected List<double> _calref = new List<double>();
    protected YAPI.CalibrationHandler imm_calhdl = null;

    public new delegate Task ValueCallback(YSensor func, string value);
    public new delegate Task TimedReportCallback(YSensor func, YMeasure measure);
    //--- (end of generated code: YSensor definitions)


        /*
         * Method used to encode calibration points into fixed-point 16-bit integers
         */
        internal virtual string _encodeCalibrationPoints(List<double?> rawValues, List<double?> refValues, string actualCparams) {
            int npt = (rawValues.Count < refValues.Count ? rawValues.Count : refValues.Count);
            int rawVal, refVal;
            int calibType;
            int minRaw = 0;
            string res;

            if (npt == 0) {
                return "0";
            }
            int pos = actualCparams.IndexOf(',');
            if (actualCparams.Equals("") || actualCparams.Equals("0") || pos >= 0) {
                _throw(YAPI.NOT_SUPPORTED, "Device does not support new calibration parameters. Please upgrade your firmware");
                return "0";
            }
            List<int> iCalib = YAPIContext.imm_decodeWords(actualCparams);
            int calibrationOffset = iCalib[0];
            int divisor = iCalib[1];
            if (divisor > 0) {
                calibType = npt;
            }
            else {
                calibType = 10 + npt;
            }
            res = Convert.ToString(calibType);
            if (calibType <= 10) {
                for (int i = 0; i < npt; i++) {
                    rawVal = (int)(rawValues[i] * divisor - calibrationOffset + .5);
                    if (rawVal >= minRaw && rawVal < 65536) {
                        refVal = (int)(refValues[i] * divisor - calibrationOffset + .5);
                        if (refVal >= 0 && refVal < 65536) {
                            res += string.Format(",{0:D},{1:D}", rawVal, refVal);
                            minRaw = rawVal + 1;
                        }
                    }
                }
            }
            else {
                // 16-bit floating-point decimal encoding
                for (int i = 0; i < npt; i++) {
                    rawVal = (int) YAPIContext.imm_doubleToDecimal(rawValues[i].Value);
                    refVal = (int) YAPIContext.imm_doubleToDecimal(refValues[i].Value);
                    res += string.Format(",{0:D},{1:D}", rawVal, refVal);
                }
            }
            return res;
        }


        /*
         * Method used to decode calibration points from fixed-point 16-bit integers
         */
        internal static int _decodeCalibrationPoints(string calibParams, List<int> intPt, List<double> rawPt, List<double> calPt) {

            intPt.Clear();
            rawPt.Clear();
            calPt.Clear();
            if (calibParams.Equals("") || calibParams.Equals("0")) {
                // old format: no calibration
                return 0;
            }
            if (calibParams.IndexOf(',') != -1) {
                // old format -> device must do the calibration
                return -1;
            }
            // new format
            List<int> iCalib = YAPIContext.imm_decodeWords(calibParams);
            if (iCalib.Count < 2) {
                // bad format
                return -1;
            }
            if (iCalib.Count == 2) {
                // no calibration
                return 0;
            }
            int pos = 0;
            double calibrationOffset = iCalib[pos++];
            double divisor = iCalib[pos++];
            int calibType = iCalib[pos++];
            if (calibType == 0) {
                return 0;
            }
            // parse calibration parameters
            while (pos < iCalib.Count) {
                int ival = iCalib[pos++];
                double fval;
                if (calibType <= 10) {
                    fval = (ival + calibrationOffset) / divisor;
                }
                else {
                    fval = YAPIContext.imm_decimalToDouble(ival);
                }
                intPt.Add(ival);
                if ((intPt.Count & 1) == 1) {
                    rawPt.Add(fval);
                }
                else {
                    calPt.Add(fval);
                }
            }
            if (intPt.Count < 10) {
                return -1;
            }
            return calibType;
        }


        /// <param name="func"> : functionid </param>
        protected internal YSensor(YAPIContext yctx, string func, string classename) : base(yctx, func, classename) {
            //--- (generated code: YSensor attributes initialization)
        //--- (end of generated code: YSensor attributes initialization)
        }

        protected internal YSensor(YAPIContext yctx, string func) : this(yctx, func, "Sensor")
        {}

        protected internal YSensor(string func) : this(YAPI.imm_GetYCtx(), func)
        {}


        //--- (generated code: YSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("unit")) {
            _unit = json_val.getString("unit");
        }
        if (json_val.has("currentValue")) {
            _currentValue = Math.Round(json_val.getDouble("currentValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("lowestValue")) {
            _lowestValue = Math.Round(json_val.getDouble("lowestValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("highestValue")) {
            _highestValue = Math.Round(json_val.getDouble("highestValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("currentRawValue")) {
            _currentRawValue = Math.Round(json_val.getDouble("currentRawValue") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("logFrequency")) {
            _logFrequency = json_val.getString("logFrequency");
        }
        if (json_val.has("reportFrequency")) {
            _reportFrequency = json_val.getString("reportFrequency");
        }
        if (json_val.has("advMode")) {
            _advMode = json_val.getInt("advMode");
        }
        if (json_val.has("calibrationParam")) {
            _calibrationParam = json_val.getString("calibrationParam");
        }
        if (json_val.has("resolution")) {
            _resolution = Math.Round(json_val.getDouble("resolution") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.has("sensorState")) {
            _sensorState = json_val.getInt("sensorState");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the measuring unit for the measure.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the measuring unit for the measure
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.UNIT_INVALID</c>.
     * </para>
     */
    public async Task<string> get_unit()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return UNIT_INVALID;
            }
        }
        res = _unit;
        return res;
    }


    /**
     * <summary>
     *   Returns the current value of the measure, in the specified unit, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current value of the measure, in the specified unit,
     *   as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.CURRENTVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTVALUE_INVALID;
            }
        }
        res = await this._applyCalibration(_currentRawValue);
        if (res == CURRENTVALUE_INVALID) {
            res = _currentValue;
        }
        res = res * _iresol;
        res = Math.Round(res) / _iresol;
        return res;
    }


    /**
     * <summary>
     *   Changes the recorded minimal value observed.
     * <para>
     *   Can be used to reset the value returned
     *   by get_lowestValue().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the recorded minimal value observed
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
    public async Task<int> set_lowestValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("lowestValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the minimal value observed for the measure since the device was started.
     * <para>
     *   Can be reset to an arbitrary value thanks to set_lowestValue().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the minimal value observed for the measure since the device was started
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.LOWESTVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_lowestValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LOWESTVALUE_INVALID;
            }
        }
        res = _lowestValue * _iresol;
        res = Math.Round(res) / _iresol;
        return res;
    }


    /**
     * <summary>
     *   Changes the recorded maximal value observed.
     * <para>
     *   Can be used to reset the value returned
     *   by get_lowestValue().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the recorded maximal value observed
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
    public async Task<int> set_highestValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("highestValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximal value observed for the measure since the device was started.
     * <para>
     *   Can be reset to an arbitrary value thanks to set_highestValue().
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the maximal value observed for the measure since the device was started
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.HIGHESTVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_highestValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HIGHESTVALUE_INVALID;
            }
        }
        res = _highestValue * _iresol;
        res = Math.Round(res) / _iresol;
        return res;
    }


    /**
     * <summary>
     *   Returns the uncalibrated, unrounded raw value returned by the sensor, in the specified unit, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the uncalibrated, unrounded raw value returned by the
     *   sensor, in the specified unit, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.CURRENTRAWVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentRawValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CURRENTRAWVALUE_INVALID;
            }
        }
        res = _currentRawValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the datalogger recording frequency for this function, or "OFF"
     *   when measures are not stored in the data logger flash memory.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the datalogger recording frequency for this function, or "OFF"
     *   when measures are not stored in the data logger flash memory
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.LOGFREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<string> get_logFrequency()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return LOGFREQUENCY_INVALID;
            }
        }
        res = _logFrequency;
        return res;
    }


    /**
     * <summary>
     *   Changes the datalogger recording frequency for this function.
     * <para>
     *   The frequency can be specified as samples per second,
     *   as sample per minute (for instance "15/m") or in samples per
     *   hour (eg. "4/h"). To disable recording for this function, use
     *   the value "OFF".
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the datalogger recording frequency for this function
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
    public async Task<int> set_logFrequency(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("logFrequency",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the timed value notification frequency, or "OFF" if timed
     *   value notifications are disabled for this function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the timed value notification frequency, or "OFF" if timed
     *   value notifications are disabled for this function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.REPORTFREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<string> get_reportFrequency()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REPORTFREQUENCY_INVALID;
            }
        }
        res = _reportFrequency;
        return res;
    }


    /**
     * <summary>
     *   Changes the timed value notification frequency for this function.
     * <para>
     *   The frequency can be specified as samples per second,
     *   as sample per minute (for instance "15/m") or in samples per
     *   hour (eg. "4/h"). To disable timed value notifications for this
     *   function, use the value "OFF".
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the timed value notification frequency for this function
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
    public async Task<int> set_reportFrequency(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("reportFrequency",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the measuring mode used for the advertised value pushed to the parent hub.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YSensor.ADVMODE_IMMEDIATE</c>, <c>YSensor.ADVMODE_PERIOD_AVG</c>,
     *   <c>YSensor.ADVMODE_PERIOD_MIN</c> and <c>YSensor.ADVMODE_PERIOD_MAX</c> corresponding to the
     *   measuring mode used for the advertised value pushed to the parent hub
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.ADVMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_advMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ADVMODE_INVALID;
            }
        }
        res = _advMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the measuring mode used for the advertised value pushed to the parent hub.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YSensor.ADVMODE_IMMEDIATE</c>, <c>YSensor.ADVMODE_PERIOD_AVG</c>,
     *   <c>YSensor.ADVMODE_PERIOD_MIN</c> and <c>YSensor.ADVMODE_PERIOD_MAX</c> corresponding to the
     *   measuring mode used for the advertised value pushed to the parent hub
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
    public async Task<int> set_advMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("advMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_calibrationParam()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALIBRATIONPARAM_INVALID;
            }
        }
        res = _calibrationParam;
        return res;
    }


    public async Task<int> set_calibrationParam(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("calibrationParam",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the resolution of the measured physical values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   when displaying value. It does not change the precision of the measure itself.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the resolution of the measured physical values
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
    public async Task<int> set_resolution(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("resolution",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the resolution of the measured values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   of the measures, which is not always the same as the actual precision of the sensor.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the resolution of the measured values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.RESOLUTION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_resolution()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RESOLUTION_INVALID;
            }
        }
        res = _resolution;
        return res;
    }


    /**
     * <summary>
     *   Returns the sensor health state code, which is zero when there is an up-to-date measure
     *   available or a positive code if the sensor is not able to provide a measure right now.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the sensor health state code, which is zero when there is an up-to-date measure
     *   available or a positive code if the sensor is not able to provide a measure right now
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSensor.SENSORSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_sensorState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SENSORSTATE_INVALID;
            }
        }
        res = _sensorState;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a sensor for a given identifier.
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
     *   This function does not require that the sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSensor.isOnline()</c> to test if the sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the sensor
     * </param>
     * <returns>
     *   a <c>YSensor</c> object allowing you to drive the sensor.
     * </returns>
     */
    public static YSensor FindSensor(string func)
    {
        YSensor obj;
        obj = (YSensor) YFunction._FindFromCache("Sensor", func);
        if (obj == null) {
            obj = new YSensor(func);
            YFunction._AddToCache("Sensor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSensor.isOnline()</c> to test if the sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the sensor
     * </param>
     * <returns>
     *   a <c>YSensor</c> object allowing you to drive the sensor.
     * </returns>
     */
    public static YSensor FindSensorInContext(YAPIContext yctx,string func)
    {
        YSensor obj;
        obj = (YSensor) YFunction._FindFromCacheInContext(yctx,  "Sensor", func);
        if (obj == null) {
            obj = new YSensor(yctx, func);
            YFunction._AddToCache("Sensor",  func, obj);
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
        _valueCallbackSensor = callback;
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
        if (_valueCallbackSensor != null) {
            await _valueCallbackSensor(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public override async Task<int> _parserHelper()
    {
        int position;
        int maxpos;
        List<int> iCalib = new List<int>();
        int iRaw;
        int iRef;
        double fRaw;
        double fRef;
        _caltyp = -1;
        _scale = -1;
        _isScal32 = false;
        _calpar.Clear();
        _calraw.Clear();
        _calref.Clear();
        // Store inverted resolution, to provide better rounding
        if (_resolution > 0) {
            _iresol = Math.Round(1.0 / _resolution);
        } else {
            _iresol = 10000;
            _resolution = 0.0001;
        }
        // Old format: supported when there is no calibration
        if (_calibrationParam == "" || _calibrationParam == "0") {
            _caltyp = 0;
            return 0;
        }
        if ((_calibrationParam).IndexOf(",") >= 0) {
            // Plain text format
            iCalib = YAPIContext.imm_decodeFloats(_calibrationParam);
            _caltyp = ((iCalib[0]) / (1000));
            if (_caltyp > 0) {
                if (_caltyp < YAPI.YOCTO_CALIB_TYPE_OFS) {
                    // Unknown calibration type: calibrated value will be provided by the device
                    _caltyp = -1;
                    return 0;
                }
                imm_calhdl = _yapi.imm_getCalibrationHandler(_caltyp);
                if (!(imm_calhdl != null)) {
                    // Unknown calibration type: calibrated value will be provided by the device
                    _caltyp = -1;
                    return 0;
                }
            }
            // New 32bit text format
            _isScal = true;
            _isScal32 = true;
            _offset = 0;
            _scale = 1000;
            maxpos = iCalib.Count;
            _calpar.Clear();
            position = 1;
            while (position < maxpos) {
                _calpar.Add(iCalib[position]);
                position = position + 1;
            }
            _calraw.Clear();
            _calref.Clear();
            position = 1;
            while (position + 1 < maxpos) {
                fRaw = iCalib[position];
                fRaw = fRaw / 1000.0;
                fRef = iCalib[position + 1];
                fRef = fRef / 1000.0;
                _calraw.Add(fRaw);
                _calref.Add(fRef);
                position = position + 2;
            }
        } else {
            // Recorder-encoded format, including encoding
            iCalib = YAPIContext.imm_decodeWords(_calibrationParam);
            // In case of unknown format, calibrated value will be provided by the device
            if (iCalib.Count < 2) {
                _caltyp = -1;
                return 0;
            }
            // Save variable format (scale for scalar, or decimal exponent)
            _isScal = (iCalib[1] > 0);
            if (_isScal) {
                _offset = iCalib[0];
                if (_offset > 32767) {
                    _offset = _offset - 65536;
                }
                _scale = iCalib[1];
                _decexp = 0;
            } else {
                _offset = 0;
                _scale = 1;
                _decexp = 1.0;
                position = iCalib[0];
                while (position > 0) {
                    _decexp = _decexp * 10;
                    position = position - 1;
                }
            }
            // Shortcut when there is no calibration parameter
            if (iCalib.Count == 2) {
                _caltyp = 0;
                return 0;
            }
            _caltyp = iCalib[2];
            imm_calhdl = _yapi.imm_getCalibrationHandler(_caltyp);
            // parse calibration points
            if (_caltyp <= 10) {
                maxpos = _caltyp;
            } else {
                if (_caltyp <= 20) {
                    maxpos = _caltyp - 10;
                } else {
                    maxpos = 5;
                }
            }
            maxpos = 3 + 2 * maxpos;
            if (maxpos > iCalib.Count) {
                maxpos = iCalib.Count;
            }
            _calpar.Clear();
            _calraw.Clear();
            _calref.Clear();
            position = 3;
            while (position + 1 < maxpos) {
                iRaw = iCalib[position];
                iRef = iCalib[position + 1];
                _calpar.Add(iRaw);
                _calpar.Add(iRef);
                if (_isScal) {
                    fRaw = iRaw;
                    fRaw = (fRaw - _offset) / _scale;
                    fRef = iRef;
                    fRef = (fRef - _offset) / _scale;
                    _calraw.Add(fRaw);
                    _calref.Add(fRef);
                } else {
                    _calraw.Add(YAPIContext.imm_decimalToDouble(iRaw));
                    _calref.Add(YAPIContext.imm_decimalToDouble(iRef));
                }
                position = position + 2;
            }
        }
        return 0;
    }

    /**
     * <summary>
     *   Checks if the sensor is currently able to provide an up-to-date measure.
     * <para>
     *   Returns false if the device is unreachable, or if the sensor does not have
     *   a current measure to transmit. No exception is raised if there is an error
     *   while trying to contact the device hosting $THEFUNCTION$.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>true</c> if the sensor can provide an up-to-date measure, and <c>false</c> otherwise
     * </returns>
     */
    public virtual async Task<bool> isSensorReady()
    {
        if (!(await this.isOnline())) {
            return false;
        }
        if (!(_sensorState == 0)) {
            return false;
        }
        return true;
    }

    /**
     * <summary>
     *   Returns the YDatalogger object of the device hosting the sensor.
     * <para>
     *   This method returns an object of
     *   class YDatalogger that can control global parameters of the data logger. The returned object
     *   should not be freed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an YDataLogger object or null on error.
     * </returns>
     */
    public virtual async Task<YDataLogger> get_dataLogger()
    {
        YDataLogger logger;
        YModule modu;
        string serial;
        string hwid;

        modu = await this.get_module();
        serial = await modu.get_serialNumber();
        if (serial == YAPI.INVALID_STRING) {
            return null;
        }
        hwid = serial + ".dataLogger";
        logger  = YDataLogger.FindDataLogger(hwid);
        return logger;
    }

    /**
     * <summary>
     *   Starts the data logger on the device.
     * <para>
     *   Note that the data logger
     *   will only save the measures on this sensor if the logFrequency
     *   is not set to "OFF".
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     */
    public virtual async Task<int> startDataLogger()
    {
        byte[] res;

        res = await this._download("api/dataLogger/recording?recording=1");
        if (!((res).Length>0)) { this._throw( YAPI.IO_ERROR, "unable to start datalogger"); return YAPI.IO_ERROR; }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Stops the datalogger on the device.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     */
    public virtual async Task<int> stopDataLogger()
    {
        byte[] res;

        res = await this._download("api/dataLogger/recording?recording=0");
        if (!((res).Length>0)) { this._throw( YAPI.IO_ERROR, "unable to stop datalogger"); return YAPI.IO_ERROR; }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a DataSet object holding historical data for this
     *   sensor, for a specified time interval.
     * <para>
     *   The measures will be
     *   retrieved from the data logger, which must have been turned
     *   on at the desired time. See the documentation of the DataSet
     *   class for information on how to get an overview of the
     *   recorded data, and how to load progressively a large set
     *   of measures from the data logger.
     * </para>
     * <para>
     *   This function only works if the device uses a recent firmware,
     *   as DataSet objects are not supported by firmwares older than
     *   version 13000.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="startTime">
     *   the start of the desired measure time interval,
     *   as a Unix timestamp, i.e. the number of seconds since
     *   January 1, 1970 UTC. The special value 0 can be used
     *   to include any meaasure, without initial limit.
     * </param>
     * <param name="endTime">
     *   the end of the desired measure time interval,
     *   as a Unix timestamp, i.e. the number of seconds since
     *   January 1, 1970 UTC. The special value 0 can be used
     *   to include any meaasure, without ending limit.
     * </param>
     * <returns>
     *   an instance of YDataSet, providing access to historical
     *   data. Past measures can be loaded progressively
     *   using methods from the YDataSet object.
     * </returns>
     */
    public virtual async Task<YDataSet> get_recordedData(long startTime,long endTime)
    {
        string funcid;
        string funit;

        funcid = await this.get_functionId();
        funit = await this.get_unit();
        return new YDataSet(this, funcid, funit, startTime, endTime);
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every periodic timed notification.
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
     *   arguments: the function object of which the value has changed, and an YMeasure object describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public virtual async Task<int> registerTimedReportCallback(TimedReportCallback callback)
    {
        YSensor sensor;
        sensor = this;
        if (callback != null) {
            await YFunction._UpdateTimedReportCallbackList(sensor, true);
        } else {
            await YFunction._UpdateTimedReportCallbackList(sensor, false);
        }
        _timedReportCallbackSensor = callback;
        return 0;
    }

    public virtual async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackSensor != null) {
            await _timedReportCallbackSensor(this, value);
        } else {
        }
        return 0;
    }

    /**
     * <summary>
     *   Configures error correction data points, in particular to compensate for
     *   a possible perturbation of the measure caused by an enclosure.
     * <para>
     *   It is possible
     *   to configure up to five correction points. Correction points must be provided
     *   in ascending order, and be in the range of the sensor. The device will automatically
     *   perform a linear interpolation of the error correction between specified
     *   points. Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     *   For more information on advanced capabilities to refine the calibration of
     *   sensors, please contact support@yoctopuce.com.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="rawValues">
     *   array of floating point numbers, corresponding to the raw
     *   values returned by the sensor for the correction points.
     * </param>
     * <param name="refValues">
     *   array of floating point numbers, corresponding to the corrected
     *   values for the correction points.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> calibrateFromPoints(List<double> rawValues,List<double> refValues)
    {
        string rest_val;
        int res;

        rest_val = await this._encodeCalibrationPoints(rawValues, refValues);
        res = await this._setAttr("calibrationParam", rest_val);
        return res;
    }

    /**
     * <summary>
     *   Retrieves error correction data points previously entered using the method
     *   <c>calibrateFromPoints</c>.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="rawValues">
     *   array of floating point numbers, that will be filled by the
     *   function with the raw sensor values for the correction points.
     * </param>
     * <param name="refValues">
     *   array of floating point numbers, that will be filled by the
     *   function with the desired values for the correction points.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadCalibrationPoints(List<double> rawValues,List<double> refValues)
    {
        rawValues.Clear();
        refValues.Clear();
        // Load function parameters if not yet loaded
        if (_scale == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
        }
        if (_caltyp < 0) {
            this._throw(YAPI.NOT_SUPPORTED, "Calibration parameters format mismatch. Please upgrade your library or firmware.");
            return YAPI.NOT_SUPPORTED;
        }
        rawValues.Clear();
        refValues.Clear();
        for (int ii = 0; ii < _calraw.Count; ii++) {
            rawValues.Add(_calraw[ii]);
        }
        for (int ii = 0; ii < _calref.Count; ii++) {
            refValues.Add(_calref[ii]);
        }
        return YAPI.SUCCESS;
    }

    public virtual async Task<string> _encodeCalibrationPoints(List<double> rawValues,List<double> refValues)
    {
        string res;
        int npt;
        int idx;
        int iRaw;
        int iRef;
        npt = rawValues.Count;
        if (npt != refValues.Count) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid calibration parameters (size mismatch)");
            return YAPI.INVALID_STRING;
        }
        // Shortcut when building empty calibration parameters
        if (npt == 0) {
            return "0";
        }
        // Load function parameters if not yet loaded
        if (_scale == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return YAPI.INVALID_STRING;
            }
        }
        // Detect old firmware
        if ((_caltyp < 0) || (_scale < 0)) {
            this._throw(YAPI.NOT_SUPPORTED, "Calibration parameters format mismatch. Please upgrade your library or firmware.");
            return "0";
        }
        if (_isScal32) {
            // 32-bit fixed-point encoding
            res = ""+Convert.ToString(YAPI.YOCTO_CALIB_TYPE_OFS);
            idx = 0;
            while (idx < npt) {
                res = ""+ res+","+YAPIContext.imm_floatToStr( rawValues[idx])+","+YAPIContext.imm_floatToStr(refValues[idx]);
                idx = idx + 1;
            }
        } else {
            if (_isScal) {
                // 16-bit fixed-point encoding
                res = ""+Convert.ToString(npt);
                idx = 0;
                while (idx < npt) {
                    iRaw = (int) Math.Round(rawValues[idx] * _scale + _offset);
                    iRef = (int) Math.Round(refValues[idx] * _scale + _offset);
                    res = ""+ res+","+Convert.ToString( iRaw)+","+Convert.ToString(iRef);
                    idx = idx + 1;
                }
            } else {
                // 16-bit floating-point decimal encoding
                res = ""+Convert.ToString(10 + npt);
                idx = 0;
                while (idx < npt) {
                    iRaw = (int) YAPIContext.imm_doubleToDecimal(rawValues[idx]);
                    iRef = (int) YAPIContext.imm_doubleToDecimal(refValues[idx]);
                    res = ""+ res+","+Convert.ToString( iRaw)+","+Convert.ToString(iRef);
                    idx = idx + 1;
                }
            }
        }
        return res;
    }

    public virtual async Task<double> _applyCalibration(double rawValue)
    {
        if (rawValue == CURRENTVALUE_INVALID) {
            return CURRENTVALUE_INVALID;
        }
        if (_caltyp == 0) {
            return rawValue;
        }
        if (_caltyp < 0) {
            return CURRENTVALUE_INVALID;
        }
        if (!(imm_calhdl != null)) {
            return CURRENTVALUE_INVALID;
        }
        return imm_calhdl(rawValue, _caltyp, _calpar, _calraw, _calref);
    }

    public virtual async Task<YMeasure> _decodeTimedReport(double timestamp,List<int> report)
    {
        int i;
        int byteVal;
        int poww;
        int minRaw;
        int avgRaw;
        int maxRaw;
        int sublen;
        int difRaw;
        double startTime;
        double endTime;
        double minVal;
        double avgVal;
        double maxVal;
        startTime = _prevTimedReport;
        endTime = timestamp;
        _prevTimedReport = endTime;
        if (startTime == 0) {
            startTime = endTime;
        }
        if (report[0] == 2) {
            // 32bit timed report format
            if (report.Count <= 5) {
                // sub-second report, 1-4 bytes
                poww = 1;
                avgRaw = 0;
                byteVal = 0;
                i = 1;
                while (i < report.Count) {
                    byteVal = report[i];
                    avgRaw = avgRaw + poww * byteVal;
                    poww = poww * 0x100;
                    i = i + 1;
                }
                if (((byteVal) & (0x80)) != 0) {
                    avgRaw = avgRaw - poww;
                }
                avgVal = avgRaw / 1000.0;
                if (_caltyp != 0) {
                    if (imm_calhdl != null) {
                        avgVal = imm_calhdl(avgVal, _caltyp, _calpar, _calraw, _calref);
                    }
                }
                minVal = avgVal;
                maxVal = avgVal;
            } else {
                // averaged report: avg,avg-min,max-avg
                sublen = 1 + ((report[1]) & (3));
                poww = 1;
                avgRaw = 0;
                byteVal = 0;
                i = 2;
                while ((sublen > 0) && (i < report.Count)) {
                    byteVal = report[i];
                    avgRaw = avgRaw + poww * byteVal;
                    poww = poww * 0x100;
                    i = i + 1;
                    sublen = sublen - 1;
                }
                if (((byteVal) & (0x80)) != 0) {
                    avgRaw = avgRaw - poww;
                }
                sublen = 1 + ((((report[1]) >> (2))) & (3));
                poww = 1;
                difRaw = 0;
                while ((sublen > 0) && (i < report.Count)) {
                    byteVal = report[i];
                    difRaw = difRaw + poww * byteVal;
                    poww = poww * 0x100;
                    i = i + 1;
                    sublen = sublen - 1;
                }
                minRaw = avgRaw - difRaw;
                sublen = 1 + ((((report[1]) >> (4))) & (3));
                poww = 1;
                difRaw = 0;
                while ((sublen > 0) && (i < report.Count)) {
                    byteVal = report[i];
                    difRaw = difRaw + poww * byteVal;
                    poww = poww * 0x100;
                    i = i + 1;
                    sublen = sublen - 1;
                }
                maxRaw = avgRaw + difRaw;
                avgVal = avgRaw / 1000.0;
                minVal = minRaw / 1000.0;
                maxVal = maxRaw / 1000.0;
                if (_caltyp != 0) {
                    if (imm_calhdl != null) {
                        avgVal = imm_calhdl(avgVal, _caltyp, _calpar, _calraw, _calref);
                        minVal = imm_calhdl(minVal, _caltyp, _calpar, _calraw, _calref);
                        maxVal = imm_calhdl(maxVal, _caltyp, _calpar, _calraw, _calref);
                    }
                }
            }
        } else {
            // 16bit timed report format
            if (report[0] == 0) {
                // sub-second report, 1-4 bytes
                poww = 1;
                avgRaw = 0;
                byteVal = 0;
                i = 1;
                while (i < report.Count) {
                    byteVal = report[i];
                    avgRaw = avgRaw + poww * byteVal;
                    poww = poww * 0x100;
                    i = i + 1;
                }
                if (_isScal) {
                    avgVal = this.imm_decodeVal(avgRaw);
                } else {
                    if (((byteVal) & (0x80)) != 0) {
                        avgRaw = avgRaw - poww;
                    }
                    avgVal = this.imm_decodeAvg(avgRaw);
                }
                minVal = avgVal;
                maxVal = avgVal;
            } else {
                // averaged report 2+4+2 bytes
                minRaw = report[1] + 0x100 * report[2];
                maxRaw = report[3] + 0x100 * report[4];
                avgRaw = report[5] + 0x100 * report[6] + 0x10000 * report[7];
                byteVal = report[8];
                if (((byteVal) & (0x80)) == 0) {
                    avgRaw = avgRaw + 0x1000000 * byteVal;
                } else {
                    avgRaw = avgRaw - 0x1000000 * (0x100 - byteVal);
                }
                minVal = this.imm_decodeVal(minRaw);
                avgVal = this.imm_decodeAvg(avgRaw);
                maxVal = this.imm_decodeVal(maxRaw);
            }
        }
        return new YMeasure(startTime, endTime, minVal, avgVal, maxVal);
    }

    public virtual double imm_decodeVal(int w)
    {
        double val;
        val = w;
        if (_isScal) {
            val = (val - _offset) / _scale;
        } else {
            val = YAPIContext.imm_decimalToDouble(w);
        }
        if (_caltyp != 0) {
            if (imm_calhdl != null) {
                val = imm_calhdl(val, _caltyp, _calpar, _calraw, _calref);
            }
        }
        return val;
    }

    public virtual double imm_decodeAvg(int dw)
    {
        double val;
        val = dw;
        if (_isScal) {
            val = (val / 100 - _offset) / _scale;
        } else {
            val = val / _decexp;
        }
        if (_caltyp != 0) {
            if (imm_calhdl != null) {
                val = imm_calhdl(val, _caltyp, _calpar, _calraw, _calref);
            }
        }
        return val;
    }

    /**
     * <summary>
     *   Continues the enumeration of sensors started using <c>yFirstSensor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSensor</c> object, corresponding to
     *   a sensor currently online, or a <c>null</c> pointer
     *   if there are no more sensors to enumerate.
     * </returns>
     */
    public YSensor nextSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of sensors currently accessible.
     * <para>
     *   Use the method <c>YSensor.nextSensor()</c> to iterate on
     *   next sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSensor</c> object, corresponding to
     *   the first sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSensor FirstSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Sensor");
        if (next_hwid == null)  return null;
        return FindSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of sensors currently accessible.
     * <para>
     *   Use the method <c>YSensor.nextSensor()</c> to iterate on
     *   next sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSensor</c> object, corresponding to
     *   the first sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSensor FirstSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Sensor");
        if (next_hwid == null)  return null;
        return FindSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YSensor implementation)
     }

}