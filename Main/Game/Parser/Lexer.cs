using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Lexer : RefCounted
{
    private static readonly List<Lexem> LEXEMS = new  List<Lexem> {
        new Lexem(Token.TOKEN_TYPE.TAB,"^[\\t]"),
       // new Lexem(Token.TOKEN_TYPE.NEWLINE,"^[\\n]"),
        new Lexem(Token.TOKEN_TYPE.SPACE,"^[\\s\\n\\r\\v\\f]", true),
        new Lexem(Token.TOKEN_TYPE.COMMENT,"^//.*", true),
        new Lexem(Token.TOKEN_TYPE.STATE,"^(state)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.FUNC,"^(func)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.TYPE_TEXT,"^(text)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.TYPE_NUMBER,"^(num)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.TYPE_BOOL,"^(bool)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.VECTOR, @"^vec\((-?\d+(?:\.\d+)?),\s*(-?\d+(?:\.\d+)?)\)"),
        new Lexem(Token.TOKEN_TYPE.IF,"^(if)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.ELIF,"^(elif)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.ELSE,"^(else)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.TRUE,"^(true)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.FALSE,"^(false)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.AND,"^(and)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.OR,"^(or)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.RETURN,"^(return)(?!\\w)"),
        new Lexem(Token.TOKEN_TYPE.NEXT,"^(->)"),
        new Lexem(Token.TOKEN_TYPE.EQUAL,"^(==)"),
        new Lexem(Token.TOKEN_TYPE.NOT_EQUAL,"^(!=)"),
        new Lexem(Token.TOKEN_TYPE.LESS_EQUAL,"^(<=)"),
        new Lexem(Token.TOKEN_TYPE.MORE_EQUAL,"^(>=)"),
        new Lexem(Token.TOKEN_TYPE.NOT,"^[!]"),
        new Lexem(Token.TOKEN_TYPE.DOT,"^[.]"),
        new Lexem(Token.TOKEN_TYPE.COMMA,"^[,]"),
        new Lexem(Token.TOKEN_TYPE.COLON,"^[:]"),
        new Lexem(Token.TOKEN_TYPE.MORE,"^[>]"),
        new Lexem(Token.TOKEN_TYPE.LESS,"^[<]"),
        new Lexem(Token.TOKEN_TYPE.ASSIGN,"^[=]"),
        new Lexem(Token.TOKEN_TYPE.MINUS,"^[-]"),
        new Lexem(Token.TOKEN_TYPE.PLUS,"^[+]"),
        new Lexem(Token.TOKEN_TYPE.MULTIPLE,"^[*]"),
        new Lexem(Token.TOKEN_TYPE.DIVISION,"^[/]"),
        new Lexem(Token.TOKEN_TYPE.NUMBER,@"^(-?\d+(?:\.\d+)?)"),
        new Lexem(Token.TOKEN_TYPE.TEXT,"^\"[^\"]*\""),
        new Lexem(Token.TOKEN_TYPE.NAME,"^[a-z0-9_]+"),
        new Lexem(Token.TOKEN_TYPE.LPAR,"^[(]"),
        new Lexem(Token.TOKEN_TYPE.RPAR,"^[)]")
    };

    private String _error;
    private void ErrorSet(String value) {
        _error = value;
    }
    public String ErrorGet() {
        return _error;
    }

    public List<Token> Analyze(String text) {
        ErrorSet("");
        int position = 0;
        List<Token> tokens = new List<Token>();
        while (text.Length > 0) {
            bool is_error = true;
            foreach(Lexem lexem in LEXEMS) {
                Match match = lexem.TokenFind(text);
                if (!match.Success) {
                    continue;
                }
                if (lexem.IsFilteredGet() == false) {
                    tokens.Add(new Token(
                                    lexem.TokenTypeGet(),
                                    match.Value,
                                    position));
                }
                position += match.Length;
                text = text.Substring(match.Length);
                is_error = false;
                break;
            }
            if (is_error == true) {
                ErrorSet($"{position}: UNKNOWN CHARACTER EXPRESSION");
                return new List<Token>();
            }
        }
        return tokens; 
    }

    public Lexer() {

    }
}
