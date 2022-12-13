using Godot;
using System;

public class EngineModule : Part
{
	[Export] public float Thrust;
	[Export] public float GimbalAmountDegrees;
	public float Throttle = 0;
	public float EffectiveThrust
	{
		get
		{
			return Thrust * Throttle;
		}
	}

	private Node2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<Node2D>("ColorRectHolder");
	}

	public override void _Process(float delta)
	{
		Throttle = Input.IsActionPressed("up") ? 1 : 0;

		base._Process(delta);
	}

	public override void _PhysicsProcess(float delta)
	{
		// do rocket engine stuff

		float finalGimbalAmount = 0;
		if (Input.IsActionPressed("left")) finalGimbalAmount += 1;
		if (Input.IsActionPressed("right")) finalGimbalAmount -= 1;
		finalGimbalAmount *= GimbalAmountDegrees;
		finalGimbalAmount = Mathf.Deg2Rad(finalGimbalAmount);

		sprite.Rotation = finalGimbalAmount;

		ApplyCentralImpulse((Vector2.Up * EffectiveThrust).Rotated(Rotation + finalGimbalAmount));
	}
}
