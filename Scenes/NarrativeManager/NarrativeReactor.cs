using Godot;
using System;
using System.Collections.Generic;

public partial class NarrativeReactor : Node
{
    [Export]
    public FairyActionContainer Container { get; set; }

    [Export]
    public Node2D[] Targets { get; set; }

    [Export]
    public bool singleUse = false;

    public void SendActions(Node2D body)
    {
        if (Targets.Length > 0)
        {
            int count = 0;
            foreach (var action in Container.Actions)
            {
                if(action.ActionType == E_FairyAction.FocusOnObject)
                {
                    ((FairyActionFocus)action).Target = Targets[count];
                    count++;
                }
            }
        }
        NarrativeManager.Instance.ReceiveActions(Container);
        if (singleUse)
        {
            QueueFree();
        }
    }
}
