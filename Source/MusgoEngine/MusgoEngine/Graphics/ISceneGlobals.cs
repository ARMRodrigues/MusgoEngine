using MusgoEngine.Math;

namespace MusgoEngine.Graphics;

public interface ISceneGlobals
{
    void Initialize();
    void Update(SceneEnvironment sceneEnvironment, Vector3 directionLight);
    void Shutdown();
}
