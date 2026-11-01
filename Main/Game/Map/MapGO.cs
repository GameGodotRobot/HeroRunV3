using Godot;
using System;
using System.Collections.Generic;

public partial class MapGO : RefCounted, IWeightMap, ICodeMapGO
{
    
    private static readonly Vector2I _cell_size = new Vector2I(32,32);
    public static Vector2I CellSizeGet(){
        return _cell_size;
    }

    private TilesGO _tiles_go;
    private void TilesGOSet(TilesGO value) {
        _tiles_go = value;
    }
    private TilesGO TilesGoGet(){
        return _tiles_go;
    }


    private TimeGO _time = new TimeGO();
    private void TilesGOSet(TimeGO value) {
        _time = value;
    }
    private TimeGO TimeGoGet(){
        return _time;
    }

    private int _level = 1;
    private void LevelSet(int value){
        _level = value;
    }
    public int LevelGet() {
        return _level;
    }
    private void LevelRestart() {
        LevelSet(1);
    }

    private Random _random = new Random(42);
    private void RandomSet(Random value){
        _random = value;
    }
    public Random RandomGet() {
        return _random;
    }

    private List<RoomGO> _rooms;
    private void RoomsSet(List<RoomGO> value) {
        _rooms = value;
    }
    public List<RoomGO> RoomsGet() {
        return _rooms;
    }
    public RoomGO StartRoomGet() {
        return RoomsGet()[0];
    }
    public RoomGO EndRoomGet() {
        return RoomsGet()[RoomsGet().Count - 1];
    }

    private List<TunnelGO> _tunnels;
    private void TunnelsSet(List<TunnelGO> value) {
        _tunnels = value;
    }
    public List<TunnelGO> TunnelsGet() {
        return _tunnels;
    }

    public int TileWeightGet(Vector2I position) {
        return _tiles_go.WeightGet(position);
    }
    public bool TileIsPathless(Vector2I position) {
        if (CharactersGOGet().IsContains(position) == true) {
            return false;
        }
        return _tiles_go.IsPathless(position);
    }

    private CharactersGO _characters;
    private void CharactersGOSet(CharactersGO value){
        _characters = value;
    }
    public CharactersGO CharactersGOGet() {
        return _characters;
    }

    public void Executes() {
        CharactersGOGet().Execute(this);
    }

    public void Restart(Vector2I max_size) {
        TimeGoGet().Restart();
        LevelRestart();
        TilesGOSet(new TilesGO(max_size));
        TilesGoGet().Fill(TilesGO.TILE_TYPE.WALL);
        RoomsSet(MapGOGenerator.DefaultRoomsBuild(TilesGoGet().MaxSizeGet(), RandomGet()));
        MapGOGenerator.Brush(TilesGoGet(), RoomsGet());
        TunnelsSet(MapGOGenerator.DefaultTunnelsBuild(RoomsGet(), RandomGet(), this));
        MapGOGenerator.Brush(TilesGoGet(), TunnelsGet());
        CharactersGOSet(new CharactersGO());
    }

    public MapGO Init(Vector2I max_size, CharacterVisualGO character_visual_go, String character_name) {
        Restart(max_size);
        // персонаж создан только один раз и не рестартится
        CharactersGOGet().CharacterPlayerCreate(character_visual_go, character_name, (Vector2I)StartRoomGet().CenterGet(), this);
        return this;
    }
}
