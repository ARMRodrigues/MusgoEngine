namespace MusgoEngine.Bindings;

public interface IProcAddressProvider
{
    unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddressPointer();
}
