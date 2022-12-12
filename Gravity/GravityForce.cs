using Godot;
using System;

namespace Gravity;

public class GravityForce : Node2D
{
	[Export] public NodePath TargetPath;
	private RigidBody2D target;
	private GravityManager gravityManager;

	public override void _Ready()
	{
		target = GetNode<RigidBody2D>(TargetPath);
		gravityManager = GetTree().GetNodesInGroup("GravityManager")[0] as GravityManager;
		gravityManager.RegisterBody(target);
	}

	public override void _PhysicsProcess(float delta)
	{
		var totalForce = Vector2.Zero;
		foreach (var body in gravityManager.Bodies)
		{
			if (body == target) continue;

			var displacement = body.GlobalPosition - target.GlobalPosition;
			var distanceSq = displacement.LengthSquared();
			var forceMag = gravityManager.GravitationalConstant * (body.Mass * target.Mass) / distanceSq;
			totalForce += displacement.Normalized() * forceMag;
		}
		target.ApplyCentralImpulse(totalForce);
	}

	public override void _ExitTree()
	{
		// Clean up our interactions with other objects before disappearing

		// Exiting the tree might be somewhat uncontrolled, so we should check if the manager exists
		if (gravityManager != null)
		{
			gravityManager.UnregisterBody(target);
		}
	}
}
