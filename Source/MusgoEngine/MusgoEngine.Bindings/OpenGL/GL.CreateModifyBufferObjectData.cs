namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, nuint, void*, uint, void> _glBufferData;

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
}
