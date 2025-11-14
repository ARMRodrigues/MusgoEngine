using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class SceneGlobalsFactory
{
    public static ISceneGlobals Create()
    {
        var apiType = GraphicsDevice.Instance.ApiType;
        return apiType switch
        {
            GraphicApiType.GLES => new GLESSceneGlobals(),
            _ => throw new NotSupportedException("Unsupported API for SceneUBO.")
        };
    }
}
