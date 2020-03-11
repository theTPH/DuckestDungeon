using Godot;
using System;

public class Minimap : MarginContainer
{
    public Sprite PlayerLocation;
    public Camera2D MapLocationCam;
    

    public override void _Ready()
    {
        PlayerLocation = GetNode<Sprite>("MarginContainer/MapContainer/MapView/Map/PlayerLocation");
        MapLocationCam = GetNode<Camera2D>("MarginContainer/MapContainer/MapView/Camera2D");
        MapLocationCam.Target = PlayerLocation;
    }
}
