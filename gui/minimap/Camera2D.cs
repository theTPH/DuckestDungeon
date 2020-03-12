using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    public Sprite Target = null;

    public override void _Process(float delta)
    {
        // bind camera to target sprite
        if (Target != null)
        {
            Position = Target.Position;
        }
    }
}
