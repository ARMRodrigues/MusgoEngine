using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class MaterialFactory
{
    public static Material Create(Shader shader)
    {
        return GraphicsDevice.Instance.ApiType switch
        {
            GraphicApiType.GLES => new GLESMaterial(shader),
            _ => throw new NotSupportedException("Unsupported API for material creation.")
        };
    }
}
