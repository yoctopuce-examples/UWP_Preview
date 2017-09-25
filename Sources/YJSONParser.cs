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
        protected string _data;
        protected int _data_start;
        protected int _data_len;
        protected int _data_boundary;
        protected string debug;

        public YJSONContent(string data, int start, int stop)
        {
            _data = data;
            _data_start = start;
            _data_boundary = stop;
            debug = data.Substring(start, stop - start);

        }

        public YJSONContent()
        {
            _data = null;
        }

        public abstract int Parse();

        protected int SkipGarbage(int i)
        {
            if (_data.Length <= i) {
                return i;
            }
            char sti = _data[i];
            while (i < _data_boundary && (sti == '\n' | sti == '\r' | sti == ' ')) {
                i++;
            }
            return i;
        }
        protected string FormatError(string errmsg, int cur_pos)
        {
            int ststart = cur_pos - 10;
            int stend = cur_pos + 10;
            if (ststart < 0) ststart = 0;
            if (stend > _data_boundary) stend = _data_boundary;
            return errmsg + " near " + _data.Substring(ststart, cur_pos - ststart) + _data.Substring(cur_pos, stend - cur_pos);
        }

        public abstract string ToJSON();
    }

    internal class YJSONArray : YJSONContent
    {
        private List<YJSONContent> _arrayValue = new List<YJSONContent>();

        public YJSONArray(string data, int start, int stop) : base(data, start, stop)
        {}

        public YJSONArray(string data) : this(data,0, data.Length)
        {}

        public YJSONArray() : base()
        {
            
        }

        public int Length
        {
            get { return _arrayValue.Count; }
        }

        public override int Parse()
        {
            int cur_pos = _data_start;
            cur_pos = SkipGarbage(cur_pos);

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
                            int len = jobj.Parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == '[') {
                            YJSONArray jobj = new YJSONArray(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == '"') {
                            YJSONString jobj = new YJSONString(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == '-' || (sti >= '0' && sti <= '9')) {
                            GetYJSONNumber jobj = new GetYJSONNumber(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            _arrayValue.Add(jobj);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == ']') { 
                            _data_len = cur_pos + 1 - _data_start;
                            return _data_len;
                        } else if (sti != ' ' & sti != '\n' & sti != ' ') {
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
                            if (sti != ' ' && sti != '\n' && sti != ' ') {
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

       

        public YJSONObject GetYJSONObject(int i)
        {
            return (YJSONObject)_arrayValue[i];
        }

        public string GetString(int i)
        {
            YJSONString ystr = (YJSONString)_arrayValue[i];
            return ystr.GetString();
        }

        public YJSONContent Get(int i)
        {
            return _arrayValue[i];
        }

        public YJSONArray GetYJSONArray(int i)
        {
            return (YJSONArray)_arrayValue[i];
        }

        public int GetInt(int i)
        {
            GetYJSONNumber ystr = (GetYJSONNumber)_arrayValue[i];
            return ystr.GetInt();
        }

        public long GetLong(int i)
        {
            GetYJSONNumber ystr = (GetYJSONNumber)_arrayValue[i];
            return ystr.GetLong();
        }

        public void Put(string flatAttr)
        {
            YJSONString strobj = new YJSONString();
            strobj.setContent(flatAttr);
            _arrayValue.Add(strobj);
        }

        public override string ToJSON()
        {
            StringBuilder res = new StringBuilder();
            res.Append('[');
            string sep = "";
            foreach (YJSONContent yjsonContent in _arrayValue) {
                string subres = yjsonContent.ToJSON();
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
            string sep ="";
            foreach (YJSONContent yjsonContent in _arrayValue)
            {
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

        public YJSONString(string data, int start, int stop) : base(data, start, stop)
        {
        }

        public YJSONString(string data): this(data,0, data.Length)
        {
        }

        public YJSONString()
        {
        }

        public override int Parse()
        {
            int str_start;
            string value = "";
            int cur_pos = _data_start;
            cur_pos = SkipGarbage(cur_pos);

            if (_data[cur_pos] != '"') {
                throw new System.Exception(FormatError("double quote was expected", cur_pos));
            }
            cur_pos++;
            str_start = cur_pos;
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

        public override string ToJSON()
        {
            StringBuilder res = new StringBuilder(_stringValue.Length*2);
            res.Append('"');
            foreach (char c in _stringValue)
            {
                switch (c)
                {
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

        public string GetString()
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


    internal class GetYJSONNumber : YJSONContent
    {
        private long _intValue = 0;
        private double _doubleValue = 0;
        private bool _isFloat = false;

        public GetYJSONNumber(string data, int start, int stop) : base(data, start, stop)
        {
        }

        public override int Parse()
        {

            bool neg = false;
            int start, dotPos;
            char sti;
            int cur_pos = _data_start;
            cur_pos = SkipGarbage(cur_pos);
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
            throw new System.Exception(FormatError("unexpected end of data", cur_pos)); throw new NotImplementedException();
        }

        public override string ToJSON()
        {
            if (_isFloat)
                return _doubleValue.ToString();
            else
                return _intValue.ToString();
        }

        public long GetLong()
        {
            return _intValue;
        }
        public int GetInt()
        {
            return (int) _intValue;
        }

        public double GetDouble()
        {
            return _doubleValue;
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
        Dictionary<string, YJSONContent> parsed = new Dictionary<string, YJSONContent>();


        public YJSONObject(string data) : base(data, 0, data.Length)
        {
        }

        public YJSONObject(string data, int start, int len) : base(data, start, len)
        {
        }

        public override int Parse()
        {
            string current_name = "";
            int name_start = _data_start;
            int cur_pos = _data_start;
            cur_pos = SkipGarbage(cur_pos);

            if (_data.Length <=cur_pos ||  _data[cur_pos] != '{') {
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
                            if (sti != ' ' && sti != '\n' && sti != ' ') {
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
                                throw new System.Exception(
                                    FormatError("invalid char: was expecting an identifier compliant char", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORCOLON:
                        if (sti == ':') {
                            state = Tjstate.JWAITFORDATA;
                        } else {
                            if (sti != ' ' & sti != '\n' & sti != ' ') {
                                throw new System.Exception(
                                    FormatError("invalid char: was expecting \"", cur_pos));
                            }
                        }
                        break;
                    case Tjstate.JWAITFORDATA:
                        if (sti == '{') {
                            YJSONObject jobj = new YJSONObject(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == '[') {
                            YJSONArray jobj = new YJSONArray(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti == '"') {
                            YJSONString jobj = new YJSONString(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti=='-' || (sti >= '0' && sti <= '9')) {
                            GetYJSONNumber jobj = new GetYJSONNumber(_data, cur_pos, _data_boundary);
                            int len = jobj.Parse();
                            cur_pos += len;
                            parsed.Add(current_name, jobj);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                            //cur_pos is allready incremented
                            continue;
                        } else if (sti != ' ' & sti != '\n' & sti != ' ') {
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
                            if (sti != ' ' && sti != '\n' && sti != ' ') {
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


        public bool Has(string key)
        {
            return parsed.ContainsKey(key);
        }

        public YJSONObject GetYJSONObject(string key)
        {
            return (YJSONObject)parsed[key];
        }

        public YJSONString GetYJSONString(string key)
        {
            return (YJSONString)parsed[key];
        }

        public YJSONArray GetYJSONArray(string key)
        {
            return (YJSONArray)parsed[key];
        }

        public List<string> Keys()
        {
            return parsed.Keys.ToList();
        }

        public GetYJSONNumber GetYJSONNumber(string key)
        {
            return (GetYJSONNumber)parsed[key];
        }

        public void Remove(string key)
        {
            parsed.Remove(key);
        }

        public string GetString(string key)
        {
            YJSONString ystr =(YJSONString)parsed[key];
            return ystr.GetString();
        }

        public int GetInt(string key)
        {
            GetYJSONNumber yint = (GetYJSONNumber) parsed[key];
            return yint.GetInt();
        }

        public YJSONContent Get(string key)
        {
            return parsed[key];
        }

        public long GetLong(string key)
        {
            GetYJSONNumber yint = (GetYJSONNumber)parsed[key];
            return yint.GetLong();
        }

        public double GetDouble(string key)
        {
            GetYJSONNumber yint = (GetYJSONNumber)parsed[key];
            return yint.GetDouble();
        }

        public override string ToJSON()
        {
            StringBuilder res = new StringBuilder();
            res.Append('{');
            string sep = "";
            foreach (string key in parsed.Keys.ToArray()) {
                YJSONContent subContent = parsed[key];
                string subres = subContent.ToJSON();
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
            foreach (string key in parsed.Keys.ToArray())
            {
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

    }

}
