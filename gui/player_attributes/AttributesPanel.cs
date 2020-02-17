using Godot;
using System;

public class AttributesPanel : Panel
{
    public Global Global;

    public STRAttributeContainer AStr;
    public HPAttributeContainer AHp;
    public AGAttributeContainer AAg;
    public APBoxContainer APBox;

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");

        // get the attribute container nodes
        AStr = GetNode<STRAttributeContainer>("HBoxContainer/STRAttributeContainer");
        AHp = GetNode<HPAttributeContainer>("HBoxContainer/HPAttributeContainer");
        AAg = GetNode<AGAttributeContainer>("HBoxContainer/AGAttributeContainer");
        APBox = GetNode<APBoxContainer>("HBoxContainer/CenterContainer/APBoxContainer");

    }

    public void LoadAttributeData()
    {
        AStr.SetPointsLabel(Global.PlayerAttributes.Str);
        AHp.SetPointsLabel(Global.PlayerAttributes.Hp);
        AAg.SetPointsLabel(Global.PlayerAttributes.Ag);
        APBox.SetAttributePoints(Global.PlayerAttributes.Ap);
    }

}
