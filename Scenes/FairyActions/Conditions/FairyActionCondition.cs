using Godot;
using System;

[GlobalClass]
public abstract partial class FairyActionCondition : Resource
{
    public virtual bool TestCondition() { return true; }
}
