using Godot;
using System;

[GlobalClass]
public partial class FairyActionPlayerFocus : FairyAction
{
    public FairyActionPlayerFocus() : this(E_FairyAction.FocusOnPlayer, 0) { }

    public FairyActionPlayerFocus(E_FairyAction actionType, float actionTime)
    {
        ActionType = actionType;
        ActionTime = actionTime;
    }
}
