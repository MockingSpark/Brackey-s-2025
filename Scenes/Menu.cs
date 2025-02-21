using Godot;
using System.Diagnostics;

public partial class Menu : Node2D
{
    private Button startButton;
    private Button creditsButton;
    public override void _Ready()
    {
        startButton = GetNode<Button>("StartButton");
        creditsButton = GetNode<Button>("CreditsButton");
        startButton.Pressed += StartButton_ButtonPressed;
        creditsButton.Pressed += CreditsButton_ButtonPressed;
     }

    private void CreditsButton_ButtonPressed()
    {
// throw new NotImplementedException();
    }

    private void StartButton_ButtonPressed()
    {
        Debug.Print("TNJUHRURH3");
        GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn"); 
    }
}
