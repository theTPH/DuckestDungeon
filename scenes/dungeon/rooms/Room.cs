using Godot;
using System;

public class Room : Node2D
{
    private PackedScene _playerScene = (PackedScene)GD.Load("res://character/player/Player.tscn");
    private PackedScene _enemyScene = (PackedScene)GD.Load("res://character/enemy/Enemy.tscn");
    
    private Player player;
    private Enemy enemy;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        // for functioning combat system: nodes must be named "player" and "enemy"
        // instanciate  player and enemy in room scene
        player = (Player)_playerScene.Instance();
        enemy = (Enemy)_enemyScene.Instance();

        AddChild(player);
        AddChild(enemy);

        // initiate attributes for both characters -> needed for functioning combat
        player.init(50, 5, 5, "duckey", 1, 0, 0, 10);
        enemy.init(25, 3, 3);
 
    }

    // on click "action" -> combat will be performed (beta)
    public void GetInput()
    {
        TurnQueue turnQueue = new TurnQueue();
        turnQueue.setChars(player, enemy);

        if(Input.IsActionJustPressed("action"))
        {
            GD.Print("flag1");

            if (turnQueue.combat())
            {
                RemoveChild(player);
            }
            else
            {
                RemoveChild(enemy);
            } 
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
