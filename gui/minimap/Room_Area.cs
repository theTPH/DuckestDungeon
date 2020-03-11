using Godot;
using System;

public class Room_Area : Control
{
    private bool myMouseOnArea = false;

    public const string ROOM_GROUP = "Rooms";
    [Signal]
    public delegate void AreaClicked(Vector2 position);

    public override void _Ready()
    {
        AddToGroup(ROOM_GROUP);
    }

    public void OnRoomAreaInputEvent(Node viewport, InputEvent @event, int shapeIdX)
    {
        if (@event is InputEventMouseButton && Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            EmitSignal(nameof(AreaClicked), RectPosition);
            GD.Print("AREA CLICKED!");
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {

    }

    public void OnRoomAreaMouseEntered()
    {
        myMouseOnArea = true;
        GD.Print("ON AREA");
    }

    public void OnRoomAreaMouseExited()
    {
        myMouseOnArea = false;
        GD.Print("NOT ON AREA");
    }


}
