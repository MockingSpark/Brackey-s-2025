using Godot;
using System;

public partial class CharacterMovement : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;

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
		Vector2 direction = Input.GetVector("Left_Move", "Right_Move", "ui_up", "ui_down");
		if (direction.X != 0)
		{
			if(IsOnFloor())
			{
				velocity.X = Mathf.Lerp(Velocity.X, direction.X * Speed, 0.3f);
			}
			else if(direction.X * velocity.X > 0)
			{
				velocity.X = Mathf.Lerp(Velocity.X, direction.X * Speed, 0.3f);
			}
			else
			{
				velocity.X = Mathf.Lerp(Velocity.X, velocity.X + (direction.X * Speed / 5), 0.3f);
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
}
