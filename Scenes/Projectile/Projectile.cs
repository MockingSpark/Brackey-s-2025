using Godot;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

public partial class Projectile : Node2D
{
	[Export]
	public float projectileSpeed = 900.0f;
	[Export]
	public PackedScene bounceSpear;

	private CollisionShape2D platformBody;

	bool travelling = true;
	bool bouncing = false;
	bool isLeft = false;

	public override void _Ready()
	{
		platformBody = GetNode<CollisionShape2D>("PlatformBody/PlatformShape");
		if (platformBody != null)
		{
			platformBody.SetDeferred("disabled", true);
		}
	}

	public override void _Process(double delta)
	{
		if (travelling)
		{
			Position += Transform.X * projectileSpeed * (float)delta;
		}
		return;
	}


	public void SetUpProjectile(bool isLeft)
	{
		this.isLeft = isLeft;
		if (isLeft)
		{
			if (platformBody != null)
			{
				platformBody.Rotate(Mathf.Pi);
			}
		}
	}

	public void OnBounce(Node2D node)
	{
		if (!travelling || bouncing) return;
		bouncing = true;

		var newProjectile = bounceSpear.Instantiate<BounceSpear>();
		newProjectile.Transform = Transform;
		newProjectile.Position -= newProjectile.Transform.X * 20;
		GetTree().Root.CallDeferred("add_child", newProjectile);
		if (isLeft)
		{
			var rdmX = GD.RandRange(100, 350.0);
			var rdmY = GD.RandRange(-400, -600);
			newProjectile.ApplyCentralImpulse(new Vector2((float)rdmX, (float)rdmY));
		}
		else
		{
			var rdmX = GD.RandRange(-100, -350.0);
			var rdmY = GD.RandRange(-400, -600);
			newProjectile.ApplyCentralImpulse(new Vector2((float)rdmX, (float)rdmY));
		}
        Destroy();
	}

	public void OnStick(Node2D node)
	{
		if (!travelling) return;
		travelling = false;
		if (platformBody != null)
		{
			platformBody.SetDeferred("disabled", false);
		}
	}

	public void Destroy()
	{
		QueueFree();
	}
}
