using Godot;
using System;

public class Room : Node2D
{
    private PackedScene _playerScene = (PackedScene)GD.Load("res://character/player/Player.tscn");
    private PackedScene _enemyScene = (PackedScene)GD.Load("res://character/enemy/Enemy.tscn");
    
    public Player player;
    public Enemy enemy;
    
    public int CooldownSpecial = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        // for functioning combat system: nodes must be named "player" and "enemy"
        player = (Player)_playerScene.Instance();
        enemy = (Enemy)_enemyScene.Instance();

        /*
        player = new Player("duckey", 1, 0.0, 0.0, 10.0);
        enemy = new Enemy();
        */

        AddChild(player);
        AddChild(enemy);

        player.init(50, 5, 5, "duckey", 1, 0, 0, 10);
        enemy.init(25, 3, 3);

        TurnQueue turnQueue = new TurnQueue();
        turnQueue.setChars(player, enemy);

        if (turnQueue.combat())
        {
            RemoveChild(player);
        }
        else
        {
            RemoveChild(enemy);
        }    
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
