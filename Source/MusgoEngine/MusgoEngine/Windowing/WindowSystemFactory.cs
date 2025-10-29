namespace MusgoEngine.Windowing;

public static class WindowSystemFactory
{
    public static IWindowSystem Create(GraphicApiType apiType)
    {
        return apiType switch
        {
            GraphicApiType.HeadlessApi => new HeadlessWindowSystem(),
            GraphicApiType.GLES => new GlfwWindowSystem(),
            _ => throw new NotSupportedException($"Unsupported API: {apiType}")
        };
    }
}
