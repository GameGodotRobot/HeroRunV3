extends Control

class_name CharacterSelector

const CHARACTER_VISUAL_GO = preload("res://Main/Game/Character/CharacterVisualGO.cs");
const CHARACTER_GO = preload("res://Main/Game/Character/CharacterGO.cs");

var _character_visual_go:RefCounted = CHARACTER_VISUAL_GO.new();

func _ready()->void:
	if $NewGame.connect("pressed", Callable(self,"_on_new_game")) != OK:
		push_error("pressed not connected _on_new_game");
	if $Panel/Race.connect("value_changed", Callable(self,"_on_race_value_changed")) != OK:
		push_error("value_changed not connected _on_race_value_changed");
	if $Panel/Sliders/HairColor.connect("value_changed", Callable(self,"_on_hair_color_value_changed")) != OK:
		push_error("value_changed not connected _on_hair_color_value_changed");
	if $Panel/Sliders/EyeColor.connect("value_changed", Callable(self,"_on_eye_color_value_changed")) != OK:
		push_error("value_changed not connected _on_eye_color_value_changed");
	if $Panel/Sliders/BodyColor.connect("value_changed", Callable(self,"_on_body_color_value_changed")) != OK:
		push_error("value_changed not connected _on_body_color_value_changed");
	if $Panel/Sliders/DressColor.connect("value_changed", Callable(self,"_on_dress_color_value_changed")) != OK:
		push_error("value_changed not connected _on_dress_color_value_changed");
	if $Panel/Sliders/PantsColor.connect("value_changed", Callable(self,"_on_pants_color_value_changed")) != OK:
		push_error("value_changed not connected _on_pants_color_value_changed");
	if $Panel/Sliders/Hairstyle.connect("value_changed", Callable(self,"_on_hairstyle_value_changed")) != OK:
		push_error("value_changed not connected _on_pants_color_value_changed");
	if $Panel/Sliders/Details.connect("value_changed", Callable(self,"_on_details_value_changed")) != OK:
		push_error("value_changed not connected _on_pants_color_value_changed");
	self._on_race_value_changed(_character_visual_go.RaceGet()-1);

func _on_new_game()->void:
	if !$Panel/LineEdit.validation($Panel/LineEdit.text):
		return;
	$Panel/Sliders.hide();
	GlobalSignal.emit_signal("character_selector__new_game", _character_visual_go, $Panel/LineEdit.text);

func _on_race_value_changed(value:int)->void:
	_character_visual_go.RaceSet(value+1);
	$Panel/CharacterPodium.race_set(_character_visual_go.RaceGet());
	$Panel/Sliders/Hairstyle.init(_character_visual_go.HairstyleTypesSize()-1);
	$Panel/Sliders/HairColor.init(_character_visual_go.HairColorTypesSize()-1);
	$Panel/Sliders/EyeColor.init(_character_visual_go.EyeColorTypesSize()-1);
	$Panel/Sliders/BodyColor.init(_character_visual_go.BodyColorTypesSize()-1);
	$Panel/Sliders/DressColor.init(_character_visual_go.DressColorTypesSize()-1);
	$Panel/Sliders/PantsColor.init(_character_visual_go.PantsColorTypesSize()-1);
	$Panel/Sliders/Details.init(_character_visual_go.DetailsTypesSize()-1);
	
	await get_tree().process_frame;
	$Panel/CharacterPodium.visual_change(_character_visual_go);

func _on_hair_color_value_changed(value:int)->void:
	_character_visual_go.HairColorSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_eye_color_value_changed(value:int)->void:
	_character_visual_go.EyeColorSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_body_color_value_changed(value:int)->void:
	_character_visual_go.BodyColorSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_dress_color_value_changed(value:int)->void:
	_character_visual_go.DressColorSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_pants_color_value_changed(value:int)->void:
	_character_visual_go.PantsColorSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_hairstyle_value_changed(value:int)->void:
	_character_visual_go.HairstyleSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
func _on_details_value_changed(value:int)->void:
	_character_visual_go.DetailsSetByTypeId(value);
	$Panel/CharacterPodium.visual_change(_character_visual_go);
