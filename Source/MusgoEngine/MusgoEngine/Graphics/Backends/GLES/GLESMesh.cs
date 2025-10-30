using MusgoEngine.Bindings.OpenGL;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESMesh : Mesh
{
    private readonly uint _vao;
    private readonly uint _vbo;
    private readonly uint _ebo;
    private readonly int _indexCount;

    public GLESMesh(string name, MeshData data) : base(name, data)
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        var vertexBuffer = data.GetVertexBuffer();
        GL.BindBuffer(GLBufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(GLBufferTarget.ArrayBuffer, vertexBuffer, GLBufferUsageHint.StaticDraw);

        GL.BindBuffer(GLBufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(GLBufferTarget.ElementArrayBuffer, data.Indices, GLBufferUsageHint.StaticDraw);

        const int stride = 17 * sizeof(float);

        // pos3, normal3, tangent4, color3, uv0(2), uv1(2)
        GL.EnableVertexAttribArray(0); GL.VertexAttribPointer(0, 3, GLVertexAttribPointerType.Float, false, stride, 0);
        CheckGLError("VertexAttribPointer 0");

        GL.EnableVertexAttribArray(1); GL.VertexAttribPointer(1, 3, GLVertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        CheckGLError("VertexAttribPointer 1");

        GL.EnableVertexAttribArray(2); GL.VertexAttribPointer(2, 4, GLVertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
        CheckGLError("VertexAttribPointer 2");

        GL.EnableVertexAttribArray(3); GL.VertexAttribPointer(3, 3, GLVertexAttribPointerType.Float, false, stride, 10 * sizeof(float));
        CheckGLError("VertexAttribPointer 3");

        GL.EnableVertexAttribArray(4); GL.VertexAttribPointer(4, 2, GLVertexAttribPointerType.Float, false, stride, 13 * sizeof(float));
        CheckGLError("VertexAttribPointer 4");

        GL.EnableVertexAttribArray(5); GL.VertexAttribPointer(5, 2, GLVertexAttribPointerType.Float, false, stride, 15 * sizeof(float));
        CheckGLError("VertexAttribPointer 5");

        GL.BindVertexArray(0);
    }

    public override void Bind() => GL.BindVertexArray(_vao);

    public override void Unbind() => GL.BindVertexArray(0);

    public override void Draw()
    {
        foreach (var sub in Data.SubMeshes)
        {
            GL.DrawElements(
                GLPrimitiveType.Triangles,
                sub.IndexCount,
                GLDrawElementsType.UnsignedInt,
                (IntPtr)(sub.IndexStart * sizeof(uint))
            );
        }
    }

    public override void Destroy()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
    }

    private static void CheckGLError(string label)
    {
        var error = GL.GetError();
        if (error != GLErrorCode.NoError)
            Console.WriteLine($"[OpenGL Error] {label}: {error}");
    }
}

