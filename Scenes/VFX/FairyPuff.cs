using Godot;
using System;

public partial class FairyPuff : Node2D
{
	public void run()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("emit");
	}
}
