using MusgoEngine.Bindings;
using MusgoEngine.Bindings.EGL;
using MusgoEngine.Windowing;

namespace MusgoEngine.Graphics;

public class GraphicsDevice
{
    public static GraphicsDevice Instance { get; } = new();

    public GraphicApiType ApiType { get; private set; }
    public IGraphicApi? Api { get; private set; }
    public IWindowSystem? Window { get; private set; }
    public IProcAddressProvider? ProcAddressProvider { get; private set; }

    private GraphicsDevice() { }

    public void Initialize(GraphicApiType apiType, Platform platform, IGraphicApi api, IWindowSystem window)
    {
        ApiType = apiType;
        Api = api;
        Window = window;

        ProcAddressProvider = (apiType, platform) switch
        {
            (GraphicApiType.GLES, Platform.Desktop) => new EGLProcAddressProvider(),
            //(GraphicApiType.EGL, Platform.Android) => new SdlEglProcAddressProvider(),
            _ => null
        };
    }
}
