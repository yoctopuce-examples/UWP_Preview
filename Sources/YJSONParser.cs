using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace com.yoctopuce.YoctoAPI

{
    internal enum Tjstate
    {
        JSTART,
        JWAITFORNAME,
        JWAITFORENDOFNAME,
        JWAITFORCOLON,
        JWAITFORDATA,
        JWAITFORNEXTSTRUCTMEMBER,
        JWAITFORNEXTARRAYITEM,
        JWAITFORSTRINGVALUE,
        JWAITFORSTRINGVALUE_ESC,
        JWAITFORINTVALUE,
        JWAITFORBOOLVALUE
    }
    internal abstract class YJSONContent
    {
        internal string _data;
        internal int _data_start;
        internal int _data_len;
        internal int _data_boundary;

        internal YJSONType _type;
        //protected string debug;

        internal enum YJSONType
        {
            STRING,
            NUMBER,
            ARRAY,
            OBJECT
        }

        protected enum Tjstate
        {
            JSTART,
            JWAITFORNAME,
            JWAITFORENDOFNAME,
            JWAITFORCOLON,
            JWAITFORDATA,
            JWAITFORNEXTSTRUCTMEMBER,
            JWAITFORNEXTARRAYITEM,
            JWAITFORSTRINGVALUE,
            JWAITFORSTRINGVALUE_ESC,
            JWAITFORINTVALUE,
            JWAITFORBOOLVALUE
        }

        internal static YJSONContent ParseJson(string data, int start, int stop)
        {
            int cur_pos = SkipGarbage(data, start, stop);
            YJSONContent res;
            if (data[cur_pos] == '[') {
                res = new YJSONArray(data, start, stop);
            } else if (data[cur_pos] == '{') {
                res = new YJSONObject(data, start, stop);
            } else if (data[cur_pos] == '"') {
                res = new YJSONString(data, start, stop);
            } else {
                res = new YJSONNumber(data, start, stop);
            }
            res.parse();
            return res;
        }

        protected YJSONContent(string data, int start, int stop, YJSONType type)
        {
            _data = data;
            _data_start = start;
            _data_boundary = stop;
            _type = type;
        }

        protected YJSONContent(YJSONType type)
        {
            _data = null;
        }

        public YJSONType getJSONType()
        {
            return _type;
        }

        public abstract int parse();

        protected static int SkipGarbage(string data, int start, int stop)
        {
            if (data.Length <= start) {
                return start;
            }
            char sti = data[start];
            while (start < stop && (sti == '\n' || sti == '\r' || sti == ' ')) {
                start++;
            }
            return start;
        }

        protected string FormatError(string errmsg, int cur_pos)
        {
            int ststart = cur_pos - 10;
            int stend = cur_pos + 10;
            if (ststart < 0)
                ststart = 0;
            if (stend > _data_boundary)
                stend = _data_boundary;
            if (_data == null) {
                return errmsg;
            }
            return errmsg + " near " + _data.Substring(ststart, cur_pos - ststart) + _data.Substring(cur_pos, stend - cur_pos);
        }

        public abstract string toJSON();
    }


    internal class YJSONArray : YJSONContent
    {
        private List<YJSONContent> _arrayValue = new List<YJSONContent>();

        public YJSONArray(string data, int start, int stop) : base(data, start, stop, YJSONType.ARRAY)
        { }

        public YJSONArray(string data) : this(data, 0, data.Length)
        { }

        public YJSONArray() : base(YJSONType.ARRAY)
        { }

        public int Length {
            get {
                return _arrayValue.Count;
            }
        }

        public override int parse()
        {
            int cur_pos = SkipGarbage(_data, _data_start, _data_boundary);

            if (_data[cur_pos] != '[') {
                throw new System.Exception(FormatError("Opening braces was expected", cur_pos));
            }
            cur_pos++;
            Tjstate state = Tjstate.JWAITFORDATA;

            while (cur_pos < _data_boundary) {
                char sti = _data[cur_pos];
                switch (state) {
                    case Tjstate.JWAITFORDATA:
                        if (sti == '{') {
                            YJSONObject jobj = new YJSONObject(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '[') {
                            YJSONArray jobj = new YJSONArray(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '"') {
                            YJSONString jobj = new YJSONString(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '-' || (sti >= '0' && sti <= '9')) {
                            YJSONNumber jobj = new YJSONNumber(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == ']') {
                            _data_len = cur_pos + 1 - _data_start;
                            return _data_len;
                        } else if (sti != ' ' && sti != '\n' && sti != '\r') {
                            throw new System.Exception(FormatError("invalid char: was expecting  \",0..9,t or f", cur_pos));
                        }
                        break;
                    case Tjstate.JWAITFORNEXTARRAYITEM:
                        if (sti == ',') {
                            state = Tjstate.JWAITFORDATA;
                        } else if (sti == ']') {
                            _data_len = cur_pos + 1 - _data_start;
                            return _data_len;
                        } else {
                            if (sti != ' ' && sti != '\n' && sti != '\r') {
                                throw new System.Exception(FormatError("invalid char: was expecting ,", cur_pos));
                            }
                        }
                        break;
                    default:
                        throw new System.Exception(FormatError("invalid state for YJSONObject", cur_pos));
                }
                cur_pos++;
            }
            throw new System.Exception(FormatError("unexpected end of data", cur_pos));
        }

        public YJSONObject getYJSONObject(int i)
        {
            return (YJSONObject) _arrayValue[i];
        }

        public string getString(int i)
        {
            YJSONString ystr = (YJSONString) _arrayValue[i];
            return ystr.getString();
        }

        public YJSONContent get(int i)
        {
            return _arrayValue[i];
        }

        public YJSONArray getYJSONArray(int i)
        {
            return (YJSONArray) _arrayValue[i];
        }

        public int getInt(int i)
        {
            YJSONNumber ystr = (YJSONNumber) _arrayValue[i];
            return ystr.getInt();
        }

        public long getLong(int i)
        {
            YJSONNumber ystr = (YJSONNumber) _arrayValue[i];
            return ystr.getLong();
        }

        public void put(string flatAttr)
        {
            YJSONString strobj = new YJSONString();
            strobj.setContent(flatAttr);
            _arrayValue.Add(strobj);
        }

        public override string toJSON()
        {
            StringBuilder res = new StringBuilder();
            res.Append('[');
            string sep = "";
            foreach (YJSONContent yjsonContent in _arrayValue) {
                string subres = yjsonContent.toJSON();
                res.Append(sep);
                res.Append(subres);
                sep = ",";
            }
            res.Append(']');
            return res.ToString();
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append('[');
            string sep = "";
            foreach (YJSONContent yjsonContent in _arrayValue) {
                string subres = yjsonContent.ToString();
                res.Append(sep);
                res.Append(subres);
                sep = ",";
            }
            res.Append(']');
            return res.ToString();
        }
    }

    internal class YJSONString : YJSONContent
    {
        private string _stringValue;

        public YJSONString(string data, int start, int stop) : base(data, start, stop, YJSONType.STRING)
        { }

        public YJSONString(string data) : this(data, 0, data.Length)
        { }

        public YJSONString() : base(YJSONType.STRING)
        { }

        public override int parse()
        {
            string value = "";
            int cur_pos = SkipGarbage(_data, _data_start, _data_boundary);

            if (_data[cur_pos] != '"') {
                throw new System.Exception(FormatError("double quote was expected", cur_pos));
            }
            cur_pos++;
            int str_start = cur_pos;
            Tjstate state = Tjstate.JWAITFORSTRINGVALUE;

            while (cur_pos < _data_boundary) {
                char sti = _data[cur_pos];
                switch (state) {
                    case Tjstate.JWAITFORSTRINGVALUE:
                        if (sti == '\\') {
                            value += _data.Substring(str_start, cur_pos - str_start);
                            str_start = cur_pos;
                            state = Tjstate.JWAITFORSTRINGVALUE_ESC;
                        } else if (sti == '"') {
                            value += _data.Substring(str_start, cur_pos - str_start);
                            _stringValue = value;
                            _data_len = (cur_pos + 1) - _data_start;
                            return _data_len;
                        } else if (sti < 32) {
                            throw new System.Exception(FormatError("invalid char: was expecting string value", cur_pos));
                        }
                        break;
                    case Tjstate.JWAITFORSTRINGVALUE_ESC:
                        value += sti;
                        state = Tjstate.JWAITFORSTRINGVALUE;
                        str_start = cur_pos + 1;
                        break;
                    default:
                        throw new System.Exception(FormatError("invalid state for YJSONObject", cur_pos));
                }
                cur_pos++;
            }
            throw new System.Exception(FormatError("unexpected end of data", cur_pos));
        }

        public override string toJSON()
        {
            StringBuilder res = new StringBuilder(_stringValue.Length * 2);
            res.Append('"');
            foreach (char c in _stringValue) {
                switch (c) {
                    case '"':
                        res.Append("\\\"");
                        break;
                    case '\\':
                        res.Append("\\\\");
                        break;
                    case '/':
                        res.Append("\\/");
                        break;
                    case '\b':
                        res.Append("\\b");
                        break;
                    case '\f':
                        res.Append("\\f");
                        break;
                    case '\n':
                        res.Append("\\n");
                        break;
                    case '\r':
                        res.Append("\\r");
                        break;
                    case '\t':
                        res.Append("\\t");
                        break;
                    default:
                        res.Append(c);
                        break;
                }
            }
            res.Append('"');
            return res.ToString();
        }

        public string getString()
        {
            return _stringValue;
        }

        public override string ToString()
        {
            return _stringValue;
        }

        public void setContent(string value)
        {
            _stringValue = value;
        }
    }


    internal class YJSONNumber : YJSONContent
    {
        private long _intValue = 0;
        private double _doubleValue = 0;
        private bool _isFloat = false;

        public YJSONNumber(string data, int start, int stop) : base(data, start, stop, YJSONType.NUMBER)
        { }

        public override int parse()
        {

            bool neg = false;
            int start, dotPos;
            char sti;
            int cur_pos = SkipGarbage(_data, _data_start, _data_boundary);
            sti = _data[cur_pos];
            if (sti == '-') {
                neg = true;
                cur_pos++;
            }
            start = cur_pos;
            dotPos = start;
            while (cur_pos < _data_boundary) {
                sti = _data[cur_pos];
                if (sti == '.' && _isFloat == false) {
                    string int_part = _data.Substring(start, cur_pos - start);
                    _intValue = Convert.ToInt64(int_part);
                    _isFloat = true;
                } else if (sti < '0' || sti > '9') {
                    string numberpart = _data.Substring(start, cur_pos - start);
                    if (_isFloat) {
                        _doubleValue = Convert.ToDouble(numberpart);
                    } else {
                        _intValue = Convert.ToInt64(numberpart);
                    }
                    if (neg) {
                        _doubleValue = 0 - _doubleValue;
                        _intValue = 0 - _intValue;
                    }
                    return cur_pos - _data_start;
                }
                cur_pos++;
            }
            throw new System.Exception(FormatError("unexpected end of data", cur_pos));
        }

        public override string toJSON()
        {
            if (_isFloat)
                return _doubleValue.ToString();
            else
                return _intValue.ToString();
        }

        public long getLong()
        {
            if (_isFloat)
                return (long)_doubleValue;
            else
                return _intValue;
        }

        public int getInt()
        {
            if (_isFloat)
                return (int)_doubleValue;
            else
                return (int)_intValue;
        }

        public double getDouble()
        {
            if (_isFloat)
                return _doubleValue;
            else
                return _intValue;
        }

        public override string ToString()
        {
            if (_isFloat)
                return _doubleValue.ToString();
            else
                return _intValue.ToString();
        }
    }


    internal class YJSONObject : YJSONContent
    {
        readonly Dictionary<string, YJSONContent> parsed = new Dictionary<string, YJSONContent>();
        readonly List<string> _keys = new List<string>(16);

        public YJSONObject(string data) : base(data, 0, data.Length, YJSONType.OBJECT)
        { }

        public YJSONObject(string data, int start, int len) : base(data, start, len, YJSONType.OBJECT)
        { }

        public override int parse()
        {
            string current_name = "";
            int name_start = _data_start;
            int cur_pos = SkipGarbage(_data, _data_start, _data_boundary);

            if (_data.Length <= cur_pos || _data[cur_pos] != '{') {
                throw new System.Exception(FormatError("Opening braces was expected", cur_pos));
            }
            cur_pos++;
            Tjstate state = Tjstate.JWAITFORNAME;


            while (cur_pos < _data_boundary) {
                char sti = _data[cur_pos];
                switch (state) {
                    case Tjstate.JWAITFORNAME:
                        if (sti == '"') {
                            state = Tjstate.JWAITFORENDOFNAME;
                            name_start = cur_pos + 1;
                        } else if (sti == '}') {
                            _data_len = cur_pos + 1 - _data_start;
                            return _data_len;
                        } else {
                            if (sti != ' ' && sti != '\n' && sti != '\r') {
                                throw new System.Exception(FormatError("invalid char: was expecting \"", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORENDOFNAME:
                        if (sti == '"') {
                            current_name = _data.Substring(name_start, cur_pos - name_start);
                            state = Tjstate.JWAITFORCOLON;

                        } else {
                            if (sti < 32) {
                                throw new System.Exception(FormatError("invalid char: was expecting an identifier compliant char", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORCOLON:
                        if (sti == ':') {
                            state = Tjstate.JWAITFORDATA;
                        } else {
                            if (sti != ' ' && sti != '\n' && sti != '\r') {
                                throw new System.Exception(
                                    FormatError("invalid char: was expecting \"", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORDATA:
                        if (sti == '{') {
                            YJSONObject jobj = new YJSONObject(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            _keys.Add(current_name);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '[') {
                            YJSONArray jobj = new YJSONArray(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            _keys.Add(current_name);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '"') {
                            YJSONString jobj = new YJSONString(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            _keys.Add(current_name);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti == '-' || (sti >= '0' && sti <= '9')) {
                            YJSONNumber jobj = new YJSONNumber(_data, cur_pos, _data_boundary);
                            int len = jobj.parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            _keys.Add(current_name);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is already incremented
                            continue;
                        } else if (sti != ' ' && sti != '\n' && sti != '\r') {
                            throw new System.Exception(FormatError("invalid char: was expecting  \",0..9,t or f", cur_pos));
                        }
                        break;
                    case Tjstate.JWAITFORNEXTSTRUCTMEMBER:
                        if (sti == ',') {
                            state = Tjstate.JWAITFORNAME;
                            name_start = cur_pos + 1;
                        } else if (sti == '}') {
                            _data_len = cur_pos + 1 - _data_start;
                            return _data_len;
                        } else {
                            if (sti != ' ' && sti != '\n' && sti != '\r') {
                                throw new System.Exception(FormatError("invalid char: was expecting ,", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORNEXTARRAYITEM:
                    case Tjstate.JWAITFORSTRINGVALUE:
                    case Tjstate.JWAITFORINTVALUE:
                    case Tjstate.JWAITFORBOOLVALUE:
                        throw new System.Exception(FormatError("invalid state for YJSONObject", cur_pos));
                }
                cur_pos++;
            }
            throw new System.Exception(FormatError("unexpected end of data", cur_pos));
        }

        public bool has(string key)
        {
            return parsed.ContainsKey(key);
        }

        public YJSONObject getYJSONObject(string key)
        {
            return (YJSONObject)parsed[key];
        }

        internal YJSONString getYJSONString(string key)
        {
            return (YJSONString)parsed[key];
        }

        internal YJSONArray getYJSONArray(string key)
        {
            return (YJSONArray)parsed[key];
        }

        public List<string> keys()
        {
            return parsed.Keys.ToList();
        }

        internal YJSONNumber getYJSONNumber(string key)
        {
            return (YJSONNumber)parsed[key];
        }

        public void remove(string key)
        {
            parsed.Remove(key);
        }

        public string getString(string key)
        {
            YJSONString ystr = (YJSONString)parsed[key];
            return ystr.getString();
        }

        public int getInt(string key)
        {
            YJSONNumber yint = (YJSONNumber)parsed[key];
            return yint.getInt();
        }

        public YJSONContent get(string key)
        {
            return parsed[key];
        }

        public long getLong(string key)
        {
            YJSONNumber yint = (YJSONNumber)parsed[key];
            return yint.getLong();
        }

        public double getDouble(string key)
        {
            YJSONNumber yint = (YJSONNumber)parsed[key];
            return yint.getDouble();
        }

        public override string toJSON()
        {
            StringBuilder res = new StringBuilder();
            res.Append('{');
            string sep = "";
            foreach (string key in parsed.Keys.ToArray()) {
                YJSONContent subContent = parsed[key];
                string subres = subContent.toJSON();
                res.Append(sep);
                res.Append('"');
                res.Append(key);
                res.Append("\":");
                res.Append(subres);
                sep = ",";
            }
            res.Append('}');
            return res.ToString();
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append('{');
            string sep = "";
            foreach (string key in parsed.Keys.ToArray()) {
                YJSONContent subContent = parsed[key];
                string subres = subContent.ToString();
                res.Append(sep);
                res.Append(key);
                res.Append("=>");
                res.Append(subres);
                sep = ",";
            }
            res.Append('}');
            return res.ToString();
        }



        public void parseWithRef(YJSONObject reference)
        {
            if (reference != null) {
                try {
                    YJSONArray yzon = new YJSONArray(_data, _data_start, _data_boundary);
                    yzon.parse();
                    convert(reference, yzon);
                    return;
                } catch (Exception) {

                }
            }
            this.parse();
        }

        private void convert(YJSONObject reference, YJSONArray newArray)
        {
            int length = newArray.Length;
            for (int i = 0; i < length; i++) {
                string key = reference.getKeyFromIdx(i);
                YJSONContent new_item = newArray.get(i);
                YJSONContent reference_item = reference.get(key);

                if (new_item.getJSONType() == reference_item.getJSONType()) {
                    parsed.Add(key, new_item);
                    _keys.Add(key);
                } else if (new_item.getJSONType() == YJSONType.ARRAY && reference_item.getJSONType() == YJSONType.OBJECT) {
                    YJSONObject jobj = new YJSONObject(new_item._data, new_item._data_start, reference_item._data_boundary);
                    jobj.convert((YJSONObject) reference_item, (YJSONArray) new_item);
                    parsed.Add(key, jobj);
                    _keys.Add(key);
                } else {
                    throw new System.Exception("Unable to convert "+ new_item.getJSONType().ToString() + " to "+ reference.getJSONType().ToString());

                }
            }
        }

        private string getKeyFromIdx(int i)
        {
            return _keys[i];
        }

    }
}