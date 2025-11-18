namespace MusgoEngine.Native.EGL;

public readonly struct EGLConfig(IntPtr handle)
{
    public IntPtr Handle { get; } = handle;
    public bool IsValid => Handle != IntPtr.Zero;
    public static implicit operator IntPtr(EGLConfig c) => c.Handle;
    public override string ToString() => $"EGLConfig(0x{Handle:X})";
}
