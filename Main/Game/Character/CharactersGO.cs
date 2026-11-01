using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CharactersGO : RefCounted, IManagerCharacterGO
{
    private Dictionary<Vector2I, CharacterGO> _characters_position = new Dictionary<Vector2I, CharacterGO>();
    private void CharactersPositionSet(Dictionary<Vector2I, CharacterGO> value){
        _characters_position = value;
    }
    public Dictionary<Vector2I, CharacterGO> CharactersPositionGet() {
        return _characters_position;
    }
    public CharacterGO[] CharactersGetArray() {
        return _characters_position.Values.ToArray();
    }
    public bool CharacterPositionChange(CharacterGO character_go, Vector2I old_position, Vector2I new_position) {
        if(IsContains(new_position) == true) {
            return false;
        }
        _characters_position[new_position] = character_go;
        _characters_position.Remove(old_position);
        return true;
    }
    public bool IsContains(Vector2I position) {
        return _characters_position.ContainsKey(position);
    }

    public CharacterGO CharacterPlayerCreate(CharacterVisualGO character_visual_go, String character_name, Vector2I position, IWeightMap wm) {
        CharacterGO player = new CharacterGO(character_name, character_visual_go, position, true, this, wm);
        return player;
    }
    public CharacterGO MobCreate(CharacterVisualGO character_visual_go, String character_name, Vector2I position, IWeightMap wm) {
        CharacterGO mob = new CharacterGO(character_name, character_visual_go, position, false, this, wm);
        return mob;
    }

    public void Execute(ICodeMapGO code_map_go) {
        foreach (CharacterGO character_go in _characters_position.Values) {
            character_go.Execute(code_map_go);
        }
    }

    public CharactersGO() {
    }
}
