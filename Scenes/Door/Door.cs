using Godot;
using System;

public partial class Door : Node2D
{
	public Vector2 openPosition;
	public Vector2 closePosition;

	[Export]
	public bool isVertical = true;
	[Export]
	public bool movesLeft = false;

	Vector2 destination = new Vector2();
	bool shouldMove = false;

    public override void _Ready()
    {
		closePosition = GlobalPosition;
		if (isVertical)
		{
			openPosition = GlobalPosition + new Vector2(0, 124);
		}
		else
		{
			if (movesLeft)
            {
                openPosition = GlobalPosition + new Vector2(-124, 0);
            }
			else
            {
                openPosition = GlobalPosition + new Vector2(124, 0);
            }
		}
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
