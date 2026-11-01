extends Area2D

class_name CharacterController

var _character_go:RefCounted = null:
	set = _character_go_set, get = character_go_get;
func _character_go_set(value:RefCounted)->void:
	_character_go = value;
	$Model.visual_change(_character_go.CharacterVisualGOGet());
func character_go_get()->RefCounted:
	return _character_go;

func init(character_go:RefCounted)->void:
	self._character_go_set(character_go);
	self.position = Map.to_world_position(self.character_go_get().PositionGet());
	if character_go.connect("SaySignal", Callable(self, "_on_say")) != OK:
		push_error('SaySignal not connected _on_say');
	if character_go.connect("MoveSignal", Callable(self, "_on_move")) != OK:
		push_error('MoveSignal not connected _on_move');
	if character_go.connect("LogSignal", Callable(self, "_on_log")) != OK:
		push_error('LogSignal not connected _on_log');
	self._player_move_emit(true);

func _on_log(level:int, text:String, line:int)->void:
	GlobalSignal.emit_signal('mind__log', level, text, line);

func _player_move_emit(is_spawn:bool)->void:
	if self.character_go_get().IsPlayerGet():
		GlobalSignal.emit_signal('character_controller__player_move', self.global_position, is_spawn);

func _ready()->void:
	if self.connect("input_event", Callable(self, '_on_input_event')) != OK:
		push_error('input_event not connected _on_input_event');

func _on_say(text:String)->void:
	$Interface/Say.text = text;

func _on_move(pos:Vector2)->void:
	self.position = Map.to_world_position(pos);
	self._player_move_emit(false);

func _on_input_event(_viewport:Viewport, event:InputEvent, _shape_idx:int):
	if !(event is InputEventScreenTouch):
		return;
	if event.is_echo():
		return;
	if !event.is_released():
		return;
	GlobalSignal.emit_signal('character_controller__click', self.character_go_get());
