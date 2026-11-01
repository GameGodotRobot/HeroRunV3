extends Camera2D

class_name Camera

const _MAX_ZOOM:float = 4.5;
const _MIN_ZOOM:float = 0.2;
const _ZOOM_STEP:float = 1.1;
const _PAN_GESTURE_ZOOM_SPEED:float = 0.05;
const _FOLLOW_SPEED:float = 8.0;

var _start_pos:Vector2:
	set= _start_pos_set, get = start_pos_get;
func _start_pos_set(value:Vector2)->void:
	_start_pos = value;
func start_pos_get()->Vector2:
	return _start_pos;

var _target_pos:Vector2;
var _has_target:bool = false;

func _ready()->void:
	self._start_pos_set(self.global_position);
	self.position_smoothing_enabled = true;
	self.position_smoothing_speed = _FOLLOW_SPEED;
	if GlobalSignal.connect("character_controller__player_move", Callable(self, "_on_player_move")) != OK:
		push_error("character_controller__player_move not connected _on_player_move");

func _on_player_move(pos:Vector2, is_spawn:bool)->void:
	_target_pos = pos;
	_has_target = true;
	self._cam_set(pos);
	if is_spawn:
		self.reset_smoothing();

var _multitouch:Dictionary = {};
func _event_add(event:InputEventScreenDrag)->void:
	_multitouch[event.index] = event;
func _event_del(event:InputEventScreenTouch)->void:
	if !_multitouch.erase(event.index):
		pass;
func _event_get(indx:int)->InputEventScreenDrag:
	return _multitouch.get(indx);
func event_size()->int:
	return _multitouch.size();

var _last_gesture:String = "";
var _last_gesture_frame:int = -1;
func _is_duplicate_gesture(event:InputEventGesture)->bool:
	var text:String = event.as_text();
	var frame:int = Engine.get_process_frames();
	if text == _last_gesture and frame == _last_gesture_frame:
		return true;
	_last_gesture = text;
	_last_gesture_frame = frame;
	return false;

func _unhandled_input(event:InputEvent)->void:
	if event.is_echo():
		return;
	if (event is InputEventMouseButton or event is InputEventGesture) and self.get_viewport().gui_get_hovered_control() != null:
		return;
	if event is InputEventMouseButton and event.is_pressed():
		if event.is_action("scroll_up"):
			self._cam_zoom(_ZOOM_STEP);
		elif event.is_action("scroll_down"):
			self._cam_zoom(1.0/_ZOOM_STEP);
	if event is InputEventGesture and self._is_duplicate_gesture(event):
		return;
	if event is InputEventMagnifyGesture:
		self._cam_zoom(event.factor);
	if event is InputEventPanGesture:
		self._cam_zoom(1.0 - event.delta.y*_PAN_GESTURE_ZOOM_SPEED);
	if event is InputEventKey and event.is_pressed() and event.keycode == KEY_SPACE and _has_target:
		self._cam_set(_target_pos);
	if event is InputEventScreenDrag:
		self._event_add(event);
		if self.event_size() == 1:
			self._cam_move(event.relative);
		else:
			var first:InputEventScreenDrag = self._event_get(0);
			var second:InputEventScreenDrag = self._event_get(1);
			var first_new_pos:Vector2 = first.position+first.relative;
			var second_new_pos:Vector2 = second.position+second.relative;
			if second_new_pos.distance_to(second.position)<first_new_pos.distance_to(first.position):
				self._cam_zoom(_ZOOM_STEP);
			else:
				self._cam_zoom(1.0/_ZOOM_STEP);
	if event is InputEventScreenTouch:
		if !event.is_pressed():
			self._event_del(event);

func _cam_move(relative:Vector2)->void:
	self._cam_set(self.global_position - relative*(1.0/self.zoom.x)*Config.MOUSE_SENSETIVITY);

func _cam_set(pos:Vector2)->void:
	self.global_position = self._validate_position(pos);
	GlobalSignal.emit_signal("camera__position_change", self.global_position);

func _validate_position(vec:Vector2)->Vector2:
	vec.x = max(vec.x, self.limit_left+self.get_viewport_rect().size.x/(self.zoom.x*2.0));
	vec.x = min(vec.x, self.limit_right-self.get_viewport_rect().size.x/(self.zoom.x*2.0));
	vec.y = max(vec.y, self.limit_top+self.get_viewport_rect().size.y/(self.zoom.x*2.0));
	vec.y = min(vec.y, self.limit_bottom-self.get_viewport_rect().size.y/(self.zoom.x*2.0));
	return vec;

func _cam_zoom(factor:float)->void:
	var value:float = clamp(self.zoom.x*factor, _MIN_ZOOM, _MAX_ZOOM);
	if is_equal_approx(value, self.zoom.x):
		return;
	self.zoom = Vector2.ONE*value;
	GlobalSignal.emit_signal("camera__zoom_change", self.zoom);
