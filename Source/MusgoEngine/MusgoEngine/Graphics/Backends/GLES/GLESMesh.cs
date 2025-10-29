using MusgoEngine.Bindings.OpenGL;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESMesh : Mesh
{
    private readonly uint _vao;
    private readonly uint _vbo;
    private readonly uint _ebo;
    private readonly int _indexCount;

    public GLESMesh(string name, float[] vertices, uint[] indices) : base(name)
    {
        _indexCount = indices.Length;

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(GLBufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(GLBufferTarget.ArrayBuffer, vertices, GLBufferUsageHint.StaticDraw);

        GL.BindBuffer(GLBufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(GLBufferTarget.ElementArrayBuffer, indices, GLBufferUsageHint.StaticDraw);

        // Exemplo de layout: posição + cor (3+3 floats)
        GL.VertexAttribPointer(0, 3, GLVertexAttribPointerType.Float, false, 6 * sizeof(float), IntPtr.Zero);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, GLVertexAttribPointerType.Float, false, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
    }

    public override void Bind() => GL.BindVertexArray(_vao);

    public override void Unbind() => GL.BindVertexArray(0);

    public override void Draw() => GL.DrawElements(GLPrimitiveType.Triangles, _indexCount, GLDrawElementsType.UnsignedInt, IntPtr.Zero);

    public override void Destroy()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
    }
}

