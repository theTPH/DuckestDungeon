using Godot;
using System;

public class Room : Node2D
{
    public EventSpot EventSpot;
    public Global Global;

    private Button myClearedButton;
    [Signal]
    public delegate void DungeonCleared();

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        EventSpot = GetNode<EventSpot>("EventSpot");
        myClearedButton = GetNode<Button>("DungeonClearedButton");

        EventSpot.SetEvent(1, 1400, 680);
        GD.Print(Global.GetCurrentRoomId());

        if (Global.GetCurrentRoomId() == 7)
        {
            myClearedButton.Visible = true;
        }

        Connect("DungeonCleared", Global, "OnDungeonCleared");
    }

    public void OnDungeonClearedButtonClicked()
    {
        EmitSignal(nameof(DungeonCleared));
    }
}
