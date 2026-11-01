using Godot;
using System;

public partial class Print : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        GD.Print(Convert.ToString(scope.VariableGet("text")));
        return null;
    }

    public Print(int position = 0):base(position) {
    }
}
