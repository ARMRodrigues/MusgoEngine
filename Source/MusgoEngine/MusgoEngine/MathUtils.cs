using System.Numerics;

namespace MusgoEngine;

public static class MathUtils
{
    public static float ToRadians(this float degrees) => degrees * (MathF.PI / 180f);

    public static float[] ToOpenGLMatrixArray(in Matrix4x4 m)
    {
        return
        [
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44
        ];
    }
}
