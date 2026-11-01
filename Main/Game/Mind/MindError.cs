using Godot;
using System;
using System.Collections.Generic;

public partial class MindError : RefCounted
{
    public enum LOG_LEVEL {
        INFO = 1,
        WARNING = 2,
        ERROR = 3
    }

    private String _text = "";
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

    public bool IsEmpty() {
        return TextGet() == "";
    }

    public void Init(String text, int position){
        TextSet(text);
        PositionSet(position);
    }
    public MindError() {
    }
}
