using Godot;
using System;

namespace Common;

public class HomeScreen : Control
{
	private Prototype[] prototypes =
	{
		new("Basic Gravity", "res://Gravity/Gravity.tscn"),
		new("Railed MVC", "res://RailedMvc/RailedMvc.tscn")
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
