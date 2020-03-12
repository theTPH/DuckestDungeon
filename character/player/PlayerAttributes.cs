using Godot;
using System;
using static System.Math;

public class PlayerAttributes
{
    public string Name;
    public int Level;
    public int Experience;
    public double ExperienceTotal;
    public int ExperienceRequired;
    
    public int AbilityPoints;
    public int Strength;
    public int MaxHp;
    public int Agility;


    public PlayerAttributes()
    {
        Name = "Serious Duck";
        Level = 1;
        Experience = 0;
        ExperienceRequired = 100;

        // attribute data
        AbilityPoints = 3;
        Strength = 5;
        MaxHp = 12;
        Agility = 5;
    }

    #region PlayerData

    public Godot.Collections.Dictionary<string, object> Save()
    {
        GD.Print(AbilityPoints, Strength, MaxHp, Agility);
        return new Godot.Collections.Dictionary<string, object>
        {
            {"name", Name},
            {"level", Level},
            {"xp", Experience},
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
        ExperienceRequired = Convert.ToInt32(playerData["xp_to_levelup"]);

        AbilityPoints = Convert.ToInt32(playerData["ap"]);
        Strength = Convert.ToInt32(playerData["str"]);
        MaxHp = Convert.ToInt32(playerData["hp"]);
        Agility = Convert.ToInt32(playerData["ag"]);
    }

    #endregion

    #region Attributes
    private int randomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    public int GetRequiredExperience(int level)
    {
        int experienceRequired = (int)Round(Pow(level, 1.8) + level * 4); 
        return experienceRequired;
    }

    public void GainExperience(int amount)
    {
        ExperienceTotal += amount;
        Experience += amount;

        while(Experience >= ExperienceRequired)
        {
            Experience -= ExperienceRequired;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level += 1;
        ExperienceRequired = GetRequiredExperience(Level + 1);

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

    #endregion
}