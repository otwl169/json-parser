using LexerNS;
using JSON;
using System.Security.Cryptography;

namespace ParserNS;

class Parser {
    private Lexer L;
    private Token cur;

    public Parser(Lexer l) {
        L  = l;
        cur = L.GetTok();
    }

    public JSONValue parse() {
        JSONValue parsed = parseValue();

        if (cur.Type != TokenType.EOF) {
            throw new FormatException($"Error in Parser.parse(), finished parsing before reaching EOF");
        }

        return parsed;
    }

    private Token eatTok() {
        // return the token that you ate, progress lexer by 1
        Token eaten = cur;
        cur = L.GetTok();
        return eaten;
    }

    private JSONValue parseValue() {
        Token start = cur;
        return start.Type switch {
            TokenType.NULL => parseNull(),
            TokenType.NUM  => parseNum(),
            TokenType.STR  => parseString(),
            TokenType.BOOL => parseBool(),
            TokenType.LARR => parseArray(),
            TokenType.LBRA => parseObject(),
            _ => throw new FormatException($"Error in Parser.parse(), JSON begins with unexpected character {start.Lexeme}"),
        };
    }

    private JSONNull parseNull() {
        Token next = eatTok();
        if (next.Type != TokenType.NULL) {
            throw new FormatException($"Error in Parser.parseNull(), expected NULL, received {next.Type}"); 
        }

        return new JSONNull();
    }

    private JSONNum parseNum() {
        Token next = eatTok();
        if (next.Type != TokenType.NUM) {
            throw new FormatException($"Error in Parser.parseNum(), expected NUM, received {next.Type}"); 
        }

        int num;
        if (int.TryParse(next.Lexeme, out num)) {
            return new JSONNum(num);
        }
        
        throw new FormatException($"Error in Parser.parseNum(), couldnt parse lexeme '{next.Lexeme}'");
    }

    private JSONString parseString() {
        Token next = eatTok();

        if (next.Type != TokenType.STR) {
            throw new FormatException($"Error in Parser.parseStr(), expected STR, received {next.Type}"); 
        }

        return new JSONString(next.Lexeme);
    }

    private JSONBool parseBool() {
        Token next = eatTok();

        if (next.Type != TokenType.BOOL) {
            throw new FormatException($"Error in Parser.parseBool(), expected BOOL, received {next.Type}"); 
        }

        return next.Lexeme switch {
            "true"  => new JSONBool(true),
            "false" => new JSONBool(false),
            _ => throw new FormatException($"Error in Parser.parseBool(), expected 'true' or 'false', received {next.Lexeme}")
        };
    }

    private JSONArray parseArray() {
        Token prev = eatTok();
        List<JSONValue> array = [];

        if (prev.Type != TokenType.LARR) {
            throw new FormatException($"Error in Parser.parseArray(), expected LARR, received {prev.Type}");
        }

        while (cur.Type != TokenType.RARR) {
            array.Add(parseValue());

            if (cur.Type == TokenType.COMMA) {
                eatTok(); // eat ,
            }
            else if (!(cur.Type == TokenType.RARR || cur.Type == TokenType.STR)) {
                throw new FormatException($"Error in Parser.parseArray(), expected RARR or STR, received {cur.Type}");
            }
        }

        eatTok(); // eat ]
        return new JSONArray(array);
    }

    private JSONObject parseObject() {
        Token prev = eatTok();
        Dictionary<string, JSONValue> obj = [];
        JSONString key;
        JSONValue value;

        if (prev.Type != TokenType.LBRA) {
            throw new FormatException($"Error in Parser.parseObject(), expected LBRA, received {prev.Type}");
        }

        do {
            key = parseString();

            if ((prev = eatTok()).Type != TokenType.SC) {
                throw new FormatException($"Error in Parser.parseObject(), expected SC, received {prev.Type}");
            }
            
            value = parseValue();

            obj.Add(key.Val, value);

            if (cur.Type == TokenType.COMMA) {
                eatTok(); // eat ,
            }
            else if (!(cur.Type == TokenType.RBRA || cur.Type == TokenType.STR)) {
                throw new FormatException($"Error in Parser.parseObject(), expected RBRA or STR, received {cur.Type}");
            }
        } while (cur.Type != TokenType.RBRA);

        eatTok(); // eat }
        return new JSONObject(obj);
    }
}