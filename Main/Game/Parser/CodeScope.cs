using Godot;
using System;
using System.Collections.Generic;


public partial class CodeScope<T> : RefCounted
{
    private int _scope_counter = 0;
    private int _scope_id = 0;
    private Dictionary<int, int> _scope_tree = new  Dictionary<int, int>();
    private Dictionary<(int, String), T> _variables = new Dictionary<(int, String), T>();
    public void ScopeStart() {
        _scope_counter += 1;
        _scope_tree[_scope_counter] = _scope_id;
        _scope_id = _scope_counter;
    }
    public void  ScopeEnd(){
        _scope_id = _scope_tree[_scope_id];
    }
    public void VariableSet(String name, T value) {
        _variables[(_scope_id, name)] = value;
    }
    public T VariableGet(String name) {
        int scope_id = _scope_id;
        while (scope_id != 0) {
            T command = _variables.GetValueOrDefault((scope_id, name));
            if (command != null) {
                return command;
            }
            scope_id = _scope_tree[scope_id];
        }
        return default(T);
    }

    public CodeScope() {
    }
}
