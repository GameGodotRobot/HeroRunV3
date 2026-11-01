using Godot;
using System;

public interface ICodeCharacterGO
{
    void Say(String text);
    bool Move(Vector2 position);
    void Log(MindError.LOG_LEVEL level, String text);
}
