namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<float, float, float, float, void> _glClearColor;
    private static delegate* unmanaged[Cdecl]<uint, void> _glClear;

    /// <summary>
    /// Specify clear values for the color buffers. The initial values are all 0.
    /// </summary>
    /// <param name="red">Specify the red value used when the color buffers are cleared.</param>
    /// <param name="green">Specify the green value used when the color buffers are cleared.</param>
    /// <param name="blue">Specify blue value used when the color buffers are cleared.</param>
    /// <param name="alpha">Specify alpha value used when the color buffers are cleared.</param>
    public static void ClearColor(float red, float green, float blue, float alpha) =>
        _glClearColor(red, green, blue, alpha);

    /// <summary>
    /// Clear buffers to preset values
    /// </summary>
    /// <param name="mask">Bitwise OR of masks that indicate the buffers to be cleared. The three masks are ColorBufferBit, DepthBufferBit, and StencilBufferBit.</param>
    public static void Clear(GLClearBufferMask mask) => _glClear((uint)mask);
}
