using Godot;
using System;

[GlobalClass]
public partial class FairyActionAngerChange : FairyAction
{
    [Export]
    public int ChangeAmount { get; set; }

    public FairyActionAngerChange() : this(E_FairyAction.SayRandom, 0, 0) { }

    public FairyActionAngerChange(E_FairyAction actionType, float actionTime, int changeAmount)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        ChangeAmount = changeAmount;
    }
}
