using Godot;
using System;
using System.Collections.Generic;

public partial class Token : RefCounted
{
    public enum TOKEN_TYPE {
        NONE = 0,
        SPACE = 1,
        TAB = 2,
        STATE = 3,
        COLON = 4,
        VAR = 5,
        EQUAL = 6,
        NOT_EQUAL = 7,
        MORE = 8,
        LESS = 9,
        LESS_EQUAL = 10,
        MORE_EQUAL = 11,
        AND = 12,
        OR = 13,
        ASSIGN = 14,
        MINUS = 15,
        PLUS = 16,
        NUMBER = 17,
        TEXT = 18,
        NAME = 19,
        LPAR = 20,
        RPAR = 21,
        COMMENT = 22,
        IF = 23,
        ELSE = 24,
        ELIF = 25,
        TRUE = 26,
        FALSE = 27,
        MULTIPLE = 28,
        DIVISION = 29,
        NEWLINE = 30,
        NOT = 31,
        DOT = 32,
        FUNC = 33,
        TYPE_TEXT = 34,
        TYPE_NUMBER = 35,
        TYPE_BOOL = 36,
        COMMA = 37,
        NEXT = 38,
        RETURN = 39,
        TYPE_VECTOR = 40
    }

    private readonly static HashSet<TOKEN_TYPE> MATH_OPERATOR_TOKEN_TYPE = new HashSet<TOKEN_TYPE> {
        TOKEN_TYPE.PLUS, 
        TOKEN_TYPE.MINUS,
        TOKEN_TYPE.MULTIPLE, 
        TOKEN_TYPE.DIVISION
    };
    public static bool IsMathOperator(TOKEN_TYPE tt) {
        return MATH_OPERATOR_TOKEN_TYPE.Contains(tt);
    }

    private readonly static HashSet<TOKEN_TYPE> COMPARSION_OPERATOR_TOKEN_TYPE = new HashSet<TOKEN_TYPE> {
        TOKEN_TYPE.EQUAL,
        TOKEN_TYPE.NOT_EQUAL,
        TOKEN_TYPE.LESS, 
        TOKEN_TYPE.MORE,
        TOKEN_TYPE.LESS_EQUAL, 
        TOKEN_TYPE.MORE_EQUAL
    };
    public static bool IsComparsionOperator(TOKEN_TYPE tt) {
        return COMPARSION_OPERATOR_TOKEN_TYPE.Contains(tt);
    }

    private readonly static HashSet<TOKEN_TYPE> BIT_OPERATOR_TOKEN_TYPE = new HashSet<TOKEN_TYPE> {
        TOKEN_TYPE.AND, 
        TOKEN_TYPE.OR,
        TOKEN_TYPE.NOT
    };
    public static bool IsBitOperator(TOKEN_TYPE tt) {
        return BIT_OPERATOR_TOKEN_TYPE.Contains(tt);
    }

    private TOKEN_TYPE _token_type;
    private void TokenTypeSet(TOKEN_TYPE value) {
        _token_type = value;
    }
    public TOKEN_TYPE TokenTypeGet() {
        return _token_type;
    }

    private String _text;
    private void TextSet(String value) {
        _text = value;
    }
    public String TextGet() {
        return _text;
    }

    private int _position;
    private void PositionSet(int value) {
        _position = value;
    }
    public int PositionGet() {
        return _position;
    }

    public Token(TOKEN_TYPE token_type, String text, int position) {
        TokenTypeSet(token_type);
        TextSet(text);
        PositionSet(position);
    }
}
