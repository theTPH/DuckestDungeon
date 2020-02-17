using Godot;
using System;

public class SingleAttributeContainer : VBoxContainer
{
    public Global Global;

    [Export]
    public string AName;
    [Export]
    public int AValue;

    public Label ANameLabel;
    public Label APointsLabel;

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        APointsLabel = GetNode<Label>("SingleAttributePanel/VBoxContainer/APointsLabel");
        ANameLabel = GetNode<Label>("SingleAttributePanel/VBoxContainer/ANameLabel");
    
        ANameLabel.Text = AName;
        APointsLabel.Text = AValue.ToString();
        
    }

    public void OnPlusButtonClick()
    {
        IncreasePointsValue();
    }

    public void OnMinusButtonClick()
    {
        DecreasePointsValue();
    }

    public void IncreasePointsValue()
    {
        AValue++;
        SetPointsLabel(AValue);
    }

    public void DecreasePointsValue()
    {
        AValue--;
        SetPointsLabel(AValue);
    }

    public void SetPointsLabel(int newValue)
    {
        APointsLabel.Text = newValue.ToString();
    }

}
