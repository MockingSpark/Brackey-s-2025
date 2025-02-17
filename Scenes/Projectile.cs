using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public float projectileSpeed = 900.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * projectileSpeed * (float)delta;
	}
}
