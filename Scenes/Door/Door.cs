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
	[Export]
	public bool invert = false;
	[Export]
	public float moveDelay = 0;

	Vector2 destination = new Vector2();
	bool shouldMove = false;

	public override void _Ready()
	{
		if (invert)
		{
			openPosition = GlobalPosition;
			if (isVertical)
			{
				closePosition = GlobalPosition + new Vector2(0, -124);
			}
			else
			{
				if (movesLeft)
				{
					closePosition = GlobalPosition + new Vector2(124, 0);
				}
				else
				{
					closePosition = GlobalPosition + new Vector2(-124, 0);
				}
			}
		}
		else
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
		
	}

	public void OpenDoor()
	{
		DelayOpen();
	}

	async void DelayOpen()
    {
		if(moveDelay > 0)
        {
            await ToSignal(CreateTween().TweenInterval(moveDelay), Tween.SignalName.Finished);
        }
        destination = openPosition;
        shouldMove = true;
    }


	public void CloseDoor()
	{
		DelayClose();
	}

	async void DelayClose()
    {
        if (moveDelay > 0)
        {
            await ToSignal(CreateTween().TweenInterval(moveDelay), Tween.SignalName.Finished);
        }
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
