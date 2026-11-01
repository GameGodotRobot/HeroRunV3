using Godot;
using System;

public partial class Print : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false) {
            return null;
        }
        String text = Convert.ToString(scope.VariableGet("text"));
        GD.Print(text);
        i_am.Log(MindError.LOG_LEVEL.INFO, text);
        return null;
    }

    public Print(int position = 0):base(position) {
    }
}
