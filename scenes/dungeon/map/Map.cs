using Godot;
using System;

public class Map : Node2D
{
    public int RoomId;
    public Sprite PlayerLocation;
    public Global Global;
    
    public enum Orientation
    {        
        LeftRight,
        UpDown
    }
    
    private Orientation myMovement;

    private const string WORLD_FLOOR_SCENE = "res://scenes/dungeon/floors/Floor.tscn";
    private const string GUI_FLOOR_SCENE = "res://gui/dungeon_menu/DungeonMenu.tscn";
    private int myTileSize;



    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        RoomId = 1;
        myTileSize = 64;
        PlayerLocation = GetNode<Sprite>("PlayerLocation");
        myMovement = Orientation.LeftRight;
        Global.SwitchRoomMode = true;
    }

    public void OnAreaClicked(Vector2 position)
    {
        if (Global.SwitchRoomMode)
        {
            // move playerdot right 
            if (position.x > PlayerLocation.Position.x
            && position.y == PlayerLocation.Position.y
            && (position.x-PlayerLocation.Position.x) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Right * myTileSize * 2;
                myMovement = Orientation.LeftRight;
                GD.Print("right");
            }
            // move playerdot left
            else if (position.x < PlayerLocation.Position.x
            && position.y == PlayerLocation.Position.y
            && (PlayerLocation.Position.x - position.x) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Left * myTileSize * 2;
                myMovement = Orientation.LeftRight;
                GD.Print("left");
            }
            // move playerdot up
            else if (position.x == PlayerLocation.Position.x
            && position.y < PlayerLocation.Position.y
            && (position.y-PlayerLocation.Position.y) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Up * myTileSize * 2;
                myMovement = Orientation.UpDown;
                GD.Print("up");
            }
            // move playerdot down
            else if (position.x == PlayerLocation.Position.x
            && position.y > PlayerLocation.Position.y
            && (PlayerLocation.Position.y - position.y) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Down * myTileSize * 2;
                myMovement = Orientation.UpDown;
                GD.Print("down");
            }

            Global.SwitchRoomMode = false;
            Global.ChangeScene(null, WORLD_FLOOR_SCENE);
        }
    }

    public void ChangePlayerPosition(bool exitedRight)
    {
        int modifier = 1;

        if (Global.SwitchRoomMode)
        {
            modifier = Global.RoomSpawnLeft == true ? 2 : -2;
        }
        
        if (exitedRight)
        {
            if (myMovement == Orientation.LeftRight)
            {
                PlayerLocation.Position += Vector2.Right * myTileSize * modifier;
            }
            else
            {
                PlayerLocation.Position += Vector2.Down * myTileSize * modifier;
            }
        }
        else
        {
           if (myMovement == Orientation.LeftRight)
            {
                PlayerLocation.Position += Vector2.Left * myTileSize * modifier;
            }
            else
            {
                PlayerLocation.Position += Vector2.Up * myTileSize * modifier;
            }    
        }
    }
}
