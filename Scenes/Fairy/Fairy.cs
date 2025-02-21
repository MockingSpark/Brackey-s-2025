using Godot;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Fairy : Node2D
{
	[Export]
	public float Speed = 1;
	private CharacterController player;
	[Export]
	public float maxPlayerFollowDistance = 300f;
	[Export]
	public float minPlayerFollowDistance = 100f;
	private Vector2 playerFollowPoint = new Vector2();
	[Export]
	private float newPointTimer = 2f;
	[Export]
	private float inWallTimer = 1f;
	private float moveTimer = 0;
	private bool isInWall = false;
	
	private Color fairyColor;
	[Export]
	public Color FairyColor
	{
		get
		{
			return fairyColor;
		}
		set
		{
			fairyColor = value;
			if(IsNodeReady())
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").SetModulate(fairyColor);
		}
	}

	[Export]
	private DialogueBubble roundBubble;
	[Export]
	private DialogueBubble boldBubble;

	private Node2D focusPoint;
	private Vector2 focusOffset;
	private bool emergency = false;

	[Signal]
	public delegate void FairyActionErrorEventHandler();

	public CharacterController Player 
	{
		get 
		{
			if(player == null)
				player = GetParent().GetNode<CharacterController>("Player");

			return player; 
		} 
		set => player = value; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNewFollowPoint();
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
		boldBubble.Visible = false;
		roundBubble.Visible = false;
		NarrativeManager.Instance.RegisterFairy(this);
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").SetModulate(fairyColor);
	}

	protected void ReadDialogue(Dialogue dialogue)
	{
		HideBubbles();
		if(dialogue.isBold)
		{
			boldBubble.ShowBubble();
			boldBubble.Text = dialogue.text;
		}
		else
		{
			roundBubble.ShowBubble();
			roundBubble.Text = dialogue.text;
		}
	}

	private void GiveScore(float score)
	{
		boldBubble.Visible = true;
		boldBubble.Text = score.ToString("0.0");
	}

	private void HideBubbles()
	{
		boldBubble.HideBubble();
		roundBubble.HideBubble();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player == null)
			return;

		if(focusPoint == null)
		{
			moveTimer += (float)delta;
			if (isInWall && moveTimer > inWallTimer)
			{
				GetNewFollowPoint();
			}
			else if(moveTimer > newPointTimer)
			{
				GetNewFollowPoint();
			}
		}

		Vector2 position = focusPoint == null ? Player.GlobalPosition - playerFollowPoint : focusPoint.GlobalPosition + focusOffset;

		float speed = emergency ? 0.9f : (float)(Speed * delta);

		GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = !ShouldLookLeft();


		GlobalPosition = GlobalPosition.Lerp(position, speed);
	}

	public bool ShouldLookLeft()
	{
		if (focusPoint != null)
		{
			return Position.X - focusPoint.Position.X > 0;
		}
		else
		{
			return Position.X - Player.Position.X > 0;
		}
	}

	public void ImpactedWall(Node2D wall)
	{
		if (focusPoint != null) return;

		isInWall = true;
		GetNewFollowPoint();
	}
	public void ExitedWall(Node2D wall)
	{
		if (focusPoint != null) return;

		isInWall = false;
	}

	public void GetNewFollowPoint()
	{
		if(Player == null) return;

		moveTimer = 0;

		float playerFollowDistance = (float)GD.RandRange(minPlayerFollowDistance, maxPlayerFollowDistance);

		float y = (float)GD.RandRange(playerFollowDistance / 4, playerFollowDistance / 2);
		float x = playerFollowDistance - y;

		if(GD.RandRange(0,1) == 0)
		{
			x = -x;
		}

		playerFollowPoint = new Vector2(x, y);
	}


	#region Fairy Actions
	public void NoAction()
	{
		emergency = false;
		focusPoint = null;
		GetNewFollowPoint();
	}

	public void FocusOnPlayer(float actionTimer)
	{
		emergency = false;
		focusPoint = null;
		GetNewFollowPoint();
	}

	public void FocusOnNode(Node2D node, Vector2 offset, float actionTimer)
	{
		emergency = false;
		focusPoint = node;
		focusOffset = offset;
	}

	public void ReadDialogueAction(Dialogue dialogue, float actionTimer)
	{
		if (dialogue == null)
        {
            SendErrorNotif();
            return;
		}
		ReadDialogue(dialogue);
	}

	public void HideText()
	{
		HideBubbles();
	}

	public void GivePlayerScore(float score, float actionTimer)
	{
		GiveScore(score);
	}

	public void ProtectPlayer(float actionTimer)
	{
		if (Player == null)
        {
            SendErrorNotif();
            return;
		}

		emergency = true;
		focusPoint = player;
	}
	public void SavePlayer(float actionTimer)
	{
		if (Player == null)
		{
			SendErrorNotif();
			return;
		}

		emergency = true;
		focusPoint = player;
	}

	public void RemoveEmergency()
	{
		emergency = false;
	}

	private void SendErrorNotif()
    {
        EmitSignal(SignalName.FairyActionError);
    }
	#endregion
}
