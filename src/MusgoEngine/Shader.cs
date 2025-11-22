using System;
using MusgoEngine.Native.OpenGL;
using System.Numerics;

namespace MusgoEngine
{
    public class Shader(string vertexSrc, string fragmentSrc)
    {
        public uint Program { get; private set; } = CompileShader(vertexSrc, fragmentSrc);

        public void Use() => GL.UseProgram(Program);

        public void SetUniform(string name, Vector3 v)
        {
            var loc = GL.GetUniformLocation(Program, name);
            GL.Uniform3f(loc, v.X, v.Y, v.Z);
        }

        public void SetUniform(string name, float f)
        {
            var loc = GL.GetUniformLocation(Program, name);
            GL.Uniform1f(loc, f);
        }

        public void SetUniform(string name, Matrix4x4 m)
        {
            var loc = GL.GetUniformLocation(Program, name);
            GL.UniformMatrix4fv(loc, false, m);
        }

        public void Shutdown()
        {
            if (Program != 0) GL.DeleteProgram(Program);
        }

        private static uint CompileShader(string vertexSrc, string fragSrc)
        {
            var vertex = GL.CreateShader(GLShaderType.VertexShader);
            GL.ShaderSource(vertex, vertexSrc);
            GL.CompileShader(vertex);
            GL.GetShaderiv(vertex, GLShaderParameter.CompileStatus, out int status);
            if(status==0) throw new Exception(GL.GetShaderInfoLog(vertex));

            var frag = GL.CreateShader(GLShaderType.FragmentShader);
            GL.ShaderSource(frag, fragSrc);
            GL.CompileShader(frag);
            GL.GetShaderiv(frag, GLShaderParameter.CompileStatus, out status);
            if(status==0) throw new Exception(GL.GetShaderInfoLog(frag));

            var program = GL.CreateProgram();
            GL.AttachShader(program, vertex);
            GL.AttachShader(program, frag);
            GL.LinkProgram(program);
            GL.GetProgramiv(program, GLGetProgramParameterName.LinkStatus, out status);
            if(status==0) throw new Exception(GL.GetProgramInfoLog(program));

            GL.DeleteShader(vertex);
            GL.DeleteShader(frag);

            return program;
        }
    }
}

