using System.Runtime.InteropServices;

namespace MusgoEngine.Native.EGL;

public static class EGLLoader
{
    private static nint _libHandle;

    public static void Load()
    {
        string libName;
        string subDir;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            libName = "libEGL.dll";
            subDir = "win-x64";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            libName = "libEGL.so";
            subDir = "linux-x64";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            libName = "libEGL.dylib";
            subDir = "osx-x64";
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported OS for EGL.");
        }

        var candidatePath = Path.Combine(AppContext.BaseDirectory, "runtimes", subDir, "native", libName);
        _libHandle = NativeLibrary.Load(File.Exists(candidatePath) ? candidatePath : libName);

        EGL.LoadFunctions(_libHandle);
    }

    public static void Unload()
    {
        if (_libHandle == IntPtr.Zero)
            return;

        NativeLibrary.Free(_libHandle);
        _libHandle = IntPtr.Zero;
    }

}

