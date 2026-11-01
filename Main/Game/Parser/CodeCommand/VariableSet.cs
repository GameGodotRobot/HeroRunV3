using Godot;
using System;

public partial class VariableSet : CodeCommand
{
    private String _name;
    private void NameSet(String value) {
        _name = value;
    }
    public String NameGet() {
        return _name;
    }

    CodeCommand _command;
    private void CommandSet(CodeCommand value){
        _command = value;
    }
    public CodeCommand CommandGet() {
        return _command;
    }
    
    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        scope.VariableSet(NameGet(), CommandGet().Execute(scope, error, map_go, i_am));
        return null;
    }

    public VariableSet(String name, CodeCommand formula, int position):base(position) {
        NameSet(name);
        CommandSet(formula);
    }
}
