using MusgoEngine.Core;

namespace MusgoEngine;

public class Scene(string name)
{
    private readonly List<GameSystem> _gameSystems = [];

    public readonly EntityManager EntityManager = new();
    public string Name { get; private set; } = name;

    public void AddGameSystem(GameSystem gameSystem)
    {
        if (_gameSystems.Contains(gameSystem)) return;

        _gameSystems.Add(gameSystem);
    }

    public void Initialize()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Initialize();
        }
    }

    public void BeginFrame()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.BeginFrame();
        }
    }

    public void Update()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Update();
        }
    }

    public void PhysicsUpdate()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.PhysicsUpdate();
        }
    }

    public void Render()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Render();
        }
    }

    public void EndFrame()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.EndFrame();
        }

        EntityManager.ProcessRemovals();
    }

    public void Shutdown()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Shutdown();
        }
    }
}
