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
    public Button PlusButton;
    public Button MinusButton;

    [Signal]
    public delegate void PointsIncreased();
    [Signal]
    public delegate void PointsDecreased();

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");

        APointsLabel = GetNode<Label>("SingleAttributePanel/VBoxContainer/APointsLabel");
        ANameLabel = GetNode<Label>("SingleAttributePanel/VBoxContainer/ANameLabel");
        PlusButton = GetNode<Button>("PlusButton");
        MinusButton = GetNode<Button>("MinusButton");
    
        // if no save game exists -> set to default value
        if (!Global.SaveGameLoaded)
        {
            ANameLabel.Text = AName;
            APointsLabel.Text = AValue.ToString(); 
        }
        
    }

    public void OnPlusButtonClick()
    {
        IncreasePointsValue();
    }

    public void OnMinusButtonClick()
    {
        DecreasePointsValue();
    }

    public void OnApZeroed()
    {
        PlusButton.Disabled = true;
    }

    public void ToggleEditButtons()
    {
        PlusButton.Disabled = !PlusButton.Disabled;
        MinusButton.Disabled = !MinusButton.Disabled;
    }

    public void IncreasePointsValue()
    {
        AValue++;
        SetPointsLabel(AValue);
        EmitSignal(nameof(PointsIncreased));
    }

    public void DecreasePointsValue()
    {
        AValue--;
        SetPointsLabel(AValue);
        EmitSignal(nameof(PointsDecreased));
    }

    public void SetPointsLabel(int newValue)
    {
        AValue = newValue;
        APointsLabel.Text = newValue.ToString();
    }

    public int GetPoints()
    {
        return Convert.ToInt32(APointsLabel.Text);
    }
}
