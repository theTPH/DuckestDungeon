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
}

public class ScoreLineWrapper
{
    private static PackedScene scoreLineScene = (PackedScene)ResourceLoader.Load("res://gui/scoreboard/ScoreLine.tscn");
    public virtual ScoreLine ScoreLine { get; set;} = (ScoreLine)scoreLineScene.Instance();

    public ScoreLineWrapper()
    {
        this.ScoreLine.Init();
        this.ScoreLine.AddToGroup("ScoreLines");
    }

    public virtual void SetScoreLine(int rank, string username, int points)
    {
        this.Ranking = rank;
        this.Username = username;
        this.Points = points;
    }

    public virtual int Id {get; set;} = 0;

    public virtual int Points
    {
        get
        {
            return this.ScoreLine.PointsLabel.Text == "" ? 0 : Int32.Parse(this.ScoreLine.PointsLabel.Text);
        }
        set
        {
            this.ScoreLine.PointsLabel.Text = value == 0 ? "" : value.ToString();
        }
    }


    public virtual string Username
    {
        get
        {
            return this.ScoreLine.UsernameLabel.Text;
        }
        set
        {
            this.ScoreLine.UsernameLabel.Text = value;
        }
    }

    public virtual int Ranking
    {
        get
        {
            return this.ScoreLine.RankingLabel.Text == "" ? 0 : Int32.Parse(this.ScoreLine.RankingLabel.Text);
        }
        set
        {
            this.ScoreLine.RankingLabel.Text = value.ToString();
        }
    }
}