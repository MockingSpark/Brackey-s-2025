using Godot;
using System;

public partial class CreditsScrolling : Node2D
{
    private Node2D creditsContainer;
    [Export]
    public float scrollingSpeed; 
    public override void _Ready()
    {
        creditsContainer = GetNode<Node2D>("CreditsContainer");

    }
    private void ExitCredits ()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
    }
    public override void _Process(double delta)
    {
        creditsContainer.Position -= creditsContainer.Transform.Y * scrollingSpeed * (float)delta;
        if (creditsContainer.Position.Y < -590 )
        {
            ExitCredits(); 
        }
        return;
    }


}
