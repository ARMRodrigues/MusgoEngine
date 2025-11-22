using System.Numerics;

namespace MusgoEngine;

class Chunk
{
    public List<CubeMesh> Cubes = new();
    public int Width = 16;
    public int Height = 16;
    public int Depth = 16;

    public void Generate(OpenSimplexNoise noise, uint shaderProgram)
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
        for (int z = 0; z < Depth; z++)
        {
            float heightValue = (float)(noise.Evaluate(x * 0.1, z * 0.1) * Height / 2 + Height / 2);
            if (y <= heightValue)
            {
                var cube = new CubeMesh(new Vector3(x, y, z), shaderProgram, 0f);
                cube.Start();
                Cubes.Add(cube);
            }
        }
    }
}
