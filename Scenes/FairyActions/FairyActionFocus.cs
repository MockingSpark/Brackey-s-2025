using Godot;
using System;
using static Godot.GodotThread;

[GlobalClass]
public partial class FairyActionFocus : FairyAction
{
    public Node2D Target { get; set; }
    public Vector2 FocusOffset { get; set; }

    public FairyActionFocus() : this(E_FairyAction.Container, 0, null, new Vector2()) { }

    public FairyActionFocus(E_FairyAction actionType, float actionTime, Node2D target, Vector2 offset)
    {
        ActionType = actionType;
        ActionTime = actionTime;
        Target = target;
        FocusOffset = offset;
    }
}
