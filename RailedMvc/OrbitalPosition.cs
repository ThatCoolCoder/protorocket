using Godot;
using System;

namespace RailedMvc;

public struct OrbitalPosition
{
    // Class representing orbital data of some object.
    // It does not manage what the thing is orbiting around - that is not relevant to this.
    // Uses a very basic model of orbits - does not believe in inclination or eccentricity
    // In additon to data, contains basic methods for working out basic values

    public double OrbitalRadius;
    public double OrbitalDistance // Distance around the orbit that it has gone
    {
        get
        {
            return _orbitalDistance;
        }
        set
        {
            _orbitalDistance = value;
            var circumference = GetOrbitalCircumference();
            while (_orbitalDistance >= circumference) OrbitalDistance -= circumference;
            while (_orbitalDistance < 0) OrbitalDistance += circumference;
        }
    }
    private double _orbitalDistance;
    public double PhaseAngle // angle around the orbit that it has gone. From 0 to tau.
    {
        get
        {
            return Mathf.Tau * (OrbitalDistance / GetOrbitalCircumference());
        }
    }
    public bool Clockwise; // whether the orbit goes clockwise.

    public Vector2 GetRelativePosition()
    {
        // Calculate position of the orbiting body relative to position of the orbited body.
        return Vector2.Right.Rotated((float)PhaseAngle) * (float)OrbitalRadius;
    }

    public double GetOrbitalCircumference()
    {
        return Mathf.Tau * OrbitalRadius;
    }
}