using Godot;
using System;

public partial class TimeGO : RefCounted
{
    private int _time = 0;
    private void TimeSet(int value) {
        _time = value;
    }
    public int TimeGet() {
        return _time;
    }
    public void TimeChange() {
        TimeSet(TimeGet() + 1);
    }

    private bool _is_paused = true;
    public void IsPausedSet(bool value) {
        _is_paused = value;
    }
    public bool IsPausedGet() {
        return _is_paused;
    }
    
    public void Restart() {
        _time = 0;
        _is_paused = true;
    }
    
    public TimeGO() {
    }
}
