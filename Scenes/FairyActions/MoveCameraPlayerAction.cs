using Godot;
using System;

[GlobalClass]
public partial class MoveCameraPlayerAction : FairyAction
{
    [Export]
    public float ZoomValue { get; private set; }
    [Export]
    public bool ZoomInRatio { get; private set; }

    public MoveCameraPlayerAction() : this(E_FairyAction.CameraMoveTarget, 0, -1, false) { }

    public MoveCameraPlayerAction(E_FairyAction actionType, float actionTime, float zoom, bool isRatio)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        ZoomValue = zoom;
        ZoomInRatio = isRatio;
    }
}
