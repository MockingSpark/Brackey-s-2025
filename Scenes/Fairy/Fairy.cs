using Godot;
using Godot.Collections;

using System.Linq;

public partial class Fairy : Node2D
{
	[Export]
	public float Speed = 150f;
	[Export]
	public CharacterBody2D player;

    protected Array<Node> interestPoints;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
        interestPoints = GetTree().GetNodesInGroup("InterestPoint");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (player == null)
			return;

		Position = Position.MoveToward(player.Position, (float)(Speed * delta));

	}
}
