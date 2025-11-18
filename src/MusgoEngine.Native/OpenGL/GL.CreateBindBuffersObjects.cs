namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, uint, void> _glBindBuffer;
    private static delegate* unmanaged[Cdecl]<uint, uint, uint, void> _glBindBufferBase;

    public static void BindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

    public static void BindBuffer(GLBufferTarget target, uint buffer) => _glBindBuffer((uint)target, buffer);

    public static void BindBufferBase(GLBufferTarget target, uint index, uint buffer)
    {
        _glBindBufferBase((uint)target, index, buffer);
    }

    public static void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer)
    {
        _glBindBufferBase((uint)target, index, buffer);
    }
}
