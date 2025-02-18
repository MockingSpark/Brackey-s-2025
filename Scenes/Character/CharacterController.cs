using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

	private bool jumping = false;
	private int playerDir = 1;
	private Vector2 inputDir;
    private AnimationPlayer animationPLayer;
	[Export]
	private PackedScene projectile;
	private Node2D rightThrowPoint;
	private Node2D leftThrowPoint;

	private int projectileCount = 0;
	private List<Base_Interactable> interactables = new List<Base_Interactable>();

	public int ProjectileCount { get => projectileCount; set => projectileCount = value; }

	public override void _Ready()
	{
		animationPLayer = GetNode<AnimationPlayer>("AnimationPlayer");
		rightThrowPoint = GetNode<Node2D>("RightThrowPoint");
		leftThrowPoint = GetNode<Node2D>("LeftThrowPoint");
	}

	public override void _Process(double delta)
	{
		HandleAnimation();
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
			jumping = true;
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		inputDir = Input.GetVector("Left_Move", "Right_Move", "ui_up", "ui_down");

		if(inputDir.X != 0)
        {
            playerDir = inputDir.X > 0 ? 1 : -1;
        }

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

	void HandleAnimation()
    {
		if (!IsOnFloor())
        {
			if(jumping)
            {
				jumping = false;
                string animToPlay = "";
                animToPlay += playerDir == 1 ? "R_" : "L_";
                animToPlay += "Jump";
                animToPlay += projectileCount > 0 ? "Spear" : "Empty";
                animationPLayer.Play(animToPlay);
            }
			else
            {
                string animToPlay = "";
                animToPlay += playerDir == 1 ? "R_" : "L_";
                animToPlay += "JumpIdle";
                animToPlay += projectileCount > 0 ? "Spear" : "Empty";
                animationPLayer.Play(animToPlay);
            }

        }
		else if(IsOnFloor())
        {
            string animToPlay = "";
            animToPlay += playerDir == 1 ? "R_" : "L_";
            animToPlay += inputDir.X != 0 ? "Walk" : "Idle";
            animToPlay += projectileCount > 0 ? "Spear" : "Empty";
            animationPLayer.Play(animToPlay);
        }
    }

	private void ThrowProjectile()
	{
		if (projectileCount == 0) return;

		projectileCount--;

		var newProjectile = projectile.Instantiate<Projectile>();
		GetTree().Root.AddChild(newProjectile);
		if (playerDir < 0)
		{
			((Node2D)newProjectile).Transform = leftThrowPoint.GlobalTransform;
		}
		else
		{
			((Node2D)newProjectile).Transform = rightThrowPoint.GlobalTransform;
		}
		newProjectile.SetUpProjectile(playerDir < 0);
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
