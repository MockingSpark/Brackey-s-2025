using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static InterestPoint;

public partial class Fairy : Node2D
{
	[Export]
	public float Speed = 150f;
	[Export]
	public CharacterBody2D player;

    protected List<InterestPoint> interestPointList = new List<InterestPoint>();

    protected InterestPoint currentPointFollowed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

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

    private void InterestPoint_OnPointActivated(InterestPoint point)
    {
        currentPointFollowed = point;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

		if (player == null)
			return;

        Vector2 position = currentPointFollowed == null ? player.Position : currentPointFollowed.Position;

        Position = Position.MoveToward(position, (float)(Speed * delta));

	}
}
