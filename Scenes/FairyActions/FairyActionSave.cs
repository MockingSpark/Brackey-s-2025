using Godot;
using System;

[GlobalClass]
public partial class FairyActionSave : FairyAction
{
    public FairyActionSave() : this(E_FairyAction.CatchPlayer, 0) { }

    public FairyActionSave(E_FairyAction actionType, float actionTime)
    {
        ActionType = actionType;
        ActionTime = actionTime;
    }
}
