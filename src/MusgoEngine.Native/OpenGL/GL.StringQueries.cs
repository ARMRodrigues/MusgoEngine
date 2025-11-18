using System.Runtime.InteropServices;

namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<uint, IntPtr> _glGetString;

    /// <summary>
    /// Return a string describing the current GL connection.
    /// </summary>
    /// <param name="name">Specifies a symbolic constant, one of Vendor, Renderer, Version, or ShadingLanguageVersion.</param>
    /// <returns></returns>
    public static string GetString(GLStringName name)
    {
        var ptr = _glGetString((uint)name);
        return Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
    }
}
