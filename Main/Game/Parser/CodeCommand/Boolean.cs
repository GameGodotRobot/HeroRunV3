using Godot;
using System;

public partial class Boolean : CodeCommand
{
    private bool _value;
    private void ValueSet(bool value) {
        _value = value;
    }
    public bool ValueGet() {
        return _value;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.BOOL;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return ValueGet();
    }

    public Boolean(bool value, int position):base(position) {
        ValueSet(value);
    }
}
