using Godot;
using System;

public class DungeonMenu : Control
{
    MenuBar MenuBar;

    [Signal]
    public delegate void XpObtained(int xp);
    [Signal]
    public delegate void DungeonCleared();

    public override void _Ready()
    {
        MenuBar = GetNode<MenuBar>("MenuBar");

        // connect signals
        GetParent().Connect("XpObtained", this, "OnXpObtained");
        GetParent().Connect("DungeonCleared", this, "OnDungeonCleared");
        Connect("XpObtained", MenuBar.GetNode<ExpContainer>("MarginContainer/TextureRect/MarginContainer/GridContainer/ExpContainer"), "OnXpObtained");
        Connect("DungeonCleared", MenuBar, "OnDungeonCleared");
    }

    public void OnXpObtained(int xp)
    {
        EmitSignal(nameof(XpObtained), xp);
    }

    public void OnDungeonCleared()
    {
        EmitSignal(nameof(DungeonCleared));
    }
}
