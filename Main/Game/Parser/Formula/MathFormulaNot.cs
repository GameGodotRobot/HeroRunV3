using Godot;
using System;

public partial class MathFormulaNot : CodeCommand
{
    private CodeCommand _first;
    private void FirstSet(CodeCommand value) {
        _first = value;
    }
    public CodeCommand FirstGet() {
        return _first;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.NUMBER;
    }
    
    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false){
            return null;
        }
        return -(double)FirstGet().Execute(scope, error, map_go, i_am);
    }

    public MathFormulaNot(CodeCommand first, int position):base(position) {
        FirstSet(first);
    }
}
