/*********************************************************************
 *
 * $Id: pic24config.php 25098 2016-07-29 10:24:38Z mvuilleu $
 *
 * Implements FindRefFrame(), the high-level API for RefFrame functions
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

//--- (YRefFrame return codes)
//--- (end of YRefFrame return codes)
//--- (YRefFrame class start)
/**
 * <summary>
 *   YRefFrame Class: Reference frame configuration
 * <para>
 *   This class is used to setup the base orientation of the Yocto-3D, so that
 *   the orientation functions, relative to the earth surface plane, use
 *   the proper reference frame. The class also implements a tridimensional
 *   sensor calibration process, which can compensate for local variations
 *   of standard gravity and improve the precision of the tilt sensors.
 * </para>
 * </summary>
 */
public class YRefFrame : YFunction
{
//--- (end of YRefFrame class start)
//--- (YRefFrame definitions)
    /**
     * <summary>
     *   invalid mountPos value
     * </summary>
     */
    public const  int MOUNTPOS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid bearing value
     * </summary>
     */
    public const  double BEARING_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid calibrationParam value
     * </summary>
     */
    public const  string CALIBRATIONPARAM_INVALID = YAPI.INVALID_STRING;
    public enum MOUNTPOSITION {
        BOTTOM = 0,
        TOP = 1,
        FRONT = 2,
        REAR = 3,
        RIGHT = 4,
        LEFT = 5}

    public enum MOUNTORIENTATION {
        TWELVE = 0,
        THREE = 1,
        SIX = 2,
        NINE = 3}

    protected int _mountPos = MOUNTPOS_INVALID;
    protected double _bearing = BEARING_INVALID;
    protected string _calibrationParam = CALIBRATIONPARAM_INVALID;
    protected ValueCallback _valueCallbackRefFrame = null;
    protected bool _calibV2;
    protected int _calibStage = 0;
    protected string _calibStageHint;
    protected int _calibStageProgress = 0;
    protected int _calibProgress = 0;
    protected string _calibLogMsg;
    protected string _calibSavedParams;
    protected int _calibCount = 0;
    protected int _calibInternalPos = 0;
    protected int _calibPrevTick = 0;
    protected List<int> _calibOrient = new List<int>();
    protected List<double> _calibDataAccX = new List<double>();
    protected List<double> _calibDataAccY = new List<double>();
    protected List<double> _calibDataAccZ = new List<double>();
    protected List<double> _calibDataAcc = new List<double>();
    protected double _calibAccXOfs = 0;
    protected double _calibAccYOfs = 0;
    protected double _calibAccZOfs = 0;
    protected double _calibAccXScale = 0;
    protected double _calibAccYScale = 0;
    protected double _calibAccZScale = 0;

    public new delegate Task ValueCallback(YRefFrame func, string value);
    public new delegate Task TimedReportCallback(YRefFrame func, YMeasure measure);
    //--- (end of YRefFrame definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRefFrame(YAPIContext ctx, string func)
        : base(ctx, func, "RefFrame")
    {
        //--- (YRefFrame attributes initialization)
        //--- (end of YRefFrame attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRefFrame(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YRefFrame implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.Has("mountPos")) {
            _mountPos = json_val.GetInt("mountPos");
        }
        if (json_val.Has("bearing")) {
            _bearing = Math.Round(json_val.GetDouble("bearing") * 1000.0 / 65536.0) / 1000.0;
        }
        if (json_val.Has("calibrationParam")) {
            _calibrationParam = json_val.GetString("calibrationParam");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_mountPos()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MOUNTPOS_INVALID;
            }
        }
        return _mountPos;
    }


    public async Task<int> set_mountPos(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("mountPos",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the reference bearing used by the compass.
     * <para>
     *   The relative bearing
     *   indicated by the compass is the difference between the measured magnetic
     *   heading and the reference bearing indicated here.
     * </para>
     * <para>
     *   For instance, if you setup as reference bearing the value of the earth
     *   magnetic declination, the compass will provide the orientation relative
     *   to the geographic North.
     * </para>
     * <para>
     *   Similarly, when the sensor is not mounted along the standard directions
     *   because it has an additional yaw angle, you can set this angle in the reference
     *   bearing so that the compass provides the expected natural direction.
     * </para>
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the reference bearing used by the compass
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
    public async Task<int> set_bearing(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("bearing",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the reference bearing used by the compass.
     * <para>
     *   The relative bearing
     *   indicated by the compass is the difference between the measured magnetic
     *   heading and the reference bearing indicated here.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the reference bearing used by the compass
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRefFrame.BEARING_INVALID</c>.
     * </para>
     */
    public async Task<double> get_bearing()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BEARING_INVALID;
            }
        }
        return _bearing;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_calibrationParam()
    {
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALIBRATIONPARAM_INVALID;
            }
        }
        return _calibrationParam;
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
     *   Retrieves a reference frame for a given identifier.
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
     *   This function does not require that the reference frame is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRefFrame.isOnline()</c> to test if the reference frame is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a reference frame by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the reference frame
     * </param>
     * <returns>
     *   a <c>YRefFrame</c> object allowing you to drive the reference frame.
     * </returns>
     */
    public static YRefFrame FindRefFrame(string func)
    {
        YRefFrame obj;
        obj = (YRefFrame) YFunction._FindFromCache("RefFrame", func);
        if (obj == null) {
            obj = new YRefFrame(func);
            YFunction._AddToCache("RefFrame",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a reference frame for a given identifier in a YAPI context.
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
     *   This function does not require that the reference frame is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRefFrame.isOnline()</c> to test if the reference frame is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a reference frame by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the reference frame
     * </param>
     * <returns>
     *   a <c>YRefFrame</c> object allowing you to drive the reference frame.
     * </returns>
     */
    public static YRefFrame FindRefFrameInContext(YAPIContext yctx,string func)
    {
        YRefFrame obj;
        obj = (YRefFrame) YFunction._FindFromCacheInContext(yctx,  "RefFrame", func);
        if (obj == null) {
            obj = new YRefFrame(yctx, func);
            YFunction._AddToCache("RefFrame",  func, obj);
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
        _valueCallbackRefFrame = callback;
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
        if (_valueCallbackRefFrame != null) {
            await _valueCallbackRefFrame(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Returns the installation position of the device, as configured
     *   in order to define the reference frame for the compass and the
     *   pitch/roll tilt sensors.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among the <c>YRefFrame.MOUNTPOSITION</c> enumeration
     *   (<c>YRefFrame.MOUNTPOSITION_BOTTOM</c>,   <c>YRefFrame.MOUNTPOSITION_TOP</c>,
     *   <c>YRefFrame.MOUNTPOSITION_FRONT</c>,    <c>YRefFrame.MOUNTPOSITION_RIGHT</c>,
     *   <c>YRefFrame.MOUNTPOSITION_REAR</c>,     <c>YRefFrame.MOUNTPOSITION_LEFT</c>),
     *   corresponding to the installation in a box, on one of the six faces.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<MOUNTPOSITION> get_mountPosition()
    {
        int position;
        position = await this.get_mountPos();
        return (MOUNTPOSITION) ((position) >> (2));
    }

    /**
     * <summary>
     *   Returns the installation orientation of the device, as configured
     *   in order to define the reference frame for the compass and the
     *   pitch/roll tilt sensors.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among the enumeration <c>YRefFrame.MOUNTORIENTATION</c>
     *   (<c>YRefFrame.MOUNTORIENTATION_TWELVE</c>, <c>YRefFrame.MOUNTORIENTATION_THREE</c>,
     *   <c>YRefFrame.MOUNTORIENTATION_SIX</c>,     <c>YRefFrame.MOUNTORIENTATION_NINE</c>)
     *   corresponding to the orientation of the "X" arrow on the device,
     *   as on a clock dial seen from an observer in the center of the box.
     *   On the bottom face, the 12H orientation points to the front, while
     *   on the top face, the 12H orientation points to the rear.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<MOUNTORIENTATION> get_mountOrientation()
    {
        int position;
        position = await this.get_mountPos();
        return (MOUNTORIENTATION) ((position) & (3));
    }

    /**
     * <summary>
     *   Changes the compass and tilt sensor frame of reference.
     * <para>
     *   The magnetic compass
     *   and the tilt sensors (pitch and roll) naturally work in the plane
     *   parallel to the earth surface. In case the device is not installed upright
     *   and horizontally, you must select its reference orientation (parallel to
     *   the earth surface) so that the measures are made relative to this position.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="position">
     *   a value among the <c>YRefFrame.MOUNTPOSITION</c> enumeration
     *   (<c>YRefFrame.MOUNTPOSITION_BOTTOM</c>,   <c>YRefFrame.MOUNTPOSITION_TOP</c>,
     *   <c>YRefFrame.MOUNTPOSITION_FRONT</c>,    <c>YRefFrame.MOUNTPOSITION_RIGHT</c>,
     *   <c>YRefFrame.MOUNTPOSITION_REAR</c>,     <c>YRefFrame.MOUNTPOSITION_LEFT</c>),
     *   corresponding to the installation in a box, on one of the six faces.
     * </param>
     * <param name="orientation">
     *   a value among the enumeration <c>YRefFrame.MOUNTORIENTATION</c>
     *   (<c>YRefFrame.MOUNTORIENTATION_TWELVE</c>, <c>YRefFrame.MOUNTORIENTATION_THREE</c>,
     *   <c>YRefFrame.MOUNTORIENTATION_SIX</c>,     <c>YRefFrame.MOUNTORIENTATION_NINE</c>)
     *   corresponding to the orientation of the "X" arrow on the device,
     *   as on a clock dial seen from an observer in the center of the box.
     *   On the bottom face, the 12H orientation points to the front, while
     *   on the top face, the 12H orientation points to the rear.
     * </param>
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_mountPosition(MOUNTPOSITION position,MOUNTORIENTATION orientation)
    {
        int mixedPos;
        mixedPos = (((int)position) << (2)) + (int)orientation;
        return await this.set_mountPos(mixedPos);
    }

    /**
     * <summary>
     *   Returns the 3D sensor calibration state (Yocto-3D-V2 only).
     * <para>
     *   This function returns
     *   an integer representing the calibration state of the 3 inertial sensors of
     *   the BNO055 chip, found in the Yocto-3D-V2. Hundredths show the calibration state
     *   of the accelerometer, tenths show the calibration state of the magnetometer while
     *   units show the calibration state of the gyroscope. For each sensor, the value 0
     *   means no calibration and the value 3 means full calibration.
     * </para>
     * </summary>
     * <returns>
     *   an integer representing the calibration state of Yocto-3D-V2:
     *   333 when fully calibrated, 0 when not calibrated at all.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     *   For the Yocto-3D (V1), this function always return -3 (unsupported function).
     * </para>
     */
    public virtual async Task<int> get_calibrationState()
    {
        string calibParam;
        List<int> iCalib = new List<int>();
        int caltyp;
        int res;
        // may throw an exception
        calibParam = await this.get_calibrationParam();
        iCalib = YAPIContext.imm_decodeFloats(calibParam);
        caltyp = ((iCalib[0]) / (1000));
        if (caltyp != 33) {
            return YAPI.NOT_SUPPORTED;
        }
        res = ((iCalib[1]) / (1000));
        return res;
    }

    /**
     * <summary>
     *   Returns estimated quality of the orientation (Yocto-3D-V2 only).
     * <para>
     *   This function returns
     *   an integer between 0 and 3 representing the degree of confidence of the position
     *   estimate. When the value is 3, the estimation is reliable. Below 3, one should
     *   expect sudden corrections, in particular for heading (<c>compass</c> function).
     *   The most frequent causes for values below 3 are magnetic interferences, and
     *   accelerations or rotations beyond the sensor range.
     * </para>
     * </summary>
     * <returns>
     *   an integer between 0 and 3 (3 when the measure is reliable)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     *   For the Yocto-3D (V1), this function always return -3 (unsupported function).
     * </para>
     */
    public virtual async Task<int> get_measureQuality()
    {
        string calibParam;
        List<int> iCalib = new List<int>();
        int caltyp;
        int res;
        // may throw an exception
        calibParam = await this.get_calibrationParam();
        iCalib = YAPIContext.imm_decodeFloats(calibParam);
        caltyp = ((iCalib[0]) / (1000));
        if (caltyp != 33) {
            return YAPI.NOT_SUPPORTED;
        }
        res = ((iCalib[2]) / (1000));
        return res;
    }

    public virtual async Task<int> _calibSort(int start,int stopidx)
    {
        int idx;
        int changed;
        double a;
        double b;
        double xa;
        double xb;
        // bubble sort is good since we will re-sort again after offset adjustment
        changed = 1;
        while (changed > 0) {
            changed = 0;
            a = _calibDataAcc[start];
            idx = start + 1;
            while (idx < stopidx) {
                b = _calibDataAcc[idx];
                if (a > b) {
                    _calibDataAcc[idx-1] = b;
                    _calibDataAcc[idx] = a;
                    xa = _calibDataAccX[idx-1];
                    xb = _calibDataAccX[idx];
                    _calibDataAccX[idx-1] = xb;
                    _calibDataAccX[idx] = xa;
                    xa = _calibDataAccY[idx-1];
                    xb = _calibDataAccY[idx];
                    _calibDataAccY[idx-1] = xb;
                    _calibDataAccY[idx] = xa;
                    xa = _calibDataAccZ[idx-1];
                    xb = _calibDataAccZ[idx];
                    _calibDataAccZ[idx-1] = xb;
                    _calibDataAccZ[idx] = xa;
                    changed = changed + 1;
                } else {
                    a = b;
                }
                idx = idx + 1;
            }
        }
        return 0;
    }

    /**
     * <summary>
     *   Initiates the sensors tridimensional calibration process.
     * <para>
     *   This calibration is used at low level for inertial position estimation
     *   and to enhance the precision of the tilt sensors.
     * </para>
     * <para>
     *   After calling this method, the device should be moved according to the
     *   instructions provided by method <c>get_3DCalibrationHint</c>,
     *   and <c>more3DCalibration</c> should be invoked about 5 times per second.
     *   The calibration procedure is completed when the method
     *   <c>get_3DCalibrationProgress</c> returns 100. At this point,
     *   the computed calibration parameters can be applied using method
     *   <c>save3DCalibration</c>. The calibration process can be canceled
     *   at any time using method <c>cancel3DCalibration</c>.
     * </para>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual async Task<int> start3DCalibration()
    {
        if (!(await this.isOnline())) {
            return YAPI.DEVICE_NOT_FOUND;
        }
        if (_calibStage != 0) {
            await this.cancel3DCalibration();
        }
        _calibSavedParams = await this.get_calibrationParam();
        _calibV2 = (YAPIContext.imm_atoi(_calibSavedParams) == 33);
        await this.set_calibrationParam("0");
        _calibCount = 50;
        _calibStage = 1;
        _calibStageHint = "Set down the device on a steady horizontal surface";
        _calibStageProgress = 0;
        _calibProgress = 1;
        _calibInternalPos = 0;
        _calibPrevTick = (int) ((YAPIContext.GetTickCount()) & (0x7FFFFFFF));
        _calibOrient.Clear();
        _calibDataAccX.Clear();
        _calibDataAccY.Clear();
        _calibDataAccZ.Clear();
        _calibDataAcc.Clear();
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the sensors tridimensional calibration process previously
     *   initiated using method <c>start3DCalibration</c>.
     * <para>
     *   This method should be called approximately 5 times per second, while
     *   positioning the device according to the instructions provided by method
     *   <c>get_3DCalibrationHint</c>. Note that the instructions change during
     *   the calibration process.
     * </para>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual async Task<int> more3DCalibration()
    {
        if (_calibV2) {
            return await this.more3DCalibrationV2();
        }
        return await this.more3DCalibrationV1();
    }

    public virtual async Task<int> more3DCalibrationV1()
    {
        int currTick;
        byte[] jsonData;
        double xVal;
        double yVal;
        double zVal;
        double xSq;
        double ySq;
        double zSq;
        double norm;
        int orient;
        int idx;
        int intpos;
        int err;
        // make sure calibration has been started
        if (_calibStage == 0) {
            return YAPI.INVALID_ARGUMENT;
        }
        if (_calibProgress == 100) {
            return YAPI.SUCCESS;
        }
        // make sure we leave at least 160ms between samples
        currTick =  (int) ((YAPIContext.GetTickCount()) & (0x7FFFFFFF));
        if (((currTick - _calibPrevTick) & (0x7FFFFFFF)) < 160) {
            return YAPI.SUCCESS;
        }
        // load current accelerometer values, make sure we are on a straight angle
        // (default timeout to 0,5 sec without reading measure when out of range)
        _calibStageHint = "Set down the device on a steady horizontal surface";
        _calibPrevTick = ((currTick + 500) & (0x7FFFFFFF));
        jsonData = await this._download("api/accelerometer.json");
        xVal = YAPIContext.imm_atoi(this.imm_json_get_key(jsonData, "xValue")) / 65536.0;
        yVal = YAPIContext.imm_atoi(this.imm_json_get_key(jsonData, "yValue")) / 65536.0;
        zVal = YAPIContext.imm_atoi(this.imm_json_get_key(jsonData, "zValue")) / 65536.0;
        xSq = xVal * xVal;
        if (xSq >= 0.04 && xSq < 0.64) {
            return YAPI.SUCCESS;
        }
        if (xSq >= 1.44) {
            return YAPI.SUCCESS;
        }
        ySq = yVal * yVal;
        if (ySq >= 0.04 && ySq < 0.64) {
            return YAPI.SUCCESS;
        }
        if (ySq >= 1.44) {
            return YAPI.SUCCESS;
        }
        zSq = zVal * zVal;
        if (zSq >= 0.04 && zSq < 0.64) {
            return YAPI.SUCCESS;
        }
        if (zSq >= 1.44) {
            return YAPI.SUCCESS;
        }
        norm = Math.Sqrt(xSq + ySq + zSq);
        if (norm < 0.8 || norm > 1.2) {
            return YAPI.SUCCESS;
        }
        _calibPrevTick = currTick;
        // Determine the device orientation index
        orient = 0;
        if (zSq > 0.5) {
            if (zVal > 0) {
                orient = 0;
            } else {
                orient = 1;
            }
        }
        if (xSq > 0.5) {
            if (xVal > 0) {
                orient = 2;
            } else {
                orient = 3;
            }
        }
        if (ySq > 0.5) {
            if (yVal > 0) {
                orient = 4;
            } else {
                orient = 5;
            }
        }
        // Discard measures that are not in the proper orientation
        if (_calibStageProgress == 0) {
            idx = 0;
            err = 0;
            while (idx + 1 < _calibStage) {
                if (_calibOrient[idx] == orient) {
                    err = 1;
                }
                idx = idx + 1;
            }
            if (err != 0) {
                _calibStageHint = "Turn the device on another face";
                return YAPI.SUCCESS;
            }
            _calibOrient.Add(orient);
        } else {
            if (orient != _calibOrient[_calibStage-1]) {
                _calibStageHint = "Not yet done, please move back to the previous face";
                return YAPI.SUCCESS;
            }
        }
        // Save measure
        _calibStageHint = "calibrating..";
        _calibDataAccX.Add(xVal);
        _calibDataAccY.Add(yVal);
        _calibDataAccZ.Add(zVal);
        _calibDataAcc.Add(norm);
        _calibInternalPos = _calibInternalPos + 1;
        _calibProgress = 1 + 16 * (_calibStage - 1) + ((16 * _calibInternalPos) / (_calibCount));
        if (_calibInternalPos < _calibCount) {
            _calibStageProgress = 1 + ((99 * _calibInternalPos) / (_calibCount));
            return YAPI.SUCCESS;
        }
        // Stage done, compute preliminary result
        intpos = (_calibStage - 1) * _calibCount;
        await this._calibSort(intpos, intpos + _calibCount);
        intpos = intpos + ((_calibCount) / (2));
        _calibLogMsg = "Stage "+Convert.ToString( _calibStage)+": median is "+Convert.ToString(
        (int) Math.Round(1000*_calibDataAccX[intpos]))+","+Convert.ToString(
        (int) Math.Round(1000*_calibDataAccY[intpos]))+","+Convert.ToString((int) Math.Round(1000*_calibDataAccZ[intpos]));
        // move to next stage
        _calibStage = _calibStage + 1;
        if (_calibStage < 7) {
            _calibStageHint = "Turn the device on another face";
            _calibPrevTick = ((currTick + 500) & (0x7FFFFFFF));
            _calibStageProgress = 0;
            _calibInternalPos = 0;
            return YAPI.SUCCESS;
        }
        // Data collection completed, compute accelerometer shift
        xVal = 0;
        yVal = 0;
        zVal = 0;
        idx = 0;
        while (idx < 6) {
            intpos = idx * _calibCount + ((_calibCount) / (2));
            orient = _calibOrient[idx];
            if (orient == 0 || orient == 1) {
                zVal = zVal + _calibDataAccZ[intpos];
            }
            if (orient == 2 || orient == 3) {
                xVal = xVal + _calibDataAccX[intpos];
            }
            if (orient == 4 || orient == 5) {
                yVal = yVal + _calibDataAccY[intpos];
            }
            idx = idx + 1;
        }
        _calibAccXOfs = xVal / 2.0;
        _calibAccYOfs = yVal / 2.0;
        _calibAccZOfs = zVal / 2.0;
        // Recompute all norms, taking into account the computed shift, and re-sort
        intpos = 0;
        while (intpos < _calibDataAcc.Count) {
            xVal = _calibDataAccX[intpos] - _calibAccXOfs;
            yVal = _calibDataAccY[intpos] - _calibAccYOfs;
            zVal = _calibDataAccZ[intpos] - _calibAccZOfs;
            norm = Math.Sqrt(xVal * xVal + yVal * yVal + zVal * zVal);
            _calibDataAcc[intpos] = norm;
            intpos = intpos + 1;
        }
        idx = 0;
        while (idx < 6) {
            intpos = idx * _calibCount;
            await this._calibSort(intpos, intpos + _calibCount);
            idx = idx + 1;
        }
        // Compute the scaling factor for each axis
        xVal = 0;
        yVal = 0;
        zVal = 0;
        idx = 0;
        while (idx < 6) {
            intpos = idx * _calibCount + ((_calibCount) / (2));
            orient = _calibOrient[idx];
            if (orient == 0 || orient == 1) {
                zVal = zVal + _calibDataAcc[intpos];
            }
            if (orient == 2 || orient == 3) {
                xVal = xVal + _calibDataAcc[intpos];
            }
            if (orient == 4 || orient == 5) {
                yVal = yVal + _calibDataAcc[intpos];
            }
            idx = idx + 1;
        }
        _calibAccXScale = xVal / 2.0;
        _calibAccYScale = yVal / 2.0;
        _calibAccZScale = zVal / 2.0;
        // Report completion
        _calibProgress = 100;
        _calibStageHint = "Calibration data ready for saving";
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> more3DCalibrationV2()
    {
        int currTick;
        byte[] calibParam;
        List<int> iCalib = new List<int>();
        int cal3;
        int calAcc;
        int calMag;
        int calGyr;
        // make sure calibration has been started
        if (_calibStage == 0) {
            return YAPI.INVALID_ARGUMENT;
        }
        if (_calibProgress == 100) {
            return YAPI.SUCCESS;
        }
        // make sure we don't start before previous calibration is cleared
        if (_calibStage == 1) {
            currTick = (int) ((YAPIContext.GetTickCount()) & (0x7FFFFFFF));
            currTick = ((currTick - _calibPrevTick) & (0x7FFFFFFF));
            if (currTick < 1600) {
                _calibStageHint = "Set down the device on a steady horizontal surface";
                _calibStageProgress = ((currTick) / (40));
                _calibProgress = 1;
                return YAPI.SUCCESS;
            }
        }
        // may throw an exception
        calibParam = await this._download("api/refFrame/calibrationParam.txt");
        iCalib = YAPIContext.imm_decodeFloats(YAPI.DefaultEncoding.GetString(calibParam));
        cal3 = ((iCalib[1]) / (1000));
        calAcc = ((cal3) / (100));
        calMag = ((cal3) / (10)) - 10*calAcc;
        calGyr = ((cal3) % (10));
        if (calGyr < 3) {
            _calibStageHint = "Set down the device on a steady horizontal surface";
            _calibStageProgress = 40 + calGyr*20;
            _calibProgress = 4 + calGyr*2;
        } else {
            _calibStage = 2;
            if (calMag < 3) {
                _calibStageHint = "Slowly draw '8' shapes along the 3 axis";
                _calibStageProgress = 1 + calMag*33;
                _calibProgress = 10 + calMag*5;
            } else {
                _calibStage = 3;
                if (calAcc < 3) {
                    _calibStageHint = "Slowly turn the device, stopping at each 90 degrees";
                    _calibStageProgress = 1 + calAcc*33;
                    _calibProgress = 25 + calAcc*25;
                } else {
                    _calibStageProgress = 99;
                    _calibProgress = 100;
                }
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns instructions to proceed to the tridimensional calibration initiated with
     *   method <c>start3DCalibration</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a character string.
     * </returns>
     */
    public virtual async Task<string> get_3DCalibrationHint()
    {
        return _calibStageHint;
    }

    /**
     * <summary>
     *   Returns the global process indicator for the tridimensional calibration
     *   initiated with method <c>start3DCalibration</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer between 0 (not started) and 100 (stage completed).
     * </returns>
     */
    public virtual async Task<int> get_3DCalibrationProgress()
    {
        return _calibProgress;
    }

    /**
     * <summary>
     *   Returns index of the current stage of the calibration
     *   initiated with method <c>start3DCalibration</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer, growing each time a calibration stage is completed.
     * </returns>
     */
    public virtual async Task<int> get_3DCalibrationStage()
    {
        return _calibStage;
    }

    /**
     * <summary>
     *   Returns the process indicator for the current stage of the calibration
     *   initiated with method <c>start3DCalibration</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer between 0 (not started) and 100 (stage completed).
     * </returns>
     */
    public virtual async Task<int> get_3DCalibrationStageProgress()
    {
        return _calibStageProgress;
    }

    /**
     * <summary>
     *   Returns the latest log message from the calibration process.
     * <para>
     *   When no new message is available, returns an empty string.
     * </para>
     * </summary>
     * <returns>
     *   a character string.
     * </returns>
     */
    public virtual async Task<string> get_3DCalibrationLogMsg()
    {
        string msg;
        msg = _calibLogMsg;
        _calibLogMsg = "";
        return msg;
    }

    /**
     * <summary>
     *   Applies the sensors tridimensional calibration parameters that have just been computed.
     * <para>
     *   Remember to call the <c>saveToFlash()</c>  method of the module if the changes
     *   must be kept when the device is restarted.
     * </para>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual async Task<int> save3DCalibration()
    {
        if (_calibV2) {
            return await this.save3DCalibrationV2();
        }
        return await this.save3DCalibrationV1();
    }

    public virtual async Task<int> save3DCalibrationV1()
    {
        int shiftX;
        int shiftY;
        int shiftZ;
        int scaleExp;
        int scaleX;
        int scaleY;
        int scaleZ;
        int scaleLo;
        int scaleHi;
        string newcalib;
        if (_calibProgress != 100) {
            return YAPI.INVALID_ARGUMENT;
        }
        // Compute integer values (correction unit is 732ug/count)
        shiftX = -(int) Math.Round(_calibAccXOfs / 0.000732);
        if (shiftX < 0) {
            shiftX = shiftX + 65536;
        }
        shiftY = -(int) Math.Round(_calibAccYOfs / 0.000732);
        if (shiftY < 0) {
            shiftY = shiftY + 65536;
        }
        shiftZ = -(int) Math.Round(_calibAccZOfs / 0.000732);
        if (shiftZ < 0) {
            shiftZ = shiftZ + 65536;
        }
        scaleX = (int) Math.Round(2048.0 / _calibAccXScale) - 2048;
        scaleY = (int) Math.Round(2048.0 / _calibAccYScale) - 2048;
        scaleZ = (int) Math.Round(2048.0 / _calibAccZScale) - 2048;
        if (scaleX < -2048 || scaleX >= 2048 || scaleY < -2048 || scaleY >= 2048 || scaleZ < -2048 || scaleZ >= 2048) {
            scaleExp = 3;
        } else {
            if (scaleX < -1024 || scaleX >= 1024 || scaleY < -1024 || scaleY >= 1024 || scaleZ < -1024 || scaleZ >= 1024) {
                scaleExp = 2;
            } else {
                if (scaleX < -512 || scaleX >= 512 || scaleY < -512 || scaleY >= 512 || scaleZ < -512 || scaleZ >= 512) {
                    scaleExp = 1;
                } else {
                    scaleExp = 0;
                }
            }
        }
        if (scaleExp > 0) {
            scaleX = ((scaleX) >> (scaleExp));
            scaleY = ((scaleY) >> (scaleExp));
            scaleZ = ((scaleZ) >> (scaleExp));
        }
        if (scaleX < 0) {
            scaleX = scaleX + 1024;
        }
        if (scaleY < 0) {
            scaleY = scaleY + 1024;
        }
        if (scaleZ < 0) {
            scaleZ = scaleZ + 1024;
        }
        scaleLo = ((((scaleY) & (15))) << (12)) + ((scaleX) << (2)) + scaleExp;
        scaleHi = ((scaleZ) << (6)) + ((scaleY) >> (4));
        // Save calibration parameters
        newcalib = "5,"+Convert.ToString( shiftX)+","+Convert.ToString( shiftY)+","+Convert.ToString( shiftZ)+","+Convert.ToString( scaleLo)+","+Convert.ToString(scaleHi);
        _calibStage = 0;
        return await this.set_calibrationParam(newcalib);
    }

    public virtual async Task<int> save3DCalibrationV2()
    {
        return await this.set_calibrationParam("5,5,5,5,5,5");
    }

    /**
     * <summary>
     *   Aborts the sensors tridimensional calibration process et restores normal settings.
     * <para>
     * </para>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual async Task<int> cancel3DCalibration()
    {
        if (_calibStage == 0) {
            return YAPI.SUCCESS;
        }
        // may throw an exception
        _calibStage = 0;
        return await this.set_calibrationParam(_calibSavedParams);
    }

    /**
     * <summary>
     *   Continues the enumeration of reference frames started using <c>yFirstRefFrame()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRefFrame</c> object, corresponding to
     *   a reference frame currently online, or a <c>null</c> pointer
     *   if there are no more reference frames to enumerate.
     * </returns>
     */
    public YRefFrame nextRefFrame()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindRefFrameInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of reference frames currently accessible.
     * <para>
     *   Use the method <c>YRefFrame.nextRefFrame()</c> to iterate on
     *   next reference frames.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRefFrame</c> object, corresponding to
     *   the first reference frame currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRefFrame FirstRefFrame()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RefFrame");
        if (next_hwid == null)  return null;
        return FindRefFrameInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of reference frames currently accessible.
     * <para>
     *   Use the method <c>YRefFrame.nextRefFrame()</c> to iterate on
     *   next reference frames.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YRefFrame</c> object, corresponding to
     *   the first reference frame currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRefFrame FirstRefFrameInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RefFrame");
        if (next_hwid == null)  return null;
        return FindRefFrameInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YRefFrame implementation)
}
}

