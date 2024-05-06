namespace JSONNS;

class JSONValue;

class JSONNum(int val) : JSONValue {
    public int Val { get; set; } = val;
    public override string ToString() { return $"{Val}"; }
}

class JSONString(string val) : JSONValue {
    public string Val { get; set; } = val;
    public override string ToString() { return Val; }
}

class JSONBool(bool val) : JSONValue {
    public bool Val { get; set; } = val;
    public override string ToString() { return $"{Val}"; }
}

class JSONNull : JSONValue {
    public static int? Val = null;
}

class JSONArray(List<JSONValue> val) : JSONValue {
    public List<JSONValue> Val { get; set; } = val;
}

class JSONObject(Dictionary<string, JSONValue> val) : JSONValue {
    public Dictionary<string, JSONValue> Val { get; set; } = val;
    public JSONValue this[string s] { get => Val[s]; }
}