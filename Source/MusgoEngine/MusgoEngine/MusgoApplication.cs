using MusgoEngine.Graphics;
using MusgoEngine.Windowing;

namespace MusgoEngine;

public class MusgoApplication
{
    private readonly IWindowSystem _windowSystem;
    private readonly IGraphicApi _graphicApi;
    private readonly WindowSettings _settings;
    private volatile bool _running;

    public MusgoApplication(WindowSettings settings)
    {
        _running = true;

        _settings = settings;

        _windowSystem = WindowSystemFactory.Create(settings.ApiType);
        _graphicApi = GraphicsApiFactory.Create(settings.ApiType);

        GraphicsDevice.Instance.Initialize(settings.ApiType, settings.Platform, _graphicApi, _windowSystem);
    }

    public void Start()
    {
        _windowSystem.CreateWindow(_settings);
        _graphicApi.CreateApi(_windowSystem.GetNativeHandle());
    }

    public void Run()
    {
        while (_running && _windowSystem.IsWindowOpen())
        {
            _windowSystem.PollEvents();
            _graphicApi.BeginDraw();
            _graphicApi.Draw();
            _graphicApi.EndDraw();
        }
    }

    public void Stop()
    {
        _graphicApi.DestroyApi();
        _windowSystem.DestroyWindow();
    }
}
