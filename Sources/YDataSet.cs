/*********************************************************************
 *
 * $Id: YDataSet.cs 25163 2016-08-11 09:42:13Z seb $
 *
 * Implements yFindDataSet(), the high-level API for DataSet functions
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


namespace com.yoctopuce.YoctoAPI
{


    //--- (generated code: YDataSet class start)
/**
 * <summary>
 *   YDataSet Class: Recorded data sequence
 * <para>
 *   YDataSet objects make it possible to retrieve a set of recorded measures
 *   for a given sensor and a specified time interval. They can be used
 *   to load data points with a progress report. When the YDataSet object is
 *   instantiated by the <c>get_recordedData()</c>  function, no data is
 *   yet loaded from the module. It is only when the <c>loadMore()</c>
 *   method is called over and over than data will be effectively loaded
 *   from the dataLogger.
 * </para>
 * <para>
 *   A preview of available measures is available using the function
 *   <c>get_preview()</c> as soon as <c>loadMore()</c> has been called
 *   once. Measures themselves are available using function <c>get_measures()</c>
 *   when loaded by subsequent calls to <c>loadMore()</c>.
 * </para>
 * <para>
 *   This class can only be used on devices that use a recent firmware,
 *   as YDataSet objects are not supported by firmwares older than version 13000.
 * </para>
 * </summary>
 */
public class YDataSet
{
//--- (end of generated code: YDataSet class start)

        //--- (generated code: YDataSet definitions)
    protected YFunction _parent;
    protected string _hardwareId;
    protected string _functionId;
    protected string _unit;
    protected long _startTime = 0;
    protected long _endTime = 0;
    protected int _progress = 0;
    protected List<int> _calib = new List<int>();
    protected List<YDataStream> _streams = new List<YDataStream>();
    protected YMeasure _summary;
    protected List<YMeasure> _preview = new List<YMeasure>();
    protected List<YMeasure> _measures = new List<YMeasure>();

    //--- (end of generated code: YDataSet definitions)

        // YDataSet constructor, when instantiated directly by a function
        public YDataSet(YFunction parent, string functionId, string unit, long startTime, long endTime)
        {
            _parent = parent;
            _functionId = functionId;
            _unit = unit;
            _startTime = startTime;
            _endTime = endTime;
            _progress = -1;
            _hardwareId = "";
            _summary = new YMeasure();
        }

        // YDataSet constructor for the new datalogger
        public YDataSet(YFunction parent)
        {
            _parent = parent;
            _startTime = 0;
            _endTime = 0;
            _hardwareId = "";
            _summary = new YMeasure();
        }

        // YDataSet parser for stream list
        protected internal virtual async Task<int> _parse(string json_str)
        {
            YJSONObject json;
            YJSONArray jstreams;
            double summaryMinVal = double.MaxValue;
            double summaryMaxVal = double.Epsilon;
            double summaryTotalTime = 0;
            double summaryTotalAvg = 0;
            long streamStartTime;
            long streamEndTime;
            long startTime = 0x7fffffff;
            long endTime = 0;

            json = new YJSONObject(json_str);
            json.Parse();
            _functionId = json.GetString("id");
            _unit = json.GetString("unit");
            if (json.Has("calib")) {
                _calib = YAPIContext.imm_decodeFloats(json.GetString("calib"));
                _calib[0] = _calib[0] / 1000;
            } else {
                _calib = YAPIContext.imm_decodeWords(json.GetString("cal"));
            }
            _streams = new List<YDataStream>();
            _preview = new List<YMeasure>();
            _measures = new List<YMeasure>();
            jstreams = json.GetYJSONArray("streams");
            for (int i = 0; i < jstreams.Length; i++) {
                YDataStream stream = _parent.imm_findDataStream(this, jstreams.GetString(i));
                streamStartTime = await stream.get_startTimeUTC() - await stream.get_dataSamplesIntervalMs() / 1000;
                streamEndTime = await stream.get_startTimeUTC() + await stream.get_duration();
                if (_startTime > 0 && streamEndTime <= _startTime) {
                    // this stream is too early, drop it
                } else if (_endTime > 0 && await stream.get_startTimeUTC() > _endTime) {
                    // this stream is too late, drop it
                } else {
                    _streams.Add(stream);
                    if (startTime > streamStartTime) {
                        startTime = streamStartTime;
                    }
                    if (endTime < streamEndTime) {
                        endTime = streamEndTime;
                    }

                    if (await stream.isClosed() && await stream.get_startTimeUTC() >= _startTime && (_endTime == 0 || streamEndTime <= _endTime)) {
                        if (summaryMinVal > await stream.get_minValue()) {
                            summaryMinVal = await stream.get_minValue();
                        }
                        if (summaryMaxVal < await stream.get_maxValue()) {
                            summaryMaxVal = await stream.get_maxValue();
                        }
                        summaryTotalAvg += await stream.get_averageValue() * await stream.get_duration();
                        summaryTotalTime += await stream.get_duration();

                        YMeasure rec = new YMeasure(await stream.get_startTimeUTC(), streamEndTime, await stream.get_minValue(), await stream.get_averageValue(), await stream.get_maxValue());
                        _preview.Add(rec);
                    }
                }
            }
            if ((_streams.Count > 0) && (summaryTotalTime > 0)) {
                // update time boundaries with actual data
                if (_startTime < startTime) {
                    _startTime = startTime;
                }
                if (_endTime == 0 || _endTime > endTime) {
                    _endTime = endTime;
                }
                _summary = new YMeasure(_startTime, _endTime, summaryMinVal, summaryTotalAvg / summaryTotalTime, summaryMaxVal);
            }
            _progress = 0;
            return await this.get_progress();
        }

        public string imm_get_functionId()
        {
            return _functionId;
        }
        //--- (generated code: YDataSet implementation)
#pragma warning disable 1998

    public virtual List<int> imm_get_calibration()
    {
        return _calib;
    }

    public virtual async Task<int> processMore(int progress,byte[] data)
    {
        YDataStream stream;
        List<List<double>> dataRows = new List<List<double>>();
        string strdata;
        double tim;
        double itv;
        int nCols;
        int minCol;
        int avgCol;
        int maxCol;
        // may throw an exception
        if (progress != _progress) {
            return _progress;
        }
        if (_progress < 0) {
            strdata = YAPI.DefaultEncoding.GetString(data);
            if (strdata == "{}") {
                _parent._throw(YAPI.VERSION_MISMATCH, "device firmware is too old");
                return YAPI.VERSION_MISMATCH;
            }
            return await this._parse(strdata);
        }
        stream = _streams[_progress];
        stream.imm_parseStream(data);
        dataRows = await stream.get_dataRows();
        _progress = _progress + 1;
        if (dataRows.Count == 0) {
            return await this.get_progress();
        }
        tim = (double) await stream.get_startTimeUTC();
        itv = await stream.get_dataSamplesInterval();
        if (tim < itv) {
            tim = itv;
        }
        nCols = dataRows[0].Count;
        minCol = 0;
        if (nCols > 2) {
            avgCol = 1;
        } else {
            avgCol = 0;
        }
        if (nCols > 2) {
            maxCol = 2;
        } else {
            maxCol = 0;
        }
        
        for (int ii = 0; ii < dataRows.Count; ii++) {
            if ((tim >= _startTime) && ((_endTime == 0) || (tim <= _endTime))) {
                _measures.Add(new YMeasure(tim - itv, tim, dataRows[ii][minCol], dataRows[ii][avgCol], dataRows[ii][maxCol]));
            }
            tim = tim + itv;
        }
        
        return await this.get_progress();
    }

    public virtual async Task<List<YDataStream>> get_privateDataStreams()
    {
        return _streams;
    }

    /**
     * <summary>
     *   Returns the unique hardware identifier of the function who performed the measures,
     *   in the form <c>SERIAL.FUNCTIONID</c>.
     * <para>
     *   The unique hardware identifier is composed of the
     *   device serial number and of the hardware identifier of the function
     *   (for example <c>THRMCPL1-123456.temperature1</c>)
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that uniquely identifies the function (ex: <c>THRMCPL1-123456.temperature1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YDataSet.HARDWAREID_INVALID</c>.
     * </para>
     */
    public virtual async Task<string> get_hardwareId()
    {
        YModule mo;
        if (!(_hardwareId == "")) {
            return _hardwareId;
        }
        mo = await _parent.get_module();
        _hardwareId = ""+ await mo.get_serialNumber()+"."+await this.get_functionId();
        return _hardwareId;
    }

    /**
     * <summary>
     *   Returns the hardware identifier of the function that performed the measure,
     *   without reference to the module.
     * <para>
     *   For example <c>temperature1</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that identifies the function (ex: <c>temperature1</c>)
     * </returns>
     */
    public virtual async Task<string> get_functionId()
    {
        return _functionId;
    }

    /**
     * <summary>
     *   Returns the measuring unit for the measured value.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that represents a physical unit.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YDataSet.UNIT_INVALID</c>.
     * </para>
     */
    public virtual async Task<string> get_unit()
    {
        return _unit;
    }

    /**
     * <summary>
     *   Returns the start time of the dataset, relative to the Jan 1, 1970.
     * <para>
     *   When the YDataSet is created, the start time is the value passed
     *   in parameter to the <c>get_dataSet()</c> function. After the
     *   very first call to <c>loadMore()</c>, the start time is updated
     *   to reflect the timestamp of the first measure actually found in the
     *   dataLogger within the specified range.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the beginning of this data
     *   set (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<long> get_startTimeUTC()
    {
        return _startTime;
    }

    /**
     * <summary>
     *   Returns the end time of the dataset, relative to the Jan 1, 1970.
     * <para>
     *   When the YDataSet is created, the end time is the value passed
     *   in parameter to the <c>get_dataSet()</c> function. After the
     *   very first call to <c>loadMore()</c>, the end time is updated
     *   to reflect the timestamp of the last measure actually found in the
     *   dataLogger within the specified range.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the end of this data
     *   set (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<long> get_endTimeUTC()
    {
        return _endTime;
    }

    /**
     * <summary>
     *   Returns the progress of the downloads of the measures from the data logger,
     *   on a scale from 0 to 100.
     * <para>
     *   When the object is instantiated by <c>get_dataSet</c>,
     *   the progress is zero. Each time <c>loadMore()</c> is invoked, the progress
     *   is updated, to reach the value 100 only once all measures have been loaded.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion).
     * </returns>
     */
    public virtual async Task<int> get_progress()
    {
        if (_progress < 0) {
            return 0;
        }
        // index not yet loaded
        if (_progress >= _streams.Count) {
            return 100;
        }
        return ((1 + (1 + _progress) * 98) / ((1 + _streams.Count)));
    }

    /**
     * <summary>
     *   Loads the the next block of measures from the dataLogger, and updates
     *   the progress indicator.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion),
     *   or a negative error code in case of failure.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadMore()
    {
        string url;
        YDataStream stream;
        if (_progress < 0) {
            url = "logger.json?id="+_functionId;
        } else {
            if (_progress >= _streams.Count) {
                return 100;
            } else {
                stream = _streams[_progress];
                url = stream.imm_get_url();
            }
        }
        return await this.processMore(_progress, await _parent._download(url));
    }

    /**
     * <summary>
     *   Returns an YMeasure object which summarizes the whole
     *   DataSet.
     * <para>
     *   In includes the following information:
     *   - the start of a time interval
     *   - the end of a time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   This summary is available as soon as <c>loadMore()</c> has
     *   been called for the first time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an YMeasure object
     * </returns>
     */
    public virtual async Task<YMeasure> get_summary()
    {
        return _summary;
    }

    /**
     * <summary>
     *   Returns a condensed version of the measures that can
     *   retrieved in this YDataSet, as a list of YMeasure
     *   objects.
     * <para>
     *   Each item includes:
     *   - the start of a time interval
     *   - the end of a time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   This preview is available as soon as <c>loadMore()</c> has
     *   been called for the first time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured values during a time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_preview()
    {
        return _preview;
    }

    /**
     * <summary>
     *   Returns the detailed set of measures for the time interval corresponding
     *   to a given condensed measures previously returned by <c>get_preview()</c>.
     * <para>
     *   The result is provided as a list of YMeasure objects.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="measure">
     *   condensed measure from the list previously returned by
     *   <c>get_preview()</c>.
     * </param>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured values during a time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_measuresAt(YMeasure measure)
    {
        long startUtc;
        YDataStream stream;
        List<List<double>> dataRows = new List<List<double>>();
        List<YMeasure> measures = new List<YMeasure>();
        double tim;
        double itv;
        int nCols;
        int minCol;
        int avgCol;
        int maxCol;
        // may throw an exception
        startUtc = (long) Math.Round(measure.get_startTimeUTC());
        stream = null;
        for (int ii = 0; ii < _streams.Count; ii++) {
            if (await _streams[ii].get_startTimeUTC() == startUtc) {
                stream = _streams[ii];
            }
            ;;
        }
        if (stream == null) {
            return measures;
        }
        dataRows = await stream.get_dataRows();
        if (dataRows.Count == 0) {
            return measures;
        }
        tim = (double) await stream.get_startTimeUTC();
        itv = await stream.get_dataSamplesInterval();
        if (tim < itv) {
            tim = itv;
        }
        nCols = dataRows[0].Count;
        minCol = 0;
        if (nCols > 2) {
            avgCol = 1;
        } else {
            avgCol = 0;
        }
        if (nCols > 2) {
            maxCol = 2;
        } else {
            maxCol = 0;
        }
        
        for (int ii = 0; ii < dataRows.Count; ii++) {
            if ((tim >= _startTime) && ((_endTime == 0) || (tim <= _endTime))) {
                measures.Add(new YMeasure(tim - itv, tim, dataRows[ii][minCol], dataRows[ii][avgCol], dataRows[ii][maxCol]));
            }
            tim = tim + itv;;
        }
        return measures;
    }

    /**
     * <summary>
     *   Returns all measured values currently available for this DataSet,
     *   as a list of YMeasure objects.
     * <para>
     *   Each item includes:
     *   - the start of the measure time interval
     *   - the end of the measure time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   Before calling this method, you should call <c>loadMore()</c>
     *   to load data from the device. You may have to call loadMore()
     *   several time until all rows are loaded, but you can start
     *   looking at available data rows before the load is complete.
     * </para>
     * <para>
     *   The oldest measures are always loaded first, and the most
     *   recent measures will be loaded last. As a result, timestamps
     *   are normally sorted in ascending order within the measure table,
     *   unless there was an unexpected adjustment of the datalogger UTC
     *   clock.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured value for a given time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_measures()
    {
        return _measures;
    }

#pragma warning restore 1998
    //--- (end of generated code: YDataSet implementation)

    }


}