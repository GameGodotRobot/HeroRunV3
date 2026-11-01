using Godot;
using System;
using System.Collections.Generic;

public partial class TunnelGO : RefCounted
{

    private RoomGO _right_room;
    private void RightRoomSet(RoomGO value) {
        _right_room = value;
    }
    public RoomGO RightRoomGet(){
        return _right_room;
    }
    
    private RoomGO _left_room;
    private void LeftRoomSet(RoomGO value) {
        _left_room = value;
    }
    public RoomGO LeftRoomGet(){
        return _left_room;
    }

    private List<Vector2I> _points;
    private void PointsSet(List<Vector2I> value) {
        _points = value;
    }
    public List<Vector2I> PointsGet(){
        return _points;
    }

    public TunnelGO(RoomGO right, RoomGO Left, List<Vector2I> points) {
        RightRoomSet(right);
        LeftRoomSet(Left);
        PointsSet(points);
    }
}
