using Godot;
using System;

public class Character : Node2D
{
    public int MaxHp = 0;
    public int Strength = 0;
    public int Agility = 0;

    public Character(int hp, int str, int agi)
    {
        MaxHp = hp;
        Strength = str;
        Agility = agi;

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
