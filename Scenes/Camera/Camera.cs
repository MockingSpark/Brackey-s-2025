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
	/** node to attach when trigger _Ready() */
	public Node2D DefaultTargetNode;

	[Export]
	public float MaxCameraSpeed = 1200.0f;

    [Export]
    public float SpeedLerpStrenght = 0.5f;

    [Export]
    public float MaxZoomSpeed = 0.7f;

    [Export]
    public float ZoomLerpStrength = 0.8f;

    [ExportCategory("Debug")]
	[Export]
	protected Vector2 TargetZoom;

	[ExportCategory("Debug")]
	[Export]
	protected ECameraMode currentMode = ECameraMode.Attached;

    [ExportCategory("Debug")]
    [Export]
    protected float DefaultZoom;

    [ExportCategory("Debug")]
    [Export]
    /** node to attach when trigger _Ready() */
    protected Vector2 TargetPosition;

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

		DefaultZoom = Zoom.X;
		TargetZoom = Zoom;


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
				if(IsInstanceValid(TargetNode))
				{
                    TargetPosition = TargetNode.Position;
				}
				break;
			case ECameraMode.Static:
				break;
		}

		GlobalPosition = Utils.LerpStepped(GlobalPosition, TargetPosition, SpeedLerpStrenght, MaxCameraSpeed * (float)delta);

		Zoom = Utils.LerpStepped(Zoom, TargetZoom, ZoomLerpStrength, MaxZoomSpeed * (float)delta);
	}

	public void UseStaticMode(Nullable<Vector2> position = null)
	{
		currentMode = ECameraMode.Static;

		TargetPosition = position != null ? position.GetValueOrDefault() : GlobalPosition;
    }

	public void UseAttachMode(Node2D attachTarget)
	{
		TargetNode = attachTarget;

		currentMode = ECameraMode.Attached;
    }

    public void ResetZoom()
    {
        TransitionToZoom(DefaultZoom);
    }

    /** Transition to zoom relative to the default zoom.*/
    public void TransitionToZoomRelative(float ratioToDefault)
    {
        // Todo: Lerp to between zoom

        TransitionToZoom(ratioToDefault * DefaultZoom);
    }

    public void TransitionToZoom(float newZoom)
	{
		// Todo: Lerp to between zoom

		TargetZoom = new Vector2(newZoom, newZoom);
    }

}
