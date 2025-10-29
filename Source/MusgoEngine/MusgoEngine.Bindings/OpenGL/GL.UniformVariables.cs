namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, byte*, int> _glGetUniformLocation;

    public static int GetUniformLocation(uint program, string name)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(name + '\0');
        fixed (byte* ptr = bytes)
            return _glGetUniformLocation(program, ptr);
    }
}
