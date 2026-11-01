extends AnimationLineEdit

class_name ValidationTextEdit

func _ready()->void:
	super();

func validation(value:String)->bool:
	if value.length() < 3:
		return false;
	return true;
