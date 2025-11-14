using System.Numerics;

namespace MusgoEngine;

public class DirectionalLight : GameComponent
{
    public bool IsMainLight { get; set; } = true;
    public Color Color { get; set; } = Color.White;
    public float Intensity { get; set; } = 1f;
}
