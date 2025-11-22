using System.Numerics;
using MusgoEngine.Native.OpenGL;

namespace MusgoEngine;

public class CubeMesh
{
    private uint _vao, _vbo, _ebo;

    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public Vector3 Scale { get; set; } = Vector3.One;
    private uint _shaderProgram;

    public float RotateSpeed { get; }

    public CubeMesh(Vector3 position, uint shaderProgram, float rotateSpeed)
    {
        Position = position;
        Rotation = Vector3.Zero;

        _shaderProgram = shaderProgram;

        RotateSpeed = rotateSpeed;
    }

    public void Start()
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
    }

    public void Draw()
    {
        // Draw cube
        GL.BindVertexArray(_vao);
        GL.DrawElements(GLPrimitiveType.Triangles, 36, GLDrawElementsType.UnsignedInt, IntPtr.Zero);
        GL.BindVertexArray(0);
    }

    public void Shutdown()
    {
        // Delete buffers
        if (_ebo != 0) GL.DeleteBuffer(_ebo);
        if (_vbo != 0) GL.DeleteBuffer(_vbo);
        if (_vao != 0) GL.DeleteVertexArray(_vao);
    }
}
