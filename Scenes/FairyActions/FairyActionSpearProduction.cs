using Godot;
using System;

[GlobalClass]
public partial class FairyActionSpearProduction : FairyAction
{
    [Export]
    public bool ShouldAllowSpearProduction { get; set; }

    public FairyActionSpearProduction() : this(E_FairyAction.Container, 0, false) { }

    public FairyActionSpearProduction(E_FairyAction actionType, float actionTime, bool shouldAllow)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        ShouldAllowSpearProduction = shouldAllow;
    }
}
