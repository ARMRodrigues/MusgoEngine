namespace MusgoEngine.Native;

public interface IProcAddressProvider
{
    unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddressPointer();
}
