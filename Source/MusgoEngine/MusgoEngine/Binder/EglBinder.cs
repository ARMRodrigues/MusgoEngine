using MusgoEngine.Graphics;
using MusgoEngine.Windowing;

namespace MusgoEngine.Binder;

internal static class EglBinder
{
    public static bool Bind(IWindowSystem windowSystem, IGraphicApi graphicApi)
    {
        // Obter handle da janela nativa
        var nativeHandle = windowSystem.GetNativeHandle();
        if (nativeHandle == IntPtr.Zero)
            return false;

        // Criar contexto EGL
        if (!graphicApi.CreateEglContext(nativeHandle))
            return false;

        return true;
    }
}
