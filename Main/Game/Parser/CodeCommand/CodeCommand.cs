using Godot;
using System;
using System.Collections.Generic;

public partial class CodeCommand : RefCounted
{
    private int _position;
    protected void PositionSet(int value) {
        _position = value;
    }
    public int PositionGet() {
        return _position;
    }

    public enum RETURN_TYPE {
        VOID = 1,
        NUMBER = 2,
        BOOL = 3,
        TEXT = 4,
        ANY = 5,
        VECTOR = 6
    }
    public virtual RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.VOID;
    }
    public virtual object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return null;
    }

    public CodeCommand(int position) {
        PositionSet(position);
    }
}
