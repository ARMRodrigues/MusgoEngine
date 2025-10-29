namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, uint, void> _glBindBuffer;

    public static void BindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

    public static void BindBuffer(GLBufferTarget target, uint buffer) => _glBindBuffer((uint)target, buffer);
}
