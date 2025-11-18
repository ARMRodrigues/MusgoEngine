using System.Runtime.InteropServices;

namespace MusgoEngine.Native.GLFW;

public static class GLFWLoader
{
    private static nint _libHandle;

    public static void Load()
    {
        string libName;
        string subDir;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            libName = "glfw3.dll";
            subDir = "win-x64";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            libName = "libglfw.so.3";
            subDir = "linux-x64";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            libName = "libglfw.3.dylib";
            subDir = "osx-x64";
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported OS for GLFW.");
        }

        var candidatePath = Path.Combine(AppContext.BaseDirectory, "runtimes", subDir, "native", libName);
        _libHandle = NativeLibrary.Load(File.Exists(candidatePath) ? candidatePath : libName);

        GLFW.LoadFunctions(_libHandle);
    }

    public static void Unload()
    {
        if (_libHandle == IntPtr.Zero) return;

        NativeLibrary.Free(_libHandle);
        _libHandle = IntPtr.Zero;
    }
}
