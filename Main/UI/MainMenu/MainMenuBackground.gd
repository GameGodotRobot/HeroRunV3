extends TextureRect

class_name MainMenuBackground

func _gui_input(event:InputEvent)->void:
	if event.is_echo():
		return;
	self.accept_event();
