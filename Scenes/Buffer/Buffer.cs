using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

class InputBuffer
{
    public StringName inputName = "";
    public double timeLeft = 0f;
}

[GlobalClass]
public partial class Buffer : Node2D
{ 

    private List<InputBuffer> inputBuffer = new List<InputBuffer>();
    Array<StringName> inputsToConsider = new Array<StringName> {"Jump"};
    private Godot.Collections.Dictionary<StringName, bool> simulatedInputs = new Godot.Collections.Dictionary<StringName, bool>();

    [Export]
    double bufferTime = 0.25f;
    public CharacterController player { get; private set; }

    public override void _Ready()
    {
        player = (CharacterController)GetParent();
    }

    public void TimeAdvance(double deltaTime)
    {
        //Diminish time
        for (int i = inputBuffer.Count - 1; i >= 0; i--)
        {
            InputBuffer inputInBuffer = inputBuffer[i];
            inputInBuffer.timeLeft -= deltaTime;
            //remove too old inputs
            if(inputInBuffer.timeLeft < 0)
            {
                inputBuffer.RemoveAt(i);
            }
        }

        // Add new inputs to buffer
        foreach(var input in inputsToConsider)
        { 
            if(Input.IsActionJustPressed(input) && !CanUseActionNow(input))
            {
                InputBuffer newInputBuffer = new InputBuffer();
                newInputBuffer.inputName = input;
                newInputBuffer.timeLeft = bufferTime;

                inputBuffer.Add(newInputBuffer);
            }
        }
        // Simulate the press if we can
        for (int i = inputBuffer.Count - 1; i >= 0; i--)
        {
            InputBuffer inputInBuffer = inputBuffer[i];
            if (CanUseActionNow(inputInBuffer.inputName))
            {
                simulatedInputs[inputInBuffer.inputName] = true;
                ReleaseInputNextFrame(inputInBuffer.inputName);
                inputBuffer.RemoveAt(i);
            }

        }
    }
    void ReleaseInputNextFrame(StringName actionName)
    {
        GetTree().CreateTimer(0.01f).Timeout += () =>
        {
            simulatedInputs[actionName] = false; // Reset simulated input manually
        };
    }
    public bool WasActionSimulated(StringName action)
    {
        return simulatedInputs.ContainsKey(action) && simulatedInputs[action];
    }
    public bool CanUseActionNow(StringName action)
    {
        if(action == "Jump")
        {
            return player.IsOnFloor();
        }
        return false;
    }

}
