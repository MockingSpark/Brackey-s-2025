using Godot;
using System;

public partial class GlassDoor : Node2D
{
    [Signal]
    public delegate void DoorShatterEventHandler();

    public void ShatterDoor(Node2D node)
    {
        EmitSignal(SignalName.DoorShatter);
        QueueFree();
    }
}
