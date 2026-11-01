using Godot;
using System;

public partial class CharacterGO : RefCounted, ICodeCharacterGO
{
    [Signal]
    public delegate void SaySignalEventHandler(String text);
    [Signal]
    public delegate void MoveSignalEventHandler(Vector2I position);

    private String _name;
    private void NameSet(String value) {
        _name = value;
    }
    public String NameGet() {
        return _name;
    }
   
    private CharacterVisualGO _cvgo;
    private void CharacterVisualGOSet(CharacterVisualGO value) {
        _cvgo = value;
    } 
    public CharacterVisualGO CharacterVisualGOGet() {
        return _cvgo;
    }

    private Vector2I _position;
    public void PositionSet(Vector2I position) {
        _position = position;
    }
    public Vector2I PositionGet() {
        return _position;
    }
    public bool PositionChange(Vector2I position) {
        if (WeightMapGet().TileIsPathless(position) == false) {
            return false;
        }
        if (ManagerCharacterGOGet().CharacterPositionChange(this, position, PositionGet()) == false) {
            return false;
        }
        PositionSet(position);
        return true;
    }

    private bool _is_player = false;
    private void IsPlayerSet(bool value) {
        _is_player = value;
    }
    public bool IsPlayerGet() {
        return _is_player;
    }

    private IManagerCharacterGO _mcgo;
    private void ManagerCharacterGOSet(IManagerCharacterGO value) {
        _mcgo = value;
    }
    public IManagerCharacterGO ManagerCharacterGOGet() {
        return _mcgo;
    }

    private IWeightMap _wm;
    private void WeightMapSet(IWeightMap value) {
        _wm = value;
    }
    public IWeightMap WeightMapGet() {
        return _wm;
    }

    private MindGO _mind_go = new MindGO();
    public void MindGoSet(MindGO value) {
        _mind_go = value;
    }
    public MindGO MindGoGet() {
        return _mind_go;
    }
    // как клонировать древо майнд на кучу мобов
    public String MindCompile() {
        return MindGoGet().Compile();
    }
    public void TestExecute() {
        Execute(null);
    }
    public void MindTextSave(String text){
        MindGoGet().TextSet(text);
    }

    

    public void Say(String text) {
        EmitSignal(SignalName.SaySignal, text);
    }

    public bool Move(Vector2 position) {
        if (PositionChange((Vector2I)position) == false){
            return false;
        }
        EmitSignal(SignalName.MoveSignal, PositionGet());
        return true;
    }


    public void Execute(ICodeMapGO code_map_go) {
        MindGoGet().Execute(code_map_go, this);
    }

    public CharacterGO(
                String name, 
                CharacterVisualGO cvgo, 
                Vector2I position, 
                bool is_player, 
                IManagerCharacterGO mcgo,
                IWeightMap wm) {
        WeightMapSet(wm);
        NameSet(name);
        ManagerCharacterGOSet(mcgo);
        CharacterVisualGOSet(cvgo);
        IsPlayerSet(is_player);
        PositionChange(position);
       
    }
}
