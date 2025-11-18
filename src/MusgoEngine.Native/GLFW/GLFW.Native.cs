namespace MusgoEngine.Native.GLFW;

public static unsafe partial class GLFW
{
    private static delegate* unmanaged[Cdecl]<nint, nint> _glfwGetWin32WindowPtr;

    /// <summary>
    /// Returns the HWND of the GLFW window on Windows.
    /// Must only be called if compiled with GLFW_EXPOSE_NATIVE_WIN32.
    /// </summary>
    public static nint GetWin32Window(nint windowPtr)
    {
        return _glfwGetWin32WindowPtr == null ? throw new InvalidOperationException("glfwGetWin32Window not loaded.") :
            _glfwGetWin32WindowPtr(windowPtr);
    }
}
