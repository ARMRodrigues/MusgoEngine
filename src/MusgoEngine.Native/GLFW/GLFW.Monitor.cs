using System.Runtime.InteropServices;

namespace MusgoEngine.Native.GLFW;

public static unsafe partial class GLFW
{
    private static delegate* unmanaged[Cdecl]<nint> _glfwGetPrimaryMonitorPtr;
    private static delegate* unmanaged[Cdecl]<nint, nint> _glfwGetVideoModePtr;

    public static nint GetPrimaryMonitor() => _glfwGetPrimaryMonitorPtr();

    /// <summary>
    /// Returns the current video mode of the specified monitor.
    /// </summary>
    /// <param name="monitor">Monitor pointer.</param>
    /// <returns>VideoMode struct.</returns>
    public static VideoMode GetVideoMode(nint monitor)
    {
        var ptr = _glfwGetVideoModePtr(monitor);
        return ptr == IntPtr.Zero ? throw new InvalidOperationException("Failed to get video mode pointer.") :
            Marshal.PtrToStructure<VideoMode>(ptr);
    }
}
