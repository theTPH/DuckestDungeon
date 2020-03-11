using Godot;
using System;

public class End : Area2D
{   
    [Signal]
    public delegate void EndUsed();
    public Global Global;
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        Connect("EndUsed", Global, "OnEndEntered");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("move_up") && GetOverlappingBodies().Count > 1)
        {
            // change to room
            GD.Print("Player is on end...");
            EmitSignal(nameof(EndUsed));
        }
    }
}
