using Godot;
using System;

public partial class BitFormulaNot : CodeCommand
{
    private CodeCommand _first;
    private void FirstSet(CodeCommand value) {
        _first = value;
    }
    public CodeCommand FirstGet() {
        return _first;
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.BOOL;
    }
    
    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false){
            return null;
        }
        return !(bool)FirstGet().Execute(scope, error, map_go, i_am);
    }

    public BitFormulaNot(CodeCommand first, int position):base(position) {
        FirstSet(first);
    }
}
