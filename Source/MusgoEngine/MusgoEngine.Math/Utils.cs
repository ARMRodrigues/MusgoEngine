namespace MusgoEngine.Math;

public static class Utils
{
    public static float ToRadians(this float degrees) => degrees * (MathF.PI / 180f);

    /// <summary>
    /// Clamps a float value between min and max.
    /// </summary>
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
