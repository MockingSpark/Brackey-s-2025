using Godot;
using System;

[GlobalClass]
public partial class MoveCameraTargetAction : FairyAction
{
    public Node2D TargetNode { get; set; }
    [Export]
    public float ZoomValue { get; set; }
    [Export]
    public bool ZoomInRatio {  get; set; }

    public MoveCameraTargetAction() : this(E_FairyAction.CameraMoveTarget, 0, null, -1, false) { }

    public MoveCameraTargetAction(E_FairyAction actionType, float actionTime, Node2D target, float zoom, bool isRatio)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        TargetNode = target;
        ZoomValue = zoom;
        ZoomInRatio = isRatio;
    }
}
