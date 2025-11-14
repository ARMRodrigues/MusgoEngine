using System.Globalization;
using MusgoEngine.Math;

namespace MusgoEngine.Runtime.Desktop;

public class Test
{
    public static void RunTest()
    {
        var t = new Transform()
        {
            LocalEulerAngles = new Vector3(-60, 150, 0)
        };
        t.RebuildMatrices();

        /*// Teste 1: rotação zero
        t.LocalEulerAngles = new Vector3(0f, 0f, 0f);
        t.RebuildMatrices();
        PrintVectors("Zero rotation", t);

        // Teste 2: rotação 90° em Yaw (Y)
        t.LocalEulerAngles = new Vector3(0f, 90f, 0f);
        t.RebuildMatrices();
        PrintVectors("Yaw 90°", t);

        // Teste 3: rotação 90° em Pitch (X)
        t.LocalEulerAngles = new Vector3(90f, 0f, 0f);
        t.RebuildMatrices();
        PrintVectors("Pitch 90°", t);*/

        // Teste 4: rotação -60° Pitch, 150° Yaw (sua rotação atual)
        /*t.LocalEulerAngles = new Vector3(-60f, 150f, 0f);
        t.RebuildMatrices();
        PrintVectors("Pitch -60°, Yaw 150°", t);
        Console.WriteLine(t.LocalRotation);
        Console.WriteLine(t.LocalEulerAngles);*/

        PrintLocalMatrixDebug();
        PrintVectors("Pitch -60°, Yaw 150", t);
    }

    public static void PrintLocalMatrixDebug()
    {
        var pos = new Vector3();
        var euler = new Vector3(-60, 150, 0);
        var radians = euler * (MathF.PI / 180f);
        var rot = Quaternion.FromYawPitchRoll(radians.Y, radians.X, radians.Z);
        var scale = Vector3.One;

        var T = Matrix4.CreateTranslation(pos);
        var R = Matrix4.CreateFromQuaternion(rot);
        var S = Matrix4.CreateScale(scale);

        Console.WriteLine("Translation Matrix T:");
        PrintMatrix(T);
        Console.WriteLine("Rotation Matrix R:");
        PrintMatrix(R);
        Console.WriteLine("Scale Matrix S:");
        PrintMatrix(S);

        var local = T * R * S;
        Console.WriteLine("Local Matrix:");
        PrintMatrix(local);
    }

    static void PrintVectors(string label, Transform t)
    {
        Console.WriteLine($"\n=== {label} ===");
        Console.WriteLine($"Forward: {t.Forward}");
        Console.WriteLine($"Right:   {t.Right}");
        Console.WriteLine($"Up:      {t.Up}");
        Console.WriteLine("World Matrix:");
        PrintMatrix(t.WorldMatrix);
    }

    static void PrintMatrix(Matrix4 m)
    {
        Console.WriteLine($"[{m.M11.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M12.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M13.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M14.ToString("0.###", CultureInfo.InvariantCulture)}]");
        Console.WriteLine($"[{m.M21.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M22.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M23.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M24.ToString("0.###", CultureInfo.InvariantCulture)}]");
        Console.WriteLine($"[{m.M31.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M32.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M33.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M34.ToString("0.###", CultureInfo.InvariantCulture)}]");
        Console.WriteLine($"[{m.M41.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M42.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M43.ToString("0.###", CultureInfo.InvariantCulture)}, {m.M44.ToString("0.###", CultureInfo.InvariantCulture)}]");
    }

}
