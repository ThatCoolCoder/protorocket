using Godot;
using System;

public class PartButton : Button
{
    public PartInfo PartInfo;
    public Node Ship;

    public override void _Ready()
    {
        Connect("pressed", this, "OnPressed");
        Text = PartInfo.Name;
    }

    private void OnPressed()
    {
        var part = ResourceLoader.Load<PackedScene>(PartInfo.Path).Instance<Part>();
        part.Position = new Vector2((float)GD.RandRange(250, 750), (float)GD.RandRange(200, 400));
        part.PartMode = PartMode.HangarDragged;
        Ship.AddChild(part);
    }
}