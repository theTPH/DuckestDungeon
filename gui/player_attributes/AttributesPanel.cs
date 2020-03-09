using Godot;
using System;

public class AttributesPanel : Panel
{
    public Global Global;

    public STRAttributeContainer AStr;
    public HPAttributeContainer AHp;
    public AGAttributeContainer AAg;
    public APBoxContainer APBox;
    public int TempAp;

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

        // set TempAp to cover real AP for cancelling case
        TempAp = Global.PlayerAttributes.Ap;
    }
    
    #region Signals
    public void OnAPEditCancelled()
    {
        LoadAttributeData();
        DisableAllPlusMinusButtons();
    }

    public void OnAPEditConfirmed()
    {
        // set new player data, if edit confirmed
        Global.PlayerAttributes.Str = AStr.GetPoints();
        Global.PlayerAttributes.Hp = AHp.GetPoints();
        Global.PlayerAttributes.Ag = AAg.GetPoints();
        Global.PlayerAttributes.Ap = APBox.GetAttributePoints();

        Global.SaveGame();

        // disable plus and minus buttons
        DisableAllPlusMinusButtons();
    }

    public void OnAPEditToggled()
    {
        if (APBox.APEditMode == true)
        {
            AStr.PlusButton.Disabled = false;
            AHp.PlusButton.Disabled = false;
            AAg.PlusButton.Disabled = false;
        }
        else
        {
            DisableAllPlusMinusButtons();
        }
    }

    public void OnPointsIncreased()
    {
        TempAp--;
        APBox.SetAttributePoints(TempAp);
        CheckPlayerAp();
    }

    public void OnPointsDecreased()
    {
        TempAp++;
        APBox.SetAttributePoints(TempAp);
        CheckPlayerAp();
    }

    #endregion

    public void CheckPlayerAp()
    {
        if (TempAp <= 0)
        {
            AStr.PlusButton.Disabled = true;
            AHp.PlusButton.Disabled = true;
            AAg.PlusButton.Disabled = true;

            //if all points where spent, only let minus buttons of attributes enabled, that where used for spending points
            AStr.MinusButton.Disabled = AStr.AValue != Global.PlayerAttributes.Str ? false : true;
            AHp.MinusButton.Disabled = AHp.AValue != Global.PlayerAttributes.Hp ? false :true;
            AAg.MinusButton.Disabled = AAg.AValue != Global.PlayerAttributes.Ag ? false : true;
        }

        // if no AP where added to attributes since last save, disable minus buttons
        // (the player should not be able, to actually add AP)
        else 
        {
            
            if (TempAp == Global.PlayerAttributes.Ap)
            {
                AStr.MinusButton.Disabled = true;
                AHp.MinusButton.Disabled = true;
                AAg.MinusButton.Disabled = true;
            }
            else
            {
                //if all points where spent, only let minus buttons of attributes enabled, that where used for spending points
                AStr.MinusButton.Disabled = AStr.AValue != Global.PlayerAttributes.Str ? false : true;
                AHp.MinusButton.Disabled = AHp.AValue != Global.PlayerAttributes.Hp ? false :true;
                AAg.MinusButton.Disabled = AAg.AValue != Global.PlayerAttributes.Ag ? false : true;
            }
            
            AStr.PlusButton.Disabled = false;
            AHp.PlusButton.Disabled = false;
            AAg.PlusButton.Disabled = false;
        } 
    }

    public void DisableAllPlusMinusButtons()
    {
        AStr.PlusButton.Disabled = true;
        AHp.PlusButton.Disabled = true;
        AAg.PlusButton.Disabled = true;

        AStr.MinusButton.Disabled = true;
        AHp.MinusButton.Disabled = true;
        AAg.MinusButton.Disabled = true;
    }

}
