using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public float projectileSpeed = 900.0f;

	private StaticBody2D platformBody;

	bool stuck = false;

    public override void _Ready()
    {
		platformBody = GetNode<StaticBody2D>("PlatformBody");
		if (platformBody != null)
		{
			platformBody.ProcessMode = ProcessModeEnum.Disabled;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		if (stuck) return;

		Position += Transform.X * projectileSpeed * (float)delta;
	}

	private void OnAreaEntered(Area2D body)
	{
		stuck = true;
        if (platformBody != null)
        {
            platformBody.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
