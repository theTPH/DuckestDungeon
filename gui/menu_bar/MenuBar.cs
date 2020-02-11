using Godot;
using System;

public class MenuBar : Control
{
    public Global Global;
    private const string GUI_TITLE__SCENE = "res://gui/title_menu/TitleMenu.tscn";
    private const string WORLD_TITLE_SCENE = "res://scenes/startup/TitleScene.tscn";
    public ExpContainer ExpContainer;
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        ExpContainer = GetNode<ExpContainer>("MarginContainer/TextureRect/MarginContainer/GridContainer/ExpContainer");
        ExpContainer.SetExpContainerValues(Global.PlayerAttributes.Level, Global.PlayerAttributes.Xp, Global.PlayerAttributes.XpToLevelUp);
    }

    public void OnBackToTitleScreenPressed()
    {
        Global.ChangeScene(GUI_TITLE__SCENE, WORLD_TITLE_SCENE);
    }

    public void OnSaveAndQuitPressed()
    {
        Global.SaveGame();
        GetTree().Quit();
    }
}
