using MusgoEngine.Core;
using MusgoEngine.Windowing;

namespace MusgoEngine;

public class MusgoApplication
{
    private IWindowSystem _windowSystem;

    public MusgoApplication()
    {
        _windowSystem = new GlfwWindowSystem();
    }

    public void Start()
    {
        var settings = new WindowSettings();
        _windowSystem.CreateWindow(settings);
    }

    public void Run()
    {
        while (_windowSystem.IsWindowOpen())
        {
            _windowSystem.PollEvents();
        }
    }

    public void Stop()
    {
        _windowSystem.DestroyWindow();
    }
}
