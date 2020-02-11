using Godot;
using System;

public class ScoreLine : Control
{
    public Label RankingLabel;
    public Label UsernameLabel;
    public Label PointsLabel;

    public override void _Ready()
    {
        
    }

    public void Init()
    {
        RankingLabel = GetNode<Label>("Panel/LineContainer/RankingLabel");
        UsernameLabel = GetNode<Label>("Panel/LineContainer/UsernameLabel");
        PointsLabel = GetNode<Label>("Panel/LineContainer/PointsLabel");
    }

    public void SetScoreLine(string rank, string username, string points)
    {
        RankingLabel.Text = rank;
        UsernameLabel.Text = username;
        PointsLabel.Text = points;
        AddToGroup("ScoreLines");
    }

}
