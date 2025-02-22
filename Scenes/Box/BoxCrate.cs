using Godot;
using System;
using System.Diagnostics;
using static Godot.WebSocketPeer;

public partial class BoxCrate : RigidBody2D
{
    private Vector2 initialPosition;
    private Sprite2D sprite;
    private AnimationPlayer animationPlayer;
    bool shouldTeleport;

    public override void _Ready()
    {
        initialPosition = GlobalPosition;
        sprite = GetNode<Sprite2D>("Sprite2D");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

    }
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if(shouldTeleport)
        {
            state.Transform = new Transform2D(state.Transform.Rotation, initialPosition);
            state.LinearVelocity = Vector2.Zero;  // Reset velocity to prevent sliding
            state.AngularVelocity = 0;
            shouldTeleport = false;
        }
    }
    public void Respawn()
    {
        animationPlayer.Play("DissolveAndReappear");

    }
    public void ReturnToInitialPosition()
    {
        shouldTeleport = true;
    }
}
