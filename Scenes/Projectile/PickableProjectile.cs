using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class PickableProjectile : Base_Interactable
{
	public override void Interact()
	{
		var character = ((CharacterController)GetTree().Root.FindChild("Player"));
		if (character == null)
		{
			Debug.Fail("Could not find player in root");
			return;
		}
		Interact(character);
	}

	public override void Interact(CharacterController character)
	{
		character.ProjectileCount++;
		Debug.Print("Character has " + character.ProjectileCount + " spears");
		GetParent().QueueFree();
		QueueFree();
	}
}
