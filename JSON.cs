using System.Text;

namespace JSONNS;

abstract class JSONValue {
    public abstract JSONValue this[string s] { get; }
    public abstract JSONValue this[int i] { get; }
}

class JSONNum(int val) : JSONValue {
    public int Val { get; set; } = val;
    public override JSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONNum"); }
    public override JSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONNum"); }
    public override string ToString() { return $"{Val}"; }
}

class JSONString(string val) : JSONValue {
    public string Val { get; set; } = val;
    public override JSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONString"); }
    public override JSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONString"); }
    public override string ToString() { return $"'{Val}'"; }
}

class JSONBool(bool val) : JSONValue {
    public bool Val { get; set; } = val;
    public override JSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONBool"); }
    public override JSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONBool"); }
    public override string ToString() { return $"{Val}"; }
}

class JSONNull : JSONValue {
    public static int? Val = null;
    public override JSONValue this[string s] { get => throw new IndexOutOfRangeException("Cannot index type JSONNull"); }
    public override JSONValue this[int i] { get => throw new IndexOutOfRangeException("Cannot index type JSONNull"); }
    public override string ToString() { return  $"null"; }
}

class JSONArray(List<JSONValue> val) : JSONValue {
    public List<JSONValue> Val { get; set; } = val;
    public override JSONValue this[string s] { get => throw new IndexOutOfRangeException("Type JSONArray cannot be indexed via string"); }
    public override JSONValue this[int i] { get => Val[i]; }

    public override string ToString() {
        StringBuilder sb = new();
        sb.Append('[');
        sb.Append(String.Join(", ", Val));
        sb.Append(']');
        return sb.ToString(); 
    }
}

class JSONObject(Dictionary<string, JSONValue> val) : JSONValue {
    public Dictionary<string, JSONValue> Val { get; set; } = val;
    public override JSONValue this[string s] { get => Val[s]; }
    public override JSONValue this[int i] { get => throw new IndexOutOfRangeException("Type JSONArray cannot be indexed via int"); }

    public override string ToString() {
        StringBuilder sb = new();
        sb.Append("{");
        foreach (var (k, v) in Val) {
            sb.Append($"{k} : {v},");
        }
        sb.Length--; // get rid of trailing ,
        sb.Append('}');
        return sb.ToString(); 
    }
}