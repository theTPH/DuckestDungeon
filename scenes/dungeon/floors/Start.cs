using Godot;
using System;

public class Start : Area2D
{
    [Signal]
    public delegate void StartUsed();
    public Global Global;
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        Connect("StartUsed", Global, "OnStartEntered");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("move_up") && GetOverlappingBodies().Count > 1)
        {
            // change to room
            EmitSignal(nameof(StartUsed));
        }
    }
}
