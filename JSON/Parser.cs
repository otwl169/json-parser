using JSON.Tokeniser;
using JSON.Types;

namespace JSON.Parser;

class Parser {
    private Lexer L;
    private Token cur;

    public Parser(string s) {
        L = new(s);
        cur = L.GetTok();
    }

    public Parser(Lexer l) {
        L  = l;
        cur = L.GetTok();
    }

    public JSONValue Parse() {
        JSONValue parsed = ParseValue();

        if (cur.Type != TokenType.EOF) {
            throw new FormatException($"Error in Parser.parse(), finished parsing before reaching EOF");
        }

        return parsed;
    }

    private Token EatTok() {
        // return the token that you ate, progress lexer by 1
        Token eaten = cur;
        cur = L.GetTok();
        return eaten;
    }

    private JSONValue ParseValue() {
        Token start = cur;
        return start.Type switch {
            TokenType.NULL => ParseNull(),
            TokenType.NUM  => ParseNum(),
            TokenType.STR  => ParseString(),
            TokenType.BOOL => ParseBool(),
            TokenType.LARR => ParseArray(),
            TokenType.LBRA => ParseObject(),
            _ => throw new FormatException($"Error in Parser.parse(), JSON begins with unexpected character {start.Lexeme}"),
        };
    }

    private JSONNull ParseNull() {
        Token next = EatTok();
        if (next.Type != TokenType.NULL) {
            throw new FormatException($"Error in Parser.parseNull(), expected NULL, received {next.Type}"); 
        }

        return new JSONNull();
    }

    private JSONNum ParseNum() {
        Token next = EatTok();
        if (next.Type != TokenType.NUM) {
            throw new FormatException($"Error in Parser.parseNum(), expected NUM, received {next.Type}"); 
        }

        int num;
        if (int.TryParse(next.Lexeme, out num)) {
            return new JSONNum(num);
        }
        
        throw new FormatException($"Error in Parser.parseNum(), couldnt parse lexeme '{next.Lexeme}'");
    }

    private JSONString ParseString() {
        Token next = EatTok();

        if (next.Type != TokenType.STR) {
            throw new FormatException($"Error in Parser.parseStr(), expected STR, received {next.Type}"); 
        }

        return new JSONString(next.Lexeme);
    }

    private JSONBool ParseBool() {
        Token next = EatTok();

        if (next.Type != TokenType.BOOL) {
            throw new FormatException($"Error in Parser.parseBool(), expected BOOL, received {next.Type}"); 
        }

        return next.Lexeme switch {
            "true"  => new JSONBool(true),
            "false" => new JSONBool(false),
            _ => throw new FormatException($"Error in Parser.parseBool(), expected 'true' or 'false', received {next.Lexeme}")
        };
    }

    private JSONArray ParseArray() {
        Token prev = EatTok();
        List<JSONValue> array = [];

        if (prev.Type != TokenType.LARR) {
            throw new FormatException($"Error in Parser.parseArray(), expected LARR, received {prev.Type}");
        }

        while (cur.Type != TokenType.RARR) {
            array.Add(ParseValue());

            if (cur.Type == TokenType.COMMA) {
                EatTok(); // eat ,
            }
            else if (!(cur.Type == TokenType.RARR || cur.Type == TokenType.STR)) {
                throw new FormatException($"Error in Parser.parseArray(), expected RARR or STR, received {cur.Type}");
            }
        }

        EatTok(); // eat ]
        return new JSONArray(array);
    }

    private JSONObject ParseObject() {
        Token prev = EatTok();
        Dictionary<string, JSONValue> obj = [];
        JSONString key;
        JSONValue value;

        if (prev.Type != TokenType.LBRA) {
            throw new FormatException($"Error in Parser.parseObject(), expected LBRA, received {prev.Type}");
        }

        do {
            key = ParseString();

            if ((prev = EatTok()).Type != TokenType.SC) {
                throw new FormatException($"Error in Parser.parseObject(), expected SC, received {prev.Type}");
            }
            
            value = ParseValue();

            obj.Add(key.Val, value);

            if (cur.Type == TokenType.COMMA) {
                EatTok(); // eat ,
            }
            else if (!(cur.Type == TokenType.RBRA || cur.Type == TokenType.STR)) {
                throw new FormatException($"Error in Parser.parseObject(), expected RBRA or STR, received {cur.Type}");
            }
        } while (cur.Type != TokenType.RBRA);

        EatTok(); // eat }
        return new JSONObject(obj);
    }
}