using MusgoEngine.Core;

namespace MusgoEngine.Game;

public class Game : IGame
{
    public string WindowTitle => "Game | Musgo Engine";

    public void Initialize(SceneManager sceneManager)
    {
        var scene = new Scene("FirstScene");
        var player = scene.EntityManager.CreateEntity();
        scene.AddGameSystem(new EntitiesCountGameSystem(scene.EntityManager));
        sceneManager.SetActiveScene(scene);
    }

    public void Shutdown()
    {

    }
}
