using MusgoEngine.Math;

namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<int, float, void> _glUniform1f;
    private static delegate* unmanaged[Cdecl]<int, int, bool, float*, void> _glUniformMatrix4fv;

    public static void Uniform1f(int location, float value) => _glUniform1f(location, value);

    public static void UniformMatrix4fv(int location, int count, bool transpose, float* value) =>
        _glUniformMatrix4fv(location, count, transpose, value);

    public static void UniformMatrix4fv(int location,bool transpose, in Matrix4 matrix)
    {
        fixed (float* ptr = &matrix.M00)
        {
            _glUniformMatrix4fv(location, 1, transpose, ptr);
        }
    }

    public static void UniformMatrix4fv(int location, bool transpose, float[] values)
    {
        fixed (float* ptr = values)
        {
            _glUniformMatrix4fv(location, 1, transpose, ptr);
        }
    }
}
