using System.Numerics;

namespace MusgoEngine.Game;

public class RotateTheCamera() : GameComponent
{
    public Vector3 Target = Vector3.One;
    public float RotateSpeed = 64f;
}
