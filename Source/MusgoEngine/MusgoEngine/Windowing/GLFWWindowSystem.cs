using MusgoEngine.Bindings.Glfw3;
using MusgoEngine.Core;

namespace MusgoEngine.Windowing;

public class GlfwWindowSystem : IWindowSystem
{
    private GlfwWindow _window;

    public GlfwWindowSystem()
    {
        GlfwLoader.Load();

        if (Glfw.Init())
        {
            Console.WriteLine("GLFW INIT");
        }
    }

    public void CreateWindow(WindowSettings windowSettings)
    {
        _window = Glfw.CreateWindow(windowSettings.Width, windowSettings.Height, windowSettings.Title);
    }

    public void ShowWindow()
    {
        throw new NotImplementedException();
    }

    public void HideWindow()
    {
        throw new NotImplementedException();
    }

    public bool IsWindowOpen()
    {
        return !Glfw.WindowShouldClose(_window);
    }

    public void PollEvents()
    {
        Glfw.PollEvents();
    }

    public void DestroyWindow()
    {
        Glfw.DestroyWindow(_window);
    }

    public void Dispose()
    {
        GlfwLoader.Unload();
    }
}
