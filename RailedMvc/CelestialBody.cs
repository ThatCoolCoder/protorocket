using Godot;
using System;

namespace RailedMvc;

public class CelestialBody
{
    // Class representing the most basic of celestial bodies
    // Does not have the capability to orbit - for that subclasses are needed

    public string Name = "";
    public Color Color;
    public float Radius = 10000;
    public float Mass = 1.6e16f;

    public virtual Vector2 GetGlobalPosition()
    {
        return Vector2.Zero;
    }

    public virtual Vector2 GetRelativePosition()
    {
        return Vector2.Zero;
    }

    public virtual void Update(float delta)
    {

    }
}