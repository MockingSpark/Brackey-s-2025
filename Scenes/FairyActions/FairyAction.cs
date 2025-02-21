using Godot;
using Microsoft.VisualBasic;
using System;

public enum E_FairyAction { None, FocusOnPlayer, FocusOnObject, SaySpecific, SayRandom, HideText, CatchPlayer, ProtectPlayer, Score, LockPlayer, Container }
public enum E_FairyTalkType { Rdm, Save, Protect, Retrieve }

[GlobalClass]
public partial class FairyAction : Resource
{
    [Export]
    public E_FairyAction ActionType { get; set; }
    [Export]
    public float ActionTime { get; set; }
}