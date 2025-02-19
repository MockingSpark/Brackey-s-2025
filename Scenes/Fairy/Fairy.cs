using Godot;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Fairy : Node2D
{
	[Export]
	public float Speed = 1;
	[Export]
	public CharacterController player;
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

    protected List<InterestPoint> interestPointList = new List<InterestPoint>();

    protected InterestPoint currentPointFollowed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNewFollowPoint();
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
        var interestPointsAsNodes = GetTree().GetNodesInGroup("InterestPoint");
        foreach (Node node in interestPointsAsNodes)
        {
            if (node is InterestPoint interestPoint)
            {
                interestPointList.Add(interestPoint);
                interestPoint.OnPointActivated += InterestPoint_OnPointActivated;
            }
        }

    }

    protected void InterestPoint_OnPointActivated(InterestPoint point)
    {
        currentPointFollowed = point;
        if(currentPointFollowed.DialogueToPlay != null)
        {
            ReadDialogue(currentPointFollowed.DialogueToPlay);
        }
    }
    protected void ReadDialogue(Dialogue dialogue)
    {
        foreach(var str in dialogue.Text)
        {
            Debug.Print(str);
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (player == null)
			return;

        if(currentPointFollowed == null)
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

        Vector2 position = currentPointFollowed == null ? player.GlobalPosition - playerFollowPoint : currentPointFollowed.GlobalPosition;



        GlobalPosition = GlobalPosition.Lerp(position, (float)(Speed * delta));
	}

    public void ImpactedWall(Node2D wall)
    {
        if (currentPointFollowed != null) return;

        isInWall = true;
        GetNewFollowPoint();
    }
    public void ExitedWall(Node2D wall)
    {
        if (currentPointFollowed != null) return;

        isInWall = false;
    }

    public void GetNewFollowPoint()
    {
        if(player == null) return;

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
}
