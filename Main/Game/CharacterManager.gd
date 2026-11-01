@tool
extends Node2D
class_name CharacterManager

enum RACES {
	HUMAN = 1,
	ORK = 2,
}
@export var _RACES_PCKG:Array[PackedScene];

@warning_ignore("unused_parameter")
func config_load(characters:RefCounted)->void:
	for character in characters.CharactersGetArray():
		self._create_character(character);

func _create_character(character_go:RefCounted)->void:
	var cc:CharacterController = _RACES_PCKG[character_go.CharacterVisualGOGet().RaceGet()].instantiate();
	self.add_child(cc);
	cc.init(character_go);
