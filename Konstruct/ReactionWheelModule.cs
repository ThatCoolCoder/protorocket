using Godot;
using System;

public class ReactionWheelModule : Part
{
	[Export] public float Torque;

	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionPressed("left")) ApplyTorqueImpulse(-Torque);
		if (Input.IsActionPressed("right")) ApplyTorqueImpulse(Torque);
	}
}
