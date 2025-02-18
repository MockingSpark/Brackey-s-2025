using Godot;
using System;

public partial class Button : Node
{
    [Signal]
    public delegate void ButtonPressedEventHandler();
    [Signal]
    public delegate void ButtonReleasedEventHandler();

    int pressers = 0;

    public void OnButtonPressed(Node2D body)
    {
        pressers++;
        if( pressers == 1 )
            EmitSignal(SignalName.ButtonPressed);
    }
    public void OnButtonReleased(Node2D body)
    {
        pressers--;
        if (pressers == 0)
            EmitSignal(SignalName.ButtonReleased);
    }
}
