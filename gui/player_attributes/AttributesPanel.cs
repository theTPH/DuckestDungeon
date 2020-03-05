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

        if (Global.SaveGameLoaded)
        {
            LoadAttributeData();
        }
    }

    public void LoadAttributeData()
    {
        AStr.SetPointsLabel(Global.PlayerAttributes.Str);
        AHp.SetPointsLabel(Global.PlayerAttributes.Hp);
        AAg.SetPointsLabel(Global.PlayerAttributes.Ag);
        APBox.SetAttributePoints(Global.PlayerAttributes.Ap);
    }
    
    #region Signals
    public void OnAPEditCancelled()
    {
        LoadAttributeData();
    }

    public void OnAPEditConfirmed()
    {
        // set new player data, if edit confirmed
        Global.PlayerAttributes.Str = AStr.GetPoints();
        Global.PlayerAttributes.Hp = AHp.GetPoints();
        Global.PlayerAttributes.Ag = AAg.GetPoints();
        Global.PlayerAttributes.Ap = APBox.GetAttributePoints();

        Global.SaveGame();
    }

    public void OnAPEditToggled()
    {
        AStr.ToggleEditButtons();
        AHp.ToggleEditButtons();
        AAg.ToggleEditButtons();
    }

    public void OnPointsIncreased()
    {
        
    }

    public void OnPointsDecreased()
    {

    }

    #endregion
}
