extends TileMapLayer

class_name Map

const CELL_SIZE:Vector2i = Vector2i(32,32);
static func to_world_position(mpos:Vector2i)->Vector2:
	return (mpos*CELL_SIZE as Vector2)+CELL_SIZE/2.0;

func config_load(tiles:RefCounted)->void:
	var floors:Array = [];
	var walls:Array = [];
	for x in range(0, tiles.MaxSizeGet().x+1):
		for y in range(0, tiles.MaxSizeGet().y+1):
			match tiles.TileGet(Vector2i(x,y)):
				1: floors.append(Vector2i(x,y));
				2: walls.append(Vector2i(x,y));
	self.set_cells_terrain_connect(floors, 0, 0);
	self.set_cells_terrain_connect(walls, 0, 1);
