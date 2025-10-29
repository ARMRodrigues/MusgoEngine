namespace MusgoEngine.Graphics;

public abstract class Mesh(string name)
{
    public string Name { get; } = name;

    public abstract void Bind();
    public abstract void Unbind();
    public abstract void Draw();
    public abstract void Destroy();
}

