namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESMaterial(Shader shader) : Material(shader)
{
    public override void Bind()
    {
        Shader.Bind();
    }

    public override void Unbind() => Shader.Unbind();

    public override void Destroy()
    {
        Shader.Destroy();
    }
}

