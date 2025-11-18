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

    private uint _vao, _vbo, _ebo;
    private uint _shaderProgram;

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

        SetMesh();
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

            GL.UseProgram(_shaderProgram);

            var pos = new Vector3();
            var rot = Quaternion.CreateFromYawPitchRoll(0, 0, 0);
            var scale = Vector3.One;

            /*var model = Matrix4x4.CreateScale(scale) *
                              Matrix4x4.CreateFromQuaternion(rot) *
                              Matrix4x4.CreateTranslation(pos);*/
            angle += 1.0f * deltaTime;
            var model = Matrix4x4.CreateRotationY(angle) * Matrix4x4.CreateRotationX(angle * 0.5f);

            var camePos = new Vector3(0, 0, 10f);
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

            // Matrix
            GL.UniformMatrix4fv(GL.GetUniformLocation(_shaderProgram, "model"), false, model);
            GL.UniformMatrix4fv(GL.GetUniformLocation(_shaderProgram, "view"), false, view);
            GL.UniformMatrix4fv(GL.GetUniformLocation(_shaderProgram, "proj"), false, proj);

            // Send Light data
            GL.Uniform3f(GL.GetUniformLocation(_shaderProgram, "lightDir"), -0.25f, -0.866f, 0.433f);
            GL.Uniform3f(GL.GetUniformLocation(_shaderProgram, "lightColor"), 1f, 1f, 1f);
            GL.Uniform3f(GL.GetUniformLocation(_shaderProgram, "objectColor"), 0.8f, 0.2f, 0.2f);

            // Draw cube
            GL.BindVertexArray(_vao);
            GL.DrawElements(GLPrimitiveType.Triangles, 36, GLDrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            EGL.SwapBuffers(_eglDisplay, _eglSurface);
        }
    }

    public void Shutdown()
    {
        // Delete buffers
        if (_ebo != 0) GL.DeleteBuffer(_ebo);
        if (_vbo != 0) GL.DeleteBuffer(_vbo);
        if (_vao != 0) GL.DeleteVertexArray(_vao);

        // Delete shader
        if (_shaderProgram != 0) GL.DeleteProgram(_shaderProgram);

        if (_eglSurface != IntPtr.Zero)
            EGL.DestroySurface(_eglDisplay, _eglSurface);
        if (_eglContext != IntPtr.Zero)
            EGL.DestroyContext(_eglDisplay, _eglContext);
        if (_eglDisplay != IntPtr.Zero)
            EGL.Terminate(_eglDisplay);

        if (_window != IntPtr.Zero)
            GLFW.DestroyWindow(_window);

        Console.WriteLine("GLFW shutting down");
    }

    private void SetMesh()
    {
        float[] vertices = {
            // positions        // normals

            // Front face
            -0.5f, -0.5f,  0.5f,  0, 0, 1,
            0.5f, -0.5f,  0.5f,  0, 0, 1,
            0.5f,  0.5f,  0.5f,  0, 0, 1,
            -0.5f,  0.5f,  0.5f,  0, 0, 1,

            // Back face
            -0.5f, -0.5f, -0.5f,  0, 0,-1,
            0.5f, -0.5f, -0.5f,  0, 0,-1,
            0.5f,  0.5f, -0.5f,  0, 0,-1,
            -0.5f,  0.5f, -0.5f,  0, 0,-1,

            // Left face
            -0.5f, -0.5f, -0.5f, -1, 0, 0,
            -0.5f,  0.5f, -0.5f, -1, 0, 0,
            -0.5f,  0.5f,  0.5f, -1, 0, 0,
            -0.5f, -0.5f,  0.5f, -1, 0, 0,

            // Right face
            0.5f, -0.5f, -0.5f,  1, 0, 0,
            0.5f,  0.5f, -0.5f,  1, 0, 0,
            0.5f,  0.5f,  0.5f,  1, 0, 0,
            0.5f, -0.5f,  0.5f,  1, 0, 0,

            // Top face
            -0.5f,  0.5f, -0.5f,  0, 1, 0,
            0.5f,  0.5f, -0.5f,  0, 1, 0,
            0.5f,  0.5f,  0.5f,  0, 1, 0,
            -0.5f,  0.5f,  0.5f,  0, 1, 0,

            // Bottom face
            -0.5f, -0.5f, -0.5f,  0,-1, 0,
            0.5f, -0.5f, -0.5f,  0,-1, 0,
            0.5f, -0.5f,  0.5f,  0,-1, 0,
            -0.5f, -0.5f,  0.5f,  0,-1, 0,
        };

        uint[] indices = {
            // Front
            0, 1, 2, 2, 3, 0,
            // Back
            4, 5, 6, 6, 7, 4,
            // Left
            8, 9,10,10,11, 8,
            // Right
            12,13,14,14,15,12,
            // Top
            16,17,18,18,19,16,
            // Bottom
            20,21,22,22,23,20
        };


        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(GLBufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(GLBufferTarget.ArrayBuffer, vertices, GLBufferUsageHint.StaticDraw);

        GL.BindBuffer(GLBufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(GLBufferTarget.ElementArrayBuffer, indices, GLBufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, GLVertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, GLVertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        GL.BindVertexArray(0);

        // --- Shader básico ---
        string vertexSrc = @"#version 300 es
        precision highp float;

        layout(location = 0) in vec3 aPos;
        layout(location = 1) in vec3 aNormal;

        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 proj;

        out vec3 FragPos;
        out vec3 Normal;

        void main()
        {
            FragPos = vec3(model * vec4(aPos,1.0));
            Normal = mat3(transpose(inverse(model))) * aNormal;
            gl_Position = proj * view * vec4(FragPos,1.0);
        }
        ";

        string fragSrc = @"#version 300 es
        precision highp float;

        in vec3 FragPos;
        in vec3 Normal;

        uniform vec3 lightDir;
        uniform vec3 lightColor;
        uniform vec3 objectColor;

        out vec4 FragColor;

        void main()
        {
            float NdotL = max(dot(normalize(Normal), -lightDir), 0.0);
            vec3 diffuse = NdotL * lightColor;
            vec3 result = (diffuse + vec3(0.1)) * objectColor; // ambient 0.1
            FragColor = vec4(result,1.0);
        }
        ";

        _shaderProgram = CompileShader(vertexSrc, fragSrc);
    }

    private static uint CompileShader(string vertexSrc, string fragSrc)
    {
        uint vertex = GL.CreateShader(GLShaderType.VertexShader);
        GL.ShaderSource(vertex, vertexSrc);
        GL.CompileShader(vertex);

        GL.GetShaderiv(vertex, GLShaderParameter.CompileStatus, out var vStatus);

        if (vStatus == 0)
        {
            var info = GL.GetShaderInfoLog(vertex);
#if DEBUG
            throw new Exception($"Vertex shader compilation failed:\n{info}");
#else
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Shader Error] Vertex shader compilation failed:\n{info}");
        Console.ResetColor();
        Environment.Exit(1);
#endif
        }

        uint frag = GL.CreateShader(GLShaderType.FragmentShader);
        GL.ShaderSource(frag, fragSrc);
        GL.CompileShader(frag);
        GL.GetShaderiv(frag, GLShaderParameter.CompileStatus, out var fStatus);

        if (fStatus == 0)
        {
            var info = GL.GetShaderInfoLog(frag);
#if DEBUG
            throw new Exception($"Fragment shader compilation failed:\n{info}");
#else
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Shader Error] Fragment shader compilation failed:\n{info}");
        Console.ResetColor();
        Environment.Exit(1);
#endif
        }

        uint program = GL.CreateProgram();
        GL.AttachShader(program, vertex);
        GL.AttachShader(program, frag);
        GL.LinkProgram(program);
        GL.GetProgramiv(program, GLGetProgramParameterName.LinkStatus, out var pStatus);

        if (pStatus == 0)
        {
            var info = GL.GetProgramInfoLog(program);
#if DEBUG
            throw new Exception($"Shader program linking failed:\n{info}");
#else
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Shader Error] Program linking failed:\n{info}");
        Console.ResetColor();
        Environment.Exit(1);
#endif
        }
        GL.DeleteShader(vertex);
        GL.DeleteShader(frag);
        return program;
    }
}
