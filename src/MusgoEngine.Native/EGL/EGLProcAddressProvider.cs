namespace MusgoEngine.Native.EGL;

public class EGLProcAddressProvider : IProcAddressProvider
{
    public unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddressPointer()
        => EGL.GetProcAddress;
}
