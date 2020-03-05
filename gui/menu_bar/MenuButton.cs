using Godot;
using System;

public class MenuButton : Godot.MenuButton
{
    [Signal]
    private delegate void BackToTitleScreenPressed();
   [Signal]
    private delegate void SaveAndQuitPressed();

    private PopupMenu myPopUpMenu;

    public override void _Ready()
    {
        Node parentNode = GetNode("/root/");
        
        myPopUpMenu = GetPopup();
        myPopUpMenu.Connect("id_pressed", this, nameof(OnMenuItemPressed));
    }

    public void OnMenuItemPressed(int id)
    {
        if (id == 0)
        {
            EmitSignal(nameof(BackToTitleScreenPressed));
        }
        
        if (id == 1)
        {
            EmitSignal(nameof(SaveAndQuitPressed));
        }
    }
}
