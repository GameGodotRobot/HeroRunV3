using Godot;
using System;
using System.Collections.Generic;

public partial class CodeFunctionSet: CodeCommand
{

    private Dictionary<String, CodeCommand.RETURN_TYPE> _parameters;
    private void ParametersSet(Dictionary<String, CodeCommand.RETURN_TYPE> values) {
        _parameters = values;
    }
    public Dictionary<String, CodeCommand.RETURN_TYPE> ParametersGet() {
        return _parameters;
    }

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

    private RETURN_TYPE _return_type;
    private void ReturnTypeSet(RETURN_TYPE value) {
        _return_type = value;
    }
    public override RETURN_TYPE ReturnTypeGet() {
        return _return_type;
    }

    public override object Execute(CodeScope<object> scope, MindError error, ICodeMapGO map_go, ICodeCharacterGO i_am) {
        return null;
    }

    public CodeFunctionSet(
            int position, 
            String name, 
            Dictionary<String, CodeCommand.RETURN_TYPE> parameters, 
            ICodeBody code_body,
            RETURN_TYPE return_type
        ):base(position) {
        NameSet(name);
        ParametersSet(parameters);
        CodeBodySet(code_body);
        ReturnTypeSet(return_type);
    }
}
