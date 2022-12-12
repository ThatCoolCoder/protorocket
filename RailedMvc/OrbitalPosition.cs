using Godot;
using System;

namespace RailedMvc;

public struct OrbitalPosition
{
    // Class representing orbital data of some object.
    // It does not manage what the thing is orbiting around - that is not relevant to this.
    // Uses a very basic model of orbits - does not believe in inclination or eccentricity
    // In additon to data, contains basic methods for working out basic values

    public float OrbitalRadius;
    public float PhaseAngle // angle around the orbit that it has gone. From 0 to tau.
    {
        get
        {
            return _phaseAngle;
        }
        set
        {
            _phaseAngle = value;
            while (_phaseAngle > Mathf.Tau) _phaseAngle -= Mathf.Tau;
            while (_phaseAngle < 0) _phaseAngle += Mathf.Tau;
        }
    }
    private float _phaseAngle;
    public bool Clockwise; // whether the orbit goes clockwise.

    public Vector2 GetRelativePosition()
    {
        // Calculate position of the orbiting body relative to position of the orbited body.
        return Vector2.Right.Rotated(PhaseAngle) * OrbitalRadius;
    }

    public float GetOrbitalCircumference()
    {
        return Mathf.Tau * OrbitalRadius;
    }
}