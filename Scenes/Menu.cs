using Godot;
using System.Diagnostics;

public partial class Menu : Node2D
{
	private void CreditsButtonButtonPressed()
	{
		Blackboard.Instance.ClearAllBlackboards();
		GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
	}

	private void StartButtonButtonPressed()
	{
        GetTree().CreateTimer(0.1f).Timeout += () =>
        {
            Camera2D camera = GetTree().Root.GetNode<Camera2D>("GameCamera");
            if (camera != null)
            {
                camera.PositionSmoothingEnabled = true;
            }
        };
        GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn");
       

    }
}
