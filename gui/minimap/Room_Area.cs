using Godot;
using System;

public class Room_Area : Control
{
    private bool myMouseOnArea = false;
    public const string ROOM_GROUP = "Rooms";
    [Signal]
    public delegate void AreaClicked(Vector2 position, int nextId);
    [Export]
    public int Id;

    public override void _Ready()
    {
        AddToGroup(ROOM_GROUP);
    }

    public void OnRoomAreaInputEvent(Node viewport, InputEvent @event, int shapeIdX)
    {
        // forward room area position and id of clicked room
        if (@event is InputEventMouseButton && Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            EmitSignal(nameof(AreaClicked), RectPosition, Id);
        }
    }
}
