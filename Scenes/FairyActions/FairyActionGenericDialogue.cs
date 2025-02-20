using Godot;
using System;

[GlobalClass]
public partial class FairyActionGenericDialogue : FairyAction
{
    [Export]
    public E_FairyTalkType TalkType { get; set; }

    public FairyActionGenericDialogue() : this(E_FairyAction.SayRandom, 0, E_FairyTalkType.Rdm) { }

    public FairyActionGenericDialogue(E_FairyAction actionType, float actionTime, E_FairyTalkType talkType)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        TalkType = talkType;
    }
}
