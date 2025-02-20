using Godot;
using System;

[GlobalClass]
public partial class FairyActionNoText : FairyAction
{
    public FairyActionNoText() : this(E_FairyAction.HideText, 0) { }

    public FairyActionNoText(E_FairyAction actionType, float actionTime)
    {
        ActionType = actionType;
        ActionTime = actionTime;
    }
}
