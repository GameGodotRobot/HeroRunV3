using Godot;
using System;

public partial class Vector : CodeCommand
{
    private Vector2 _value;
    private void ValueSet(Vector2 value) {
        _value = value;
    }
    public Vector2 ValueGet() {
        return _value;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.VECTOR;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return ValueGet();
    }

    public Vector(Vector2 value, int position):base(position) {
        ValueSet(value);
    }
}
