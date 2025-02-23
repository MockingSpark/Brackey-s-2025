using Godot;
using System;


[GlobalClass]
public partial class FairyAction_GoCredits : FairyAction
{
    public FairyAction_GoCredits() : this(E_FairyAction.GoCredits, 0, 0) { }

    public FairyAction_GoCredits(E_FairyAction actionType, float actionTime, int changeAmount)
    {
        ActionType = actionType;
        ActionTime = actionTime;
    }
}
