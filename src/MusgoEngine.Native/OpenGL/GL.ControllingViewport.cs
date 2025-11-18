namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<int, int, int, int, void> _glViewport;

    public static void Viewport(int x, int y, int width, int height)
        => _glViewport(x, y, width, height);
}
