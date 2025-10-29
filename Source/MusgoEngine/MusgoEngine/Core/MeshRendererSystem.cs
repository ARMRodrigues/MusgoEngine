namespace MusgoEngine.Core;

public class MeshRendererSystem(EntityManager entityManager) : GameSystem
{
    public override void Render()
    {
        foreach (var meshRenderer in entityManager.GetComponents<MeshRenderer>())
        {
            meshRenderer.Render();
        }
    }

    public override void Shutdown()
    {
        foreach (var meshRenderer in entityManager.GetComponents<MeshRenderer>())
        {
            meshRenderer.Render();
        }
    }
}
