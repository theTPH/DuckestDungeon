using Godot;
using System;

public class Enemy : Character
{
    // private const int GRAVITY = 100;

    private Enemy() : base(25, 2, 2)
    {

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public int getAgility()
    {
        return Agility;
    }

    public int getStrength()
    {
        return Strength;
    }

    public int getMaxHp()
    {
        return MaxHp;
    }

    public int setMaxHp(int hp)
    {
        MaxHp = hp;
        return MaxHp;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
