using Godot;
using System;

[GlobalClass]
public partial class EndingChooser : Node
{
    [Export]
    FairyActionCondition Condition { get; set; }

    [Export]
    NarrativeReactor ConditionValidReactor;
    [Export]
    NarrativeReactor ConditionNotValidReactor;

    public void CheckCondition(Node2D node)
    {
        CheckCondition();
    }

    public void CheckCondition()
    {
        if(Condition.TestCondition()) 
        {
            ConditionValidReactor.Activate();
        }
        else
        {
            ConditionNotValidReactor.Activate();
        }
    }
}
