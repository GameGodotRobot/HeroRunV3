using Godot;
using System;

public partial class AStarPoint : RefCounted
{
    private Vector2I _point;
    private void PointSet(Vector2I value) {
        _point = value;
    }
    public Vector2I PointGet() {
        return _point;
    }

    private int _weight;
    private void WeightSet(int value) {
        _weight = value;
    }
    public int WeightGet() {
        return _weight;
    }
    
    private double _result;
    private void ResultSet(double value) {
        _result = value;
    }
    public double ResultGet() {
        return _result;
    }

    private AStarPoint _fpoint;
    private void FPointSet(AStarPoint value) {
        _fpoint = value;
    }
    public AStarPoint FPointGet() {
        return _fpoint;
    }

    private bool _is_ignore;
    private void IsIgnoreSet(bool value) {
        _is_ignore = value;
    }
    public bool IsIgnoreGet() {
        return _is_ignore;
    }

    public AStarPoint Init(
                Vector2I point,
                int weight = 0, 
                AStarPoint fpoint = null,
                double result = 0.0,
                bool is_ignore = false) {
        PointSet(point);
        WeightSet(weight);
        FPointSet(fpoint);
        ResultSet(result);
        IsIgnoreSet(is_ignore);
        return this;
    }

    public AStarPoint(
                Vector2I point,
                int weight = 0, 
                AStarPoint fpoint = null,
                double result = 0.0,
                bool is_ignore = false) {
        PointSet(point);
        WeightSet(weight);
        FPointSet(fpoint);
        ResultSet(result);
        IsIgnoreSet(is_ignore);
    }
}
