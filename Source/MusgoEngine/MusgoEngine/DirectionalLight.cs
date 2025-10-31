using System.Numerics;

namespace MusgoEngine;

public class DirectionalLight
{
    private Vector3 _directionDegrees = new(90, 0, 0);

    public Vector3 DirectionDegrees
    {
        get => _directionDegrees;
        set
        {
            _directionDegrees = value;
            Direction = FromAngles(value);
        }
    }

    public Vector3 Direction { get; private set; }
    public Color Color { get; set; } = Color.White;
    public float Intensity { get; set; } = 1f;

    private static Vector3 FromAngles(Vector3 angles)
    {
        var radX = MathF.PI / 180f * angles.X;
        var radY = MathF.PI / 180f * angles.Y;
        var radZ = MathF.PI / 180f * angles.Z;

        // converte ângulos em vetor de direção
        var dir = new Vector3(
            MathF.Cos(radY) * MathF.Cos(radX),
            MathF.Sin(radX),
            MathF.Sin(radY) * MathF.Cos(radX)
        );

        return Vector3.Normalize(dir);
    }
}
