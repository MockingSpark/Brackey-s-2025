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
        startButton.Pressed += StartButtonButtonPressed;
        creditsButton.Pressed += CreditsButtonButtonPressed;
     }

    private void CreditsButtonButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
    }

    private void StartButtonButtonPressed()
    {

        GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn"); 
    }
}
