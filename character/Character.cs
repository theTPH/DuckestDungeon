using Godot;
using System;

public class Character : Node2D
{
    public int MaxHp = 0;
    public int Strength = 0;
    public int Agility = 0;

    // physics
    public KinematicBody2D body;
    private Vector2 velocity = new Vector2();
    private const int GRAVITY = 100;

    public Character(int hp, int str, int agi)
    {
        MaxHp = hp;
        Strength = str;
        Agility = agi;
    }

    public Character()
    {

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        body = GetNode<KinematicBody2D>("KinematicBody2D");
        GD.Print(body);
        GD.Print("test");
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity.y += GRAVITY;
        if(body != null)
        {
            velocity = body.MoveAndSlide(velocity);
        }
        
    }
}
