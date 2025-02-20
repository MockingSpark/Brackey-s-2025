using Godot;
using System;

[GlobalClass]
public partial class FairyActionLock : FairyAction
{
    [Export]
    public bool ShouldLock { get; set; }

    public FairyActionLock() : this(E_FairyAction.LockPlayer, 0, false) { }

    public FairyActionLock(E_FairyAction actionType, float actionTime, bool shouldLock)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        ShouldLock = shouldLock;
    }
}
