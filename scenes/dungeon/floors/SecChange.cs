using Godot;
using System;

public class SecChange : Area2D
{
    [Signal]
    public delegate void SecChanged(int id);
    public Global Global;
    private bool myVoteTriggered;

    [Signal]
    public delegate void VoteEnded(string result);
    [Signal]
    public delegate void VoteStarted(string option1, string option2);
    
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        myVoteTriggered = false;

        // connect signals
        Connect("SecChanged", Global, "OnSecExited");
        Connect("VoteStarted", Global.Gui, "OnVoteStarted");
        Connect("VoteEnded", Global.Gui, "OnVoteEnded");
    }

    public void OnSecAreaChanged(KinematicBody2D body)
    {
        bool exitedRight = body.Position.x > this.Position.x ? true : false;
        EmitSignal(nameof(SecChanged), exitedRight);

        // trigger twitch vote if trigger set
        if (exitedRight && myVoteTriggered && Global.TwitchMode)
        {
            // start a vote
            startVote();
        }

    }

    public void SetObstacleTrigger()
    {
        myVoteTriggered = true;
    }

    private void startVote()
    {
        Log.log.Debug("\nStarting vote...");
        Random rnd = new Random();

        // example vote
        MessageVote vote = new MessageVote();
        vote.option1 = "\nOption1 - Springen";
        vote.option2 = "\nOption2 - Entfernen";

        WebSocketImpl.getInstance().send(vote, VoteCallback);
        EmitSignal(nameof(VoteStarted), vote.option1, vote.option2);
    }

    public void VoteCallback(MessageVote vote)
    {
        string result = $"\nDas Wahlergebnis war: {(vote.option1Chosen ? vote.option1 : vote.option2)}";
        EmitSignal(nameof(VoteEnded), result);
    }
}
