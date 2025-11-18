using System.Runtime.InteropServices;

namespace MusgoEngine.Native.EGL;

public static unsafe class EGL
{
    private static delegate* unmanaged[Cdecl]<int> _eglGetError;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr> _eglGetDisplay;
    private static delegate* unmanaged[Cdecl]<IntPtr, out int, out int, int> _eglInitialize;
    private static delegate* unmanaged[Cdecl]<uint, int> _eglBindApi;
    private static delegate* unmanaged[Cdecl]<IntPtr, int[], IntPtr[], int, out int, int> _eglChooseConfig;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, int[], IntPtr> _eglCreateContext;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, int*, IntPtr> _eglCreateWindowSurface;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, IntPtr, int> _eglMakeCurrent;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int> _eglSwapBuffers;
    private static delegate* unmanaged[Cdecl]<IntPtr, int, int> _eglSwapInterval;
    private static delegate* unmanaged[Cdecl]<byte*, nint> _eglGetProcAddress;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int> _eglDestroySurface;
    private static delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int> _eglDestroyContext;
    private static delegate* unmanaged[Cdecl]<IntPtr, int> _eglTerminate;

    public static void LoadFunctions(IntPtr handle)
    {
        _eglGetError = (delegate* unmanaged[Cdecl]<int>)
            NativeLibrary.GetExport(handle, "eglGetError");
        _eglGetDisplay = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr>)
            NativeLibrary.GetExport(handle, "eglGetDisplay");
        _eglInitialize = (delegate* unmanaged[Cdecl]<IntPtr, out int, out int, int>)
            NativeLibrary.GetExport(handle, "eglInitialize");
        _eglBindApi = (delegate* unmanaged[Cdecl]<uint, int>)
            NativeLibrary.GetExport(handle, "eglBindAPI");
        _eglChooseConfig = (delegate* unmanaged[Cdecl]<IntPtr, int[], IntPtr[], int, out int, int>)
            NativeLibrary.GetExport(handle, "eglChooseConfig");
        _eglCreateContext = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, int[], IntPtr>)
            NativeLibrary.GetExport(handle, "eglCreateContext");
        _eglCreateWindowSurface = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, int*, IntPtr>)
            NativeLibrary.GetExport(handle, "eglCreateWindowSurface");
        _eglMakeCurrent = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, IntPtr, int>)
            NativeLibrary.GetExport(handle, "eglMakeCurrent");
        _eglSwapBuffers = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int>)
            NativeLibrary.GetExport(handle, "eglSwapBuffers");
        _eglSwapInterval = (delegate* unmanaged[Cdecl]<IntPtr, int, int>)
            NativeLibrary.GetExport(handle, "eglSwapInterval");
        _eglGetProcAddress = (delegate* unmanaged[Cdecl]<byte*, nint>)
            NativeLibrary.GetExport(handle, "eglGetProcAddress");
        _eglDestroySurface = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int>)
            NativeLibrary.GetExport(handle, "eglDestroySurface");
        _eglDestroyContext = (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int>)
            NativeLibrary.GetExport(handle, "eglDestroyContext");
        _eglTerminate = (delegate* unmanaged[Cdecl]<IntPtr, int>)NativeLibrary.GetExport(handle, "eglTerminate");
    }

    public static int GetError() => _eglGetError();

    public static EGLDisplay GetDisplay(IntPtr nativeDisplayId) =>
        new(_eglGetDisplay(nativeDisplayId));

    public static bool Initialize(EGLDisplay display, out int major, out int minor) =>
        _eglInitialize(display.Handle, out major, out minor) != 0;

    public static bool BindApi(EGLApi api) => _eglBindApi((uint)api) != 0;

    public static bool ChooseConfig(EGLDisplay display, int[] attribList, IntPtr[] configs, int configSize, out int numConfig)
    {
        if (configs == null || configs.Length == 0)
            throw new ArgumentException("Configs array must not be null or empty");

        if (configSize > configs.Length)
            throw new ArgumentException("configSize cannot be larger than configs.Length");

        // Chama a função nativa
        var result = _eglChooseConfig(display.Handle, attribList, configs, configSize, out numConfig) != 0;

        return result;
    }

    public static EGLContext CreateContext(EGLDisplay display, EGLConfig config, EGLContext shareContext, int[] attribList) =>
        new(_eglCreateContext(display.Handle, config.Handle, shareContext.Handle, attribList));

    public static EGLSurface CreateWindowSurface(EGLDisplay display, EGLConfig config, IntPtr nativeWindow, int[]? attribList)
    {
        int* attribPtr = null;
        if (attribList is { Length: > 0 })
        {
            fixed (int* ptr = attribList)
            {
                attribPtr = ptr;
            }
        }

        var surface = _eglCreateWindowSurface(display.Handle, config.Handle, nativeWindow, attribPtr);
        return new EGLSurface(surface);
    }

    public static bool MakeCurrent(EGLDisplay display, EGLSurface drawSurface, EGLSurface readSurface, EGLContext context) =>
        _eglMakeCurrent(display.Handle, drawSurface.Handle, readSurface.Handle, context.Handle) != 0;

    public static bool SwapBuffers(EGLDisplay display, EGLSurface surface) =>
        _eglSwapBuffers(display.Handle, surface.Handle) != 0;

    public static bool SwapInterval(EGLDisplay display, int interval) =>
        _eglSwapInterval(display.Handle, interval) != 0;

    public static delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddress
        => _eglGetProcAddress;

    public static bool DestroySurface(EGLDisplay display, EGLSurface surface) =>
        _eglDestroySurface(display.Handle, surface.Handle) != 0;

    public static bool DestroyContext(EGLDisplay display, EGLContext context) =>
        _eglDestroyContext(display.Handle, context.Handle) != 0;

    public static bool Terminate(EGLDisplay display) =>
        _eglTerminate(display.Handle) != 0;
}

