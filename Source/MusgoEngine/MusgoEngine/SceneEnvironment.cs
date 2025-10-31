using System.Numerics;

namespace MusgoEngine;

public class SceneEnvironment
{
    public DirectionalLight MainLight { get; set; } = new();
    public Vector3 AmbientColor { get; set; } = Vector3.One;
}
