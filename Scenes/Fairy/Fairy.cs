using Godot;
using System;
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

	private AngryFairy angerVfx;
	private float maxAnger = 5;
	[Export]
	Gradient angerGradient;

    [Export]
    PackedScene boldBubbleScene;
    [Export]
    PackedScene roundBubbleScene;

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

	protected List<DialogueBubble>  inWorldDialogueBubbles = new List<DialogueBubble>();

	private Node2D focusPoint;
	private Vector2 focusOffset;
	private bool emergency = false;

	private bool allowSpearProduction = false;
	[Export]
	private PackedScene spearScene;

	[Signal]
	public delegate void FairyActionErrorEventHandler();

	public CharacterController Player 
	{
		get 
		{
			if(player == null)
            {
                player = GetParent().GetNode<CharacterController>("Player");
				player.OnRequestSpear += GiveSpear;
            }

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
		angerVfx = GetNode<AngryFairy>("angryFairyVFX");
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").SetModulate(fairyColor);
    }

	protected void ReadDialogue(Dialogue dialogue)
	{
		if (dialogue.linkedToFairy)
		{
			ReadBubbleAttached(dialogue);
        }
		else 
		{
			CreateBubble(dialogue);
		}
	}

    private void CreateBubble(Dialogue dialogue)
    {
        DialogueBubble newBubble;
		if(dialogue.isBold)
		{
			newBubble = boldBubbleScene.Instantiate<DialogueBubble>();
		}
		else
        {
            newBubble = roundBubbleScene.Instantiate<DialogueBubble>();
        }

        NarrativeManager.Instance.CallDeferred("add_child", newBubble);
		newBubble.SetDeferred("Text", dialogue.GetText());
		newBubble.GlobalPosition = dialogue.position;
		newBubble.Resize();
		newBubble.SetSize(newBubble.Size * dialogue.bubbleScaleForInWorld);

		newBubble.ShowBubble();

		inWorldDialogueBubbles.Add(newBubble);
    }

    protected void ReadBubbleAttached(Dialogue dialogue)
    {
        HideBubbles();
        if (dialogue.isBold)
        {
            boldBubble.ShowBubble();
            boldBubble.Text = dialogue.GetText();
        }
        else
        {
            roundBubble.ShowBubble();
            roundBubble.Text = dialogue.GetText();
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

	public void AllowSpearProdution(bool allow)
	{
		allowSpearProduction = allow;
	}

    private void GiveSpear()
    {
        if (allowSpearProduction && focusPoint == null)
        {
            var newProjectile = spearScene.Instantiate<BounceSpear>();
            newProjectile.Transform = Transform;
				GetTree().Root.GetNode<Node>("SpearParent").CallDeferred("add_child", newProjectile);
            newProjectile.CallDeferred("InitialBounce", !ShouldLookLeft());
            Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.ProjectileCreated, 1);
            Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.HelpReceived, 1);
        }
    }


    #region Fairy Actions
    public void NoAction()
	{
		emergency = false;
		focusPoint = null;
		GetNewFollowPoint();
	}

	public void UpdateAnger(int modification)
	{
		int newAnger = Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.AngerValue, modification);
		if (modification > 0)
		{
			angerVfx.run();
		}
		FairyColor = angerGradient.Sample(newAnger / maxAnger);
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

	public void HideText(bool HideFairys, bool HideGlobal)
	{
		if(HideFairys)
		{
			HideBubbles();
		}

		if(HideGlobal)
		{
			foreach(DialogueBubble bubble in inWorldDialogueBubbles)
			{
				bubble.QueueFree();
			}

			inWorldDialogueBubbles.Clear();
		}
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

        Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.Saves, 1);
        Blackboard.Instance.OffsetValue(EBlackboardType.Level, EBlackboardKey.HelpReceived, 1);
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
