using Godot;
using System;

public class Room : Node2D
{
    private PackedScene _playerScene = (PackedScene)GD.Load("res://character/player/Player.tscn");
    private PackedScene _enemyScene = (PackedScene)GD.Load("res://character/enemy/Enemy.tscn");


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Node player = _playerScene.Instance();
        Node enemy = _enemyScene.Instance();
        AddChild(player);
        AddChild(enemy);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
