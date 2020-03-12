using Godot;
using System;

public class Console : RichTextLabel
{
    public override void _Ready()
    {
        
    }
    
    public void ShowVoteStarted(string option1, string option2)
    {
        AddText("Eine Abstimmung hat begonnen!");
        AddText(option1);
        AddText(option2);
    }

    public void ShowVoteResult(string result)
    {
        AddText(result);
    }

}
