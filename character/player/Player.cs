using Godot;
using System;
using static System.Math;

public class Player : Character
{   
    public string PlayerName = "Serious Duck";
    public int AbilityPoints = 3;
    public int Level = 1;
    public double Experience = 0;
    public double ExperienceTotal = 0;
    public double ExperienceRequired = 100;


    public Player(string name, int lvl, double xp, double xpTotal, double xpReq) : base(50, 5, 5)
    {
        GD.Print("flag_player");
        PlayerName = name;
        Level = lvl;
        Experience = xp;
        ExperienceTotal = xpTotal;
        ExperienceRequired = xpReq;
    }

    private Player()
    {
        GD.Print("constr.");
    }

    public void init(int hp, int str, int agi,
            string name, int lvl, int xp, int xpTotal, int xpReq)
    {
        MaxHp = hp;
        Strength = str;
        Agility = agi;
        PlayerName = name;
        Level = lvl;
        Experience = xp;
        ExperienceTotal = xpTotal;
        ExperienceRequired = xpReq;
    }

    public int getAgility()
    {
        return Agility;
    }

    public int getStrength()
    {
        return Strength;
    }

    public int getMaxHp()
    {
        return MaxHp;
    }

    public int setMaxHp(int hp)
    {
        MaxHp = hp;
        return MaxHp;
    }

    public int randomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    public int getRequiredExperience(int level)
    {
        int experienceRequired = (int)Round(Pow(level, 1.8) + level * 4); 
        return experienceRequired;
    }

    public void gainExperience(double amount)
    {
        ExperienceTotal += amount;
        Experience += amount;

        while(Experience >= ExperienceRequired)
        {
            Experience -= ExperienceRequired;
            levelUp();
        }
    }

    public void levelUp()
    {
        Level += 1;
        ExperienceRequired = getRequiredExperience(Level + 1);

        // gain ap with each levelUp
        AbilityPoints += 3;

        // random stats upgrade each levelUp
        string[] stats = new string[3] {"MaxHp", "Strength", "Agility"};
        int randomStat = randomNumber(0, 2);

        if(randomStat == 0)
        {
            MaxHp += 5;
        }
        if(randomStat == 1)
        {
            Strength += 1;
        }
        if(randomStat == 2)
        {
            Agility += 1;
        }
    }

    public Godot.Collections.Dictionary<string, object> Save()
    {
        GD.Print(AbilityPoints, Strength, MaxHp, Agility);
        return new Godot.Collections.Dictionary<string, object>
        {
            {"name", Name},
            {"level", Level},
            {"xp", Experience},
            {"xp_total", ExperienceTotal},
            {"xp_to_levelup", ExperienceRequired},
            {"ap", AbilityPoints},
            {"str", Strength},
            {"hp", MaxHp},
            {"ag", Agility}
        };
    }

    public void Load(Godot.Collections.Dictionary playerData)
    {
        Name = (string)playerData["name"];
        Level = Convert.ToInt32(playerData["level"]);
        Experience = Convert.ToInt32(playerData["xp"]);
        ExperienceTotal = Convert.ToInt32(playerData["xp_total"]);
        ExperienceRequired = Convert.ToInt32(playerData["xp_to_levelup"]);

        AbilityPoints = Convert.ToInt32(playerData["ap"]);
        Strength = Convert.ToInt32(playerData["str"]);
        MaxHp = Convert.ToInt32(playerData["hp"]);
        Agility = Convert.ToInt32(playerData["ag"]);
    }

    public override void _Ready()
    {
        base._Ready();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
