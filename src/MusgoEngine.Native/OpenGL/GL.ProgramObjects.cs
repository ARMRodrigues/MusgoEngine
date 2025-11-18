namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint> _glCreateProgram;
    private static delegate* unmanaged[Cdecl]<uint, uint, void> _glAttachShader;
    private static delegate* unmanaged[Cdecl]<uint, void> _glLinkProgram;
    private static delegate* unmanaged[Cdecl]<uint, void> _glUseProgram;
    private static delegate* unmanaged[Cdecl]<uint, void> _glDeleteProgram;

    public static uint CreateProgram() => _glCreateProgram();
    public static void AttachShader(uint program, uint shader) => _glAttachShader(program, shader);
    public static void LinkProgram(uint program) => _glLinkProgram(program);
    public static void UseProgram(uint program) => _glUseProgram(program);
    public static void DeleteProgram(uint program) => _glDeleteProgram(program);
}
