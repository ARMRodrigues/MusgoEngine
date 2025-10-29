using MusgoEngine.Graphics.Backends;
using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class GraphicsApiFactory
{
    public static IGraphicApi Create(GraphicApiType apiType)
    {
        return apiType switch
        {
            GraphicApiType.HeadlessApi => new HeadlessGraphicApi(),
            GraphicApiType.GLES => new GLESGraphicApi(),
            _ => throw new NotSupportedException($"Unsupported API: {apiType}")
        };
    }
}
