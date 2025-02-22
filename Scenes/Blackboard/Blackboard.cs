using Godot;
using Godot.Collections;
using System;

public enum EBlackboardType
{
    Permanent,
    Level
}

public enum EBlackboardKey
{
    InteractionCount,
    ProjectileThrowCount,
    AngerValue
}

public partial class Blackboard : Node
{
    public static Blackboard Instance { get; private set; }

    Dictionary<EBlackboardType, Dictionary<EBlackboardKey, int>> Blackboards = new Dictionary<EBlackboardType, Dictionary<EBlackboardKey, int>>();

    public override void _Ready()
    {
        base._Ready();

        Instance = this;

        foreach(EBlackboardType type in Enum.GetValues(typeof(EBlackboardType)))
        {
            Blackboards.Add(type, new Dictionary<EBlackboardKey, int>());
        }
    }
    public int GetValueAnyBoard(EBlackboardKey key, int defaultValue = int.MinValue)
    {
        foreach(Dictionary<EBlackboardKey, int> blackboard in Blackboards.Values)
        {
            if(blackboard.ContainsKey(key))
            {
                return blackboard[key];
            }
        }

        return defaultValue;
    }

    public int GetValue(EBlackboardType blackboard, EBlackboardKey key, int defaultValue = int.MinValue)
    {
        if (Blackboards[blackboard].ContainsKey(key))
        {
            return Blackboards[blackboard][key];
        }
        return defaultValue;
    }

    public void SetValue(EBlackboardType blackboard, EBlackboardKey key, int value)
    {
        Blackboards[blackboard][key] = value;
    }

    public int OffsetValue(EBlackboardType blackboard, EBlackboardKey key, int value)
    {
        if (Blackboards[blackboard].ContainsKey(key))
        {
            Blackboards[blackboard][key] += value;
        }
        else
        {
            Blackboards[blackboard][key] = value;
        }

        return Blackboards[blackboard][key];
    }

    public void ClearBlackBoard(EBlackboardType blackboard)
    {
        Blackboards[blackboard].Clear();
    }

    public void ClearAllBlackboards() 
    { 
        foreach(Dictionary<EBlackboardKey, int> blackboard in Blackboards.Values)
        {
            blackboard.Clear();
        }
    }

    public string FullDebugString()
    {
        string result = "Blackboard count: " + Blackboards.Count + "\n";

        foreach(EBlackboardType type in Blackboards.Keys)
        {
            result += "  " + type + ": " + Blackboards[type].Count + " elements\n";

            foreach(EBlackboardKey key in Blackboards[type].Keys)
            {
                result += "    * " + key + ": " + Blackboards[type][key] + "\n";
            }
        }

        return result;
    }
}
