extends Node

## camera
@warning_ignore("unused_signal")
signal camera__zoom_change(zoom);
@warning_ignore("unused_signal")
signal camera__position_change(position);

## MainMenuButtons
@warning_ignore("unused_signal")
signal main_menu_buttons__start_game;
@warning_ignore("unused_signal")
signal main_menu_buttons__load_game;

## CharacterSelector
@warning_ignore("unused_signal")
signal character_selector__new_game(character_visual_go, character_name);

## SplashScreens
@warning_ignore("unused_signal")
signal spash_screens__start;
@warning_ignore("unused_signal")
signal spash_screens__end;

## CharacterController
@warning_ignore("unused_signal")
signal character_controller__click(character_go);

## Map
@warning_ignore("unused_signal")
signal map__click;

## Editor
@warning_ignore("unused_signal")
signal editor__click;
@warning_ignore("unused_signal")
signal editor__compile(text);
