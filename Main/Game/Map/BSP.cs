using Godot;
using System;
using System.Collections.Generic;

public partial class BSP : RefCounted
{

    const int MIN_LEAF_SIZE = 8;

    private Rect2 _rect;
    private void RectSet(Rect2 value){
       _rect = value; 
    }
    public Rect2 RectGet() {
        return _rect;
    }

    private BSP _left;
    private void LeftSet(BSP value) {
        _left = value;
    }
    private BSP LeftGet() {
        return _left;
    }
    
    private BSP _right;
    private void RightSet(BSP value) {
        _right = value;
    }
    private BSP RightGet() {
        return _right;
    }
    
    private double GetSizeBySplit(bool is_split_y){
        if (is_split_y == true) {
            return RectGet().Size.Y;
        }
        return RectGet().Size.X;
    }

    private bool Split(Random rand) {
        bool is_split_y = rand.Next(0,2) != 0;
        if (RectGet().Position.Y/RectGet().Position.X < 1.25) {
            is_split_y = true;
        }
        else if (RectGet().Position.X/RectGet().Position.Y < 1.25) {
            is_split_y = false;
        }

        double split_max_pos = GetSizeBySplit(is_split_y) - MIN_LEAF_SIZE - rand.Next(0, 2);
        if (split_max_pos < MIN_LEAF_SIZE) {
            return false;
        }
        int split_pos = rand.Next(MIN_LEAF_SIZE, (int)split_max_pos);
        if (is_split_y == true ){
            LeftSet(
                new BSP(
                    new Rect2(
                        RectGet().Position,
                        new Vector2(RectGet().Size.X, split_pos)
                    ),
                    rand
                )
            );
            Vector2 vec = new Vector2(0, split_pos);
            RightSet(
                new BSP(
                    new Rect2(
                        RectGet().Position + vec,
                        RectGet().Size - vec
                    ),
                    rand
                )
            ); 
        } else {
            LeftSet(
                new BSP(
                    new Rect2(
                        RectGet().Position,
                        new Vector2(split_pos, RectGet().Size.Y)
                    ),
                    rand
                )
            );
            Vector2 vec = new Vector2(split_pos, 0);
            RightSet(
                new BSP(
                    new Rect2(
                        RectGet().Position + vec,
                        RectGet().Size - vec
                    ),
                    rand
                )
            );
        }

        return true;
    }

    public void LastLeafRectsGet(List<Rect2> rects) {
        if (LeftGet() == null && RightGet() == null) {
            rects.Add(RectGet());
        } else {
            LeftGet().LastLeafRectsGet(rects);
            RightGet().LastLeafRectsGet(rects);
        }
    }

    public BSP(Rect2 rect, Random rand) {
        RectSet(rect);
        if (Split(rand) == false) {
        }
    }
}
