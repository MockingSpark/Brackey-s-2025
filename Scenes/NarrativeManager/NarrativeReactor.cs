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
    public NarrativeReactor[] ReactorsToActivate { get; set; }

    [Export]
    public bool flush = false;

    [Export]
    public bool singleUse = false;

    [Export]
    public bool ready = true;



    public void SendActions(Node2D body)
    {
        SendActions();
    }

    public void SendActions()
    {
        if (!ready) return;

        if (Targets.Length > 0)
        {
            int count = 0;
            foreach (var action in Container.Actions)
            {
                if (action.ActionType == E_FairyAction.FocusOnObject)
                {
                    ((FairyActionFocus)action).Target = Targets[count];
                    count++;
                }
                else if (action.ActionType == E_FairyAction.CameraMoveTarget)
                {
                    ((MoveCameraTargetAction)action).TargetNode = Targets[count];
                    count++;
                }
            }
        }
        if (ReactorsToActivate.Length > 0)
        {
            foreach (var reactor in ReactorsToActivate)
            {
                reactor.Activate();
            }
        }
        if(Container != null)
        {
            NarrativeManager.Instance.ReceiveActions(Container, flush);
        }
        if (singleUse)
        {
            Deactivate();
        }
    }

    public void Deactivate(Node2D body)
    {
        Deactivate();
    }
    public void Deactivate()
    {
        QueueFree();
    }
    public void Activate(Node2D body)
    {
        Activate();
    }
    public void Activate()
    {
        ready = true;
    }
}
