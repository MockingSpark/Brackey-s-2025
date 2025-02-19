using Godot;
using System;

[Tool]
public partial class DialogueBubble : Control
{
    private string text;
    [Export]
    bool isBold = false;

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
        if(isBold)
        {
            GetNode<CollisionShape2D>("StaticBody2D/BubbleShape").SetDeferred("disabled", true);
        }
    }

    public void Resize()
    {
        if (isBold)
        {
            GetNode<CollisionShape2D>("StaticBody2D/BubbleShape").SetDeferred("disabled", false);
            ((RectangleShape2D)GetNode<CollisionShape2D>("StaticBody2D/BubbleShape").Shape).Size = GetNode<PanelContainer>("PanelContainer").Size;
        }
    }
}
