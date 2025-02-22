using Godot;
using System.Diagnostics;

public partial class Menu : Node2D
{
	private void CreditsButtonButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
	}

	private void StartButtonButtonPressed()
	{

		GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn"); 
	}
}
