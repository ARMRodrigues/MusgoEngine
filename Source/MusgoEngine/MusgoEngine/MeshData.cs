using System.Numerics;

namespace MusgoEngine;

public class MeshData
{
    public List<Vector3> Positions = new();
    public List<Vector3> Normals = new();
    public List<Vector4> Tangents = new();
    public List<Vector3> Colors = new();
    public List<Vector2> UV0 = new();
    public List<Vector2> UV1 = new();
    public uint[] Indices = Array.Empty<uint>();
    public List<SubMesh> SubMeshes = new();

    public float[] GetVertexBuffer()
    {
        int vertexCount = Positions.Count;
        // pos3 + normal3 + tangent4 + color3 + uv0(2) + uv1(2) = 17 floats
        var buffer = new float[vertexCount * 17];

        for (int i = 0; i < vertexCount; i++)
        {
            int offset = i * 17;
            buffer[offset + 0] = Positions[i].X;
            buffer[offset + 1] = Positions[i].Y;
            buffer[offset + 2] = Positions[i].Z;

            buffer[offset + 3] = Normals.Count > i ? Normals[i].X : 0f;
            buffer[offset + 4] = Normals.Count > i ? Normals[i].Y : 0f;
            buffer[offset + 5] = Normals.Count > i ? Normals[i].Z : 0f;

            buffer[offset + 6] = Tangents.Count > i ? Tangents[i].X : 0f;
            buffer[offset + 7] = Tangents.Count > i ? Tangents[i].Y : 0f;
            buffer[offset + 8] = Tangents.Count > i ? Tangents[i].Z : 0f;
            buffer[offset + 9] = Tangents.Count > i ? Tangents[i].W : 0f;

            buffer[offset + 10] = Colors.Count > i ? Colors[i].X : 1f;
            buffer[offset + 11] = Colors.Count > i ? Colors[i].Y : 1f;
            buffer[offset + 12] = Colors.Count > i ? Colors[i].Z : 1f;

            buffer[offset + 13] = UV0.Count > i ? UV0[i].X : 0f;
            buffer[offset + 14] = UV0.Count > i ? UV0[i].Y : 0f;

            buffer[offset + 15] = UV1.Count > i ? UV1[i].X : 0f;
            buffer[offset + 16] = UV1.Count > i ? UV1[i].Y : 0f;
        }

        return buffer;
    }
}
