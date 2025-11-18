namespace MusgoEngine.Native.GLFW;

public static unsafe partial class GLFW
{
    private static delegate* unmanaged[Cdecl]<IntPtr, void> _glfwMakeContextCurrentPtr;
    private static delegate* unmanaged[Cdecl]<byte*, nint> _glfwGetProcAddressPtr;

    /// <summary>
    /// Makes the OpenGL or OpenGL ES context of the specified window current on the calling thread.
    /// </summary>
    public static void MakeContextCurrent(GLFWWindow window)
    {
        _glfwMakeContextCurrentPtr(window.Handle);
    }

    /// <summary>
    /// Gets the native function pointer for an OpenGL or OpenGL ES function.
    /// </summary>
    /// <remarks>
    /// The returned pointer uses <c>delegate* unmanaged[Cdecl]&lt;byte*, nint&gt;</c>.
    /// Pass ASCII/UTF-8 function names as <c>byte*</c>.
    /// Returns zero if not found.
    /// </remarks>
    public static unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddress
        => _glfwGetProcAddressPtr;
}
