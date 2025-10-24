namespace MusgoEngine.Bindings.Glfw3;

public readonly struct GlfwWindow(IntPtr handle)
{
    public readonly IntPtr Handle = handle;

    public bool IsNull => Handle == IntPtr.Zero;

    public static implicit operator IntPtr(GlfwWindow window) => window.Handle;
    public static implicit operator GlfwWindow(IntPtr ptr) => new(ptr);
}
