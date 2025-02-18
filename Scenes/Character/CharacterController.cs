using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterController : CharacterBody2D
{
	[Export]
	public float speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;
	[Export]
	public float pushForce = 200.0f;
    [Export]
    public float pushMaxSpeed= 10.0f;

    private AnimatedSprite2D animatedSprite;
	private Vector2 inputDir;
	[Export]
	private PackedScene projectile;
	private Node2D rightThrowPoint;
	private Node2D leftThrowPoint;

	private int projectileCount = 0;
	private List<Base_Interactable> interactables = new List<Base_Interactable>();

	public int ProjectileCount { get => projectileCount; set => projectileCount = value; }

	public override void _Ready()
	{
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

		if (Input.IsActionJustPressed("Attack"))
		{
			ThrowProjectile();
		}
		else if (Input.IsActionJustPressed("Action"))
		{
			if (interactables.Count > 0)
			{
				interactables[0].Interact(this);
			}
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

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider() as RigidBody2D;
			if (collider != null && collider.IsInGroup("Pushable"))
			{
				if(MathF.Abs(collider.LinearVelocity.X) < pushMaxSpeed)
				{
					collider.ApplyCentralForce(collision.GetNormal() * -pushForce);
				}
			}
		}

		MoveAndSlide();
	}

	private void ThrowProjectile()
	{
		if (projectileCount == 0) return;

		projectileCount--;

		var newProjectile = projectile.Instantiate<Projectile>();
		GetTree().Root.AddChild(newProjectile);
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

	public void AddInteractable(Base_Interactable interactable)
	{
		interactables.Add(interactable);
	}

	internal void RemoveInteractable(Base_Interactable base_Interactable)
	{
		interactables.Remove(base_Interactable);
	}
}
