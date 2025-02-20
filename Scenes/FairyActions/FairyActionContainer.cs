using Godot;
using System;

[GlobalClass]
public partial class FairyActionContainer : FairyAction
{
    [Export]
    public FairyAction[] Actions { get; set; }
    [Export]
    public int Priority { get; set; }

    public FairyActionContainer() : this(E_FairyAction.Container, 0, new FairyAction[0], 0) { }

    public FairyActionContainer(E_FairyAction actionType, float actionTime, FairyAction[] actions, int priority)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        Actions = actions;
        Priority = priority;
    }
}
