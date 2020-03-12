using Godot;
using System;

public class test : Node2D
{
    public override void _Ready()
    {
        Log.log.Debug("Test _Ready");
    }

    public void OnStartSurvey()
    {
        Log.log.Debug("Test-Button Pressed");
        Random rnd = new Random();

        MessageVote vote = new MessageVote();
        vote.Option1 = "Option1 - " + rnd.Next(0, 10);
        vote.Option2 = "Option2 - " + rnd.Next(0, 10);

        WebSocketImpl.GetInstance().Send(vote, VoteCallback);
    }

    public void VoteCallback(MessageVote vote)
    {
        Label Label = GetNode<Label>("Label");
        Label.Text = "Das Wahlergebnis war: " + (vote.Option1Chosen ? vote.Option1 : vote.Option2);
    }

}
