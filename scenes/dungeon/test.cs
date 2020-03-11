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
        vote.option1 = "Option1 - " + rnd.Next(0, 10);
        vote.option2 = "Option2 - " + rnd.Next(0, 10);

        WebSocketImpl.getInstance().send(vote, VoteCallback);

        /*
        // auch als Lambda möglich:
        WebSocketImpl.getInstance().send(vote, (v) => {
            Log.log.Debug("Das Ergebnis war " + (vote.option1Chosen ? vote.option1 : vote.option2));
        });
        */
    }

    public void VoteCallback(MessageVote vote)
    {
        Label Label = GetNode<Label>("Label");
        Label.Text = "Das Wahlergebnis war: " + (vote.option1Chosen ? vote.option1 : vote.option2);
    }

}
