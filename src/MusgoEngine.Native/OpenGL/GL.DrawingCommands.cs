namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, int, uint, IntPtr, void> _glDrawElements;
    private static delegate* unmanaged[Cdecl]<uint, int, uint, IntPtr, int, void> _glDrawElementsBaseVertex;

    public static void DrawElements(uint mode, int count, uint type, IntPtr indices)
        => _glDrawElements(mode, count, type, indices);

    public static void DrawElements(GLPrimitiveType mode, int count, GLDrawElementsType type, IntPtr indices)
    {
        DrawElements((uint)mode, count, (uint)type, indices);
    }

    public static void DrawElementsBaseVertex(GLPrimitiveType mode, int count, GLDrawElementsType type, IntPtr indices, int baseVertex)
    {
        _glDrawElementsBaseVertex((uint)mode, count, (uint)type, indices, baseVertex);
    }
}
