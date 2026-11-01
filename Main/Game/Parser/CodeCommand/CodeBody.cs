using Godot;
using System;
using System.Collections.Generic;

public partial class CodeBody : CodeCommand, ICodeBody
{

    List<CodeCommand> _commands;
    private void CommandsSet(List<CodeCommand> value){
        _commands = value;
    }
    public List<CodeCommand> CommandsGet() {
        return _commands;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am){
        foreach(CodeCommand command in _commands) {
            if (error.IsEmpty() == false) {
                return null;
            }
            object result = command.Execute(scope, error, map_go, i_am);
            if (result is ExecutionReturn) {
                return result;
            }
        }
        return null;
    }
    
    public CodeBody(List<CodeCommand> commands, int position):base(position) {
        CommandsSet(commands);
    }
}
