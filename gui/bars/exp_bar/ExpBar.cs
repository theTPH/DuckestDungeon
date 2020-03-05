using Godot;
using System;

public class ExpBar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        ValueName = "XP";
    }

}
