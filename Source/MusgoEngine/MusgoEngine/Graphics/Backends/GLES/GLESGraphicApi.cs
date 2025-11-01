using MusgoEngine.Bindings.EGL;
using MusgoEngine.Bindings.OpenGL;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESGraphicApi : IGraphicApi
{
    private EGLDisplay _eglDisplay;
    private EGLContext _eglContext;
    private EGLSurface _eglSurface;

    public GLESGraphicApi()
    {
        EGLLoader.Load();
    }

    public void CreateApi(IntPtr nativeWindowHandle)
    {
        if (nativeWindowHandle == IntPtr.Zero)
            throw new Exception("Native window handle is zero");

        _eglDisplay = EGL.GetDisplay(IntPtr.Zero);
        if (_eglDisplay.Handle == IntPtr.Zero)
            throw new Exception($"eglGetDisplay failed: 0x{EGL.GetError():X}");

        if (!EGL.Initialize(_eglDisplay, out var major, out var minor))
            throw new Exception($"eglInitialize failed: 0x{EGL.GetError():X}");

        if (!EGL.BindApi(EGLApi.OpenGLES))
            throw new Exception($"eglBindAPI failed: 0x{EGL.GetError():X}");

        int[] configAttribs =
        [
            (int)EGLAttribute.RedSize, 8,
            (int)EGLAttribute.GreenSize, 8,
            (int)EGLAttribute.BlueSize, 8,
            (int)EGLAttribute.AlphaSize, 8,
            (int)EGLAttribute.DepthSize, 24,
            (int)EGLAttribute.StencilSize, 8,
            (int)EGLAttribute.SurfaceType, (int)EGLSurfaceType.WindowBit,
            (int)EGLAttribute.RenderableType, (int)EGLAttribute.OpenGLES3,
            (int)EGLAttribute.None
        ];

        var configs = new IntPtr[1];
        if (!EGL.ChooseConfig(_eglDisplay, configAttribs, configs, 1, out var numConfigs) || numConfigs == 0)
            throw new Exception($"eglChooseConfig failed: 0x{EGL.GetError():X}");

        var config = new EGLConfig(configs[0]);

        int[] contextAttribs =
        [
            (int)EGLAttribute.ContextClientVersion, 3,
            (int)EGLAttribute.None
        ];

        _eglContext = EGL.CreateContext(_eglDisplay, config, new EGLContext(IntPtr.Zero), contextAttribs);
        if (_eglContext.Handle == IntPtr.Zero)
            throw new Exception($"eglCreateContext failed: 0x{EGL.GetError():X}");

        try
        {
            _eglSurface = EGL.CreateWindowSurface(_eglDisplay, config, nativeWindowHandle, null);

            if (_eglSurface.Handle == IntPtr.Zero)
                throw new Exception($"eglCreateWindowSurface failed: 0x{EGL.GetError():X}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating surface: {ex.Message}");
            throw;
        }

        if (!EGL.MakeCurrent(_eglDisplay, _eglSurface, _eglSurface, _eglContext))
            throw new Exception($"eglMakeCurrent failed: 0x{EGL.GetError():X}");

        if (GraphicsDevice.Instance.ProcAddressProvider != null)
            GL.Initialize(GraphicsDevice.Instance.ProcAddressProvider);

        GL.Viewport(0, 0, 1280, 720);
        GL.Enable(0x0B71);
        GL.Disable(0x0B44);
        //GL.Disable(0x0B71);
        //GL.Disable(0x0B44);

        EGL.SwapInterval(_eglDisplay, 0);

        Console.WriteLine("Vendor:   " + GL.GetString(GLStringName.Vendor));
        Console.WriteLine("Renderer: " + GL.GetString(GLStringName.Renderer));
        Console.WriteLine("Version:  " + GL.GetString(GLStringName.Version));
    }

    public void BeginDraw()
    {
        GL.ClearColor(27/255f, 42/255f, 33/255f, 1.0f);
        GL.Clear(GLClearBufferMask.ColorBufferBit | GLClearBufferMask.DepthBufferBit);
    }

    public void Draw()
    {
       // throw new NotImplementedException();
    }

    public void EndDraw()
    {
        EGL.SwapBuffers(_eglDisplay, _eglSurface);
    }

    public void DestroyApi()
    {
        if (_eglSurface != IntPtr.Zero)
            EGL.DestroySurface(_eglDisplay, _eglSurface);
        if (_eglContext != IntPtr.Zero)
            EGL.DestroyContext(_eglDisplay, _eglContext);
        if (_eglDisplay != IntPtr.Zero)
            EGL.Terminate(_eglDisplay);
    }
}
