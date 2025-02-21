using Godot;
using System;

public partial class EnemyDie : Node2D
{
	public void run()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("deathAnimation");
	}
}
