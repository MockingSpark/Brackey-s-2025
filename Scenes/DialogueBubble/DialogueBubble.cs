using Godot;
using System;

[Tool]
public partial class DialogueBubble : Control
{
    private string text;

    [Export(PropertyHint.MultilineText)]
    public string Text
    { 
        get => text;
        set
        {
            if (IsNodeReady())
            {
                GetNode<Label>("%Label").Text = value;
            }
            text = value;
        }
    }

    public override void _Ready()
    {
        GetNode<Label>("%Label").Text = text;
    }
}
