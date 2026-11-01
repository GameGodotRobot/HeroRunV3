using Godot;
using System;
using System.Collections.Generic;

public partial class CodeFunctionGet: CodeCommand
{
    private CodeCommand _parent;
    private void ParentSet(CodeCommand value) {
        _parent = value;
    }
    public CodeCommand ParentGet() { // не используется, а должно
        return _parent;
    }

    private Dictionary<String, CodeCommand> _parameters;
    private void ParametersSet(Dictionary<String, CodeCommand> values) {
        _parameters = values;
    }
    public Dictionary<String, CodeCommand> ParametersGet() {
        return _parameters;
    }

    private ICodeBody _code_body;
    private void CodeBodySet(ICodeBody code_body) {
        _code_body = code_body;
    }
    public ICodeBody CodeBodyGet(){
        return _code_body;
    }

    private RETURN_TYPE _return_type;
    private void ReturnTypeSet(RETURN_TYPE value) {
        _return_type = value;
    }
    public override RETURN_TYPE ReturnTypeGet() {
        return _return_type;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        scope.ScopeStart();
        
        foreach(String name in ParametersGet().Keys) {
            scope.VariableSet(name, _parameters[name].Execute(scope, error, map_go, i_am));
        }
        object value = CodeBodyGet().Execute(scope, error, map_go, i_am);
        scope.ScopeEnd();
        if (error.IsEmpty() == false){
            return null;
        }
        if (value is ExecutionReturn) {
            return (value as ExecutionReturn).ResultGet();
        }
        return value;
    }

    public CodeFunctionGet(
            int position,
            Dictionary<String, CodeCommand> parameters, 
            ICodeBody code_body,
            RETURN_TYPE return_type,
            CodeCommand parent
        ):base(position) {
        ParametersSet(parameters);
        CodeBodySet(code_body);
        ReturnTypeSet(return_type);
        ParentSet(parent);
    }
}
