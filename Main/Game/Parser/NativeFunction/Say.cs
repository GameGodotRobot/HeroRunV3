using Godot;
using System;

public partial class Say : CodeCommand, ICodeBody
{
    

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        i_am.Say(Convert.ToString(scope.VariableGet("text")));
        return null;
    }

    public Say(int position = 0):base(position) {
    }
}
