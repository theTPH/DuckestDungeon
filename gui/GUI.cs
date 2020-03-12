using Godot;
using System;

public class GUI : Control
{
    [Signal]
    public delegate void XpObtained(int xp);
    [Signal]
    public delegate void DungeonCleared();
    [Signal]
    public delegate void VoteStarted(string option1, string option2);
    [Signal]
    public delegate void VoteEnded(string result);
    
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

    public void OnVoteStarted(string option1, string option2)
    {
        EmitSignal(nameof(VoteStarted), option1, option2);
    }

    public void OnVoteEnded(string result)
    {
        if (GetChild(0) is DungeonMenu)
        {
            EmitSignal(nameof(VoteEnded), result);
        }
    }
}
