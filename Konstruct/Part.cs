using Godot;
using System;

public class Part : RigidBody2D
{
    // we would add aero+other custom physics in here probably
    // or at least coordinate that here

    public PartMode PartMode
    {
        get
        {
            return _partMode;
        }
        set
        {
            _partMode = value;
            if (_partMode == PartMode.Flight) Mode = ModeEnum.Rigid;
            else Mode = ModeEnum.Static;
        }
    }
    private PartMode _partMode = PartMode.Hangar;

    public override void _Process(float delta)
    {
        if (PartMode == PartMode.HangarDragged)
        {
            GlobalPosition = GetGlobalMousePosition();
            if (Input.IsActionJustPressed("mouse"))
            {
                PartMode = PartMode.Hangar;
            }
        }
    }
}