using Godot;
using System;

public class TitleMenu : Control
{
    private const string GUI_MENU_BAR_SCENE = "res://gui/menu_bar/MenuBar.tscn";
    private const string WORLD_TAVERN_SCENE = "res://scenes/tavern/Tavern.tscn";

    public Global Global;

    public override void _Ready()
    {
        Global = (Global)GetNode("/root/Global");
       
        // Enable ContinueButton, if save game exists
        GetNode<Button>("CenterContainer/Panel/MarginContainer/VBoxContainer/ContinueButton").Disabled = !Global.SaveGameLoaded;
        
        // Options toggled
        GetNode<Button>("CenterContainer/Panel/MarginContainer/VBoxContainer/OptionsButton").Pressed = Global.TwitchMode;

    }

    public void OnNewButtonPressed()
    {
        Global.PlayerAttributes = new PlayerAttributes();
        Global.SaveGame();
        Global.ChangeScene(GUI_MENU_BAR_SCENE, WORLD_TAVERN_SCENE);
    }

    public void OnContinueButtonPressed()
    {
        Global.LoadGame();
        Global.ChangeScene(GUI_MENU_BAR_SCENE, WORLD_TAVERN_SCENE);
    }

    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    public void OnTwitchOptionButtonToggled()
    {
        Global.TwitchMode = !Global.TwitchMode;
        GD.Print(Global.TwitchMode);
    }
}
