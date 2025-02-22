using Godot;
using System;

public partial class EnemySurveilPoint : Area2D
{
    [Export]
    public Vector2 directionToGo;
    [Signal]
    public delegate void EnemyDetectedEventHandler();

    public void OnEnemyEntered(Node2D body)
    {
        Enemy enemy = body as Enemy;
        if (enemy != null)
        {
            enemy.TurnAround(directionToGo);
            EmitSignal(SignalName.EnemyDetected);
        }
    }
}
