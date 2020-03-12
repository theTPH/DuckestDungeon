using Godot;
using System;

public class MapContainer : ViewportContainer
{
    public Viewport MapViewPort;
    public Camera2D MapCam;

    public override void _Ready()
    {
        MapViewPort = GetNode<Viewport>("MapView");
        MapCam = GetNode<Camera2D>("MapView/Camera2D");
        SetProcessInput(true);
    }

    public void OnMapContainerGuiInput(InputEvent @event)
    {
        // forward unspecific input to MapViewport
        if (@event is InputEventMouseMotion || @event is InputEventMouseButton)
        {
            MapViewPort.UnhandledInput(@event);
        }
        
        // set minimap camera zoom to mouse wheel input
        if (@event is InputEventMouseButton emb)
        {
            if (emb.IsPressed())
            {
                // zoom in
                if (emb.ButtonIndex == (int)ButtonList.WheelUp)
                {
                    if (MapCam.Zoom.x > 1.3f)
                    {
                        Vector2 zoom = MapCam.Zoom;                   
                        zoom.x -= 0.1f;
                        zoom.y -= 0.1f;
                        GD.Print(zoom);
                        MapCam.Zoom = zoom;
                    } 
                }
                // zoom out
                if (emb.ButtonIndex == (int)ButtonList.WheelDown)
                {
                    if (MapCam.Zoom.x < 3.4f)
                    {
                        Vector2 zoom = MapCam.Zoom;
                        zoom.x += 0.1f;
                        zoom.y += 0.1f;
                        GD.Print(zoom);
                        MapCam.Zoom = zoom;
                    }
                }
            }
        }
    }
}
