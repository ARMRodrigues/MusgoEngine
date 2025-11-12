using MusgoEngine.Core;
using MusgoEngine.Math;

namespace MusgoEngine.Game;

public class RotateTheCameraSystem(EntityManager entityManager)  : GameSystem
{
    public override void Update()
    {
        var dt = GameTime.DeltaTime;

        foreach (var (entity, rotateCamera) in entityManager.GetEntitiesWith<RotateTheCamera>())
        {
            if (!entityManager.TryGetComponent(entity, out Transform transform)) continue;
            {
                transform.RotateAround(rotateCamera.Target, rotateCamera.RotateSpeed * dt, Vector3.Up);

                transform.LookAt(rotateCamera.Target, Vector3.Up);
            }
        }
    }
}
