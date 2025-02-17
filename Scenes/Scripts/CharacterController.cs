using Godot;
using System;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

public partial class CharacterController : CharacterBody2D
{
	[Export]
	public float speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;

    private AnimatedSprite2D animatedSprite;
	private Vector2 inputDir;

	private PackedScene projectile;
	private Node2D rightThrowPoint;
	private Node2D leftThrowPoint;

    public override void _Ready()
    {
		projectile = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        rightThrowPoint = GetNode<Node2D>("RightThrowPoint");
        leftThrowPoint = GetNode<Node2D>("LeftThrowPoint");
    }

    public override void _Process(double delta)
    {
		if (inputDir.X < 0)
		{
			animatedSprite.FlipH = true;
			animatedSprite.Play("Run");
		}
		else if(inputDir.X > 0)
        {
            animatedSprite.FlipH = false;
            animatedSprite.Play("Run");
        }
		else
        {
            animatedSprite.Play("Idle");
		}

        // Handle Jump.
        if (Input.IsActionJustPressed("Action"))
        {
			ThrowProjectile();
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		inputDir = Input.GetVector("Left_Move", "Right_Move", "ui_up", "ui_down");
		if (inputDir.X != 0)
		{
			if(IsOnFloor())
			{
				velocity.X = Mathf.Lerp(Velocity.X, inputDir.X * speed, 0.3f);
			}
			else if(inputDir.X * velocity.X > 0)
			{
				velocity.X = Mathf.Lerp(Velocity.X, inputDir.X * speed, 0.3f);
			}
			else
			{
				velocity.X = Mathf.Lerp(Velocity.X, velocity.X + (inputDir.X * speed / 5), 0.3f);
			}
		}
		else
		{
			if(IsOnFloor())
			{
				velocity.X = Mathf.Lerp(Velocity.X, 0, 0.5f);
			}
			else
			{
				velocity.X = Mathf.Lerp(Velocity.X, 0, 0.05f);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    private void ThrowProjectile()
    {
		var newProjectile = projectile.Instantiate<Projectile>();
        Owner.AddChild(newProjectile);
		if (animatedSprite.FlipH)
		{
			((Node2D)newProjectile).Transform = leftThrowPoint.GlobalTransform;
		}
		else
		{
            ((Node2D)newProjectile).Transform = rightThrowPoint.GlobalTransform;
        }
		newProjectile.SetUpProjectile(animatedSprite.FlipH);
    }
}
