namespace MusgoEngine.Bindings.OpenGL;

[Flags]
public enum GLClearBufferMask
{
    None = 0,
    DepthBufferBit = 256,
    AccumBufferBit = 512,
    StencilBufferBit = 1024,
    ColorBufferBit = 16384,
    CoverageBufferBitNv = 32768
}
