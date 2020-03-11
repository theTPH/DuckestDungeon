using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 Speed = new Vector2(300, 1000);
    float Gravity;
    Vector2 Velocity;
    public FollowPlayerCamera Camera;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Camera = GetNode<FollowPlayerCamera>("FollowPlayerCamera");
        Speed = new Vector2(300, 1000);
        Gravity = 3000f;
        Velocity = Vector2.Zero;            
        GD.Print(Camera.GetType());
    }

    public override void _Process(float delta)
    {
        Vector2 direction = GetDirection();
        Velocity = Speed * direction;
        Velocity.y += Gravity * delta;
        Velocity = MoveAndSlide(Velocity); 
    }

    public Vector2 GetDirection()
    {
            return new Vector2(
            Input.GetActionStrength("move_right") - 
            Input.GetActionStrength("move_left"),
            1
        );
    }

    public float GetPositionX()
    {
        return Position.x;
    }
}
