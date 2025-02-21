using Godot;
using System;

public partial class AngryFairy : Node2D
{
	
	public void run()
	{
		Show();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("emit");
	}
}
