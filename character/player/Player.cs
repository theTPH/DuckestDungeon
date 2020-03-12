using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 Speed = new Vector2(300, 1000);
    float Gravity;
    Vector2 Velocity;
    public FollowPlayerCamera Camera;
    private AnimatedSprite mySprite;

   
    public override void _Ready()
    {
        Camera = GetNode<FollowPlayerCamera>("FollowPlayerCamera");
        mySprite = GetNode<AnimatedSprite>("Player");
        Speed = new Vector2(300, 1000);
        Gravity = 3000f;
        Velocity = Vector2.Zero;            
    }

    public override void _Process(float delta)
    {
        // check for input permanently and update velocity
        Vector2 direction = GetDirection();
        Velocity = Speed * direction;
        Velocity.y += Gravity * delta;
        Velocity = MoveAndSlide(Velocity); 
    }

    public Vector2 GetDirection()
    {
            // flip player sprite based on vector result from input
            float vectorResult = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"); 
            if (vectorResult < 0)
            {
                mySprite.FlipH = true;
            }
            else if (vectorResult > 0)
            {
                mySprite.FlipH = false;
            }       
            return new Vector2(vectorResult, 1);
    }

    // get x coordinate if player body
    public float GetPositionX()
    {
        return Position.x;
    }
}
