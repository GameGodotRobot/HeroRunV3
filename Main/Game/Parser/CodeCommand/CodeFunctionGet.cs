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

    private CodeFunctionSet _function;
    private void FunctionSet(CodeFunctionSet value) {
        _function = value;
    }
    public CodeFunctionSet FunctionGet() {
        return _function;
    }

    public ICodeBody CodeBodyGet(){
        return FunctionGet().CodeBodyGet();
    }

    public override RETURN_TYPE ReturnTypeGet() {
        return FunctionGet().ReturnTypeGet();
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
        if (CodeBodyGet() is CodeBody) {
            error.Init($"Функция {FunctionGet().NameGet()} завершилась без return", PositionGet());
            return null;
        }
        return value;
    }

    public CodeFunctionGet(
            int position,
            Dictionary<String, CodeCommand> parameters, 
            CodeFunctionSet function,
            CodeCommand parent
        ):base(position) {
        ParametersSet(parameters);
        FunctionSet(function);
        ParentSet(parent);
    }
}
