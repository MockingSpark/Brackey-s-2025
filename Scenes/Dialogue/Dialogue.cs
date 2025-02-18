using Godot;
using Godot.Collections;
using System;

[GlobalClass] // Allows the class to be used as a resource
public partial class Dialogue : Resource
{
    [Export] public string Tag = "";
    [Export] public Array<string> Text = new Array<string>();
}