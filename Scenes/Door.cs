using Godot;
using System;

public partial class Door : Node2D
{
	[Export]
	public Vector2 openPosition;
	[Export]
	public Vector2 closePosition;

	Vector2 destination = new Vector2();
	bool shouldMove = false;

	public void OpenDoor()
	{
		destination = openPosition;
		shouldMove = true;
	}
	public void CloseDoor()
	{
		destination = closePosition;
		shouldMove = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (shouldMove)
		{
			Position = Position.Lerp(destination, 0.2f);
			if(Position.DistanceTo(destination) < Mathf.Epsilon) 
				shouldMove = false;
		}
	}
}
