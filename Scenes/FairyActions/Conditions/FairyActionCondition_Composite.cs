using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FairyActionCondition_Composite : FairyActionCondition
{
    public enum ECompositeType
    {
        AND,
        OR,
    }
    [Export]
    ECompositeType combinaisonType;

    [Export]
    Array<FairyActionCondition> Conditions;
    public override bool TestCondition()
    {
        if(combinaisonType == ECompositeType.AND)
        {
            foreach(FairyActionCondition condition in Conditions) 
            {
                if(!condition.TestCondition())
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            foreach (FairyActionCondition condition in Conditions)
            {
                if (condition.TestCondition())
                {
                    return true;
                }
            }
            return false;
        }
    }

}
