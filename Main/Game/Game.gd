@tool
extends Node2D

const MAP_GO = preload("res://Main/Game/Map/MapGO.cs");

var _map_go:RefCounted:
	set = _map_go_set, get = map_go_get;
func _map_go_set(value:RefCounted)->void:
	_map_go = value;
func map_go_get()->RefCounted:
	return _map_go;

func _ready()->void:
	pass

func start(character_visual_go:RefCounted, character_name:String)->void:
	var time:int = Time.get_ticks_msec();
	self._map_go_set(MAP_GO.new().Init(Vector2i(64, 64), character_visual_go, character_name));
	print(Time.get_ticks_msec()-time);
	$Map.config_load(self.map_go_get().TilesGoGet());
	$CharacterManager.config_load(self.map_go_get().CharactersGOGet());
	self.show();

func end()->void:
	self.hide();
