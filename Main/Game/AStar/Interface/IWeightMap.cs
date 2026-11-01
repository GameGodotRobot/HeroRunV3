using Godot;
using System;

public interface IWeightMap
{
    int TileWeightGet(Vector2I position);
    bool TileIsPathless(Vector2I position);
}
