using Godot;
using System;
using System.Collections.Generic;

public partial class AStarAlgoritm : RefCounted
{
    const int MAX_STEP_FIND = 250;

    private static readonly List<Vector2I> DIRECTIONS = new  List<Vector2I> {
        new Vector2I(0, 1), 
        new Vector2I(0, -1), 
        new Vector2I(1, 0), 
        new Vector2I(-1, 0)
    };

    public List<Vector2I> Calculate(Vector2I start_point, Vector2I end_point, IWeightMap map, bool with_is_pathles = true) {
        if (start_point == end_point) {
            return new List<Vector2I>();
        }
        if (with_is_pathles == true && map.TileIsPathless(end_point) == false) {
            return new List<Vector2I>();
        }

        List<AStarPoint> open = new List<AStarPoint>();
        HashSet<Vector2I> open_hash = new HashSet<Vector2I>(); 
        open.Add(new AStarPoint(start_point));
        open_hash.Add(start_point);
        int step = 0;
        AStarPoint current = null;
        while(step < MAX_STEP_FIND) {
            current = open[open.Count - 1]; 
            open.RemoveAt(open.Count - 1);
            if (current.PointGet() == end_point) {
                break;
            }
            step++;
            foreach (Vector2I vec in DIRECTIONS) {
               
                Vector2I point = current.PointGet()+vec;
                if (open_hash.Contains(point) == true) {
                    continue;
                }
                if (with_is_pathles == true && map.TileIsPathless(point) == false) {
                    continue;
                }
                int weight = map.TileWeightGet(point);
                open.Add(new AStarPoint(point, weight, current, end_point.DistanceTo(point)*100 + weight));
                open_hash.Add(point);
            }
            if (open.Count == 0) {
                return new List<Vector2I>();
            }
            open.Sort((x, y) => y.ResultGet().CompareTo(x.ResultGet()));
        }
        if (step >= MAX_STEP_FIND) {
            return new List<Vector2I>();    
        }

        List<Vector2I> result = new List<Vector2I>();
        AStarPoint root = current.FPointGet();
        result.Add(current.PointGet());
        while(root.FPointGet() != null){
            root = root.FPointGet();
            result.Add(root.PointGet());
        }
        result.Reverse();

        return result;

    }

    public AStarAlgoritm() {
    }
}
