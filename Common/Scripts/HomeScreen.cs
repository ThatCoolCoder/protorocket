using Godot;
using System;

public class HomeScreen : Control
{
	private Prototype[] prototypes =
	{
		new("Basic Gravity", "res://Gravity/Gravity.tscn")
	};

	private PackedScene prototypeButton = ResourceLoader.Load<PackedScene>("res://Common/Scenes/PrototypeButton.tscn");


	public override void _Ready()
	{
		var buttonHolder = GetNode<Control>("VBoxContainer/Prototypes");
		foreach (var prototype in prototypes)
		{
			var instance = prototypeButton.Instance<PrototypeButton>();
			instance.Prototype = prototype;
			buttonHolder.AddChild(instance);
		}
	}
}
