namespace MusgoEngine;

public abstract class Mesh(string name, MeshData data)
{
    public string Name { get; } = name;
    public MeshData Data { get; } = data;

    public abstract void Bind();
    public abstract void Unbind();
    public abstract void Draw();
    public abstract void Destroy();
}

