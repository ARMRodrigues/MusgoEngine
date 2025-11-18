using System.Runtime.InteropServices;

namespace MusgoEngine.Native.OpenGL;

public static unsafe partial class GL
{
    private static unsafe delegate* unmanaged[Cdecl]<uint, uint, int*, void> _glGetShaderiv;
    private static unsafe delegate* unmanaged[Cdecl]<uint, uint, int*, void> _glGetProgramiv;
    private static unsafe delegate* unmanaged[Cdecl]<uint, int, int*, byte*, void> _glGetShaderInfoLog;
    private static unsafe delegate* unmanaged[Cdecl]<uint, int, int*, byte*, void> _glGetProgramInfoLog;

    public static unsafe void GetShaderiv(uint shader, uint parameterName, out int value)
    {
        fixed (int* p = &value)
            _glGetShaderiv(shader, parameterName, p);
    }

    public static unsafe void GetShaderiv(uint shader, GLShaderParameter parameterName, out int value)
    {
        fixed (int* p = &value)
            _glGetShaderiv(shader, (uint)parameterName, p);
    }

    public static unsafe void GetProgramiv(uint program, uint programName, out int value)
    {
        fixed (int* p = &value)
            _glGetProgramiv(program, programName, p);
    }

    public static unsafe void GetProgramiv(uint program, GLGetProgramParameterName programParameterNamename, out int value)
    {
        fixed (int* p = &value)
            _glGetProgramiv(program, (uint)programParameterNamename, p);
    }

    public static unsafe string GetShaderInfoLog(uint shader)
    {
        GetShaderiv(shader, GLShaderParameter.InfoLogLength, out var length);
        if (length == 0)
            return string.Empty;

        var buffer = stackalloc byte[length];
        _glGetShaderInfoLog(shader, length, null, buffer);
        return Marshal.PtrToStringAnsi((IntPtr)buffer) ?? string.Empty;
    }

    public static unsafe string GetProgramInfoLog(uint program)
    {
        GetProgramiv(program, GLGetProgramParameterName.InfoLogLength, out var length);
        if (length == 0)
            return string.Empty;

        var buffer = stackalloc byte[length];
        _glGetProgramInfoLog(program, length, null, buffer);
        return Marshal.PtrToStringAnsi((IntPtr)buffer) ?? string.Empty;
    }

}
