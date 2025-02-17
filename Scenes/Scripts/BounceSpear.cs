using Godot;
using System;
using System.Collections;
using System.Diagnostics;

public partial class BounceSpear : RigidBody2D
{
	bool start = true;
	public override void _Process(double delta)
	{
		if(start)
		{
			start = false;
			CallDeferred("Torque");
		}
	}

	public void Torque()
	{
		ApplyTorqueImpulse(10000);
	}


}
