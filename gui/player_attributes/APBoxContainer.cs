using Godot;
using System;

public class APBoxContainer : VBoxContainer
{

    public Global Global;
    public Label APLabel;
    public Button SetAPButton;
    public Button CancelButton;
    public bool APEditMode;

    [Signal]
    public delegate void APEditToggled();
    [Signal]
    public delegate void APEditCancelled();
    [Signal]
    public delegate void APEditConfirmed();

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");

        APLabel = GetNode<Label>("APLabel");
        SetAPButton = GetNode<Button>("HBoxContainer/SetPointsButton");
        CancelButton = GetNode<Button>("HBoxContainer/CancelPointsButton");
        APEditMode = false;

        // enable change attribute button, if ap > 0
        if (Global.PlayerAttributes.Ap > 0)
        {
            SetAPButton.Disabled = false;
        }

    }

    public void SetAttributePoints(int newValue)
    {
        APLabel.Text = newValue.ToString();
    }

    public int GetAttributePoints()
    {
        return Convert.ToInt32(APLabel.Text);
    }

    public void OnSetPointsButtonPressed()
    {
        // save new points data to player data
        if (APEditMode)
        {
            EmitSignal(nameof(APEditConfirmed));
        }

        ToggleAPEditMode();
    }

    public void OnCancelButtonPressed()
    {
        // reset points
        EmitSignal(nameof(APEditCancelled));

        ToggleAPEditMode();
    }

    public void ToggleAPEditMode()
    {
        if  (!APEditMode)
        {
            APEditMode = true;
            SetAPButton.Text = "OK";
            CancelButton.Visible = true; 
        }
        else
        {
            APEditMode = false;
            SetAPButton.Text = "+";
            CancelButton.Visible = false;

            //disable AP edit, if no AP left
            if (Global.PlayerAttributes.Ap <= 0)
            {
                SetAPButton.Disabled = true;
            }
        }

        EmitSignal(nameof(APEditToggled));
    }
}
