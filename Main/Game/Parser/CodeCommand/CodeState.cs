using Godot;
using System;

public partial class CodeState : RefCounted
{
    
    private String _name;
    private void NameSet(String value) {
        _name = value;
    }
    public String NameGet() {
        return _name;
    }

    private ICodeBody _code_body;
    private void CodeBodySet(ICodeBody code_body) {
        _code_body = code_body;
    }
    public ICodeBody CodeBodyGet(){
        return _code_body;
    }

    public void Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
       scope.ScopeStart();
       CodeBodyGet().Execute(scope, error, map_go, i_am);
       scope.ScopeEnd();
    }

    public CodeState(String name, ICodeBody code_body) {
        NameSet(name);
        CodeBodySet(code_body);
    }
}
