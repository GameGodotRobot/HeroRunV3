using Godot;
using System;

public interface ICodeCharacterGO
{
    void Say(String text);
    bool Move(Vector2 position);
    Vector2I PositionGet();
    void Log(MindError.LOG_LEVEL level, String text);
}
