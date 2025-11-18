namespace MusgoEngine.Native.EGL;

public enum EGLAttribute : int
{
    None = 0x3038,
    RedSize = 0x3024,
    GreenSize = 0x3023,
    BlueSize = 0x3022,
    AlphaSize = 0x3021,
    DepthSize = 0x3025,
    StencilSize = 0x3026,
    RenderableType = 0x3040,
    SurfaceType = 0x3033,
    ContextClientVersion = 0x3098,
    OpenGLES3 = 0x00000040
}
