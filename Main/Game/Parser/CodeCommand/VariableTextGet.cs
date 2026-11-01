using Godot;
using System;

public partial class VariableTextGet : CodeCommand
{
    private String _name;
    private void NameSet(String value) {
        _name = value;
    }
    public String NameGet() {
        return _name;
    }
    
    public override RETURN_TYPE ReturnTypeGet() {
        return RETURN_TYPE.TEXT;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return scope.VariableGet(NameGet());
    }

    public VariableTextGet(String name, int position):base(position) {
        NameSet(name);
    }
}
