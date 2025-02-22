using Godot;
using System;

[GlobalClass]
public partial class AreaDestroyBox : Area2D
{
    [Signal]
    public delegate void CrateDestroyedEventHandler();
    public override void _Ready()
    {
        BodyEntered += CheckForBox;
    }

    private void CheckForBox(Node2D body)
    {
        BoxCrate crate = body as BoxCrate;
        if (crate != null)
        {
            crate.Respawn();
            EmitSignal(SignalName.CrateDestroyed);
        }
    }
}
