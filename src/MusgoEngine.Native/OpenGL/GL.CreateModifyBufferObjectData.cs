namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, nuint, void*, uint, void> _glBufferData;
    private static delegate* unmanaged[Cdecl]<uint, int, int, void*, void> _glBufferSubData;

    public static void BufferData(uint target, float[] data, uint usage)
    {
        fixed (float* ptr = data)
        {
            _glBufferData(target, (nuint)(data.Length * sizeof(float)), ptr, usage);
        }
    }

    public static void BufferData(GLBufferTarget target, float[] data, GLBufferUsageHint usage)
    {
        BufferData((uint)target, data, (uint)usage);
    }

    public static void BufferData(uint target, uint[] data, uint usage)
    {
        fixed (uint* ptr = data)
        {
            _glBufferData(target, (nuint)(data.Length * sizeof(uint)), ptr, usage);
        }
    }

    public static void BufferData(GLBufferTarget target, uint[] data, GLBufferUsageHint usage)
    {
        BufferData((uint)target, data, (uint)usage);
    }

    public static unsafe void BufferData<T>(GLBufferTarget target, ref T data, GLBufferUsageHint usage) where T : unmanaged
    {
        fixed (T* ptr = &data)
        {
            _glBufferData((uint)target, (nuint)sizeof(T), ptr, (uint)usage);
        }
    }

    public static void BufferSubData<T>(GLBufferTarget target, int offset, T[] data) where T : unmanaged
    {
        fixed (T* ptr = &data[0])
        {
            _glBufferSubData((uint)target, offset, data.Length * sizeof(T), ptr);
        }
    }

    public static void BufferSubData(GLBufferTarget target, int offset, int sizeInBytes, void* data)
    {
        _glBufferSubData((uint)target, offset, sizeInBytes, data);
    }
}
