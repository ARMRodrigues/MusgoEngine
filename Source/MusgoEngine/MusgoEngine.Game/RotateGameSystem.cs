using System.Numerics;
using MusgoEngine.Core;

namespace MusgoEngine.Game;

public class RotateMeshSystem : GameSystem
{
    private readonly EntityManager _entityManager;

    public RotateMeshSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public override void Update()
    {
        var dt = GameTime.DeltaTime; // Supondo GameTime singleton ou passado por parâmetro

        foreach (var (entity, meshRenderer) in _entityManager.GetEntitiesWith<MeshRenderer>())
        {
            if (!_entityManager.TryGetComponent(entity, out Transform transform)) continue;
            if (!_entityManager.TryGetComponent(entity, out Rotate rotate)) continue;
            {
                Vector3 eulerDelta = new Vector3(rotate.Speed * dt);

                // Cria um quaternion incremental a partir do Euler delta
                Quaternion incremental = Quaternion.CreateFromYawPitchRoll(
                    eulerDelta.Y, eulerDelta.X, eulerDelta.Z
                );

                // Aplica a rotação incremental
                Quaternion newRotation = incremental * transform.LocalRotation;
                transform.SetLocalRotationQuat(newRotation);
            }
        }
    }
}
