using Godot;
using System;
using System.Diagnostics;

public partial class ButtonPushable : Sprite2D
{
	[Signal]
	public delegate void ButtonPressedEventHandler();
	[Signal]
	public delegate void ButtonReleasedEventHandler();

	int pressers = 0;

	public override void _Ready()
	{
		pressers = 0;
	}

	public void OnButtonPressed(Node2D body)
	{
		pressers++;
		if( pressers == 1 )
		{
			Frame = 0;
			EmitSignal(SignalName.ButtonPressed);
		}   
	}
	public void OnButtonReleased(Node2D body)
	{
		pressers--;
		if (pressers == 0)
		{
			Frame = 1;
			EmitSignal(SignalName.ButtonReleased);
		}
	}
}
