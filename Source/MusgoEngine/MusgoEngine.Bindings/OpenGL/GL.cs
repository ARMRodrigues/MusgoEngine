namespace MusgoEngine.Bindings.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<byte*, nint> _getProcAddress;

    public static void Initialize(IProcAddressProvider provider)
    {
        //_getProcAddress = !useGlfwProcAddress ? EGL.EGL.GetProcAddress : GLFW.GLFW.GetProcAddress;
        _getProcAddress = provider.GetProcAddressPointer();
        LoadFunctions();
    }

    private static void LoadFunctions()
    {
        _glClearColor = (delegate* unmanaged[Cdecl]<float, float, float, float, void>)GetProcAddressPointer("glClearColor");
        _glClear = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glClear");
        _glGetString = (delegate* unmanaged[Cdecl]<uint, IntPtr>)GetProcAddressPointer("glGetString");
    }

    private static nint GetProcAddressPointer(string name)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(name);
        fixed (byte* ptr = bytes)
        {
            return _getProcAddress(ptr);
        }
    }
}
