namespace MusgoEngine.Native.EGL;

public readonly record struct EGLSurface(IntPtr Handle)
{
    public IntPtr Handle { get; } = Handle;
    public bool IsValid => Handle != IntPtr.Zero;
    public static implicit operator IntPtr(EGLSurface s) => s.Handle;
    public override string ToString() => $"EGLSurface(0x{Handle:X})";
}
