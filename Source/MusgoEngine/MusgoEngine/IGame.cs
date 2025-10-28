using MusgoEngine.Core;

namespace MusgoEngine;

public interface IGame
{
    string WindowTitle { get; }

    void Initialize(SceneManager sceneManager);
    void Shutdown();
}
