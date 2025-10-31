using System.Numerics;
using MusgoEngine.Core;

namespace MusgoEngine.Game;

public class RotateMeshSystem(EntityManager entityManager) : GameSystem
{
    public override void Update()
    {
        var dt = GameTime.DeltaTime;

        foreach (var (entity, meshRenderer) in entityManager.GetEntitiesWith<MeshRenderer>())
        {
            if (!entityManager.TryGetComponent(entity, out Transform transform)) continue;
            if (!entityManager.TryGetComponent(entity, out Rotate rotate)) continue;
            {
                var eulerDelta = new Vector3(rotate.Speed * dt);

                var incremental = Quaternion.CreateFromYawPitchRoll(
                    eulerDelta.Y, eulerDelta.X, eulerDelta.Z
                );

                var newRotation = transform.LocalRotation * incremental;
                transform.LocalRotation = newRotation;
            }
        }
    }
}
