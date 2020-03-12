using Godot;
using System;

public class Floor : Node2D
{
    public EventSpot EventSpot;
    private Player myPlayer;
    private Camera2D myPlayerCamera;
    private Area2D Start;
    private int myEventSpotPosition;
    private int myEventType;
    private Vector2 myEventSpawn;


    public override void _Ready()
    {
        myPlayer = GetNode<Player>("Player");
        myPlayer.Camera.SetLimitRigth(3800);
        EventSpot = GetNode<EventSpot>("EventSpots/EventSpot");
        
        // generate random event spot and random event
        Random random = new Random();
        myEventSpotPosition = random.Next(1, 3);
        myEventType = random.Next(1, 4);
        float eventPositionX = 0;
        
        GD.Print(myEventSpotPosition);
        GD.Print(myEventType);

        // randomize for combat, loot or obstacle event      
        switch (myEventType)
        {
            case 1:
                if (myEventSpotPosition == 1)
                {
                    eventPositionX = 1760;
                }
                else
                {
                    eventPositionX = 2720;
                }
                break;
            case 2:
            case 3:
                if (myEventSpotPosition == 1)
                {
                    eventPositionX = 1480;
                }
                else
                {
                    eventPositionX = 2440;
                }
                break;
            default:
                break;
        }
        
        EventSpot.SetEvent(myEventType, eventPositionX, 680);
    }
}
