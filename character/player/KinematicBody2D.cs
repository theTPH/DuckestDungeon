using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
    // whole script dedicated to movement of player
    private int speed = 800;
    private float acceleration = 0.69f;
    private float friction = 0.1f;
    private Vector2 velocity = new Vector2();

    // animation
    public AnimatedSprite mySprite;

    public override void _Ready()
    {
        mySprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public void GetInput()
    {
        velocity = new Vector2();

        int dir = 0;

        if (Input.IsActionPressed("move_right"))
        {
            dir += 1;
            mySprite.FlipH = false;
        }
        if (Input.IsActionPressed("move_left"))
        {
            dir -= 1;
            mySprite.FlipH = true;
            
        }
        if (dir != 0)
        {
            velocity.x = Mathf.Lerp(velocity.x, dir * speed, acceleration);
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, friction);
        }
        
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);
    }
}
