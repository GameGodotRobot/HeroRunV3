using Godot;
using System;

public partial class Pos : CodeCommand,  ICodeBody
{

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false) {
            return null;
        }
        return (Vector2)i_am.PositionGet();
    }

    public Pos(int position = 0):base(position) {
    }
}
