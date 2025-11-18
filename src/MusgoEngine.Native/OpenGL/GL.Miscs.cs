namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint> _glGetError;
    private static delegate* unmanaged[Cdecl]<uint, void> _glEnable;
    private static delegate* unmanaged[Cdecl]<uint, void> _glDisable;

    public static GLErrorCode GetError()
    {
        return (GLErrorCode)_glGetError();
    }
    public static void Enable(uint cap) => _glEnable(cap);
    public static void Disable(uint cap) => _glDisable(cap);
}
