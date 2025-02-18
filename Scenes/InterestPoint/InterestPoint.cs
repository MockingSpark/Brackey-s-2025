using Godot;
using System;

public partial class InterestPoint : Node2D
{
    [Signal]
    public delegate void OnPointActivatedEventHandler(InterestPoint point);



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        AddToGroup("InterestPoint");
        Area2D collision = GetNode<Area2D>("Area2D");
        collision.BodyEntered += OnBodyEntered;
    }

    protected void OnBodyEntered(Node2D body)
    {
         EmitSignal(SignalName.OnPointActivated, this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
