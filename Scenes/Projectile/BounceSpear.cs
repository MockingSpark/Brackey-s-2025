using Godot;
using System.Collections.Generic;

public partial class BounceSpear : RigidBody2D
{
	public async void InitialBounce(bool isLeft)
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        GetTree().Root.GetNode<Node>("SpearParent").AddChild(this);
        ApplyTorqueImpulse(10000);
        if (isLeft)
        {
            var rdmX = GD.RandRange(100, 350.0);
            var rdmY = GD.RandRange(-400, -600);
            ApplyCentralImpulse(new Vector2((float)rdmX, (float)rdmY));
        }
        else
        {
            var rdmX = GD.RandRange(-100, -350.0);
            var rdmY = GD.RandRange(-400, -600);
            ApplyCentralImpulse(new Vector2((float)rdmX, (float)rdmY));
        }
	}


}
