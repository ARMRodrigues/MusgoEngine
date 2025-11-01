using System.Numerics;
using MusgoEngine.Core;
using MusgoEngine.Graphics;
using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Game;

public class Game : IGame
{
    public string WindowTitle => "Game | Musgo Engine";

    public void Initialize(SceneManager sceneManager)
    {
        var scene = new Scene("FirstScene");

        // Camera
        var cameraEntity = scene.EntityManager.CreateEntity();
        var camera = new Camera()
        {
            Type = CameraType.Perspective,
        };
        var cameraTransform = new Transform()
        {
            LocalPosition = new Vector3(-10, 0, 30),
            LocalEulerAngles = new Vector3(15, 0, 0),
        };
        scene.EntityManager.AddComponent(cameraEntity, cameraTransform);
        scene.EntityManager.AddComponent(cameraEntity, camera);
        scene.EntityManager.AddComponent(cameraEntity, new RotateTheCamera());

        // Mesh + Material compartilhados
        var mesh = MeshFactory.Create("Cube", MeshPrimitives.CreateCube());
        var material = MaterialFactory.Create(new SimpleGLESShader());

        var refCubeEntity = scene.EntityManager.CreateEntity();
        scene.EntityManager.AddComponent(refCubeEntity, new Transform()
        {
            LocalEulerAngles = new Vector3(-30, 0, 0),
            LocalScale = Vector3.One * 2,
        });
        scene.EntityManager.AddComponent(refCubeEntity, new MeshRenderer(mesh, material));

        var random = new Random();

        for (int i = 0; i < 10; i++)
        {
            var cubeEntity = scene.EntityManager.CreateEntity();

            var position = new Vector3(
                (float)(random.NextDouble() * 10 - 5),  // X: -5 a 5
                (float)(random.NextDouble() * 10 - 5),  // Y: -5 a 5
                (float)(random.NextDouble() * -8 - 2)); // Z: -2 a -10

            var transform = new Transform()
            {
                LocalPosition = position,
            };

            var meshRenderer = new MeshRenderer(mesh, material);

            scene.EntityManager.AddComponent(cubeEntity, transform);
            scene.EntityManager.AddComponent(cubeEntity, meshRenderer);

            // Opcional: cada cubo rotaciona em velocidades diferentes
            scene.EntityManager.AddComponent(cubeEntity, new Rotate((float)(random.NextDouble() * 2)));
        }

        // Systems
        scene.AddGameSystem(new TransformSystem(scene.EntityManager));
        scene.AddGameSystem(new CameraSystem(scene.EntityManager));
        scene.AddGameSystem(new MeshRendererSystem(scene.EntityManager));
        scene.AddGameSystem(new RotateMeshSystem(scene.EntityManager));
        scene.AddGameSystem(new RotateTheCameraSystem(scene.EntityManager));

        sceneManager.SetActiveScene(scene);
    }

    public void Shutdown()
    {
    }
}
