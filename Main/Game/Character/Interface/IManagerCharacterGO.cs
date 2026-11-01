using Godot;
using System;

public interface IManagerCharacterGO
{
    bool CharacterPositionChange(CharacterGO character_go, Vector2I old_position, Vector2I new_position);
}
