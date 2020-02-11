using Godot;
using System;

public class PlayerAttributes
{
    public string Name;
    public int Level;
    public int Xp;
    public int XpToLevelUp;


    public PlayerAttributes()
    {
        this.Name = "Serious Duck";
        this.Level = 1;
        this.Xp = 0;
        this.XpToLevelUp = 100;
    }

    public Godot.Collections.Dictionary<string, object> Save()
    {
        return new Godot.Collections.Dictionary<string, object>
        {
            {"name", Name},
            {"level", Level},
            {"xp", Xp},
            {"xp_to_levelup", XpToLevelUp}
        };
    }

    public void Load(Godot.Collections.Dictionary playerData)
    {
        this.Name = (string)playerData["name"];
        this.Level = Convert.ToInt32(playerData["level"]);
        this.Xp = Convert.ToInt32(playerData["xp"]);
        this.XpToLevelUp = Convert.ToInt32(playerData["xp_to_levelup"]);
    }
}