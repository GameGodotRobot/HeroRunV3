extends Node

class_name Main

func _ready()->void:
	if GlobalSignal.connect("character_selector__new_game", Callable(self,"_on_new_game")) != OK:
		push_error("character_selector__new_game not connected _on_new_game");
	$MainMenu.start();

func _on_new_game(character_visual_go:RefCounted, character_name:String)->void:
	await $SplashScreens.start($SplashScreens.SPLASH_SCREENS.DARK);
	$MainMenu.end();
	$Game.start(character_visual_go, character_name);
	await $SplashScreens.end();
