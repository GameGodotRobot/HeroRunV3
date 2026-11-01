extends Control

class_name Editor

enum LOG_LEVEL {
	INFO = 1,
	WARNING = 2,
	ERROR = 3,
}
const _LOG_COLORS:Dictionary = {
	LOG_LEVEL.INFO: "white",
	LOG_LEVEL.WARNING: "yellow",
	LOG_LEVEL.ERROR: "#ff6060",
}
const _ERROR_LINE_COLOR:Color = Color(1.0, 0.2, 0.2, 0.35);
const _STATE_TEMPLATE:String = "state init:\n\t";

var _error_lines:Array[int] = [];

func text_set(value:String, is_editable:bool)->void:
	if is_editable and value.strip_edges() == "":
		value = _STATE_TEMPLATE;
	$CodeEdit.text = value;
	$CodeEdit.editable = is_editable;
	$Compile.visible = is_editable;
	self._debug_clear();

func text_get()->String:
	return $CodeEdit.text;

func _ready()->void:
	if self.connect("gui_input", Callable(self, "_on_gui_input")) !=OK:
		push_error("gui_input not connected _on_gui_input");
	if $Compile.connect("pressed", Callable(self, "_on_compile_pressed")) !=OK:
		push_error("pressed not connected _on_compile_pressed");
	if $Debug.connect("meta_clicked", Callable(self, "_on_debug_meta_clicked")) !=OK:
		push_error("meta_clicked not connected _on_debug_meta_clicked");
	if $CodeEdit.connect("text_changed", Callable(self, "_error_lines_clear")) !=OK:
		push_error("text_changed not connected _error_lines_clear");
	if GlobalSignal.connect("mind__log", Callable(self, "_on_mind_log")) !=OK:
		push_error("mind__log not connected _on_mind_log");

func _gui_input(event:InputEvent)->void:
	if event.is_echo():
		return;
	self.accept_event();

func _on_gui_input(event:InputEvent):
	if event.is_echo():
		return;
	if !event.is_released():
		return;
	if event is InputEventScreenTouch:
		GlobalSignal.emit_signal("editor__click");

func _on_compile_pressed()->void:
	self._debug_clear();
	if !self._state_ensure():
		return;
	GlobalSignal.emit_signal("editor__compile", $CodeEdit.text);

func _state_ensure()->bool:
	var regex:RegEx = RegEx.create_from_string("(?m)^state\\s");
	if regex.search($CodeEdit.text):
		return true;
	var text:String = $CodeEdit.text.rstrip("\n\t ");
	if text != "":
		text += "\n\n";
	$CodeEdit.text = text + _STATE_TEMPLATE;
	var last:int = $CodeEdit.get_line_count() - 1;
	self._on_debug_meta_clicked(last);
	self._on_mind_log(LOG_LEVEL.INFO, "added \"state init:\" — write commands under it", last - 1);
	return false;

func _on_mind_log(level:int, text:String, line:int)->void:
	var color:String = _LOG_COLORS.get(level, "white");
	var prefix:String = "";
	if line >= 0:
		prefix = "[url=%d]line %d[/url]: " % [line, line + 1];
		if level == LOG_LEVEL.ERROR and line < $CodeEdit.get_line_count():
			$CodeEdit.set_line_background_color(line, _ERROR_LINE_COLOR);
			_error_lines.append(line);
	$Debug.append_text("[color=%s]%s%s[/color]\n" % [color, prefix, text.replace("[", "[lb]")]);

func _on_debug_meta_clicked(meta:Variant)->void:
	var line:int = int(meta);
	if line < 0 or line >= $CodeEdit.get_line_count():
		return;
	$CodeEdit.set_caret_line(line);
	$CodeEdit.set_caret_column($CodeEdit.get_line(line).length());
	$CodeEdit.grab_focus();

func _debug_clear()->void:
	$Debug.clear();
	self._error_lines_clear();

func _error_lines_clear()->void:
	for line in _error_lines:
		if line < $CodeEdit.get_line_count():
			$CodeEdit.set_line_background_color(line, Color(0, 0, 0, 0));
	_error_lines.clear();
