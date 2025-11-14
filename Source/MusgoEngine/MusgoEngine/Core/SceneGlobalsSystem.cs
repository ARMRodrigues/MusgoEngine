using System.Numerics;
using MusgoEngine.Graphics;

namespace MusgoEngine.Core;

public class SceneGlobalsSystem(Scene scene) : GameSystem
{
    private readonly EntityManager _entityManager = scene.EntityManager;
    private readonly SceneEnvironment _sceneEnvironment = scene.SceneEnvironment;
    private readonly ISceneGlobals _sceneGlobals = scene.SceneGlobals;

    public override void Initialize()
    {
        _sceneGlobals.Initialize();
    }

    public override void Update()
    {
        DirectionalLight? mainLight = null;
        Transform? lightTransform = null;

        foreach (var (entity, light) in _entityManager.GetEntitiesWith<DirectionalLight>())
        {
            if (!light.IsMainLight) continue;
            mainLight = light;
            _entityManager.TryGetComponent(entity, out lightTransform);
            break;
        }

        if (mainLight == null || lightTransform == null)
            return;

        /*Console.WriteLine($"The rotation in euler degrees is {lightTransform.LocalEulerAngles}");
        Console.WriteLine($"The value of Transform.Forward value is {lightTransform.Forward}");
        Console.WriteLine($"The value of world matrix is {lightTransform.WorldMatrix}");*/

        _sceneEnvironment.MainLight = mainLight;
        _sceneGlobals.Update(_sceneEnvironment, lightTransform.Forward);
    }

    public override void Shutdown()
    {
        _sceneGlobals.Shutdown();
    }
}
