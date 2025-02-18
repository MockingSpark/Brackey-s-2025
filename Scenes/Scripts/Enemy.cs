using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    AnimatedSprite2D sprite;

	Vector2 direction = new Vector2();

    public override void _Ready()
    {
        direction = Vector2.Right;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        sprite.FlipH = Velocity.X < 0;
        if (Velocity.X > Mathf.Epsilon || Velocity.X < -Mathf.Epsilon)
        {
            sprite.Play("Move");
        }
        else
        {
            sprite.Play("Idle");
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var test = GlobalPosition - new Vector2();

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        velocity.X = direction.X * Speed;

		Velocity = velocity;
		MoveAndSlide();
	}

	public void TurnAround(Vector2 directionToGo)
	{
        direction = directionToGo;
	}
}
