namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, uint> _glCreateShader;
    private static unsafe delegate* unmanaged[Cdecl]<uint, int, byte**, int*, void> _glShaderSource;
    private static delegate* unmanaged[Cdecl]<uint, void> _glCompileShader;
    private static delegate* unmanaged[Cdecl]<uint, void> _glDeleteShader;

    public static uint CreateShader(uint type) => _glCreateShader(type);

    public static uint CreateShader(GLShaderType type) => _glCreateShader((uint)type);

    public static unsafe void ShaderSource(uint shader, string source)
    {
        // Converte para ASCII e adiciona null-terminator
        var bytes = System.Text.Encoding.ASCII.GetBytes(source + "\0");

        fixed (byte* ptr = bytes)
        {
            // Cria um ponteiro único para a string
            byte** stringPtr = &ptr;

            // Tamanho da string
            int length = bytes.Length;

            // Chama a função OpenGL
            _glShaderSource(shader, 1, stringPtr, &length);
        }
    }

    public static void CompileShader(uint shader) => _glCompileShader(shader);

    public static void DeleteShader(uint shader) => _glDeleteShader(shader);
}
