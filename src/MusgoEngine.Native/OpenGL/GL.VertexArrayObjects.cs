namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<int, uint*, void> _glGenVertexArrays;
    private static delegate* unmanaged[Cdecl]<int, uint*, void> _glDeleteVertexArrays;
    private static delegate* unmanaged[Cdecl]<uint, void> _glBindVertexArray;

    public static uint GenVertexArray()
    {
        var arrays = new uint[1];

        fixed (uint* ptr = arrays)
        {
            _glGenVertexArrays(1, ptr);
        }

        return arrays[0];
    }

    public static uint[] GenVertexArrays(uint n)
    {
        var arrays = new uint[n];

        fixed (uint* ptr = arrays)
        {
            _glGenVertexArrays((int)n, ptr);
        }

        return arrays;
    }

    public static void DeleteVertexArray(uint vao)
    {
        var arr = new uint[] { vao };
        fixed (uint* ptr = arr)
        {
            _glDeleteVertexArrays(1, ptr);
        }

    }

    public static void DeleteVertexArrays(uint[] vaos)
    {
        fixed (uint* ptr = vaos)
        {
            _glDeleteVertexArrays(vaos.Length, ptr);
        }
    }

    public static void BindVertexArray(uint vao) => _glBindVertexArray(vao);

}
