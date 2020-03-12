using Godot;
using System;

public class DungeonMenu : Control
{
    MenuBar MenuBar;
    Console Console;

    [Signal]
    public delegate void XpObtained(int xp);
    [Signal]
    public delegate void DungeonCleared();
    [Signal]
    public delegate void VoteStarted(string option1, string option2);
    [Signal]
    public delegate void VoteEnded(string result);

    public override void _Ready()
    {
        MenuBar = GetNode<MenuBar>("MenuBar");
        Console = GetNode<Console>("MarginContainer/HBoxContainer/TwitchGui/Panel/Console");

        // connect signals
        GetParent().Connect("XpObtained", this, "OnXpObtained");
        GetParent().Connect("DungeonCleared", this, "OnDungeonCleared");
        GetParent().Connect("VoteStarted", this, "OnVoteStarted");
        GetParent().Connect("VoteEnded", this, "OnVoteEnded");
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

    public void OnVoteStarted(string option1, string option2)
    {
        Console.ShowVoteStarted(option1, option2);
    }

    public void OnVoteEnded(string result)
    {
        Console.ShowVoteResult(result);
    }
}
