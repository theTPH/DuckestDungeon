using Godot;
using System;

public class Floor : Node2D
{
    private Player myPlayer;
    private Camera2D myPlayerCamera;
    private Area2D Start;

    public override void _Ready()
    {
        myPlayer = GetNode<Player>("Player");
        myPlayer.Camera.SetLimitRigth(3800);
    }
}
