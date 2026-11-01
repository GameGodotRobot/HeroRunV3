@tool
extends Node2D
class_name AnimationCharacter

@export var _is_direction:bool = false:
	set = _is_direction_set, get = is_direction_get;
func _is_direction_set(value:bool)->void:
	if _is_direction == value:
		return;
	_is_direction = value;
	if _is_direction:
		self.scale = Vector2(-1,1);
	else:
		self.scale = Vector2(1,1);
func is_direction_get()->bool:
	return _is_direction;
func direction_change(vec:Vector2)->void:
	self._is_direction_set(vec.x < 0);

func animation_play(anim:String)->void:
	$AnimTree/Anim.play(anim);

func visual_change(character_visual:RefCounted)->void:
	var node:Sprite2D = self.get_node_or_null("Body");
	if node:
		node.self_modulate = character_visual.DressColorGet();
	node = self.get_node_or_null("Body/Head");
	if node:
		node.self_modulate = character_visual.BodyColorGet();
	node = self.get_node_or_null("Body/Head/Details");
	if node:
		node.region_rect.position = character_visual.DetailsGet() as Vector2;
	node = self.get_node_or_null("Body/Head/Eyes");
	if node:
		node.self_modulate = character_visual.EyeColorGet();
	node = self.get_node_or_null("Body/Head/Hair");
	if node:
		node.self_modulate = character_visual.HairColorGet();
		node.region_rect.position = character_visual.HairstyleGet() as Vector2;
	node = self.get_node_or_null("LeftLeg");
	if node:
		node.self_modulate = character_visual.PantsColorGet();
	node = self.get_node_or_null("RightLeg");
	if node:
		node.self_modulate = character_visual.PantsColorGet();

func _ready()->void:
	pass
