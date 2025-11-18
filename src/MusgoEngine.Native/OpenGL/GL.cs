namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static delegate* unmanaged[Cdecl]<byte*, nint> _getProcAddress;

    public static void Initialize(IProcAddressProvider provider)
    {
        _getProcAddress = provider.GetProcAddressPointer();
        LoadFunctions();
    }

    private static void LoadFunctions()
    {
        _glClearColor = (delegate* unmanaged[Cdecl]<float, float, float, float, void>)GetProcAddressPointer("glClearColor");
        _glClear = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glClear");
        _glGetString = (delegate* unmanaged[Cdecl]<uint, IntPtr>)GetProcAddressPointer("glGetString");

        // ShaderObjects
        _glCreateShader = (delegate* unmanaged[Cdecl]<uint, uint>)GetProcAddressPointer("glCreateShader");
        _glShaderSource = (delegate* unmanaged[Cdecl]<uint, int, byte**, int*, void>)GetProcAddressPointer("glShaderSource");
        _glCompileShader = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glCompileShader");
        _glDeleteShader = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glDeleteShader");

        // ProgramObjects
        _glCreateProgram = (delegate* unmanaged[Cdecl]<uint>)GetProcAddressPointer("glCreateProgram");
        _glAttachShader = (delegate* unmanaged[Cdecl]<uint, uint, void>)GetProcAddressPointer("glAttachShader");
        _glLinkProgram = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glLinkProgram");
        _glUseProgram = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glUseProgram");
        _glDeleteProgram = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glDeleteProgram");

        // UniformVariables
        _glGetUniformLocation = (delegate* unmanaged[Cdecl]<uint, byte*, int>)GetProcAddressPointer("glGetUniformLocation");
        _glGetActiveUniform = (delegate* unmanaged[Cdecl]<uint, uint, int, int*, int*, uint*, byte*, void>)GetProcAddressPointer("glGetActiveUniform");
        _glGetUniformBlockIndex = (delegate* unmanaged[Cdecl]<uint, byte*, uint>)GetProcAddressPointer("glGetUniformBlockIndex");

        // LoadUniformVars
        _glUniform1f = (delegate* unmanaged[Cdecl]<int, float, void>)GetProcAddressPointer("glUniform1f");
        _glUniformMatrix4fv = (delegate* unmanaged[Cdecl]<int, int, bool, float*, void>)GetProcAddressPointer("glUniformMatrix4fv");

        // VertexArrayObjects
        _glGenVertexArrays = (delegate* unmanaged[Cdecl]<int, uint*, void>)GetProcAddressPointer("glGenVertexArrays");
        _glDeleteVertexArrays = (delegate* unmanaged[Cdecl]<int, uint*, void>)GetProcAddressPointer("glDeleteVertexArrays");
        _glBindVertexArray = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glBindVertexArray");

        // BufferObjects
        _glGenBuffers = (delegate* unmanaged[Cdecl]<int, uint*, void>)GetProcAddressPointer("glGenBuffers");
        _glDeleteBuffers = (delegate* unmanaged[Cdecl]<int, uint*, void>)GetProcAddressPointer("glDeleteBuffers");

        // CreateBindBuffersObjects
        _glBindBuffer = (delegate* unmanaged[Cdecl]<uint, uint, void>)GetProcAddressPointer("glBindBuffer");
        _glBindBufferBase = (delegate* unmanaged[Cdecl]<uint, uint, uint, void>)GetProcAddressPointer("glBindBufferBase");

        // CreateModifyBufferObjectData
        _glBufferData = (delegate* unmanaged[Cdecl]<uint, nuint, void*, uint, void>)GetProcAddressPointer("glBufferData");
        _glBufferSubData = (delegate* unmanaged[Cdecl]<uint, int, int, void*, void>)GetProcAddressPointer("glBufferSubData");

        // GenericVertexAttributeArrays
        _glVertexAttribPointer = (delegate* unmanaged[Cdecl]<uint, int, uint, bool, int, IntPtr, void>)GetProcAddressPointer("glVertexAttribPointer");
        _glEnableVertexAttribArray = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glEnableVertexAttribArray");

        // DrawingCommands
        _glDrawElements = (delegate* unmanaged[Cdecl]<uint, int, uint, IntPtr, void>)GetProcAddressPointer("glDrawElements");
        _glDrawElementsBaseVertex = (delegate* unmanaged[Cdecl]<uint, int, uint, IntPtr, int, void>)GetProcAddressPointer("glDrawElementsBaseVertex");

        // ControllingViewport
        _glViewport = (delegate* unmanaged[Cdecl]<int, int, int, int, void>)GetProcAddressPointer("glViewport");

        // Misc
        _glGetError = (delegate* unmanaged[Cdecl]<uint>)GetProcAddressPointer("glGetError");
        _glEnable = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glEnable");
        _glDisable = (delegate* unmanaged[Cdecl]<uint, void>)GetProcAddressPointer("glDisable");

        // ShaderProgramQueries
        _glGetShaderiv = (delegate* unmanaged[Cdecl]<uint, uint, int*, void>)GetProcAddressPointer("glGetShaderiv");
        _glGetProgramiv = (delegate* unmanaged[Cdecl]<uint, uint, int*, void>)GetProcAddressPointer("glGetProgramiv");
        _glGetShaderInfoLog  = (delegate* unmanaged[Cdecl]<uint, int, int*, byte*, void>)GetProcAddressPointer("glGetShaderInfoLog");
        _glGetProgramInfoLog = (delegate* unmanaged[Cdecl]<uint, int, int*, byte*, void>)GetProcAddressPointer("glGetProgramInfoLog");

        // CommandExecution
        _glFinish = (delegate* unmanaged[Cdecl]<void>)GetProcAddressPointer("glFinish");
    }

    private static nint GetProcAddressPointer(string name)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(name);
        fixed (byte* ptr = bytes)
        {
            return _getProcAddress(ptr);
        }
    }
}
