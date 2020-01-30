using Godot;
using System;

public class HelloWorld : RichTextLabel
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hello World!!");
        this.Text = "Hello World";
    }
}
