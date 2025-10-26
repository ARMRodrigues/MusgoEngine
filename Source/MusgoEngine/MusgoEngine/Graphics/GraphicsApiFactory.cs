namespace MusgoEngine.Graphics;

public static class GraphicsApiFactory
{
    public static IGraphicApi Create(GraphicApiType apiType)
    {
        return apiType switch
        {
            GraphicApiType.HeadlessApi => new HeadlessGraphicApi(),
            GraphicApiType.EGL => new EGLGraphicApi(),
            _ => throw new NotSupportedException($"Unsupported API: {apiType}")
        };
    }
}
