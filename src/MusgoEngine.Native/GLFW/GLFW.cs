using System.Runtime.InteropServices;

namespace MusgoEngine.Native.GLFW;

public static unsafe partial class GLFW
{
    private static delegate* unmanaged[Cdecl]<int> _glfwInitPtr;
    private static delegate* unmanaged[Cdecl]<void> _glfwTerminatePtr;
    private static delegate* unmanaged[Cdecl]<out IntPtr, int> _glfwGetErrorPtr;

    public static void LoadFunctions(IntPtr libHandle)
    {
        _glfwInitPtr = (delegate* unmanaged[Cdecl]<int>)NativeLibrary.GetExport(libHandle, "glfwInit");
        _glfwTerminatePtr = (delegate* unmanaged[Cdecl]<void>)NativeLibrary.GetExport(libHandle, "glfwTerminate");
        _glfwGetErrorPtr = (delegate* unmanaged[Cdecl]<out IntPtr, int>)NativeLibrary.GetExport(libHandle, "glfwGetError");

        // Window
        _glfwWindowHintPtr = (delegate* unmanaged[Cdecl]<int, int, void>)NativeLibrary.GetExport(libHandle, "glfwWindowHint");
        _glfwCreateWindowPtr = (delegate* unmanaged[Cdecl]<int, int, string, nint, nint, nint>)NativeLibrary.GetExport(libHandle, "glfwCreateWindow");
        _glfwDestroyWindowPtr = (delegate* unmanaged[Cdecl]<nint, void>)NativeLibrary.GetExport(libHandle, "glfwDestroyWindow");
        _glfwWindowShouldClosePtr = (delegate* unmanaged[Cdecl]<nint, int>)NativeLibrary.GetExport(libHandle, "glfwWindowShouldClose");
        _glfwPollEventsPtr = (delegate* unmanaged[Cdecl]<void>)NativeLibrary.GetExport(libHandle, "glfwPollEvents");
        _glfwGetWindowSizePtr = (delegate* unmanaged[Cdecl]<nint, out int, out int, void>)NativeLibrary.GetExport(libHandle, "glfwGetWindowSize");
        _glfwSetWindowPosPtr = (delegate* unmanaged[Cdecl]<nint, int, int, void>)NativeLibrary.GetExport(libHandle, "glfwSetWindowPos");
        _glfwHideWindowPtr = (delegate* unmanaged[Cdecl]<nint, void>)NativeLibrary.GetExport(libHandle, "glfwHideWindow");
        _glfwShowWindowPtr = (delegate* unmanaged[Cdecl]<nint, void>)NativeLibrary.GetExport(libHandle, "glfwShowWindow");
        _glfwSwapBuffersPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)NativeLibrary.GetExport(libHandle, "glfwSwapBuffers");

        // Context
        _glfwMakeContextCurrentPtr = (delegate* unmanaged[Cdecl]<IntPtr, void>)NativeLibrary.GetExport(libHandle, "glfwMakeContextCurrent");
        _glfwGetProcAddressPtr = (delegate* unmanaged[Cdecl]<byte*, nint>)NativeLibrary.GetExport(libHandle, "glfwGetProcAddress");

        // Native
        _glfwGetWin32WindowPtr = (delegate* unmanaged[Cdecl]<nint, nint>)NativeLibrary.GetExport(libHandle, "glfwGetWin32Window");

        // Monitor
        _glfwGetPrimaryMonitorPtr = (delegate* unmanaged[Cdecl]<nint>)NativeLibrary.GetExport(libHandle, "glfwGetPrimaryMonitor");
        _glfwGetVideoModePtr = (delegate* unmanaged[Cdecl]<nint, nint>)NativeLibrary.GetExport(libHandle, "glfwGetVideoMode");
    }

    /// <summary>
    /// Initializes the GLFW library.
    /// Must be called only after <see cref="GLFWLoader.Load"/> has successfully loaded the native library.
    /// </summary>
    public static bool Init()
    {
        return _glfwInitPtr() == (int)GLFWBool.True;
    }

    public static void Terminate()
    {
        _glfwTerminatePtr();
    }

    public static string GetGlfwError()
    {
        var code = _glfwGetErrorPtr(out var desc);
        if (code != 0 && desc != IntPtr.Zero)
            return Marshal.PtrToStringAnsi(desc) ?? "Unknown error";
        return string.Empty;
    }
}
