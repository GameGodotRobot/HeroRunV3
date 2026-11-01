using Godot;
using System;

public partial class RoomGO : RefCounted
{

    public const int MIN_ROOM_SIZE = 4;

    private Rect2 _rect;
    private void RectSet(Rect2 value){
       _rect = value; 
    }
    public Rect2 RectGet() {
        return _rect;
    }
    public Vector2 CenterGet() {
        return RectGet().Position + RectGet().Size/2.0f;
    }
    public Vector2I RandomPointGet(Random rand){
        Vector2I end_position = (Vector2I)(RectGet().Position + RectGet().Size);
        return new Vector2I(
            rand.Next((int)RectGet().Position.X, end_position.X),
            rand.Next((int)RectGet().Position.Y, end_position.Y)
        );
    }

    public RoomGO(Rect2 rect) {
        RectSet(rect);
    }
}
