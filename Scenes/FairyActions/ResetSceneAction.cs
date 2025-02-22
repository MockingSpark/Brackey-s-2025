using Godot;
using System;

[GlobalClass]
public partial class ResetSceneAction : FairyAction
{
    [Export]
    public int[] ScenesToReload { get; set; }


}
