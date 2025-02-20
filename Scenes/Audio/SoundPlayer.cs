using Godot;
using System;

public partial class SoundPlayer : Node2D
{
    [Export]
    public PackedScene audioPlayer;
    [Export]
    public bool playOneShot = false;
    [Export]
    public AudioStream stream;

    public void PlaySound()
    {
        var newSound = audioPlayer.Instantiate<AudioStreamPlayer2D>();
        newSound.Transform = Transform;
        GetTree().Root.CallDeferred("add_child", newSound);
        newSound.Stream = stream;
        newSound.CallDeferred("play");
        if(playOneShot)
        {
            newSound.Finished += newSound.QueueFree;
        }
    }
}
