using Godot;
using System;

public partial class Say : CodeCommand, ICodeBody
{
    

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (error.IsEmpty() == false) {
            return null;
        }
        i_am.Say(Convert.ToString(scope.VariableGet("text"), System.Globalization.CultureInfo.InvariantCulture));
        return null;
    }

    public Say(int position = 0):base(position) {
    }
}
