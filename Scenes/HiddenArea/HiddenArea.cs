using Godot;
using System;

public partial class HiddenArea : ColorRect
{
    Tween revealTween;
    Tween hideTween;

    [Export]
    double revealSpeed = 0.5;

    public void RevealArea(Node2D body)
    {
        if(hideTween != null && hideTween.IsRunning())
        {
            hideTween.Kill();
        }
        revealTween = GetTree().CreateTween();
        revealTween.TweenProperty(this, "color", new Color(Color.R, Color.G, Color.B, 0), revealSpeed);
    }

    public void HideArea(Node2D body)
    {
        if (revealTween != null && revealTween.IsRunning())
        {
            revealTween.Kill();
        }
        hideTween = GetTree().CreateTween();
        hideTween.TweenProperty(this, "color", new Color(Color.R, Color.G, Color.B, 1), revealSpeed);
    }
}
