using MusgoEngine.Bindings.GLFW;

namespace MusgoEngine.Windowing;

public class GlfwWindowSystem : IWindowSystem
{
    private GLFWWindow _window;

    public GlfwWindowSystem()
    {
        GLFWLoader.Load();

        if (GLFW.Init())
        {
        }
    }

    public void CreateWindow(WindowSettings windowSettings)
    {
        GLFW.WindowHint(WindowHint.Visible, GLFWBool.False);
        GLFW.WindowHint(WindowHint.Decorated, GLFWBool.True);
        GLFW.WindowHint(WindowHint.ClientApi, ClientApi.NoApi);
        _window = GLFW.CreateWindow(windowSettings.Width, windowSettings.Height, windowSettings.Title);

        CenterWindow();
    }

    public IntPtr GetNativeHandle()
    {
        return GLFW.GetWin32Window(_window.Handle);
    }

    public void ShowWindow()
    {
        GLFW.ShowWindow(_window);
    }

    public void HideWindow()
    {
        GLFW.HideWindow(_window);
    }

    public void CenterWindow()
    {
        GLFW.HideWindow(_window);

        var monitor = GLFW.GetPrimaryMonitor();
        if (monitor == IntPtr.Zero) return;

        var videoMode = GLFW.GetVideoMode(monitor);
        var (windowWidth, windowHeight) = GLFW.GetWindowSize(_window);

        GLFW.SetWindowPos(_window,
            (videoMode.Width - windowWidth) / 2,
            (videoMode.Height - windowHeight) / 2
        );

        GLFW.ShowWindow(_window);
    }

    public bool IsWindowOpen()
    {
        return !GLFW.WindowShouldClose(_window);
    }

    public void PollEvents()
    {
        GLFW.PollEvents();
    }

    public void DestroyWindow()
    {
        GLFW.DestroyWindow(_window);
    }

    public void Dispose()
    {
        GLFWLoader.Unload();
    }
}
