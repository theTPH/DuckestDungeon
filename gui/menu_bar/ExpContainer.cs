using Godot;
using System;

public class ExpContainer : CenterContainer
{
    public ExpBar ExpBar;
    private Label myLevelLabel;
    private int myLevel;

    public override void _Ready()
    {
        ExpBar = GetNode<ExpBar>("HBoxContainer/ExpBar");
        myLevelLabel = GetNode<Label>("HBoxContainer/LevelLabel");
        myLevel = 1;
        myLevelLabel.Text = $"LV.{myLevel}";
    }

    public void SetExpContainerValues(int level, int currentValue, int maxValue)
    {
        ExpBar.SetProgressionBar(currentValue, maxValue);
        SetLevel(level);
    }

    public void SetLevel(int level)
    {
        myLevelLabel.Text = $"LV.{level}";
        myLevel = level;
    }

    public int GetLevel()
    {
        return myLevel;
    }

    public void OnXpObtained(int xp)
    {
        int currentValue = (int)ExpBar.Value;
        int maxValue = (int)ExpBar.MaxValue;
        
        //check if level up
        if (currentValue + xp >= maxValue)
        {
            SetLevel(myLevel + 1);
            currentValue = currentValue + xp - maxValue;
        }
        else
        {
            currentValue += xp;
        }

        // set progression bar with new values
        ExpBar.SetProgressionBar(currentValue, maxValue);
    }

}
