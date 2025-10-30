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

        var triangle = scene.EntityManager.CreateEntity();

        float[] vertices =
        [
            // posição x,y,z    // cor
            0.0f, 0.5f, -1f,   1f,0f,0f,
           -0.5f,-0.5f, -1f,   0f,1f,0f,
            0.5f,-0.5f, -1f,   0f,0f,1f
        ];

        uint[] indices = [0, 1, 2];

        var mesh = MeshFactory.Create("Triangle", vertices, indices);
        var material = MaterialFactory.Create(new SimpleGLESShader());
        var meshRenderer = new MeshRenderer(mesh, material);

        scene.EntityManager.AddComponent(triangle, new Transform()
        {
            LocalPosition = new Vector3(1f, 1f, 0f),
            LocalScale = Vector3.One * 0.25f
        });
        scene.EntityManager.AddComponent(triangle, meshRenderer);

        scene.AddGameSystem(new TransformSystem(scene.EntityManager));
        scene.AddGameSystem(new CameraSystem(scene.EntityManager));
        scene.AddGameSystem(new MeshRendererSystem(scene.EntityManager));

        sceneManager.SetActiveScene(scene);
    }

    public void Shutdown()
    {

    }
}
