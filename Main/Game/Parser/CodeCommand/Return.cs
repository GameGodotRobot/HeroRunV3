using Godot;
using System;

public partial class Return : CodeCommand
{
    private CodeCommand _value;
    private void ValueSet(CodeCommand value) {
        _value = value;
    }
    public CodeCommand ValueGet() {
        return _value;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return ValueGet().ReturnTypeGet();
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false){
            return null;
        }
        return new ExecutionReturn(ValueGet().Execute(scope, error, map_go, i_am));
    }

    public Return(CodeCommand value, int position):base(position) {
        ValueSet(value);
    }
}
