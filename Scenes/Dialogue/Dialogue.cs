using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass] // Allows the class to be used as a resource
public partial class Dialogue : Resource
{
    [Export] public string tag = "";
    [Export(PropertyHint.MultilineText)] protected string text = null;
    [Export] public bool isBold = true;
    [Export] public bool linkedToFairy = true;
    [Export] public Vector2 position = new Vector2();

    [Export] protected Array<EBlackboardKey> arguments = new Array<EBlackboardKey>();

    public string GetText() 
    {
        Array<string> strings = new Array<string>();
        
        foreach(EBlackboardKey key in arguments)
        {
            strings.Add(Blackboard.Instance.GetValueAnyBoard(key).ToString());
        }

        object[] test = strings.ToArray<string>();

        text = String.Format(text, test);
        return text; 
    }
}