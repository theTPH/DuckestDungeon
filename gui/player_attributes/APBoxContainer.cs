using Godot;
using System;

public class APBoxContainer : VBoxContainer
{

    public Global Global;
    public Label APLabel;

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        APLabel = GetNode<Label>("APLabel");

    }

    public void SetAttributePoints(int newValue)
    {
        APLabel.Text = newValue.ToString();
    }
}
