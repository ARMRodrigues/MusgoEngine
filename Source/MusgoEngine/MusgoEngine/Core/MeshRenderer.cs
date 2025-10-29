using MusgoEngine.Graphics;

namespace MusgoEngine.Core;

public class MeshRenderer(Mesh mesh, Material material) : GameComponent
{
    public Mesh Mesh { get; } = mesh;
    public Material Material { get; } = material;

    public void Render()
    {
        Material.Bind();
        Mesh.Bind();
        Mesh.Draw();
        Mesh.Unbind();
        Material.Unbind();
    }

    public void Destroy()
    {
        Material.Destroy();
        Mesh.Destroy();
    }
}
