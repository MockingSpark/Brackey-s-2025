using Godot;
using System;
using System.Diagnostics;
using System.Security;
public enum ECameraMode
{
	Attached,
	Static
}

public partial class Camera : Camera2D
{
	public static Camera Instance { get; private set; }

	[Export]
	public Node2D DefaultTargetNode;

	[ExportCategory("Debug")]
	[Export]
	protected ECameraMode currentMode = ECameraMode.Attached;
	
	/** In ECameraMode.Attached, which node to follow. */
	protected Node2D TargetNode = null;

	public override void _Ready()
	{
		if(Instance != null)
		{
			Debug.Print("WARNING: Two camera are present!");
		}
		Instance = this;

		// Set as active Camera
		this.MakeCurrent();

		if (DefaultTargetNode != null)
		{
			TargetNode = DefaultTargetNode;
			currentMode = ECameraMode.Attached;
		}
		else
		{
			currentMode = ECameraMode.Static;
		}

		//Not working during _Ready, trying to attach it directly to the level (ToDetach it from it's current parent if any)
		//Reparent(GetTree().Root.GetChildren(false)[0]);
	}

	public override void _Process(double delta)
	{
		switch (currentMode)
		{
			case ECameraMode.Attached:
				if(TargetNode != null)
				{
					GlobalPosition = TargetNode.Position;
				}
				break;

			case ECameraMode.Static:
				break;
		}
	}

	public void SetStaticMode(Nullable<Vector2> position = null)
	{
		currentMode = ECameraMode.Static;
	}


	public void SetAttachMode(Node2D attachTarget)
	{
		TargetNode = attachTarget;

		currentMode = ECameraMode.Attached;
	}
}
