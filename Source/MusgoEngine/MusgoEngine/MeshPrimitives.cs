using System.Numerics;

namespace MusgoEngine;

public static class MeshPrimitives
{
    public static MeshData CreateQuad()
    {
        var data = new MeshData();

        // Posições
        data.Positions.AddRange(new[] {
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3( 0.5f, -0.5f, 0f),
            new Vector3( 0.5f,  0.5f, 0f),
            new Vector3(-0.5f,  0.5f, 0f)
        });

        // Cores por vértice (RGB)
        data.Colors.AddRange(new[] {
            new Vector3(1f, 0f, 0f), // vermelho
            new Vector3(0f, 1f, 0f), // verde
            new Vector3(0f, 0f, 1f), // azul
            new Vector3(1f, 1f, 0f)  // amarelo
        });

        // Índices
        data.Indices = new uint[] { 0, 1, 2, 2, 3, 0 };

        // Submesh
        data.SubMeshes.Add(new SubMesh
        {
            IndexStart = 0,
            IndexCount = 6,
            MaterialIndex = 0,
        });

        return data;
    }

    public static MeshData CreateCube()
    {
        var data = new MeshData();

        // Posições
        data.Positions.AddRange(new[]
        {
            // Front face
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3( 0.5f, -0.5f, 0.5f),
            new Vector3( 0.5f,  0.5f, 0.5f),
            new Vector3(-0.5f,  0.5f, 0.5f),
            // Back face
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            // Left face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            // Right face
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            // Top face
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            // Bottom face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f)
        });

        // Normais
        data.Normals.AddRange(new[]
        {
            // Front
            new Vector3(0,0,1), new Vector3(0,0,1), new Vector3(0,0,1), new Vector3(0,0,1),
            // Back
            new Vector3(0,0,-1), new Vector3(0,0,-1), new Vector3(0,0,-1), new Vector3(0,0,-1),
            // Left
            new Vector3(-1,0,0), new Vector3(-1,0,0), new Vector3(-1,0,0), new Vector3(-1,0,0),
            // Right
            new Vector3(1,0,0), new Vector3(1,0,0), new Vector3(1,0,0), new Vector3(1,0,0),
            // Top
            new Vector3(0,1,0), new Vector3(0,1,0), new Vector3(0,1,0), new Vector3(0,1,0),
            // Bottom
            new Vector3(0,-1,0), new Vector3(0,-1,0), new Vector3(0,-1,0), new Vector3(0,-1,0)
        });

        // Tangentes (placeholder)
        for(int i=0; i<data.Positions.Count; i++)
            data.Tangents.Add(new Vector4(1,0,0,1));

        Vector3[] faceColors = {
            new Vector3(1,0,0), // Front - vermelho
            new Vector3(0,1,0), // Back - verde
            new Vector3(0,0,1), // Left - azul
            new Vector3(1,1,0), // Right - amarelo
            new Vector3(1,0,1), // Top - magenta
            new Vector3(0,1,1)  // Bottom - ciano
        };
        for (int f = 0; f < 6; f++)
        for (int v = 0; v < 4; v++)
            data.Colors.Add(faceColors[f]);


        // UV0
        for(int i=0; i<data.Positions.Count; i+=4)
        {
            data.UV0.Add(new Vector2(0,0));
            data.UV0.Add(new Vector2(1,0));
            data.UV0.Add(new Vector2(1,1));
            data.UV0.Add(new Vector2(0,1));
        }

        // UV1 (zeros)
        for(int i=0; i<data.Positions.Count; i++)
            data.UV1.Add(Vector2.Zero);

        // Índices
        data.Indices = new uint[]
        {
            0,1,2, 0,2,3,       // Front
            4,5,6, 4,6,7,       // Back
            8,9,10, 8,10,11,    // Left
            12,13,14, 12,14,15, // Right
            16,17,18, 16,18,19, // Top
            20,21,22, 20,22,23  // Bottom
        };

        // Submesh
        data.SubMeshes.Add(new SubMesh
        {
            IndexStart = 0,
            IndexCount = data.Indices.Length,
            MaterialIndex = 0
        });

        return data;
    }
}
