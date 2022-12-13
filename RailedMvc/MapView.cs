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
	[Export] public float TimeWarp = 1;

	private Vector2? focusLastWorldPosition = null;
	private CelestialBody focusedBody = null;
	private PackedScene planetScene = ResourceLoader.Load<PackedScene>("res://RailedMvc/Planet.tscn");

	private Label timeWarpLabel;
	private Label focusLabel;

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
				Clockwise = false,
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
				Clockwise = false,
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

		timeWarpLabel = GetNode<Label>("CanvasLayer/TimeWarpLabel");
		focusLabel = GetNode<Label>("CanvasLayer/FocusLabel");
	}

	public override void _Process(float delta)
	{
		ChangeTimeWarp();
		PanToFocusedBody();
		UpdateUI();
	}

	private void ChangeTimeWarp()
	{
		if (Input.IsActionJustPressed("left")) TimeWarp /= 10;
		if (Input.IsActionJustPressed("right")) TimeWarp *= 10;
	}

	private void PanToFocusedBody()
	{
		if (focusedBody != null)
		{
			var currentPosition = focusedBody.GetGlobalPosition();
			if (focusLastWorldPosition != null)
			{
				var delta = currentPosition - (Vector2)focusLastWorldPosition;
				WorldSpacePan += delta;
			}
			focusLastWorldPosition = currentPosition;

		}
	}

	private void UpdateUI()
	{
		var focusName = focusedBody?.Name ?? "None";
		timeWarpLabel.Text = $"Simulation speed: {TimeWarp}x";
		focusLabel.Text = $"Map focus: {focusName}";
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
		// Convert a world-space position into a view-space one

		return (worldPos - WorldSpacePan) * ViewScale;
	}

	public Vector2 ViewToWorldPos(Vector2 viewPos)
	{
		// Convert a view-space position into a world-space one

		return (viewPos / ViewScale) + WorldSpacePan;
	}

	public void SetFocus(CelestialBody body)
	{
		// Set the focus of the map to a specific body.
		// Doing this means that the map pans automatically as the body moves.

		focusedBody = body;
		focusLastWorldPosition = null;
	}

	public void ReleaseFocus()
	{
		// Unfocus the focused body

		focusedBody = null;
		focusLastWorldPosition = null;
	}
}
