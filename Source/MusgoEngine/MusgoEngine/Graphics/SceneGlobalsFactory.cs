using MusgoEngine.Graphics.Backends.GLES;

namespace MusgoEngine.Graphics;

public static class SceneGlobalsFactory
{
    public static ISceneGlobals Create(GraphicApiType apiType)
    {
        return apiType switch
        {
            GraphicApiType.GLES => new GLESSceneGlobals(),
            _ => throw new NotSupportedException("Unsupported API for SceneUBO.")
        };
    }
}
