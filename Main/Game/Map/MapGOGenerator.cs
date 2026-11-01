using Godot;
using System;
using System.Collections.Generic;

public partial class MapGOGenerator : RefCounted
{
    public static List<RoomGO> DefaultRoomsBuild(Vector2I max_size, Random rand) {
        List<RoomGO> rooms = new List<RoomGO>();
        List<Rect2> rects = new List<Rect2>();
        BSP bsp = new BSP(new Rect2(new Vector2I(0,0), max_size), rand);
        bsp.LastLeafRectsGet(rects);
        foreach (Rect2 rect in rects){
            rooms.Add(RectToRoomGO(rect, rand));
        };
        return rooms;
    }

    private static RoomGO RectToRoomGO(Rect2 rect, Random rand) {
        Vector2 size = new Vector2(
                            rand.Next(RoomGO.MIN_ROOM_SIZE, (int)rect.Size.X-1),
                            rand.Next(RoomGO.MIN_ROOM_SIZE, (int)rect.Size.Y-1)
                        );
        Vector2 position = new Vector2(
                            rand.Next(1, (int)(rect.Size.X-size.X)),
                            rand.Next(1, (int)(rect.Size.Y-size.Y))
                        );
        return new RoomGO(new Rect2(rect.Position + position, size));
    }

    public static List<TunnelGO> DefaultTunnelsBuild(List<RoomGO> rooms, Random rand, IWeightMap map) {
        List<TunnelGO> tunnels = new List<TunnelGO>();
        if (rooms.Count < 2) {
            return tunnels;
        }
        HashSet<(RoomGO,RoomGO)> hash = new HashSet<(RoomGO,RoomGO)>(); 
        for(int i = 0; i < rooms.Count; i++ ){
            RoomGO r1 = rooms[i];
            RoomGO room_with_min_distance = null;
            double min_distance = double.PositiveInfinity;
            for(int j = 0; j < rooms.Count; j++ ){
                RoomGO r2 = rooms[j];
                if (r1 == r2) {
                    continue;
                }
                if (hash.Contains((r1, r2)) == true) {
                    continue;
                }
                double distance = r2.CenterGet().DistanceTo(r1.CenterGet());
                if (distance < min_distance) {
                    min_distance = distance;
                    room_with_min_distance = r2;
                }
            }
            tunnels.Add(RoomGOsToTunnelGO(r1, room_with_min_distance, rand, map));
            hash.Add((room_with_min_distance, r1));
        }
        return tunnels;
    }

    private static TunnelGO RoomGOsToTunnelGO(RoomGO right, RoomGO left, Random rand, IWeightMap map) {
        Vector2I start_point = right.RandomPointGet(rand);
        Vector2I end_point = left.RandomPointGet(rand);
        AStarAlgoritm alg = new AStarAlgoritm();
        List<Vector2I> points = alg.Calculate(start_point, end_point, map, false);
        return new TunnelGO(right, left, points);
    }

    public static void Brush(TilesGO tgo, List<RoomGO> rooms) {
        foreach(RoomGO room in rooms) {
            for (int x = (int)room.RectGet().Position.X; x < room.RectGet().Position.X+room.RectGet().Size.X; x++) {
                for (int y = (int)room.RectGet().Position.Y; y < room.RectGet().Position.Y+room.RectGet().Size.Y; y++) {
                    tgo.TileSet(TilesGO.TILE_TYPE.FLOOR, new Vector2I(x,y));
                }
            }
        }
    }
    
    public static void Brush(TilesGO tgo, List<TunnelGO> tunnels) {
        foreach(TunnelGO tunnel in tunnels) {
            foreach(Vector2I point in tunnel.PointsGet()) {
                tgo.TileSet(TilesGO.TILE_TYPE.FLOOR, point);
            }
        }
    }
}
