using Godot;
using System;

public partial class Text : CodeCommand
{
    private String _value;
    private void ValueSet(String value) {
        _value = value;
    }
    public String ValueGet() {
        return _value;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.TEXT;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return ValueGet();
    }

    public Text(String value, int position):base(position) {
        ValueSet(value);
    }
}
