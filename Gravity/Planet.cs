using Godot;
using System;

namespace Gravity;

public class Planet : RigidBody2D
{
	[Export] public string PlanetName;
	[Export] public Color Color;
	[Export] public float Radius;
	[Export] public float Density;
	private Sprite sprite;
	private CollisionShape2D collisionShape;

	public override void _Ready()
	{
		sprite = GetNode<Sprite>("Sprite");
		sprite.Modulate = Color;
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateRadius();
	}

	public void UpdateRadius()
	{
		sprite.Scale = Vector2.One * (Radius / (sprite.Texture.GetWidth() / 2f));
		(collisionShape.Shape as CircleShape2D).Radius = Radius;
		Mass = Density * Radius * Radius * Mathf.Pi;
	}
}
