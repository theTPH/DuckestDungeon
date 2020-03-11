using Godot;
using System;

public class Tavern : Node2D
{
    public Global Global;
    private const string GUI_DUNGEON__SCENE = "res://gui/dungeon_menu/DungeonMenu.tscn";
    private const string WORLD_DUNGEON_SCENE = "res://scenes/dungeon/rooms/Room.tscn";
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
    }

    public void OnStartDungeonButtonPressed()
    {
        Global.SwitchRoomMode = true;
        Global.ChangeScene(GUI_DUNGEON__SCENE, WORLD_DUNGEON_SCENE);
    }
}
