using MusgoEngine.Graphics;
using MusgoEngine.Windowing;

namespace MusgoEngine.Binder;

public class WindowGraphicApiBinder
{
    public static bool Bind(GraphicApiType apiType, IWindowSystem windowSystem, IGraphicApi graphicApi)
    {
        return apiType switch
        {
            GraphicApiType.EGL => EglBinder.Bind(windowSystem, graphicApi),
            _ => false
        };
    }
}
