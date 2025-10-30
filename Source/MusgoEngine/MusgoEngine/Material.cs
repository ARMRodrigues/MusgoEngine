using MusgoEngine.Graphics;

namespace MusgoEngine;

public abstract class Material(Shader shader)
{
    public Shader Shader { get; protected set; } = shader;

    public abstract void Bind();
    public abstract void Unbind();
    public abstract void Destroy();
}

