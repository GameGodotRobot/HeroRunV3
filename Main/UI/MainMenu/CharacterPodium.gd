@tool
extends Control
class_name CharacterPodium

enum RACES {
	HUMAN = 1,
	ORK = 2,
}
@export var _RACES_PCKG:Array[PackedScene];

@export var _race:RACES = RACES.HUMAN:
	set = race_set, get = race_get;
func race_set(value:RACES)->void:
	if _race == value:
		return;
	_race = value;
	_races_load(_race);
func race_get()->RACES:
	return _race;
func _races_load(races:RACES)->void:
	self.clear();
	var ac:AnimationCharacter = _RACES_PCKG[races].instantiate();
	self.add_child(ac);
	ac.owner = get_tree().edited_scene_root;
	ac.scale = Vector2(10,10);
func clear()->void:
	for child in self.get_children():
		child.queue_free();

func visual_change(cvr:RefCounted)->void:
	self.get_child(0).visual_change(cvr);

func _ready()->void:
	self.race_set(RACES.HUMAN);
