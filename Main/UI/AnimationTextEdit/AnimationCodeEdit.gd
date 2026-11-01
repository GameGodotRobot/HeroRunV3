@tool
extends CodeEdit
class_name AnimationCodeEdit

@export_group("Animation")
var _start_size:Vector2;
var _start_position:Vector2;

@export var _is_animation:bool = false:
	set = _is_animation_set, get = is_animation_get;
func _is_animation_set(value:bool)->void:
	_is_animation = value;
func is_animation_get()->bool:
	return _is_animation;

enum EVENT_MODE {
	MOUSE_ENTER = 1
}
@export var _event_mode:EVENT_MODE = EVENT_MODE.MOUSE_ENTER:
	set = _event_mode_set, get = event_mode_get;
func _event_mode_set(value:EVENT_MODE)->void:
	_event_mode = value;
func event_mode_get()->EVENT_MODE:
	return _event_mode;
	
enum LENGHT_MODE {
	RIGHT = 1,
	LEFT = 2,
	CENTER = 3,
}
@export var _lenght_mode:LENGHT_MODE = LENGHT_MODE.RIGHT:
	set = _lenght_mode_set, get = lenght_mode_get;
func _lenght_mode_set(value:LENGHT_MODE)->void:
	_lenght_mode = value;
func lenght_mode_get()->LENGHT_MODE:
	return _lenght_mode;

@export_range(0.0,2.0,0.01) var _animation_lenght_from:float = 1.0;
var _lenght_size:float = 1.0:
	set = _lenght_size_set, get = lenght_size_get;
func _lenght_size_set(value:float)->void:
	_lenght_size = value;
	self._position_and_size_calculate();
func lenght_size_get()->float:
	return _lenght_size;
func lenght_size_anim(value:float, time:float)->void:
	if !_is_animation:
		return;
	TweenSingleton.tween_get(self,'lenght_size').tween_method(
		Callable(self, "_lenght_size_set"),
		self.lenght_size_get(),
		value,
		time
	).set_trans(
		Tween.TRANS_CUBIC
	).set_ease(
		Tween.EASE_OUT
	);

func _position_and_size_calculate()->void:
	if !_is_animation:
		return;
	match self.lenght_mode_get():
		LENGHT_MODE.RIGHT:
			self.size.x = _start_size.x*_lenght_size;
		LENGHT_MODE.CENTER:
			self.size.x = _start_size.x*_lenght_size;
			self.position.x = _start_position.x-self.size.x/2.0+_start_size.x/2.0;
		LENGHT_MODE.LEFT:
			self.size.x = _start_size.x*_lenght_size;
			self.position.x = _start_position.x-self.size.x+_start_size.x;


func _ready()->void:
	_start_size = self.size;
	_start_position = self.position;
	if self.connect("mouse_entered", Callable(self,"_on_mouse_entered")) != OK:
		push_error("mouse_entered not connected _on_mouse_entered");
	if self.connect("mouse_exited", Callable(self,"_on_mouse_exited")) != OK:
		push_error("mouse_entered not connected _on_mouse_entered");

func _on_mouse_entered()->void:
	if self.event_mode_get() != EVENT_MODE.MOUSE_ENTER:
		return;
	self.lenght_size_anim(_animation_lenght_from, 0.4);
	

func _on_mouse_exited()->void:
	if self.event_mode_get() != EVENT_MODE.MOUSE_ENTER:
		return;
	self.lenght_size_anim(1.0, 0.4);
