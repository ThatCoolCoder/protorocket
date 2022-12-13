using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Build : Node2D
{
	private List<PartInfo> partInformation = new()
	{
		new()
		{
			Name = "Tank",
		},
		new()
		{
			Name = "Engine",
		},
		new()
		{
			Name = "ReactionWheel",
		}
	};

	private PackedScene partButton = ResourceLoader.Load<PackedScene>("res://Konstruct/PartButton.tscn");

	public override void _Ready()
	{
		GetNode<Button>("CanvasLayer/FlyButton").Connect("pressed", this, "GoFly");

		var partButtonHolder = GetNode<HBoxContainer>("CanvasLayer/PartButtons");
		var rocket = GetNode<Node2D>("Rocket");
		foreach (var partInfo in partInformation)
		{
			var button = partButton.Instance<PartButton>();
			button.PartInfo = partInfo;
			button.Ship = rocket;
			partButtonHolder.AddChild(button);
		}
	}

	private void GoFly()
	{
		// Stupid code to get the child nodes as a list
		// why could godot not add a .ToList() in their Array wrapper?
		var nodes = new List<Node>();
		var rocketBase = GetNode("Rocket");
		foreach (var node in rocketBase.GetChildren()) nodes.Add((Node)node);

		var parts = nodes.Where(x => x is Part).Cast<Part>().ToList();

		HangarToFlight.GoFly(GetTree(), this, rocketBase, parts);
	}
}

static class HangarToFlight
{
	public static void GoFly(SceneTree tree, Node currentScene, Node rocketBase, List<Part> parts)
	{
		var root = parts.OrderByDescending(x => x.Mass).First();

		foreach (var part in parts)
		{
			part.PartMode = PartMode.Flight;
			if (part == root) continue;

			part.GetParent().AddChild(CreateJoint(root, part, root.Position));
			part.GetParent().AddChild(CreateJoint(root, part, part.Position));
		}

		var newScene = ResourceLoader.Load<PackedScene>("res://Konstruct/Fly.tscn").Instance<Node2D>();

		rocketBase.GetParent().RemoveChild(rocketBase);
		currentScene.QueueFree();

		newScene.AddChild(rocketBase);

		tree.Root.AddChild(newScene);

		var camera = newScene.GetNode("Camera2D");
		camera.GetParent().RemoveChild(camera);
		root.AddChild(camera);
	}

	private static PinJoint2D CreateJoint(Node nodeA, Node nodeB, Vector2 position)
	{
		var joint = new PinJoint2D();
		joint.NodeA = new NodePath("../" + nodeA.Name);
		joint.NodeB = new NodePath("../" + nodeB.Name);
		joint.Bias = 0.9f;
		joint.Position = position;
		return joint;
	}
}
