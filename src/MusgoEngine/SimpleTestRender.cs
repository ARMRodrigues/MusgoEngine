using System.Diagnostics;
using System.Numerics;
using MusgoEngine.Native.EGL;
using MusgoEngine.Native.GLFW;
using MusgoEngine.Native.OpenGL;

namespace MusgoEngine;

public class SimpleTestRender
{
    private GLFWWindow _window;
    private EGLDisplay _eglDisplay;
    private EGLContext _eglContext;
    private EGLSurface _eglSurface;

    private List<CubeMesh> _cubeMeshes;

    private DefaultShader _defaultShader;

    public void Start()
    {
        GLFWLoader.Load();

        GLFW.Init();

        GLFW.WindowHint(WindowHint.Visible, GLFWBool.True);
        GLFW.WindowHint(WindowHint.Decorated, GLFWBool.True);
        GLFW.WindowHint(WindowHint.ClientApi, ClientApi.NoApi);

        _window = GLFW.CreateWindow(1280, 720, "Renderer test");

        EGLLoader.Load();

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
            _eglSurface = EGL.CreateWindowSurface(_eglDisplay, config, GLFW.GetWin32Window(_window.Handle), null);

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

        var procAddressProvider = new EGLProcAddressProvider();

        GL.Initialize(procAddressProvider);

        GL.Viewport(0, 0, 1280, 720);
        GL.Enable(0x0B71);
        GL.Disable(0x0B44);
        //GL.Disable(0x0B71);
        //GL.Disable(0x0B44);

        EGL.SwapInterval(_eglDisplay, 0);

        Console.WriteLine("Vendor:   " + GL.GetString(GLStringName.Vendor));
        Console.WriteLine("Renderer: " + GL.GetString(GLStringName.Renderer));
        Console.WriteLine("Version:  " + GL.GetString(GLStringName.Version));

        _defaultShader = new DefaultShader();

        _cubeMeshes = new List<CubeMesh>();

        var seed = 987654321;
        var openSimple = new OpenSimplexNoise(seed);
        var chunk = new Chunk();
        chunk.Generate(openSimple, _defaultShader.Program);

        _cubeMeshes = chunk.Cubes;
    }

    public void Loop()
    {
        var stopwatch = Stopwatch.StartNew();
        var lastTime = 0f;
        var angle = 0f;

        while (!GLFW.WindowShouldClose(_window))
        {
            GLFW.PollEvents();

            var currentTime = (float)stopwatch.Elapsed.TotalSeconds;
            var deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            GL.ClearColor(27/255f, 42/255f, 33/255f, 1.0f);
            GL.Clear(GLClearBufferMask.ColorBufferBit | GLClearBufferMask.DepthBufferBit);

            GL.UseProgram(_defaultShader.Program);

            var camePos = new Vector3(10, 8, 10f);
            var cameraForward = -Vector3.UnitZ;

            var view = Matrix4x4.CreateLookAt(
                camePos,
                camePos + cameraForward,
                Vector3.UnitY
            );

            var proj = Matrix4x4.CreatePerspectiveFieldOfView(
                MathF.PI / 4f,
                1280f / 720f,
                0.1f,
                100f
            );

            angle += 1.0f * deltaTime;

            foreach (var cube in _cubeMeshes)
            {
                //var rot = Matrix4x4.CreateRotationY(angle) * Matrix4x4.CreateRotationX(angle * cube.RotateSpeed);
                var rot = Matrix4x4.CreateFromQuaternion(new Quaternion());
                var model = Matrix4x4.CreateScale(cube.Scale) *
                  rot *
                  Matrix4x4.CreateTranslation(cube.Position);

                GL.UniformMatrix4fv(GL.GetUniformLocation(_defaultShader.Program, "model"), false, model);
                GL.UniformMatrix4fv(GL.GetUniformLocation(_defaultShader.Program, "view"), false, view);
                GL.UniformMatrix4fv(GL.GetUniformLocation(_defaultShader.Program, "proj"), false, proj);

                GL.Uniform3f(GL.GetUniformLocation(_defaultShader.Program, "lightDir"), -0.721f, -0.588f, -0.367f);
                GL.Uniform3f(GL.GetUniformLocation(_defaultShader.Program, "lightColor"), 1f, 1f, 1f);
                GL.Uniform3f(GL.GetUniformLocation(_defaultShader.Program, "objectColor"), 1f, 1f, 1f);

                cube.Draw();
            }

            EGL.SwapBuffers(_eglDisplay, _eglSurface);
        }
    }

    public void Shutdown()
    {
        _defaultShader.Shutdown();

        if (_eglSurface != IntPtr.Zero)
            EGL.DestroySurface(_eglDisplay, _eglSurface);
        if (_eglContext != IntPtr.Zero)
            EGL.DestroyContext(_eglDisplay, _eglContext);
        if (_eglDisplay != IntPtr.Zero)
            EGL.Terminate(_eglDisplay);

        foreach (var cubes in _cubeMeshes)
        {
            cubes.Shutdown();
        }

        if (_window != IntPtr.Zero)
            GLFW.DestroyWindow(_window);

        Console.WriteLine("GLFW shutting down");
    }
}
