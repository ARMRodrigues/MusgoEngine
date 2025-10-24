using System.Runtime.InteropServices;

namespace MusgoEngine.Bindings.Glfw3;

public static unsafe partial class Glfw
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
    }

    /// <summary>
    /// Initializes the GLFW library.
    /// Must be called only after <see cref="GlfwLoader.Load"/> has successfully loaded the native library.
    /// </summary>
    public static bool Init()
    {
        return _glfwInitPtr() == (int)GlfwBool.True;
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
