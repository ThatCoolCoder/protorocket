using Godot;
using System;
using Common;

namespace RailedMvc;

public class Planet : Sprite
{
	// Visual representation of a map view object
	// Also drives the physics but we should probably get a proper simulation runnerin a new prototype - that would also allow custom simulation hz.

	private MapView mapView;
	private CelestialBody body;

	private bool mouseOver = false;
	private Color hoverColorChange = new Color(0.2f, 0.2f, 0.2f);
	private const float mouseExtraDistance = 10; // click can be this far outside self and still count

	public void Init(MapView _mapView, CelestialBody _body)
	{
		mapView = _mapView;
		body = _body;
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(float delta)
	{
		Position = mapView.WorldToViewPos(body.GetGlobalPosition());
		UpdateAppearance();
		CheckIfMouseOver();
		CheckIfFocused();
	}

	public override void _PhysicsProcess(float delta)
	{
		body.Update(delta * mapView.TimeWarp);
	}

	private void CheckIfFocused()
	{
		if (Input.IsActionJustPressed("mouse") && mouseOver)
		{
			mapView.SetFocus(body);
		}
	}

	private void CheckIfMouseOver()
	{
		var length = GetGlobalMousePosition().DistanceSquaredTo(mapView.WorldToViewPos(body.GetGlobalPosition()));
		var requiredDistance = body.Radius * mapView.ViewScale + mouseExtraDistance;
		mouseOver = length < requiredDistance * requiredDistance;

	}

	public void UpdateAppearance()
	{
		var screenRadius = Mathf.Max(body.Radius * mapView.ViewScale, 3);
		Scale = Vector2.One * (screenRadius / (Texture.GetWidth() / 2f));

		Modulate = body.Color;
		if (mouseOver)
		{
			Modulate += (body.Color.Brightness() > 0.5f ? -1 : 1) * hoverColorChange;
		}
	}
}
