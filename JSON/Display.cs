using JSON.Types;

namespace JSON.Display;

class PrettyPrinter()
{
    public static void PrintValue(IJSONValue v)
    {
        PrintValue("", v);
    }

    public static void PrintValue(string prefix, IJSONValue v)
    {
        if (v.GetType() == typeof(JSONNum)) {
            PrintValue(prefix, (JSONNum) v);
        }
        else if (v.GetType() == typeof(JSONString)) {
            PrintValue(prefix, (JSONString) v);
        }
        else if (v.GetType() == typeof(JSONBool)) {
            PrintValue(prefix, (JSONBool) v);
        }
        else if (v.GetType() == typeof(JSONNull)) {
            PrintValue(prefix, (JSONNull) v);
        }
        else if (v.GetType() == typeof(JSONArray)) {
            PrintValue(prefix, (JSONArray) v);
        }
        else if (v.GetType() == typeof(JSONObject)) {
            PrintValue(prefix, (JSONObject) v);
        }
    }

    public static void PrintValue(string _, JSONNum v)
    { 
        Console.Write($"{v.Val}"); 
    }

    public static void PrintValue(string _, JSONString v)
    { 
        Console.Write($"'{v.Val}'"); 
    }
    public static void PrintValue(string _, JSONBool v)
    { 
        Console.Write($"{v.Val}"); 
    }
    public static void PrintValue(string _, JSONNull v)
    { 
        Console.Write($"null"); 
    }

    public static void PrintValue(string prefix, JSONArray v)
    { 
        Console.Write("[");
        foreach (var o in v.Val) {
            Console.Write($"\n{prefix}   ");
            PrintValue(prefix + "  ", o);
        }
        Console.Write($"\n{prefix}]");
    }

    public static void PrintValue(string prefix, JSONObject v)
    { 
        Console.Write("{");
        foreach (var (key, o) in v.Val) {
            Console.Write($"\n{prefix}  {key} : ");
            PrintValue(prefix + "  ", o);
            Console.Write(", ");
        }
        Console.Write("\n" + prefix + " }");
    }
}