namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, void> _glEnable;
    private static delegate* unmanaged[Cdecl]<uint, void> _glDisable;

    public static void Enable(uint cap) => _glEnable(cap);
    public static void Disable(uint cap) => _glDisable(cap);
}
