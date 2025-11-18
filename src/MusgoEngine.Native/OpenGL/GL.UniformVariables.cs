namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, byte*, int> _glGetUniformLocation;
    private static delegate* unmanaged[Cdecl]<uint, uint, int, int*, int*, uint*, byte*, void> _glGetActiveUniform;
    private static delegate* unmanaged[Cdecl]<uint, byte*, uint> _glGetUniformBlockIndex;


    public static int GetUniformLocation(uint program, string name)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(name + '\0');
        fixed (byte* ptr = bytes)
            return _glGetUniformLocation(program, ptr);
    }

    public static string GetActiveUniform(uint program, uint index, out int size, out uint type)
    {
        const int bufferSize = 256;
        var length = 0;
        size = 0;
        type = 0;

        var nameBuffer = stackalloc byte[bufferSize];

        fixed (int* sizePtr = &size)
        fixed (uint* typePtr = &type)
        {
            _glGetActiveUniform(program, index, bufferSize, &length, sizePtr, typePtr, nameBuffer);
        }

        return System.Text.Encoding.UTF8.GetString(nameBuffer, length);
    }

    public static uint GetUniformBlockIndex(uint program, string uniformBlockName)
    {
        var nameBytes = System.Text.Encoding.ASCII.GetBytes(uniformBlockName + '\0');
        fixed (byte* ptr = nameBytes)
        {
            return _glGetUniformBlockIndex(program, ptr);
        }
    }

}
