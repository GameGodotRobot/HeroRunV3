using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MindGO : RefCounted
{
    private String _text;
    public void TextSet(String value) {
        _text = value;
    }
    public String TextGet() {
        return _text;
    }

    private Dictionary<String, CodeState> _states = new Dictionary<String, CodeState>();
    private void StatesSet(Dictionary<String, CodeState> value) {
        _states = value;
    }
    public Dictionary<String, CodeState> StatesGet() {
        return _states;
    }
    public MindError Execute(ICodeMapGO map_go, ICodeCharacterGO i_am) {
        if (StatesGet().Count == 0) {
            return new MindError();
        }
        MindError err = new MindError();
        StatesGet().First().Value.Execute(new CodeScope<object>(), err, map_go, i_am);
        return err;
    }

    public String Compile() {
        Parser parser = new Parser();
        Dictionary<String, CodeState> states = parser.Parse(TextGet());
        if (parser.ErrorGet() == "") {
            StatesSet(states);
            return parser.ErrorGet();
        } else {
            return parser.ErrorGet();
        }
    }

    public MindGO() {
    }
}
