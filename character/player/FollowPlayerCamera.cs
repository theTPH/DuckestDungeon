using Godot;
using System;

public class FollowPlayerCamera : Camera2D
{
    // set maximum camera frame
    public void SetLimitRigth(int pixel)
    {
        LimitRight = pixel;
    }
}
