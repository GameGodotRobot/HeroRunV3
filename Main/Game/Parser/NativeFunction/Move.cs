using Godot;
using System;

public partial class Move : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return i_am.Move((Vector2)scope.VariableGet("position"));
    }

    public Move(int position = 0):base(position) {
    }
}
