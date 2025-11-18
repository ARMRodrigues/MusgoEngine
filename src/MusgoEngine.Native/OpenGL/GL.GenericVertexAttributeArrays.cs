namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, int, uint, bool, int, IntPtr, void> _glVertexAttribPointer;
    private static delegate* unmanaged[Cdecl]<uint, void> _glEnableVertexAttribArray;

    public static void VertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer)
        => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);

    public static void VertexAttribPointer(uint index, int size, GLVertexAttribPointerType type, bool normalized, int stride, IntPtr pointer)
    {
        VertexAttribPointer(index, size, (uint)type, normalized, stride, pointer);
    }

    public static void EnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);
}
