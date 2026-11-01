using Godot;
using System;

public partial class Vec : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false) {
            return null;
        }
        return new Vector2((float)(double)scope.VariableGet("x"), (float)(double)scope.VariableGet("y"));
    }

    public Vec(int position = 0):base(position) {
    }
}
