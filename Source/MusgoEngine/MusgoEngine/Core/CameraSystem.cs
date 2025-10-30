using System.Numerics;

namespace MusgoEngine.Core;

public class CameraSystem(EntityManager entityManager) : GameSystem
{
    private float _width = 1280;
    private float _height = 720;

    public override void Update()
    {
        foreach (var entity in entityManager.Entities)
        {
            if (!entityManager.TryGetComponent(entity, out Camera camera)) continue;
            if (!entityManager.TryGetComponent(entity, out Transform transform)) continue;

            if (!transform.HasChanged && !camera.HasChanged) continue;

            if (transform.LocalMatrix == Matrix4x4.Identity)
                transform.RebuildLocalMatrix();

            camera.View = Matrix4x4.CreateLookAt(
                transform.Position,
                transform.Position + transform.Forward,
                transform.Up
            );

            camera.Aspect = (_height > 0f) ? (_width / _height) : 1f;

            if (camera.Type == CameraType.Perspective)
            {
                var fovRad = MathF.PI / 180f * camera.FieldOfViewDegrees;
                camera.Projection = Matrix4x4.CreatePerspectiveFieldOfView(
                    fovRad,
                    camera.Aspect,
                    camera.NearPlane,
                    camera.FarPlane
                );
            }
            else
            {
                var left   = -camera.OrthographicSize * camera.Aspect;
                var right  =  camera.OrthographicSize * camera.Aspect;
                var bottom = -camera.OrthographicSize;
                var top    =  camera.OrthographicSize;

                camera.Projection = Matrix4x4.CreateOrthographicOffCenter(
                    left, right, bottom, top,
                    camera.NearPlane, camera.FarPlane
                );
            }
        }
    }

    public void OnResize(float w, float h)
    {
        _width = w;
        _height = h;

        // TODO
    }
}
