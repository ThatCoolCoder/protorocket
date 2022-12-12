using Godot;
using System;
using System.Collections.Generic;

namespace RailedMvc;

public class MapView : Node2D
{
	// Base of the map view, stores information about the view state.
	// Yes, this could also be done with a Godot camera that moves and zooms,
	// but it was decided that it wouldn't appreciate the scale of the solar system.

	[Export] public float ViewScaleSpeed = 1.1f;
	[Export] public float ViewScale = 1e-10f; // size in pixels = size in sim * scale
	[Export] public Vector2 WorldSpacePan = Vector2.Zero;
	[Export] public float CameraSpacePanSpeed = 500;

	private PackedScene planetScene = ResourceLoader.Load<PackedScene>("res://RailedMvc/Planet.tscn");

	public override void _Ready()
	{
		var sun = new CelestialBody()
		{
			Name = "Sun",
			Color = new Color(0.7f, 0.7f, 0),
			Radius = 696340 * 1000,
			Mass = 1.989e30f
		};
		var earth = new OrbitingCelestialBody()
		{
			Name = "Earth",
			Color = new Color(0, 0.6f, 0),
			Radius = 6371 * 1000,
			Mass = 5.972e24f,
			OrbitCenter = sun,
			OrbitalPosition = new OrbitalPosition
			{
				OrbitalRadius = 1.496e11f
			}
		};
		var moon = new OrbitingCelestialBody()
		{
			Name = "Moon",
			Color = new Color(0.4f, 0.4f, 0.4f),
			Radius = 1737 * 1000,
			Mass = 7.347e22f,
			OrbitCenter = earth,
			OrbitalPosition = new OrbitalPosition
			{
				OrbitalRadius = 384400 * 1000
			}
		};

		var bodies = new List<CelestialBody>() { sun, earth, moon };
		foreach (var body in bodies)
		{
			var instance = planetScene.Instance<Planet>();
			instance.Init(this, body);
			AddChild(instance);
		}

		WorldSpacePan = -ViewToWorldPos(GetViewportRect().Size / 2);
	}

	public override void _Process(float delta)
	{
		KeyboardPan(delta);
	}

	private void KeyboardPan(float delta)
	{
		var speed = Vector2.Zero;
		if (Input.IsActionPressed("left")) speed.x -= CameraSpacePanSpeed;
		if (Input.IsActionPressed("right")) speed.x += CameraSpacePanSpeed;
		if (Input.IsActionPressed("up")) speed.y -= CameraSpacePanSpeed;
		if (Input.IsActionPressed("down")) speed.y += CameraSpacePanSpeed;
		var scaledSpeed = speed / ViewScale;
		WorldSpacePan += scaledSpeed * delta;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{

			var newViewScale = ViewScale;
			if (mouseEvent.ButtonIndex == 4) // 4 = up
			{
				newViewScale *= ViewScaleSpeed;
			}
			if (mouseEvent.ButtonIndex == 5) // 5 = down
			{
				newViewScale /= ViewScaleSpeed;
			}

			var origMousePos = ViewToWorldPos(GetGlobalMousePosition());
			ViewScale = newViewScale;
			var newMousePos = ViewToWorldPos(GetGlobalMousePosition());
			WorldSpacePan -= (newMousePos - origMousePos);
		}
		if (inputEvent is InputEventMouseMotion motionEvent && Input.IsActionPressed("mouse"))
		{
			WorldSpacePan += -motionEvent.Relative / ViewScale;
		}
	}

	public Vector2 WorldToViewPos(Vector2 worldPos)
	{
		return (worldPos - WorldSpacePan) * ViewScale;
	}

	public Vector2 ViewToWorldPos(Vector2 viewPos)
	{
		return (viewPos / ViewScale) + WorldSpacePan;
	}
}
