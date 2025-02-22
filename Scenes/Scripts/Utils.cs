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
        Vector2 direction = to - from;

        float distance = Mathf.Clamp(direction.Length() * weight, -max, max);
        Vector2 result = from + direction.Normalized() * distance;

        return result;
    }
}