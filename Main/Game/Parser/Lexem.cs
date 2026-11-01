using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Lexem : RefCounted
{
    private Token.TOKEN_TYPE _token_type;
    private void TokenTypeSet(Token.TOKEN_TYPE value) {
        _token_type = value;
    }
    public Token.TOKEN_TYPE TokenTypeGet() {
        return _token_type;
    }

    private Regex _regex;
    private void RegexSet(Regex value){
        _regex = value;
    }
    public Regex RegexGet() {
        return _regex;
    }

    public Match TokenFind(String text) {
        return RegexGet().Match(text);
    }

    private bool _is_filtered;
    private void IsFilteredSet(bool value) {
        _is_filtered = value;
    }
    public bool IsFilteredGet() {
        return _is_filtered;
    }

    public Lexem(Token.TOKEN_TYPE token_type, String regex, bool is_filtered = false) {
        TokenTypeSet(token_type);
        RegexSet(new Regex(@regex, RegexOptions.Compiled | RegexOptions.IgnoreCase));
        IsFilteredSet(is_filtered);
    }
}
