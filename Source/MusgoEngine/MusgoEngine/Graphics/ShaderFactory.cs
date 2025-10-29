using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class ShaderFactory
{
    public static Shader CreateFromFiles(string name, string vertexPath, string fragmentPath)
    {
        var vertexSrc = File.ReadAllText(vertexPath);
        var fragmentSrc = File.ReadAllText(fragmentPath);

        return GraphicsDevice.Instance.ApiType switch
        {
            GraphicApiType.GLES => new GLESShader(name, vertexSrc, fragmentSrc),
            _ => throw new NotSupportedException("Unsupported API for shader creation.")
        };
    }
}
