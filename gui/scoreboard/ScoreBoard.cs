using Godot;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class ScoreBoard : VBoxContainer
{
    
    public Global Global;
    public Timer UpdateTimer;

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        GD.Print();
        if(Global.TwitchMode)
        {
            UpdateViewerScoreBoard();
            UpdateTimer = GetNode<Timer>("UpdateTimer");
            UpdateTimer.Start();
        }
    }

    public void UpdateViewerScoreBoard()
    {
        var scoreLines = GetTree().GetNodesInGroup("ScoreLines");

        foreach(var sl in scoreLines)
        {
            this.RemoveChild((Node)sl); 
            GD.Print(sl.ToString());
        }  

        // open db connection
        Global.connection.Open();

        // select top ten scorer from
        string queryString = $"SELECT * FROM IngameDonations ORDER BY points DESC LIMIT 10";
        SqliteCommand command = new SqliteCommand(queryString, Global.connection);
        SqliteDataAdapter dataAdapter = new SqliteDataAdapter(command);

        // create new local data table
        DataTable dataTable = new DataTable();

        // put the data table into the adapter
        dataAdapter.Fill(dataTable);

        // fill the actual scoreboard with data
        PackedScene scoreLineScene = (PackedScene)ResourceLoader.Load("res://gui/scoreboard/ScoreLine.tscn");
        int rank = 1;

        foreach (DataRow dr in dataTable.Rows)
        {
			ScoreLine sl = (ScoreLine)scoreLineScene.Instance();
            sl.Init();
            sl.SetScoreLine(rank.ToString(), dr["username"].ToString(), dr["points"].ToString());
            this.AddChild(sl);
            rank++;
        }

        // fill empty ranks up to 10
        while(rank <= 10)
        {
            ScoreLine sl = (ScoreLine)scoreLineScene.Instance();
            sl.Init();
            sl.SetScoreLine(rank.ToString(), string.Empty, string.Empty);
            this.AddChild(sl);
            rank++;
        }

        // close db connection
        Global.connection.Close();

    }


    public void OnUpdateTimerTimeOut()
    {
        GD.Print("Update Scoreboard!");
        UpdateViewerScoreBoard();
    }

}