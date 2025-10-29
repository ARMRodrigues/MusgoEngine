namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, int, uint, IntPtr, void> _glDrawElements;

    public static void DrawElements(uint mode, int count, uint type, IntPtr indices)
        => _glDrawElements(mode, count, type, indices);

    public static void DrawElements(GLPrimitiveType mode, int count, GLDrawElementsType type, IntPtr indices)
    {
        DrawElements((uint)mode, count, (uint)type, indices);
    }
}
