namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<void> _glFinish;

    public static void Finish() => _glFinish();
}
