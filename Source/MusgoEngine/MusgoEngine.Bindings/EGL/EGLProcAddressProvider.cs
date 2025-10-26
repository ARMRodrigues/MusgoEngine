namespace MusgoEngine.Bindings.EGL;

public class EGLProcAddressProvider : IProcAddressProvider
{
    public unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddressPointer()
        => EGL.GetProcAddress;
}
