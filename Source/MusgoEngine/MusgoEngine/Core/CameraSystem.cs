using MusgoEngine.Math;

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

            if (transform.LocalMatrix == Matrix4.Identity)
                transform.RebuildMatrices();

            camera.View = GetViewMatrix(transform);

            camera.Aspect = (_height > 0f) ? (_width / _height) : 1f;

            if (camera.Type == CameraType.Perspective)
            {
                camera.Projection = Matrix4.CreatePerspectiveFieldOfView(
                    camera.FieldOfViewDegrees,
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

                camera.Projection = Matrix4.CreateOrthographicOffCenter(
                    left, right, bottom, top,
                    camera.NearPlane, camera.FarPlane
                );
            }
        }
    }

    private static Matrix4 GetViewMatrix(Transform transform)
    {
        var position = transform.Position;
        var forward = transform.Forward;
        var up = transform.Up;

        return Matrix4.CreateLookAt(position, position + forward, up);
    }

    public void OnResize(float w, float h)
    {
        _width = w;
        _height = h;

        // TODO
    }
}
