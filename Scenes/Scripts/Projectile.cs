using Godot;
using System;
using System.Diagnostics;

public partial class Projectile : Area2D
{
	[Export]
	public float projectileSpeed = 900.0f;

	private CollisionShape2D platformBody;

	bool stuck = false;

    public override void _Ready()
    {
		platformBody = GetNode<CollisionShape2D>("PlatformBody/PlatformShape");
		if (platformBody != null)
		{
			platformBody.SetDeferred("disabled", true);
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		if (stuck) return;

		Position += Transform.X * projectileSpeed * (float)delta;
	}

	public void SetUpProjectile(bool isLeft)
	{
		if (isLeft)
		{
			if (platformBody != null)
            {
				Debug.Print("Before rot " + platformBody.Transform.Rotation);
				platformBody.Rotate(Mathf.Pi);
                Debug.Print("After rot " + platformBody.Transform.Rotation);
            }
		}
	}

	private void OnAreaEntered(Area2D body)
	{
        stuck = true;
        if (platformBody != null)
        {
            platformBody.SetDeferred("disabled", false);
        }
	}
}
