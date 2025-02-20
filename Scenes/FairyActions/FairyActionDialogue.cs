using Godot;
using System;

[GlobalClass]
public partial class FairyActionDialogue : FairyAction
{
    [Export]
    public Dialogue Dialogue { get; set; }
    public FairyActionDialogue() : this(E_FairyAction.SayRandom, 0, null) { }

    public FairyActionDialogue(E_FairyAction actionType, float actionTime, Dialogue dialogue)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        Dialogue = dialogue;
    }
}
