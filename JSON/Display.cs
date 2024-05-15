using JSON.Types;
using System.Reflection.Metadata.Ecma335;

namespace JSON.Display;

class PrettyPrinter()
{
    public static void PrintValue(IJSONValue v)
    {
        PrintValue("", v);
    }

    public static void PrintValue(string prefix, IJSONValue v)
    {
        switch (v)
        {
            case JSONNum val:    PrintValue(prefix, val); break;
            case JSONString val: PrintValue(prefix, val); break;
            case JSONBool val:   PrintValue(prefix, val); break;
            case JSONNull val:   PrintValue(prefix, val); break;
            case JSONArray val:  PrintValue(prefix, val); break;
            case JSONObject val: PrintValue(prefix, val); break;
        };
    }

    private static void PrintNotImplementedJSONValue()
    {
        throw new FormatException("This type of JSONValue doesn#t have an implementation for PrettyPrinter");
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