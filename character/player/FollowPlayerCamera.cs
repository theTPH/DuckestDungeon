using Godot;
using System;

public class FollowPlayerCamera : Camera2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void SetLimitRigth(int pixel)
    {
        LimitRight = pixel;
    }
}
