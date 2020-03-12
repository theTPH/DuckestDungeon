using Godot;
using System;

public class MenuBar : Control
{
    public Global Global;
    private const string GUI_TITLE__SCENE = "res://gui/title_menu/TitleMenu.tscn";
    private const string WORLD_TITLE_SCENE = "res://scenes/startup/TitleScene.tscn";
    public ExpContainer ExpContainer;
    private MenuButton myMenuButton;
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        ExpContainer = GetNode<ExpContainer>("MarginContainer/TextureRect/MarginContainer/GridContainer/ExpContainer");
        myMenuButton = GetNode<MenuButton>("MarginContainer/TextureRect/MarginContainer/GridContainer/MenuButton");
        Refresh();

        if (GetParent() is DungeonMenu)
        {
            myMenuButton.GetPopup().AddItem("Aus Dungeon fliehen");
        }
    }

    public void Refresh()
    {
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

    public void OnBackToTavernPressed()
    {
        Global.ChangeScene(Global.GUI_TAVERN_PATH, Global.WORLD_TAVERN_PATH);
    }

    public void OnDungeonCleared()
    {
        // if dungeon successfully cleared -> save new xp and level ups
        Global.PlayerAttributes.Xp = (int)ExpContainer.ExpBar.Value;
        Global.PlayerAttributes.Level = ExpContainer.GetLevel();
        Global.PlayerAttributes.Save();

        // reset room ids and switch to tavern scene
        Global.SetCurrentRoomId(0);
        Global.SetNextRoomId(0);
        Global.ChangeScene(Global.GUI_TAVERN_PATH, Global.WORLD_TAVERN_PATH);
    }
}
