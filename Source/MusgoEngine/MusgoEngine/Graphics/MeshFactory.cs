using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class MeshFactory
{
    public static Mesh Create(string name, MeshData meshData)
    {
        return GraphicsDevice.Instance.ApiType switch
        {
            GraphicApiType.GLES => new GLESMesh(name, meshData),
            _ => throw new NotSupportedException("Unsupported API for mesh creation.")
        };
    }
}

