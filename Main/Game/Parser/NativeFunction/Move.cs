using Godot;
using System;

public partial class Move : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false) {
            return null;
        }
        Vector2 position = (Vector2)scope.VariableGet("position");
        bool is_moved = i_am.Move(position);
        if (is_moved == false) {
            i_am.Log(MindError.LOG_LEVEL.WARNING, $"move({position.X}, {position.Y}): tile is blocked");
        }
        return is_moved;
    }

    public Move(int position = 0):base(position) {
    }
}
