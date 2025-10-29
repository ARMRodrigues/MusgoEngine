using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class MeshFactory
{
    public static Mesh Create(string name, float[] vertices, uint[] indices)
    {
        return GraphicsDevice.Instance.ApiType switch
        {
            GraphicApiType.GLES => new GLESMesh(name, vertices, indices),
            _ => throw new NotSupportedException("Unsupported API for mesh creation.")
        };
    }
}

