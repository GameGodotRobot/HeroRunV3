using Godot;
using System;
using System.Collections.Generic;


public partial class ExecutionReturn : RefCounted
{
    private object _result;
    private void ResultSet(object value) {
        _result = value;
    }
    public object ResultGet(){
        return _result;
    }

    public ExecutionReturn(object result) {
        ResultSet(result);
    }
}
