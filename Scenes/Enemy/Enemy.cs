using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Enemy : CharacterBody2D
{
    [Signal]
    public delegate void PlayerHitEventHandler();

    [Export]
    public float Speed = 300.0f;

    AnimatedSprite2D sprite;
    bool dead = false;
    bool stunned = false;
    [Export]
    float stunTime = 10;
    double stunTimer = 0;

	Vector2 direction = new Vector2();

    Node2D target;

    public override void _Ready()
    {
        direction = Vector2.Right;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        if(stunned)
        {
            sprite.SelfModulate = new Color(sprite.SelfModulate.R, sprite.SelfModulate.G, sprite.SelfModulate.B, (float)stunTimer - MathF.Floor((float)stunTimer) + 0.2f);
            stunTimer += delta;
            if(stunTimer > stunTime)
            {
                stunned = false;
                stunTimer = 0;
            }
        }

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
        if (!stunned && !dead)
        {
            Vector2 velocity = Velocity;
            var test = GlobalPosition - new Vector2();

            if (!IsOnFloor())
            {
                velocity += GetGravity() * (float)delta;
            }
            if (target != null)
            {
                Vector2 directionToTarget = GlobalPosition.DirectionTo(target.GlobalPosition);
                velocity.X = directionToTarget.X * Speed;
            }
            else
            {
                velocity.X = direction.X * Speed;
            }
            Velocity = velocity;
        }

		MoveAndSlide();
	}

    public void OnHit(Node2D body)
    {
        if(body as CharacterController != null)
        {
            Velocity = new Vector2(0, 0);
            GlobalPosition -= Position.DirectionTo(body.GlobalPosition) * 10;
            stunned = true;
            EmitSignal(SignalName.PlayerHit);
        }
        else if(body.GetParent<Projectile>() != null)
        {        
            body.GetParent<Projectile>().Destroy();
            Die();
        }
    }

    private void Die()
    {
        QueueFree();
    }

	public void TurnAround(Vector2 directionToGo)
	{
        direction = directionToGo;
	}

    public void DetectPlayer(Node2D body)
    {
        target = body;
    }

    public void LosePlayer(Node2D body)
    {
        if(target == body)
        {
            target = null;
        }
    }
}
