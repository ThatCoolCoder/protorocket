using Godot;
using System;

public class PrototypeButton : Button
{
	public Prototype Prototype;

	public override void _Ready()
	{
		if (Prototype == null)
		{
			GD.PushError("Error: prototype instance was not set for button");
			return;
		}
		Text = Prototype.Name;
		Connect("pressed", this, "OnClicked");
	}

	private void OnClicked()
	{
		GetTree().ChangeScene(Prototype.RootScenePath);
	}
}
