using Godot;
using System;

public abstract partial class Base_Interactable : Node
{
	public void OnPlayerEntered(Node2D body)
	{
		var character = body as CharacterController;
		if(character != null)
		{
			character.AddInteractable(this);
		}
	}

	public void OnPlayerExit(Node2D body)
	{
		var character = body as CharacterController;
		if (character != null)
		{
			character.RemoveInteractable(this);
		}
	}

	public abstract void Interact();
	public abstract void Interact(CharacterController character);
}
