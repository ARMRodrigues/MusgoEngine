using System.Numerics;
using MusgoEngine.Core;
using MusgoEngine.Graphics;
using MusgoEngine.Graphics.Backends;
using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Game;

public class Game : IGame
{
    public string WindowTitle => "Game | Musgo Engine";

    public void Initialize(SceneManager sceneManager)
    {
        var scene = new Scene("FirstScene");

        var cameraEntity = scene.EntityManager.CreateEntity();
        var camera = new Camera()
        {
            Type = CameraType.Perspective,
        };
        var cameraTransform = new Transform()
        {
            LocalPosition = new Vector3(0, 0, 3.0f)
        };
        scene.EntityManager.AddComponent(cameraEntity, cameraTransform);
        scene.EntityManager.AddComponent(cameraEntity, camera);

        var cubeEntity = scene.EntityManager.CreateEntity();

        var mesh = MeshFactory.Create("Triangle", MeshPrimitives.CreateCube());
        var material = MaterialFactory.Create(new SimpleGLESShader());
        var meshRenderer = new MeshRenderer(mesh, material);

        scene.EntityManager.AddComponent(cubeEntity, new Transform()
        {
            LocalPosition = new Vector3(1f, 1f, 0f),
            LocalScale = Vector3.One * 0.25f
        });
        scene.EntityManager.AddComponent(cubeEntity, meshRenderer);

        scene.EntityManager.AddComponent(cubeEntity, new Rotate(2));

        scene.AddGameSystem(new TransformSystem(scene.EntityManager));
        scene.AddGameSystem(new CameraSystem(scene.EntityManager));
        scene.AddGameSystem(new MeshRendererSystem(scene.EntityManager));
        scene.AddGameSystem(new RotateMeshSystem(scene.EntityManager));

        sceneManager.SetActiveScene(scene);
    }

    public void Shutdown()
    {

    }
}
