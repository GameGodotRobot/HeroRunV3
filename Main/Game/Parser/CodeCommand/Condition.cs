using Godot;
using System;

public partial class Condition : CodeCommand
{
    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am){
        return null;
    }
    public Condition(int position):base(position) {
    }
}
