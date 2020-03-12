using Godot;
using System;

public class GUI : Control
{
    [Signal]
    public delegate void XpObtained(int xp);
    [Signal]
    public delegate void DungeonCleared();
    
    public override void _Ready()
    {

    }

    public void OnXpObtained(int xp)
    {
        if (GetChild(0) is DungeonMenu)
        {
            EmitSignal(nameof(XpObtained), xp);
        }
    }

    public void OnDungeonCleared()
    {
        if (GetChild(0) is DungeonMenu)
        {
            EmitSignal(nameof(DungeonCleared));
        }
    }
}
