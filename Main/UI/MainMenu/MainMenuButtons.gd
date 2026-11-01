extends Control

class_name MainMenuButtons

func _ready()->void:
	if $Start.connect("pressed", Callable(self,"_on_start_pressed")) != OK:
		push_error("pressed not connected _on_start_pressed");
	if $Load.connect("pressed", Callable(self,"_on_load_pressed")) != OK:
		push_error("pressed not connected _on_load_pressed");
	if $Exit.connect("pressed", Callable(self,"_on_exit_pressed")) != OK:
		push_error("pressed not connected _on_exit_pressed");

func _on_start_pressed()->void:
	GlobalSignal.emit_signal("main_menu_buttons__start_game");

func _on_load_pressed()->void:
	GlobalSignal.emit_signal("main_menu_buttons__load_game");

func _on_exit_pressed()->void:
	self.get_tree().quit();
