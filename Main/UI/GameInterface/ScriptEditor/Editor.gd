extends Control

class_name Editor

func text_set(value:String, is_editable:bool)->void:
	$CodeEdit.text = value;
	$CodeEdit.editable = is_editable;
	$Compile.visible = is_editable;

func text_get()->String:
	return $CodeEdit.text;

func _ready()->void:
	if self.connect("gui_input", Callable(self, "_on_gui_input")) !=OK:
		push_error("gui_input not connected _on_gui_input");
	if $Compile.connect("pressed", Callable(self, "_on_compile_pressed")) !=OK:
		push_error("pressed not connected _on_compile_pressed");

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
	GlobalSignal.emit_signal("editor__compile", $CodeEdit.text);
