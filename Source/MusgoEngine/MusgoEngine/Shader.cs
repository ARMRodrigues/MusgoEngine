using System.Numerics;
using MusgoEngine.Math;

namespace MusgoEngine;

public abstract class Shader(string name)
{
    public string Name { get; } = name;

    public abstract void Bind();
    public abstract void Unbind();
    public abstract void SetUniform(string name, float value);
    public abstract void SetUniform(string name, in Matrix4 value, bool transpose = false);
    public abstract void SetUniform(string name, float[] value, bool transpose = false);
    public abstract void Destroy();
}
