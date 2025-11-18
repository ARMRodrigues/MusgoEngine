namespace MusgoEngine.Native.EGL;

public readonly record struct EGLContext(IntPtr Handle)
{
    public IntPtr Handle { get; } = Handle;
    public bool IsValid => Handle != IntPtr.Zero;

    public static readonly EGLContext Null = new(IntPtr.Zero);

    public static implicit operator IntPtr(EGLContext c) => c.Handle;
    public override string ToString() => $"EGLContext(0x{Handle:X})";
}
