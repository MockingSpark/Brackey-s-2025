using Godot;
using System;

[GlobalClass]
public partial class FairyActionProtect : FairyAction
{
    public FairyActionProtect() : this(E_FairyAction.ProtectPlayer, 0) { }

    public FairyActionProtect(E_FairyAction actionType, float actionTime)
    {
        ActionType = actionType;
        ActionTime = actionTime;
    }
}
