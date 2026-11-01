extends Control

class_name Rulers

const _FONT:Font = preload("res://Res/Font/Bold.otf");
const _FONT_SIZE:int = 11;
const _THICKNESS:float = 24.0;
const _LABEL_SPACING_X:float = 30.0;
const _LABEL_SPACING_Y:float = 18.0;
const _LABEL_STEPS:Array[int] = [1, 2, 5, 10, 20, 50];
const _BG_COLOR:Color = Color(0.05, 0.03, 0.06, 0.6);
const _TICK_COLOR:Color = Color(1.0, 1.0, 1.0, 0.2);
const _LABEL_COLOR:Color = Color(1.0, 1.0, 1.0, 0.5);
const _PLAYER_COLOR:Color = Color(0.96, 0.77, 0.26, 1.0);
const _PLAYER_TEXT_COLOR:Color = Color(0.12, 0.08, 0.12, 1.0);
const _HOVER_COLOR:Color = Color(1.0, 1.0, 1.0, 0.9);
const _HOVER_TILE_COLOR:Color = Color(1.0, 1.0, 1.0, 0.45);

var _map_rect:Rect2i = Rect2i(0, 0, 0, 0);
var _mouse:Vector2 = Vector2(-1, -1);
var _player_tile:Vector2i = Vector2i.ZERO;
var _player_box:StyleBoxFlat;
var _hover_box:StyleBoxFlat;

func _ready()->void:
	self.mouse_filter = Control.MOUSE_FILTER_IGNORE;
	self.hide();
	_player_box = self._box_create(_PLAYER_COLOR, Color(0, 0, 0, 0));
	_hover_box = self._box_create(_BG_COLOR, _HOVER_COLOR);
	if GlobalSignal.connect("character_controller__player_move", Callable(self, "_on_player_move")) != OK:
		push_error("character_controller__player_move not connected _on_player_move");

func _box_create(bg:Color, border:Color)->StyleBoxFlat:
	var box:StyleBoxFlat = StyleBoxFlat.new();
	box.bg_color = bg;
	box.border_color = border;
	box.set_border_width_all(1 if border.a > 0 else 0);
	box.set_corner_radius_all(4);
	return box;

func _on_player_move(pos:Vector2, is_spawn:bool)->void:
	_player_tile = Vector2i((pos/Vector2(Map.CELL_SIZE)).floor());
	if is_spawn:
		var game:Node = self.get_tree().root.find_child("Game", true, false);
		if game:
			_map_rect = Rect2i(Vector2i.ZERO, game.map_go_get().TilesGoGet().MaxSizeGet());
	self.show();

func _input(event:InputEvent)->void:
	if event is InputEventMouseMotion:
		_mouse = event.position;

func _process(_delta:float)->void:
	if self.visible:
		self.queue_redraw();

func _draw()->void:
	var xf:Transform2D = self.get_viewport().get_canvas_transform();
	var cell:Vector2 = Vector2(Map.CELL_SIZE)*xf.get_scale();
	var origin:Vector2 = xf.origin;
	var view:Vector2 = self.size;
	var hover:Vector2i = Vector2i(((xf.affine_inverse()*_mouse)/Vector2(Map.CELL_SIZE)).floor());
	var is_hover:bool = _map_rect.has_point(hover) and hover != _player_tile and self.get_viewport().gui_get_hovered_control() == null;
	if _mouse.y < _THICKNESS or _mouse.x > view.x - _THICKNESS:
		is_hover = false;

	if is_hover:
		self.draw_rect(Rect2(origin + Vector2(hover)*cell, cell), _HOVER_TILE_COLOR, false, 1.5);
	self._player_label_draw(origin, cell);

	self.draw_rect(Rect2(0, 0, view.x - _THICKNESS, _THICKNESS), _BG_COLOR);
	self.draw_rect(Rect2(view.x - _THICKNESS, 0, _THICKNESS, view.y), _BG_COLOR);

	var step_x:int = self._label_step(cell.x, _LABEL_SPACING_X);
	for x in range(_map_rect.position.x, _map_rect.end.x):
		var left:float = origin.x + x*cell.x;
		if left + cell.x < 0 or left > view.x - _THICKNESS:
			continue;
		if cell.x >= 6.0:
			self.draw_line(Vector2(left, _THICKNESS - 4), Vector2(left, _THICKNESS), _TICK_COLOR);
		if x % step_x == 0:
			self._text_draw(str(x), Vector2(left + cell.x/2.0, _THICKNESS/2.0), _LABEL_COLOR);

	var step_y:int = self._label_step(cell.y, _LABEL_SPACING_Y);
	for y in range(_map_rect.position.y, _map_rect.end.y):
		var top:float = origin.y + y*cell.y;
		if top + cell.y < _THICKNESS or top > view.y:
			continue;
		if cell.y >= 6.0:
			self.draw_line(Vector2(view.x - _THICKNESS, top), Vector2(view.x - _THICKNESS + 4, top), _TICK_COLOR);
		if y % step_y == 0:
			self._text_draw(str(y), Vector2(view.x - _THICKNESS/2.0, top + cell.y/2.0), _LABEL_COLOR);

	if is_hover:
		self._marker_draw(str(hover.x), Vector2(origin.x + (hover.x + 0.5)*cell.x, _THICKNESS/2.0), _hover_box, _HOVER_COLOR);
		self._marker_draw(str(hover.y), Vector2(view.x - _THICKNESS/2.0, origin.y + (hover.y + 0.5)*cell.y), _hover_box, _HOVER_COLOR);
	self._marker_draw(str(_player_tile.x), Vector2(origin.x + (_player_tile.x + 0.5)*cell.x, _THICKNESS/2.0), _player_box, _PLAYER_TEXT_COLOR);
	self._marker_draw(str(_player_tile.y), Vector2(view.x - _THICKNESS/2.0, origin.y + (_player_tile.y + 0.5)*cell.y), _player_box, _PLAYER_TEXT_COLOR);

func _player_label_draw(origin:Vector2, cell:Vector2)->void:
	var center:Vector2 = origin + (Vector2(_player_tile) + Vector2(0.5, 1.0))*cell + Vector2(0, 10);
	self._marker_draw("%d, %d" % [_player_tile.x, _player_tile.y], center, _player_box, _PLAYER_TEXT_COLOR);

func _label_step(cell:float, spacing:float)->int:
	for step in _LABEL_STEPS:
		if step*cell >= spacing:
			return step;
	return _LABEL_STEPS[-1];

func _text_size(text:String)->Vector2:
	return _FONT.get_string_size(text, HORIZONTAL_ALIGNMENT_LEFT, -1, _FONT_SIZE);

func _text_draw(text:String, center:Vector2, color:Color)->void:
	var text_size:Vector2 = self._text_size(text);
	var baseline:float = center.y + (_FONT.get_ascent(_FONT_SIZE) - _FONT.get_descent(_FONT_SIZE))/2.0;
	self.draw_string(_FONT, Vector2(center.x - text_size.x/2.0, baseline), text, HORIZONTAL_ALIGNMENT_LEFT, -1, _FONT_SIZE, color);

func _marker_draw(text:String, center:Vector2, box:StyleBoxFlat, color:Color)->void:
	var marker_size:Vector2 = self._text_size(text) + Vector2(10, 4);
	marker_size.x = max(marker_size.x, 20.0);
	self.draw_style_box(box, Rect2(center - marker_size/2.0, marker_size));
	self._text_draw(text, center, color);
