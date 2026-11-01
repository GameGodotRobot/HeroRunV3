using Godot;
using System;

public partial class MathFormulaNotEqual : CodeCommand
{
    private CodeCommand _first;
    private void FirstSet(CodeCommand value) {
        _first = value;
    }
    public CodeCommand FirstGet() {
        return _first;
    }

    private CodeCommand _second;
    private void SecondSet(CodeCommand value) {
        _second = value;
    }
    public CodeCommand SecondGet() {
        return _second;
    }
    
    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.BOOL;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false){
            return null;
        }
        return (double)FirstGet().Execute(scope, error, map_go, i_am) != (double)SecondGet().Execute(scope, error, map_go, i_am);
    }

    public MathFormulaNotEqual(CodeCommand first, CodeCommand second, int position):base(position) {
        FirstSet(first);
        SecondSet(second);
    }
}
