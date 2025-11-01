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

    // TODO : Remove and read from the game config files
    private readonly uint _targetFps = 120;

    public MusgoApplication(WindowSettings settings, IGame game)
    {
        _settings = settings;
        _game = game;

        _headless = _settings.ApiType == GraphicApiType.HeadlessApi;
        _running = true;

        _windowSystem = WindowSystemFactory.Create(_settings.ApiType);
        _graphicApi = GraphicsApiFactory.Create(_settings.ApiType);
        _sceneManager = new SceneManager(SceneGlobalsFactory.Create(_settings.ApiType));
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
        TimerPrecision.EnableHighPrecision();
        while (_running && _windowSystem.IsWindowOpen())
        {
            RunFrame();
        }
        TimerPrecision.DisableHighPrecision();
        _sceneManager.ShutdownActiveScene();
        _game.Shutdown();
    }

    private void RunFrame()
    {
        var frameStart = Stopwatch.GetTimestamp();

        _windowSystem.PollEvents();
        _gameTime.Update(frameStart);

        _sceneManager.BeginFrameActiveScene();
        _sceneManager.UpdateActiveScene();
        _sceneManager.PhysicsUpdateActiveScene();

        _graphicApi.BeginDraw();
        if (!_headless)
            _sceneManager.RenderActiveScene();
        _graphicApi.EndDraw();

        _sceneManager.EndFrameActiveScene();

        if (!_vsyncEnabled && _targetFps > 0)
            LimitFramerate(frameStart);
    }

    private void LimitFramerate(long frameStart)
    {
        var targetTicks = Stopwatch.Frequency / (double)_targetFps;
        var targetTime = frameStart + (long)targetTicks;

        while (true)
        {
            var now = Stopwatch.GetTimestamp();
            var remainingTicks = targetTime - now;

            if (remainingTicks <= 0)
                break;

            var remainingMs = remainingTicks * 1000.0 / Stopwatch.Frequency;

            if (remainingMs > 3.0)
            {
                Thread.Sleep(1);
            }
            else
            {
                Thread.SpinWait(50);
            }
        }
    }


    public void Stop()
    {
        _graphicApi.DestroyApi();
        _windowSystem.DestroyWindow();
    }
}
