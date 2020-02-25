using Godot;
using System;

public class PlayerAttributes
{
    public string Name;
    public int Level;
    public int Xp;
    public int XpToLevelUp;
    
    public int Ap;
    public int Str;
    public int Hp;
    public int Ag;


    public PlayerAttributes()
    {
        Name = "Serious Duck";
        Level = 1;
        Xp = 0;
        XpToLevelUp = 100;

        // attribute data
        Ap = 3;
        Str = 5;
        Hp = 12;
        Ag = 5;
    }

    public Godot.Collections.Dictionary<string, object> Save()
    {
        GD.Print(Ap, Str, Hp, Ag);
        return new Godot.Collections.Dictionary<string, object>
        {
            {"name", Name},
            {"level", Level},
            {"xp", Xp},
            {"xp_to_levelup", XpToLevelUp},
            {"ap", Ap},
            {"str", Str},
            {"hp", Hp},
            {"ag", Ag}
        };
    }

    public void Load(Godot.Collections.Dictionary playerData)
    {
        Name = (string)playerData["name"];
        Level = Convert.ToInt32(playerData["level"]);
        Xp = Convert.ToInt32(playerData["xp"]);
        XpToLevelUp = Convert.ToInt32(playerData["xp_to_levelup"]);

        Ap = Convert.ToInt32(playerData["ap"]);
        Str = Convert.ToInt32(playerData["str"]);
        Hp = Convert.ToInt32(playerData["hp"]);
        Ag = Convert.ToInt32(playerData["ag"]);
    }
}