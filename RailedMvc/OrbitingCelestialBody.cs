using Godot;
using System;

namespace RailedMvc;

public class OrbitingCelestialBody : CelestialBody
{
    public OrbitalPosition OrbitalPosition;
    public CelestialBody OrbitCenter;

    public const float GravitationalConstant = 6.673e-11f;

    public override void Update(float delta)
    {
        // Orbit!

        var distanceTravelled = GetOrbitalSpeed() * delta;
        var angleChange = distanceTravelled / OrbitalPosition.GetOrbitalCircumference() * Mathf.Tau;

        if (!OrbitalPosition.Clockwise) angleChange = -angleChange;
        OrbitalPosition.PhaseAngle += angleChange;
    }

    public float GetOrbitalSpeed()
    {
        var numerator = GravitationalConstant * OrbitCenter.Mass;
        return Mathf.Sqrt(numerator / OrbitalPosition.OrbitalRadius);
    }

    public override Vector2 GetRelativePosition()
    {
        // Get position of this relative to orbitCenter.
        return OrbitalPosition.GetRelativePosition();
    }

    public override Vector2 GetGlobalPosition()
    {
        // Get position of this relative to root of the universe.
        return OrbitCenter.GetGlobalPosition() + GetRelativePosition();
    }
}