using Godot;
using System;

public class MapContainer : ViewportContainer
{
    public Viewport MapViewPort;

    public override void _Ready()
    {
        MapViewPort = GetNode<Viewport>("MapView");
        SetProcessInput(true);
    }

    public void OnMapContainerGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion || @event is InputEventMouseButton)
        {
            MapViewPort.UnhandledInput(@event);
        }
        
    }
}
