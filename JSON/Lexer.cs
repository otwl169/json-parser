namespace JSON.Tokeniser;

enum TokenType
{
    LBRA, // {
    RBRA, // }
    LARR, // [
    RARR, // ]
    SC, // :
    COMMA, // ,

    NUM, // number
    STR, // "string"
    BOOL, // true | false
    NULL, // null

    EOF, // end of file
}

struct Token
{
    public TokenType Type { get; private set; }
    public string Lexeme { get; private set; }

    public Token(TokenType type, string lexeme)  { 
        Type = type; 
        Lexeme  = lexeme; 
    }

    public override string ToString() {
        return $"Token (Type: {this.Type}, Lexeme: '{this.Lexeme}')";
    }
}

class Lexer
{
    private StreamReader sr;

    public Lexer(string fileName)
    {
        sr = new(fileName);
    }

    public Token GetTok()
    {
        if (sr.Peek() < 0) {
            return new Token(TokenType.EOF, "");
        }

        char c = (char) sr.Read();

        return c switch {
            ' '  => GetTok(),
            '\n' => GetTok(),
            '\r' => GetTok(),
            '\t' => GetTok(),
            '{'  => new Token(TokenType.LBRA, "{"),
            '}'  => new Token(TokenType.RBRA, "}"),
            '['  => new Token(TokenType.LARR, "["),
            ']'  => new Token(TokenType.RARR, "]"),
            ':'  => new Token(TokenType.SC, ":"),
            ','  => new Token(TokenType.COMMA, ","),
            '"'  => ReadString(),
            't'  => ReadTrue(),
            'f'  => ReadFalse(),
            'n'  => ReadNull(),
            
            _    => Char.IsDigit(c) ? ReadNum(c) : throw new FormatException($"Error in Lexer.GetTok: invalid input character: {c}.")
        };
    }

    private Token ReadString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        while (sr.Peek() >= 0) {
            char c = (char) sr.Read();

            if (c == '"') {
                return new Token(TokenType.STR, sb.ToString());
            }

            sb.Append(c);
        }

        throw new FormatException("Error in Lexer.ReadStr: reached EOF before string was closed\n");
    }

    private Token ReadNum(char start)
    {
        if (!Char.IsDigit(start)) {
            throw new FormatException("Error in Lexer.ReadNum: invalid number start");
        }

        char lookahead;
        char c;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(start);
        while (sr.Peek() >= 0) {
            lookahead = (char) sr.Peek();

            if (Char.IsDigit(lookahead)) {
                c = (char) sr.Read();
                sb.Append(c);
            }
            else {
                return new Token(TokenType.NUM, sb.ToString());
            }
        }

        throw new FormatException("Error in Lexer.ReadNum: reached EOF before num ended\n");
    }

    private Token ReadTrue()
    {
        char[] next = new char[3];
        if (sr.Read(next, 0, 3) != 3) {
            throw new FormatException("Error in Lexer.ReadTrue: reached EOF while attempting to read bool\n");
        }

        if (new string(next) == "rue") {
            return new Token(TokenType.BOOL, "true");
        }

        throw new FormatException("Error in Lexer.ReadTrue: couldnt match bool\n");
    }

    private Token ReadFalse()
    {
        char[] next = new char[4];
        if (sr.Read(next, 0, 4) != 4) {
            throw new FormatException("Error in Lexer.ReadFalse: reached EOF while attempting to read bool\n");
        }

        if (new string(next) == "alse") {
            return new Token(TokenType.BOOL, "false");
        }

        throw new FormatException("Error in Lexer.ReadFalse: couldnt match bool\n");
    }

    private Token ReadNull()
    {
        char[] next = new char[3];
        if (sr.Read(next, 0, 3) != 3) {
            throw new FormatException("Error in Lexer.ReadNull: reached EOF while attempting to read null\n");
        }

        if (new string(next) == "ull") {
            return new Token(TokenType.NULL, "null");
        }

        throw new FormatException("Error in Lexer.ReadNull: couldnt match null\n");
    }

}