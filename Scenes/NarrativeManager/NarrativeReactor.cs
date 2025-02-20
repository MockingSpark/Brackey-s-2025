using Godot;
using System;
using System.Collections.Generic;

public partial class NarrativeReactor : Node
{
    [Export]
    public FairyActionContainer Container { get; set; }

    public void SendActions(Node2D body)
    {
        NarrativeManager.Instance.ReceiveActions(Container);
    }
}
