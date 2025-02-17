using Godot;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

public partial class Projectile : Area2D
{
	[Export]
	public float projectileSpeed = 900.0f;
    [Export]
    public float rotSpeed = 10.0f;
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
		if(bouncing)
		{
			int bounceDir = isLeft ? 1 : -1;
			Rotate(bounceDir * MathF.PI * rotSpeed * (float)delta);
		}

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

	private void OnAreaEntered(Area2D body)
	{
		if(body.IsInGroup("StickyWall"))
		{
            travelling = false;
            if (platformBody != null)
            {
                platformBody.SetDeferred("disabled", false);
            }
        }
		else if(body.IsInGroup("BumpyWall"))
        {
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
            QueueFree();
        }
	}
}
