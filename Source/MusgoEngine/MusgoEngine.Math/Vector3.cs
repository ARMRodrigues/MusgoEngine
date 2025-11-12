namespace MusgoEngine.Math;

/// <summary>
/// 3D vector with X, Y, Z components. Follows GLM/OpenGL conventions (right-handed, forward = -Z).
/// </summary>
public struct Vector3(float x, float y, float z) : IEquatable<Vector3>
{
    /// <summary>X component of the vector.</summary>
    public float X = x;

    /// <summary>Y component of the vector.</summary>
    public float Y = y;

    /// <summary>Z component of the vector.</summary>
    public float Z = z;

    public static readonly Vector3 Zero = new(0f, 0f, 0f);
    public static readonly Vector3 One = new(1f, 1f, 1f);
    public static readonly Vector3 Up = new(0f, 1f, 0f);
    public static readonly Vector3 Down = new(0f, -1f, 0f);
    public static readonly Vector3 Left = new(-1f, 0f, 0f);
    public static readonly Vector3 Right = new(1f, 0f, 0f);
    public static readonly Vector3 Forward = new(0f, 0f, -1f);
    public static readonly Vector3 Backward = new(0f, 0f, 1f);

    /// <summary>Constructs a vector with all components equal to the specified value.</summary>
    public Vector3(float value) : this(value, value, value) { }

    #region Operators
    /// <summary>Adds two vectors component-wise.</summary>
    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    /// <summary>Subtracts two vectors component-wise.</summary>
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    /// <summary>Negates the vector.</summary>
    public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);

    /// <summary>Multiplies vector by scalar.</summary>
    public static Vector3 operator *(Vector3 v, float s) => new(v.X * s, v.Y * s, v.Z * s);

    /// <summary>Multiplies vector by scalar (commutative).</summary>
    public static Vector3 operator *(float s, Vector3 v) => v * s;

    /// <summary>Divides vector by scalar.</summary>
    public static Vector3 operator /(Vector3 v, float s) => new(v.X / s, v.Y / s, v.Z / s);

    /// <summary>Returns true if vectors are nearly equal.</summary>
    public static bool operator ==(Vector3 a, Vector3 b) => a.NearlyEquals(b);

    /// <summary>Returns true if vectors are not nearly equal.</summary>
    public static bool operator !=(Vector3 a, Vector3 b) => !a.NearlyEquals(b);
    #endregion

    #region Transform
    /// <summary>Transforms a vector by a 4x4 matrix (GLM/OpenGL convention).</summary>
    public static Vector3 Transform(Vector3 v, Matrix4 m)
    {
        return new Vector3(
            v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41,
            v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42,
            v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43
        );
    }

    /// <summary>Rotates a vector by a quaternion.</summary>
    public static Vector3 Transform(Vector3 v, Quaternion q)
    {
        var u = new Vector3(q.X, q.Y, q.Z);
        var s = q.W;
        return 2f * Dot(u, v) * u + (s * s - Dot(u, u)) * v + 2f * s * Cross(u, v);
    }
    #endregion

    #region Math Helpers
    /// <summary>Returns the length (magnitude) of the vector.</summary>
    public float Length() => MathF.Sqrt(X * X + Y * Y + Z * Z);

    /// <summary>Returns the squared length of the vector.</summary>
    public float LengthSquared() => X * X + Y * Y + Z * Z;

    /// <summary>Normalizes the vector in place.</summary>
    public void Normalize() { var len = Length(); if (len > 0f) { X /= len; Y /= len; Z /= len; } }

    /// <summary>Returns a normalized copy of the vector.</summary>
    public Vector3 Normalized() { var len = Length(); return len > 0f ? this / len : Zero; }

    /// <summary>Returns the dot product of two vectors.</summary>
    public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    /// <summary>Returns the cross product of two vectors.</summary>
    public static Vector3 Cross(Vector3 a, Vector3 b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );

    /// <summary>Returns the distance between two vectors.</summary>
    public static float Distance(Vector3 a, Vector3 b) => (a - b).Length();

    /// <summary>Performs linear interpolation between two vectors.</summary>
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;
    #endregion

    #region Utility
    /// <summary>Returns true if vectors are nearly equal within a small epsilon.</summary>
    private bool NearlyEquals(Vector3 other, float epsilon = 1e-5f)
        => MathF.Abs(X - other.X) < epsilon &&
           MathF.Abs(Y - other.Y) < epsilon &&
           MathF.Abs(Z - other.Z) < epsilon;

    /// <summary>Exact equality check (object override).</summary>
    public override bool Equals(object? obj) => obj is Vector3 v && this == v;

    /// <summary>Exact equality check (IEquatable).</summary>
    public bool Equals(Vector3 other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

    /// <summary>Returns the hash code for the vector.</summary>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <summary>Returns a string representation of the vector.</summary>
    public override string ToString() => $"({X:0.###}, {Y:0.###}, {Z:0.###})";

    #endregion
}
