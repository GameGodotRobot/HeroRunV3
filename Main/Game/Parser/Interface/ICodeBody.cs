using Godot;
using System;

public interface ICodeBody
{
    object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am);
    CodeCommand.RETURN_TYPE ReturnTypeGet();
}
