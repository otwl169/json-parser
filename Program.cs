using LexerNS;
using ParserNS;
using JSONNS;

class Program {
    static string fileName = "../../../test.json";

    public static void Main(string[] args) {
        TryParse();
        return;
    }

    private static void PrintLexerResult() {
        Lexer l = new(fileName);
        Console.WriteLine($"Lexing {fileName}");
        Token cur;

        while ((cur = l.GetTok()).Type != TokenType.EOF) {
            Console.WriteLine(cur);
        }

        Console.WriteLine("Reached EOF and finished Lexing!");
    }

    public static void TryParse() {
        Lexer l = new(fileName);
        Parser p = new(l);
        JSONObject val = (JSONObject) p.parse();
        Console.WriteLine("Parsed with no errors!");

        string key = "first_name";
        JSONValue t = val[key];
        Console.WriteLine($"val[{key}] = {t}");
    }
}