using Godot;
using System;

[GlobalClass]
public partial class FairyActionNoText : FairyAction
{
    [Export]
    public bool HideFairysBubble = true;

    [Export]
    public bool HideInWorldBubbles = true;

    public FairyActionNoText() : this(E_FairyAction.HideText, 0, true, true) { }

    public FairyActionNoText(E_FairyAction actionType, float actionTime, bool hideFairysBubble, bool hideInWorldBubbles)
    {
        ActionType = actionType;
        ActionTime = actionTime;

        HideFairysBubble = hideFairysBubble;
        HideInWorldBubbles = hideInWorldBubbles;
    }
}
