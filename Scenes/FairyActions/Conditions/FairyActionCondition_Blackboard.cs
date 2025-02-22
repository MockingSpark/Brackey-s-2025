using Godot;
using System;

[GlobalClass]
public partial class FairyActionCondition_Blackboard : FairyActionCondition
{
    public enum EConditionType
    {
        Lower,
        Greater,
        Equal
    }

    [Export]
    protected EConditionType ConditionType { get; set; }

    [Export]
    protected EBlackboardKey BlackboardKey { get; set; }

    [Export]
    protected EBlackboardType BlackboardType { get; set; }

    [Export]
    protected int Value;

    public override bool TestCondition()
    {
        bool result = true;

        int BBvalue = Blackboard.Instance.GetValue(BlackboardType, BlackboardKey, 0);
        switch (ConditionType)
        {
            case EConditionType.Lower:
                result = BBvalue < Value;
                break;
            case EConditionType.Greater:
                result = BBvalue > Value;
                break;
            case EConditionType.Equal:
                result = BBvalue == Value;
                break;
        }

        return result;
    }
}
