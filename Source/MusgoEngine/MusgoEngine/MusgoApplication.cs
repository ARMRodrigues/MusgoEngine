using System.Diagnostics;
using MusgoEngine.Core;
using MusgoEngine.Graphics;
using MusgoEngine.Windowing;

namespace MusgoEngine;

public class MusgoApplication
{
    private readonly IWindowSystem _windowSystem;
    private readonly IGraphicApi _graphicApi;
    private readonly SceneManager _sceneManager;
    private readonly GameTime _gameTime;
    private readonly WindowSettings _settings;
    private readonly IGame _game;
    private readonly bool _headless = false;
    private readonly bool _vsyncEnabled = false;
    private volatile bool _running;

    private readonly uint _targetFps = 75;

    public MusgoApplication(WindowSettings settings, IGame game)
    {
        _settings = settings;
        _game = game;

        _headless = _settings.ApiType == GraphicApiType.HeadlessApi;
        _running = true;

        _windowSystem = WindowSystemFactory.Create(_settings.ApiType);
        _graphicApi = GraphicsApiFactory.Create(_settings.ApiType);
        _sceneManager = new SceneManager();
        _gameTime = new GameTime();

        GraphicsDevice.Instance.Initialize(_settings.ApiType, _settings.Platform, _graphicApi, _windowSystem);
    }

    public void Start()
    {
        _windowSystem.CreateWindow(_settings);
        _graphicApi.CreateApi(_windowSystem.GetNativeHandle());
    }

    public void Run()
    {
        _game.Initialize(_sceneManager);
        _sceneManager.InitializeActiveScene();
        while (_running && _windowSystem.IsWindowOpen())
        {
            RunFrame();
        }
        _sceneManager.ShutdownActiveScene();
        _game.Shutdown();
    }

    private void RunFrame()
    {
        var frameStart = Stopwatch.GetTimestamp();

        _windowSystem.PollEvents();
        _gameTime.Update();

        _sceneManager.BeginFrameActiveScene();
        _sceneManager.UpdateActiveScene();
        _sceneManager.PhysicsUpdateActiveScene();

        _graphicApi.BeginDraw();
        if (!_headless)
            _sceneManager.RenderActiveScene();
        _graphicApi.EndDraw();

        _sceneManager.EndFrameActiveScene();

        var frameTimeMs = (Stopwatch.GetTimestamp() - frameStart) * 1000.0 / Stopwatch.Frequency;

        // Limita FPS se necessário
        var effectiveFrameTime = _targetFps > 0 ? Math.Max(frameTimeMs, 1000.0 / _targetFps) : frameTimeMs;

        // Se VSync desligado ou TargetFPS < monitor, aplica sleep
        if (_vsyncEnabled && (_targetFps <= 0 || !(effectiveFrameTime > frameTimeMs))) return;

        var sleepMs = (int)(effectiveFrameTime - frameTimeMs);
        if (sleepMs > 0)
            Thread.Sleep(sleepMs);
    }

    public void Stop()
    {
        _graphicApi.DestroyApi();
        _windowSystem.DestroyWindow();
    }
}
