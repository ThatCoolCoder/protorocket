using Godot;
using System;

namespace RailedMvc;

public class Planet : StaticBody2D
{
    // Visual representation of a map view object
    // Also drives the physics but we should probably get a proper simulation runnerin a new prototype - that would also allow custom simulation hz.

    private MapView mapView;
    private CelestialBody body;

    private Sprite sprite;
    private CollisionShape2D collisionShape;

    public void Init(MapView _mapView, CelestialBody _body)
    {
        mapView = _mapView;
        body = _body;

        sprite = GetNode<Sprite>("Sprite");
        sprite.Modulate = body.Color;
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(float delta)
    {
        Position = mapView.WorldToViewPos(body.GetGlobalPosition());
        UpdateRadius();
    }

    public override void _PhysicsProcess(float delta)
    {
        body.Update(delta * mapView.TimeWarp);
    }

    public void UpdateRadius()
    {
        var screenRadius = Mathf.Max(body.Radius * mapView.ViewScale, 3);
        sprite.Scale = Vector2.One * (screenRadius / (sprite.Texture.GetWidth() / 2f));
        (collisionShape.Shape as CircleShape2D).Radius = screenRadius;
    }
}