using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    public Sprite Target = null;

    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        if (Target != null)
        {
            Position = Target.Position;
        }
    }
}
