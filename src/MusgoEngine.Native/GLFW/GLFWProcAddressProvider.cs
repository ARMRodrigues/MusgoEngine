namespace MusgoEngine.Native.GLFW;

public class GLFWProcAddressProvider : IProcAddressProvider
{
    public unsafe delegate* unmanaged[Cdecl]<byte*, nint> GetProcAddressPointer()
        => GLFW.GetProcAddress;
}
