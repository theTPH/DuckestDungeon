using Godot;
using System;

public class Map : Node2D
{
    public int RoomId;
    public Sprite PlayerLocation;
    public Global Global;
    
    public enum Orientation
    {        
        Left,
        Right,
        Up,
        Down
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
        myMovement = Orientation.Right;
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
                myMovement = Orientation.Right;
                GD.Print("right");
                ChangeToFloorScene();
            }
            // move playerdot left
            else if (position.x < PlayerLocation.Position.x
            && position.y == PlayerLocation.Position.y
            && (PlayerLocation.Position.x - position.x) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Left * myTileSize * 2;
                myMovement = Orientation.Left;
                GD.Print("left");
                ChangeToFloorScene();
            }
            // move playerdot up
            else if (position.x == PlayerLocation.Position.x
            && position.y < PlayerLocation.Position.y
            && (position.y-PlayerLocation.Position.y) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Up * myTileSize * 2;
                myMovement = Orientation.Up;
                GD.Print("up");
                ChangeToFloorScene();
            }
            // move playerdot down
            else if (position.x == PlayerLocation.Position.x
            && position.y > PlayerLocation.Position.y
            && (PlayerLocation.Position.y - position.y) <= 7 * myTileSize)
            {
                PlayerLocation.Position += Vector2.Down * myTileSize * 2;
                myMovement = Orientation.Down;
                GD.Print("down");
                ChangeToFloorScene();
            }
        }
    }

    public void ChangePlayerPosition(bool exitedRight = false)
    {
        // the player position on map has to be changed on room mode and orientation
        int modifier = Global.SwitchRoomMode ? 2 : 1;

        if (!exitedRight)
        {
            modifier *= -1;
        }


        switch (myMovement)
            {
                case Orientation.Right:
                    PlayerLocation.Position += Vector2.Right * myTileSize * modifier;
                    break;
                case Orientation.Left:
                    PlayerLocation.Position += Vector2.Left * myTileSize * modifier;
                    break;
                case Orientation.Up:
                    PlayerLocation.Position += Vector2.Up * myTileSize * modifier;
                    break;
                case Orientation.Down:
                    PlayerLocation.Position += Vector2.Down * myTileSize * modifier;
                    break;
                default:
                    break;
            }    
    }

    public void ChangeToFloorScene()
    {
        Global.SwitchRoomMode = false;
        Global.ChangeScene(null, WORLD_FLOOR_SCENE);
    }
}
