using LexerNS;
using ParserNS;
using JSON.Types;

class Program {
    static string fileName = "../../../test.json";

    public static void Main(string[] args) {
        PrintParserResult();
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

    public static void PrintParserResult() {
        Lexer l = new(fileName);
        Parser p = new(l);
        JSONObject val = (JSONObject) p.parse();
        Console.WriteLine("Parsed with no errors!");
        Console.WriteLine($"{val}");
    }
}