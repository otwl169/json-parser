// See https://aka.ms/new-console-template for more information

using LexerNS;


string fileName = "../../../test.json";
Lexer l = new Lexer(fileName);
Console.WriteLine($"Lexing {fileName}");
Token cur;

while ((cur = l.GetTok()).Type != TokenType.EOF) {
    Console.WriteLine(cur);
}

Console.WriteLine("Reached EOF and finished Lexing");