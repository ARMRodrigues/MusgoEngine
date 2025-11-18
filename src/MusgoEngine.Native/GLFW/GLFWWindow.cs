namespace MusgoEngine.Native.GLFW;

public readonly struct GLFWWindow(IntPtr handle)
{
    public readonly IntPtr Handle = handle;

    public bool IsNull => Handle == IntPtr.Zero;

    public static implicit operator IntPtr(GLFWWindow window) => window.Handle;
    public static implicit operator GLFWWindow(IntPtr ptr) => new(ptr);
}
