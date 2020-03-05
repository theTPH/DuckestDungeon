using Godot;
using System;

public class ExpContainer : CenterContainer
{
    public ExpBar ExpBar;
    public Label LevelLabel;

    public override void _Ready()
    {
        ExpBar = GetNode<ExpBar>("HBoxContainer/ExpBar");
        LevelLabel = GetNode<Label>("HBoxContainer/LevelLabel");

        //set initial values
        SetLevel(1);
    }

    public void SetExpContainerValues(int level, int currentValue, int maxValue)
    {
        ExpBar.SetProgressionBar(currentValue, maxValue);
        SetLevel(level);
    }

    public void SetLevel(int level)
    {
        LevelLabel.Text = $"LV.{level}";
    }


}
