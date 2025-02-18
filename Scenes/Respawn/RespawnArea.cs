using Godot;
using System;

public partial class RespawnArea : Area2D
{
    [Export]
    Vector2 dropPoint = new Vector2();

    public void OnPlayerEntered(Node2D body)
    {
        var character = body as CharacterController;
        if (character != null)
        {
            character.Respawn(dropPoint);
        }
    }
}
