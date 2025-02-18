using Godot;
using System;

public partial class Door : Node2D
{
	public Vector2 openPosition;
	public Vector2 closePosition;

	Vector2 destination = new Vector2();
	bool shouldMove = false;

    public override void _Ready()
    {
		closePosition = GlobalPosition;
		openPosition = GlobalPosition + new Vector2(0, 124);
    }

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
			GlobalPosition = GlobalPosition.Lerp(destination, 0.2f);
			if(Position.DistanceTo(destination) < Mathf.Epsilon) 
				shouldMove = false;
		}
	}
}
