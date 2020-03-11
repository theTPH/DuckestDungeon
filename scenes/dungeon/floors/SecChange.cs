using Godot;
using System;

public class SecChange : Area2D
{
    [Signal]
    public delegate void SecChanged(int id);
    [Export]
    public int Id = 0;
    public Global Global;
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        Connect("SecChanged", Global, "OnSecExited");
    }

    public void OnSecAreaChanged(KinematicBody2D body)
    {
        bool exitedRight = body.Position.x > this.Position.x ? true : false;
        EmitSignal(nameof(SecChanged), exitedRight);
    }

}
