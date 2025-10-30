using System.Numerics;
using MusgoEngine.Graphics;

namespace MusgoEngine.Core;

public class MeshRenderer(Mesh mesh, Material material) : GameComponent
{
    public Mesh Mesh { get; } = mesh;
    public Material Material { get; } = material;

    public void Render(in Matrix4x4 model, in Matrix4x4 view, in Matrix4x4 projection)
    {
        Material.Bind();

        Material.Shader.SetUniform("uModel", in model);
        Material.Shader.SetUniform("uView", in view);
        Material.Shader.SetUniform("uProjection", in projection);

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
