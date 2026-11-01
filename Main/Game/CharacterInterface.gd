extends CanvasLayer

class_name CharacterInterface

var _character_go:RefCounted = null:
	set = _character_go_set, get = character_go_get;
func _character_go_set(value:RefCounted)->void:
	if _character_go and _character_go.IsPlayerGet():
		_character_go.MindTextSave($Editor.text_get());
	_character_go = value;
	if _character_go:
		$Editor.show();
		$Editor.text_set(_character_go.MindGoGet().TextGet(), _character_go.IsPlayerGet());
	else:
		$Editor.hide();
func character_go_get()->RefCounted:
	return _character_go;
func character_select(value:RefCounted)->void:
	self._character_go_set(value);

func _ready()->void:
	if GlobalSignal.connect("character_controller__click", Callable(self, "_on_character_click")) !=OK:
		push_error("character_controller__click not connected _on_character_click");
	if GlobalSignal.connect("editor__compile", Callable(self, "_on_editor_compile")) !=OK:
		push_error("editor__compile not connected _on_editor_compile");
	
func _on_character_click(character_go:RefCounted)->void:
	self._character_go_set(character_go);

func _on_editor_compile(text:String)->void:
	self.character_go_get().MindTextSave(text);
	var error:String = self.character_go_get().MindCompile();
	if error != "":
		GlobalSignal.emit_signal("mind__log", Editor.LOG_LEVEL.ERROR, error, self.character_go_get().MindErrorLineGet());
		return;
	var time = Time.get_ticks_msec();
	self.character_go_get().TestExecute();
	print('error: ',error,' time:', Time.get_ticks_msec() - time);
