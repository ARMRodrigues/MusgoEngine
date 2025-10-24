namespace MusgoEngine.Bindings.Glfw3;

public static unsafe partial class Glfw
{
    private static delegate* unmanaged[Cdecl]<int, int, void> _glfwWindowHintPtr;
    private static delegate* unmanaged[Cdecl]<int, int, string, nint, nint, nint> _glfwCreateWindowPtr;
    private static delegate* unmanaged[Cdecl]<nint, void> _glfwDestroyWindowPtr;
    private static delegate* unmanaged[Cdecl]<nint, int> _glfwWindowShouldClosePtr;
    private static delegate* unmanaged[Cdecl]<void> _glfwPollEventsPtr;

    public static void WindowHint(int hint, int value) => _glfwWindowHintPtr(hint, value);

    public static nint CreateWindow(int width, int height, string title, nint monitor = 0, nint share = 0)
        => _glfwCreateWindowPtr(width, height, title, monitor, share);

    public static void DestroyWindow(nint window) => _glfwDestroyWindowPtr(window);

    // Checa se a janela deve ser fechada
    public static bool WindowShouldClose(nint window) => _glfwWindowShouldClosePtr(window) != 0;

    // Processa eventos
    public static void PollEvents() => _glfwPollEventsPtr();
}
