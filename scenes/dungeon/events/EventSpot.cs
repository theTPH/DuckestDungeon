using Godot;
using System;

public class EventSpot : Area2D
{
    public Global Global;
    public enum EventType
    {
        Combat,
        Loot,
        Obstacle,
        None
    }

    private EventType myType;
    private string myImagePath;
    private Sprite mySprite;
    [Signal]
    private delegate void XpObtained(int xp);

    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
        mySprite = GetNode<Sprite>("Sprite");
        myType = EventType.None;

        // connect signals to global
        Connect("XpObtained", Global, "OnXpObtainedInDungeon");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("move_up") && GetOverlappingBodies().Count > 1)
        {
            // pickup loot
            if (myType == EventType.Loot)
            {
                GD.Print("XP obtained!");
                EmitSignal(nameof(XpObtained), 20);
                QueueFree();
            }
            
        }
    }

    public void SetEvent(int type, float positionX, float positionY)
    {
        switch (type)
        {
            // combat
            case 1:
                myImagePath = "res://character/enemy/ape_endbossaseprite1.png";
                myType = EventType.Combat;
                break;
            // loot chest
            case 2:
                myImagePath = "res://scenes/dungeon/events/lootchest.png";
                myType = EventType.Loot;
                break;
            // obstacle vote
            case 3:
                myImagePath = "res://scenes/dungeon/events/obstaclevote.png";
                myType = EventType.Obstacle;
                break;
            default:
                break;
        }

        mySprite.Texture = ResourceLoader.Load<Texture>(myImagePath);
        Position = new Vector2(positionX, positionY);
    }

}
