namespace MusgoEngine.Core;

public class SceneManager
{
    private readonly Dictionary<string, Scene> _scenes = new();
    private Scene? _currentScene;

    public void AddScene(Scene scene)
    {
        if (!_scenes.TryAdd(scene.Name, scene))
        {
            throw new InvalidOperationException("Scene already exists");
        }
    }

    public void SetActiveScene(Scene scene)
    {
        if (!_scenes.TryAdd(scene.Name, scene))
        {
            _currentScene = scene;
            return;
        }

        _currentScene = scene;
    }

    public void SetActiveScene(string sceneName)
    {
        if (_scenes.TryGetValue(sceneName, out _currentScene))
        {
            throw new InvalidOperationException("Scene does not exist");
        }
    }

    public void InitializeActiveScene()
    {
        _currentScene?.Initialize();
    }

    public void BeginFrameActiveScene()
    {
        _currentScene?.BeginFrame();
    }

    public void UpdateActiveScene()
    {
        _currentScene?.Update();
    }

    public void PhysicsUpdateActiveScene()
    {
        _currentScene?.PhysicsUpdate();
    }

    public void RenderActiveScene()
    {
        _currentScene?.Render();
    }

    public void EndFrameActiveScene()
    {
        _currentScene?.EndFrame();
    }

    public void ShutdownActiveScene()
    {
        _currentScene?.Shutdown();
    }
}
