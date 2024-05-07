using JSON.Types;
using JSON.Parser;
using JSON.Display;
using JSON.Tokeniser;

class Program {
    static string fileName = "../../../test.json";

    public static void Main(string[] args) {
        PrettyPrintParserResult();
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

    public static void PrettyPrintParserResult() {
        Parser p = new(fileName);
        IJSONValue val = p.Parse();
        Console.WriteLine("Parsed with no errors!");
        PrettyPrinter.PrintValue(val);
    }
}