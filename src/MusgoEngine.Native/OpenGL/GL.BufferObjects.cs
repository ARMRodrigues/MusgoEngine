namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<int, uint*, void> _glGenBuffers;
    private static delegate* unmanaged[Cdecl]<int, uint*, void> _glDeleteBuffers;

    public static uint GenBuffer()
    {
        var arr = new uint[1];
        fixed (uint* ptr = arr)
        {
            _glGenBuffers(1, ptr);
        }
        return arr[0];
    }

    public static uint[] GenBuffers(int n)
    {
        var arr = new uint[n];
        fixed (uint* ptr = arr)
        {
            _glGenBuffers(n, ptr);
        }
        return arr;
    }

    public static void DeleteBuffer(uint buffer)
    {
        unsafe
        {
            uint[] arr = new uint[] { buffer };
            fixed (uint* ptr = arr)
            {
                _glDeleteBuffers(1, ptr);
            }
        }
    }

    public static void DeleteBuffers(uint[] buffers)
    {
        unsafe
        {
            fixed (uint* ptr = buffers)
            {
                _glDeleteBuffers(buffers.Length, ptr);
            }
        }
    }
}
