using System.Numerics;
using MusgoEngine.Bindings.OpenGL;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESShader : Shader
{
    private readonly uint _programId;
    private readonly Dictionary<string, int> _uniformLocations = new();

    public GLESShader(string name, string vertexSrc, string fragmentSrc)
        : base(name)
    {
        _programId = Compile(vertexSrc, fragmentSrc);
        CacheUniformLocations();
    }

    public override void Bind() => GL.UseProgram(_programId);

    public override void Unbind() => GL.UseProgram(0);

    private int GetLocation(string name)
    {
        if (_uniformLocations.TryGetValue(name, out var loc))
            return loc;

        var newLoc = GL.GetUniformLocation(_programId, name);
        _uniformLocations[name] = newLoc;
        return newLoc;
    }

    public override void SetUniform(string name, float value)
    {
        var location = GetLocation(name);
        if (location != -1)
            GL.Uniform1f(location, value);
    }

    public override void SetUniform(string name, in Matrix4x4 value, bool transpose = false)
    {
        var location = GetLocation(name);
        if (location != -1)
            GL.UniformMatrix4fv(location, transpose, MathUtils.ToOpenGLMatrixArray(value));
    }

    public override void SetUniform(string name, float[] value, bool transpose = false)
    {
        var location = GetLocation(name);
        if (location != -1)
            GL.UniformMatrix4fv(location, transpose, value);
    }

    private static uint Compile(string vertexSrc, string fragmentSrc)
    {
        var vertex = GL.CreateShader(GLShaderType.VertexShader);
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

        var frag = GL.CreateShader(GLShaderType.FragmentShader);
        GL.ShaderSource(frag, fragmentSrc);
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

        var program = GL.CreateProgram();
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

    private void CacheUniformLocations()
    {
        GL.GetProgramiv(_programId, GLGetProgramParameterName.ActiveUniforms, out int uniformCount);

        for (var i = 0u; i < uniformCount; i++)
        {
            var name = GL.GetActiveUniform(_programId, i, out _, out _);
            var location = GL.GetUniformLocation(_programId, name);
            _uniformLocations[name] = location;
        }
    }

    public override void Destroy()
    {
        GL.DeleteProgram(_programId);
    }
}
