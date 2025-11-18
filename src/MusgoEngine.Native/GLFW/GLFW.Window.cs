namespace MusgoEngine.Native.GLFW;

public static unsafe partial class GLFW
{
    private static delegate* unmanaged[Cdecl]<int, int, void> _glfwWindowHintPtr;
    private static delegate* unmanaged[Cdecl]<int, int, string, nint, nint, nint> _glfwCreateWindowPtr;
    private static delegate* unmanaged[Cdecl]<nint, void> _glfwDestroyWindowPtr;
    private static delegate* unmanaged[Cdecl]<nint, int> _glfwWindowShouldClosePtr;
    private static delegate* unmanaged[Cdecl]<void> _glfwPollEventsPtr;
    private static delegate* unmanaged[Cdecl]<nint, out int, out int, void> _glfwGetWindowSizePtr;
    private static delegate* unmanaged[Cdecl]<nint, int, int, void> _glfwSetWindowPosPtr;
    private static delegate* unmanaged[Cdecl]<nint, void> _glfwHideWindowPtr;
    private static delegate* unmanaged[Cdecl]<nint, void> _glfwShowWindowPtr;
    private static delegate* unmanaged[Cdecl]<IntPtr, void> _glfwSwapBuffersPtr;

    public static void WindowHint(int hint, int value) => _glfwWindowHintPtr(hint, value);

    public static void WindowHint(WindowHint hint, int value) => _glfwWindowHintPtr((int)hint, value);

    public static void WindowHint(WindowHint hint, GLFWBool value) => _glfwWindowHintPtr((int)hint, (int)value);

    public static void WindowHint(WindowHint hint, OpenGLProfile value) => _glfwWindowHintPtr((int)hint, (int)value);

    public static void WindowHint(WindowHint hint, ClientApi value) => _glfwWindowHintPtr((int)hint, (int)value);

    public static nint CreateWindow(int width, int height, string title, nint monitor = 0, nint share = 0)
        => _glfwCreateWindowPtr(width, height, title, monitor, share);

    public static void DestroyWindow(nint window) => _glfwDestroyWindowPtr(window);

    public static bool WindowShouldClose(nint window) => _glfwWindowShouldClosePtr(window) != 0;

    public static void PollEvents() => _glfwPollEventsPtr();

    public static (int Width, int Height) GetWindowSize(nint window)
    {
        _glfwGetWindowSizePtr(window, out var width, out var height);
        return (width, height);
    }

    public static void SetWindowPos(nint window, int x, int y)
        => _glfwSetWindowPosPtr(window, x, y);

    public static void HideWindow(nint window) => _glfwHideWindowPtr(window);

    public static void ShowWindow(nint window) => _glfwShowWindowPtr(window);

    public static void SwapBuffers(GLFWWindow window)
    {
        _glfwSwapBuffersPtr(window.Handle);
    }
}
