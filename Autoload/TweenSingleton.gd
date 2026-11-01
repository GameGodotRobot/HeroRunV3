extends RefCounted

class_name TweenSingleton

static func tween_get(node:Node, prop:String)->Tween:
	var tween:Tween;
	if node.has_meta(&"tween_%s"%[prop]):
		tween = node.get_meta(&"tween_%s"%[prop]);
	if tween:
		tween.kill();
	tween = node.create_tween().bind_node(node);
	node.set_meta(&"tween_%s"%[prop],tween);
	return tween;
