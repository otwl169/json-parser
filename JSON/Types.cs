using System.Text;

namespace JSON.Types;

interface IJSONValue {
    public IJSONValue this[string s] { get; }
    public IJSONValue this[int i] { get; }
}

class JSONNum(int val) : IJSONValue {
    public int Val { get; set; } = val;
    public IJSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONNum"); }
    public IJSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONNum"); }
}

class JSONString(string val) : IJSONValue {
    public string Val { get; set; } = val;
    public IJSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONString"); }
    public IJSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONString"); }
}

class JSONBool(bool val) : IJSONValue {
    public bool Val { get; set; } = val;
    public IJSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONBool"); }
    public IJSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONBool"); }
}

class JSONNull : IJSONValue {
    public static int? Val = null;
    public IJSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONNull"); }
    public IJSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONNull"); }
}

class JSONArray(List<IJSONValue> val) : IJSONValue {
    public List<IJSONValue> Val { get; set; } = val;
    public IJSONValue this[string s] { get => throw new IndexOutOfRangeException("Type JSONArray cannot be indexed via string"); }
    public IJSONValue this[int i] { get => Val[i]; }
}

class JSONObject(Dictionary<string, IJSONValue> val) : IJSONValue {
    public Dictionary<string, IJSONValue> Val { get; set; } = val;
    public IJSONValue this[string s] { get => Val[s]; }
    public IJSONValue this[int i] { get => throw new IndexOutOfRangeException("Type JSONArray cannot be indexed via int"); }
}