@tool
extends CanvasLayer
class_name MainMenu

enum PANEL {
	NONE = 0,
	BUTTONS = 1,
	CHARACTER_SELECTOR = 2
}
var _current_panel:PANEL = PANEL.NONE:
	set = _current_panel_set, get = current_panel_get;
func _current_panel_set(value:PANEL)->void:
	if _current_panel == value:
		return;
	_current_panel = value;
	match _current_panel:
		PANEL.BUTTONS:
			$MainMenuButtons.show();
			$CharacterSelector.hide();
		PANEL.CHARACTER_SELECTOR:
			$MainMenuButtons.hide();
			$CharacterSelector.show();
func current_panel_get()->PANEL:
	return _current_panel;

func start()->void:
	self.show();
func end()->void:
	self.hide();

func _ready()->void:
	if GlobalSignal.connect("main_menu_buttons__start_game", Callable(self,"_on_start_game")) != OK:
		push_error("main_menu_buttons__start_game not connected _on_start_game");
	if GlobalSignal.connect("main_menu_buttons__load_game", Callable(self,"_on_load_game")) != OK:
		push_error("main_menu_buttons__load_game not connected _on_load_game");
	self._current_panel_set(PANEL.BUTTONS);

func _on_start_game()->void:
	self._current_panel_set(PANEL.CHARACTER_SELECTOR);

func _on_load_game()->void:
	pass
