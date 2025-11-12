using System.Numerics;
using MusgoEngine.Math;

namespace MusgoEngine.Core;

public class MeshRendererSystem(EntityManager entityManager) : GameSystem
{
    public override void Render()
    {
        var camera = entityManager.GetComponents<Camera>().FirstOrDefault();
        var view = camera?.View ?? Matrix4.Identity;
        var proj = camera?.Projection ?? Matrix4.Identity;

        foreach (var (entity, meshRenderer) in entityManager.GetEntitiesWith<MeshRenderer>())
        {
            if (!entityManager.TryGetComponent(entity, out Transform transform)) continue;
            meshRenderer.Render(transform.WorldMatrix, in view, in proj);
        }
    }

    public override void Shutdown()
    {
        foreach (var meshRenderer in entityManager.GetComponents<MeshRenderer>())
        {
            meshRenderer.Destroy();
        }
    }
}
