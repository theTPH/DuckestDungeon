using Godot;
using System;
using System.Collections.Generic;
public class ScoreBoard : VBoxContainer
{
    
    public Global Global;
    public Timer UpdateTimer;

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        if(Global.TwitchMode)
        {
            UpdateViewerScoreBoard();
            UpdateTimer = GetNode<Timer>("UpdateTimer");
            UpdateTimer.Start();
        }
    }

    public void UpdateViewerScoreBoard()
    {
        IList<ScoreLineWrapper> scores = null;

        // the objects are somehow cached, wich leads to some weird behaviour (especially with regard to sorting)
        Global.connection.Clear();

        using (Global.connection.BeginTransaction())
        {
            ScoreLineWrapper w = null;
            scores = Global.connection.QueryOver<ScoreLineWrapper>(() => w).Where(() => w.Points > 0).OrderBy(() => w.Points).Desc.Take(10).List();
        }
        
        var scoreLines = GetTree().GetNodesInGroup("ScoreLines");

        foreach(var sl in scoreLines)
        {
            this.RemoveChild((Node)sl); 
            Log.log.Debug($"removing {sl.ToString()} from ScoreBoard");
        }

        int rank = 1;
        if (scores != null)
        {
            foreach (ScoreLineWrapper sl in scores)
            {
                sl.Ranking = rank;
                this.AddChild(sl.ScoreLine);
                rank++;
            }
        }

        // fill empty ranks up to 10
        while(rank <= 10)
        {
            ScoreLineWrapper sl = new ScoreLineWrapper();
            sl.SetScoreLine(rank, string.Empty, 0);
            this.AddChild(sl.ScoreLine);
            rank++;
        }
    }

    public void OnUpdateTimerTimeOut()
    {
        Log.log.Debug("Update Scoreboard!");
        UpdateViewerScoreBoard();
    }

}