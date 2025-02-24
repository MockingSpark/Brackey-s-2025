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


	private bool locked = false;

	private bool respawning = false;
    private float respawnSpeed = 0.05f;
    private float respawnTolerance = 10;
	private Vector2 respawnPoint;
	private Vector2 startPosition;
	private CollisionShape2D collisionShape;

	private bool jumping = false;
	private int playerDir = 1;
	private Vector2 inputDir;
    private AnimationPlayer animationPLayer;

	[Export]
	private PackedScene projectile;
	private Node2D rightThrowPoint;
	private Node2D leftThrowPoint;

    [Export]
    private float coyoteJumpTolerance;
    private float timeLeftFloor = 0f;

    private Buffer buffer;


    Node spearParentNode;
    private int projectileCount = 0;
	private List<Base_Interactable> interactables = new List<Base_Interactable>();

        
	public int ProjectileCount { get => projectileCount; set => projectileCount = value; }

    [Signal]
    public delegate void OnRequestSpearEventHandler();
    [Signal]
    public delegate void OnNoSpearEventHandler();


    public override void _Ready()
	{
		animationPLayer = GetNode<AnimationPlayer>("AnimationPlayer");
		rightThrowPoint = GetNode<Node2D>("RightThrowPoint");
		leftThrowPoint = GetNode<Node2D>("LeftThrowPoint");
        collisionShape = GetNode<CollisionShape2D>("CharacterCollision");
        buffer = GetNode<Buffer>("Buffer");
        Camera.Instance.TeleportToTarget(this);
        startPosition = GlobalPosition;
        CreateSpearParent();
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

                Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.InteractionCount, 1);
            }
        }
        else if (Input.IsActionJustPressed("FairyInput"))
        {
            EmitSignal(SignalName.OnRequestSpear);
        }
	}

	public override void _PhysicsProcess(double delta)
	{
        if (buffer != null)
        {
            buffer.TimeAdvance(delta);
        }

        if(respawning)
        {
            HandleRespawn((float)delta);
        }
        else if(locked)
        {
            HandleLocked((float)delta);
        }
        else
        {
            HandleGeneralMovement((float)delta);
        }	
	}

    void HandleLocked(float delta)
    {
        var velocity = Velocity;
        velocity.X = 0;
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * delta;
        }
        Velocity = velocity;

        MoveAndSlide();
    }

    void HandleRespawn(float deltaTime)
    {
        if(GlobalPosition.DistanceTo(respawnPoint) < respawnTolerance)
        {
            respawning = false;
            collisionShape.SetDeferred("disabled", false);
            return;
        }

        GlobalPosition = Utils.LerpStepped(GlobalPosition, respawnPoint, respawnSpeed, Camera.Instance.MaxCameraSpeed * deltaTime);//  GlobalPosition.Lerp(respawnPoint, respawnSpeed);
    }

    public bool CanJump()
    {
        return IsOnFloor() || coyoteJumpTolerance >= timeLeftFloor;
    }

	void HandleGeneralMovement(float delta)
	{

        Vector2 velocity = Velocity;

        if(IsOnFloor())
        {
            timeLeftFloor = 0f;
        }
        else
        {
            timeLeftFloor += delta;
        }
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * delta;
        }

        // Handle Jump.
        if ((Input.IsActionJustPressed("Jump") && CanJump()) || buffer.WasActionSimulated("Jump"))
        {
            jumping = true;
            velocity.Y = JumpVelocity;

        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        inputDir = Input.GetVector("Left_Move", "Right_Move", "ui_up", "ui_down");

        if (inputDir.X != 0)
        {
            playerDir = inputDir.X > 0 ? 1 : -1;
        }

        if (inputDir.X != 0)
        {
            if (IsOnFloor())
            {
                velocity.X = Mathf.Lerp(Velocity.X, inputDir.X * speed, 0.3f);
            }
            else if (inputDir.X * velocity.X > 0)
            {
                velocity.X = Mathf.Lerp(Velocity.X, inputDir.X * speed, 0.3f);
            }
            else
            {
                velocity.X = Mathf.Lerp(Velocity.X, velocity.X + (inputDir.X * speed / 7), 0.3f);
            }
        }
        else
        {
            if (IsOnFloor())
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
                if (MathF.Abs(collider.LinearVelocity.X) < pushMaxSpeed)
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

    public void LockPlayer(bool shouldLock)
    {
        locked = shouldLock;
        inputDir = new Vector2(0,0);
    }

	private void ThrowProjectile()
	{
        if (projectileCount == 0)
        {
            EmitSignal(SignalName.OnNoSpear);
            return;
        }

		projectileCount--;

		var newProjectile = projectile.Instantiate<Projectile>();
		spearParentNode.AddChild(newProjectile);
		if (playerDir < 0)
		{
			((Node2D)newProjectile).Transform = leftThrowPoint.GlobalTransform;
		}
		else
		{
			((Node2D)newProjectile).Transform = rightThrowPoint.GlobalTransform;
		}
		newProjectile.SetUpProjectile(playerDir < 0);

        Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.ProjectileThrowCount, 1);
    }

	public void AddInteractable(Base_Interactable interactable)
	{
		interactables.Add(interactable);
	}

	public void RemoveInteractable(Base_Interactable base_Interactable)
	{
		interactables.Remove(base_Interactable);
	}

	public void Respawn(Vector2 respawnPoint)
	{
        Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.Saves, 1);
        Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.HelpReceived, 1);
		respawning = true;
        Velocity = new Vector2();
		collisionShape.SetDeferred("disabled", true);
		this.respawnPoint = respawnPoint;
	}

    public void ReturnToStart()
    {
        Respawn(startPosition);
        projectileCount = 0;
        spearParentNode.QueueFree();

        GetTree().CreateTimer(0.01f).Timeout += () =>
        {
            CreateSpearParent();
        };

    }

    public void CreateSpearParent()
    {
        spearParentNode = new Node();
        spearParentNode.Name = "SpearParent";
        GetTree().Root.CallDeferred("add_child", spearParentNode);
    }
}
