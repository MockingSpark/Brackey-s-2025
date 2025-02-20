using Godot;
using Godot.Collections;
using System;

[GlobalClass] // Allows the class to be used as a resource
public partial class Dialogue : Resource
{
    [Export] public string tag = "";
    [Export] public string text = null;
    [Export] public bool isBold = true;
    [Export] public bool linkedToFairy = true;
    [Export] public Vector2 position = new Vector2();
}