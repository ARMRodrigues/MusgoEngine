namespace MusgoEngine.Native.EGL;

[Flags]
public enum EGLRenderableType : uint
{
    OpenGLES2 = 0x00000004,
    OpenGLES3 = 0x00000040,
    OpenGL = 0x00000008
}
