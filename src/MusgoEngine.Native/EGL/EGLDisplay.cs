namespace MusgoEngine.Native.EGL;

public readonly record struct EGLDisplay(IntPtr Handle)
{
    public IntPtr Handle { get; } = Handle;

    public static implicit operator IntPtr(EGLDisplay display) => display.Handle;
    public static explicit operator EGLDisplay(IntPtr handle) => new(handle);

    public bool IsValid => Handle != IntPtr.Zero;

    public override string ToString() => $"EGLDisplay(0x{Handle:X})";
}
