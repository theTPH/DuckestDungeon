using Godot;
using System;

public class ProgressBar : TextureProgress
{
    public Label Label;
    public  string ValueName = "";

    public override void _Ready()
    {
        //Set initial values
        Label = GetNode<Label>("Label");
        MinValue = 0;
        MaxValue = 100;
        Value = 0;
        Label.Text = $"{MinValue}/{MaxValue} {ValueName}";
    }


    public void SetProgressionBar(int currentValue, int maxValue)
    {
        MaxValue = maxValue;
        Value = currentValue;
        Label.Text = $"{Value}/{MaxValue} {ValueName}";
    }
}
