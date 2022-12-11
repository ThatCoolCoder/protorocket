using Godot;
using System;
using System.Collections.Generic;

public class GravityManager : Node2D
{
	[Export] public float GravitationalConstant = 6.67e-11f; // this is rounded to 0 in the editor UI but it remains at the correct value
	public List<RigidBody2D> Bodies = new();

	public void RegisterBody(RigidBody2D body)
	{
		// Register the body as having gravity attraction
		Bodies.Add(body);
	}

	public bool UnregisterBody(RigidBody2D body)
	{
		// Unregister the body as having gravity. You should probably do this when
		// returned value is whether the body was removed.
		return Bodies.Remove(body);
	}
}
