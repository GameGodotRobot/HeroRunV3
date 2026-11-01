using Godot;
using System;

public partial class Number : CodeCommand
{
    private double _value;
    private void ValueSet(double value) {
        _value = value;
    }
    public double ValueGet() {
        return _value;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.NUMBER;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return ValueGet();
    }

    public Number(double value, int position):base(position) {
        ValueSet(value);
    }
}
