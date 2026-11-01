using Godot;
using System;

public partial class VectorField : CodeCommand
{
    private CodeCommand _target;
    private void TargetSet(CodeCommand value) {
        _target = value;
    }
    public CodeCommand TargetGet() {
        return _target;
    }

    private String _field;
    private void FieldSet(String value) {
        _field = value;
    }
    public String FieldGet() {
        return _field;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.NUMBER;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false){
            return null;
        }
        Vector2 value = (Vector2)TargetGet().Execute(scope, error, map_go, i_am);
        return (double)(FieldGet() == "x" ? value.X : value.Y);
    }

    public VectorField(CodeCommand target, String field, int position):base(position) {
        TargetSet(target);
        FieldSet(field);
    }
}
