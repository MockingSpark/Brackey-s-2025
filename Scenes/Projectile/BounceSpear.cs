using Godot;

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
