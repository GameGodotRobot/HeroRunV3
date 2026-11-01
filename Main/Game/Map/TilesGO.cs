using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class TilesGO : RefCounted
{
    public enum TILE_TYPE
    {
        NONE = 0,
        FLOOR = 1,
        WALL = 2
    }

    static readonly Dictionary<TILE_TYPE, int> WEIGHT_MAP = new Dictionary<TILE_TYPE, int>{
        [TILE_TYPE.NONE] = int.MaxValue,
        [TILE_TYPE.FLOOR] = 20,
        [TILE_TYPE.WALL]= 80
    };


    public Vector2I MaxSizeGet() {
        return (Vector2I)_rect.Size;
    }
    private Rect2 _rect;
    private void RectSet(Rect2 value) {
        _rect = value;
    }
    public Rect2 RectGet() {
        return _rect;
    }

    private TILE_TYPE[,] _tiles;
    public void TileSet(TILE_TYPE tt, Vector2I position) {
        _tiles[position.X, position.Y] = tt;
        _weights[position.X, position.Y] = WEIGHT_MAP[tt];
    } 
    public TILE_TYPE TileGet(Vector2I position) {
        if (RectGet().HasPoint(position) == false) {
            return TILE_TYPE.NONE;
        }
        return _tiles[position.X, position.Y];
    }
   

    private int[,] _weights;
    public int WeightGet(Vector2I position){
        if (RectGet().HasPoint(position) == false) {
            return WEIGHT_MAP[TILE_TYPE.NONE];
        }
        return _weights[position.X, position.Y];
    }
    public void Fill(TILE_TYPE tt){
        MemoryMarshal.CreateSpan(ref _tiles[0, 0], _tiles.Length).Fill(tt);
        MemoryMarshal.CreateSpan(ref _weights[0, 0], _weights.Length).Fill(WEIGHT_MAP[tt]);
    }

    public bool IsPathless(Vector2I position) {
        if (RectGet().HasPoint(position) == false) {
            return false;
        }
        return _tiles[position.X, position.Y] == TILE_TYPE.FLOOR;;
    }

    public TilesGO(Vector2I max_size) {
        RectSet(new Rect2(Vector2I.Zero, max_size));
        _tiles = new TILE_TYPE[max_size.X, max_size.Y];
        _weights = new int[max_size.X, max_size.Y];
    }



}
