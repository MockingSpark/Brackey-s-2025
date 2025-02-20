using Godot;
using System;

class Utils
{
    /** Lerp but with a max step to take */
    public static float LerpStepped(float from, float to, float weight, float max)
    {
        return from + Mathf.Clamp((to - from) * weight, -max, max);
    }
    public static Vector2 LerpStepped(Vector2 from, Vector2 to, float weight, float max)
    {
        Vector2 result;
        result.X = LerpStepped(from.X, to.X, weight, max);
        result.Y = LerpStepped(from.Y, to.Y, weight, max);

        return result;
    }
}