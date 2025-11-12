namespace MusgoEngine.Math;

/// <summary>
/// Represents a quaternion used for 3D rotations and interpolations.
/// </summary>
public struct Quaternion : IEquatable<Quaternion>
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    /// <summary>
    /// The identity quaternion (no rotation).
    /// </summary>
    public static readonly Quaternion Identity = new(0f, 0f, 0f, 1f);

    /// <summary>
    /// Creates a new quaternion with the given components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component (scalar part).</param>
    public Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    #region Operators

    /// <summary>
    /// Multiplies two quaternions, combining their rotations.
    /// </summary>
    /// <param name="a">The first quaternion.</param>
    /// <param name="b">The second quaternion.</param>
    /// <returns>The resulting combined quaternion.</returns>
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
        );
    }

    /// <summary>
    /// Rotates a 3D vector by the given quaternion.
    /// </summary>
    /// <param name="q">The quaternion representing the rotation.</param>
    /// <param name="v">The vector to rotate.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector3 operator *(Quaternion q, Vector3 v)
    {
        var u = new Vector3(q.X, q.Y, q.Z);
        var s = q.W;

        return 2.0f * Vector3.Dot(u, v) * u
             + (s * s - Vector3.Dot(u, u)) * v
             + 2.0f * s * Vector3.Cross(u, v);
    }

    /// <summary>
    /// Compares two quaternions for near-equality.
    /// </summary>
    public static bool operator ==(Quaternion a, Quaternion b) => a.NearlyEquals(b);

    /// <summary>
    /// Compares two quaternions for inequality.
    /// </summary>
    public static bool operator !=(Quaternion a, Quaternion b) => !a.NearlyEquals(b);

    #endregion

    #region Math Helpers

    /// <summary>
    /// Normalizes this quaternion in place.
    /// </summary>
    public void Normalize()
    {
        var len = MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
        if (len > 0f)
        {
            X /= len;
            Y /= len;
            Z /= len;
            W /= len;
        }
    }

    /// <summary>
    /// Returns a normalized version of this quaternion.
    /// </summary>
    public Quaternion Normalized()
    {
        var len = MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
        return len > 0f ? new Quaternion(X / len, Y / len, Z / len, W / len) : Identity;
    }

    /// <summary>
    /// Returns the conjugate of this quaternion (negates the vector part).
    /// </summary>
    public Quaternion Conjugate() => new(-X, -Y, -Z, W);

    /// <summary>
    /// Returns the inverse of this quaternion.
    /// </summary>
    public Quaternion Inverse()
    {
        var conj = Conjugate();
        var lenSq = X * X + Y * Y + Z * Z + W * W;
        return lenSq > 0f ? new Quaternion(conj.X / lenSq, conj.Y / lenSq, conj.Z / lenSq, conj.W / lenSq) : Identity;
    }

    #endregion

    #region Conversions

    /// <summary>
    /// Creates a quaternion from an axis and an angle in radians.
    /// </summary>
    /// <param name="axis">The rotation axis (will be normalized internally).</param>
    /// <param name="angleRadians">The rotation angle in radians.</param>
    /// <returns>The resulting quaternion.</returns>
    public static Quaternion FromAxisAngle(Vector3 axis, float angleRadians)
    {
        axis = axis.Normalized();
        var half = angleRadians * 0.5f;
        var sin = MathF.Sin(half);
        var cos = MathF.Cos(half);
        return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, cos);
    }

    /// <summary>
    /// Creates a quaternion from yaw (Y), pitch (X), and roll (Z) angles in radians.
    /// </summary>
    /// <param name="yaw">Rotation around the Y axis.</param>
    /// <param name="pitch">Rotation around the X axis.</param>
    /// <param name="roll">Rotation around the Z axis.</param>
    /// <returns>The resulting quaternion.</returns>
    public static Quaternion FromYawPitchRoll(float yaw, float pitch, float roll)
    {
        var cy = MathF.Cos(yaw * 0.5f);
        var sy = MathF.Sin(yaw * 0.5f);
        var cp = MathF.Cos(pitch * 0.5f);
        var sp = MathF.Sin(pitch * 0.5f);
        var cr = MathF.Cos(roll * 0.5f);
        var sr = MathF.Sin(roll * 0.5f);

        return new Quaternion(
            sr * cp * cy - cr * sp * sy,
            cr * sp * cy + sr * cp * sy,
            cr * cp * sy - sr * sp * cy,
            cr * cp * cy + sr * sp * sy
        );
    }

    /// <summary>
    /// Converts this quaternion to a 3x3 rotation matrix.
    /// </summary>
    /// <returns>A rotation matrix representing this quaternion.</returns>
    public Matrix4 ToMatrix()
    {
        var xx = X * X;
        var yy = Y * Y;
        var zz = Z * Z;
        var xy = X * Y;
        var xz = X * Z;
        var yz = Y * Z;
        var wx = W * X;
        var wy = W * Y;
        var wz = W * Z;

        var m = Matrix4.Identity;

        m.M11 = 1f - 2f * (yy + zz);
        m.M12 = 2f * (xy - wz);
        m.M13 = 2f * (xz + wy);

        m.M21 = 2f * (xy + wz);
        m.M22 = 1f - 2f * (xx + zz);
        m.M23 = 2f * (yz - wx);

        m.M31 = 2f * (xz - wy);
        m.M32 = 2f * (yz + wx);
        m.M33 = 1f - 2f * (xx + yy);

        return m;
    }

    /// <summary>
    /// Creates a quaternion from a 3x3 rotation matrix.
    /// </summary>
    /// <param name="m">The input rotation matrix.</param>
    /// <returns>The resulting quaternion.</returns>
    public static Quaternion FromMatrix(in Matrix4 m)
    {
        var trace = m.M11 + m.M22 + m.M33;
        if (trace > 0f)
        {
            var s = MathF.Sqrt(trace + 1f) * 2f;
            var invS = 1f / s;
            return new Quaternion(
                (m.M32 - m.M23) * invS,
                (m.M13 - m.M31) * invS,
                (m.M21 - m.M12) * invS,
                0.25f * s
            );
        }
        else if (m.M11 > m.M22 && m.M11 > m.M33)
        {
            var s = MathF.Sqrt(1f + m.M11 - m.M22 - m.M33) * 2f;
            var invS = 1f / s;
            return new Quaternion(
                0.25f * s,
                (m.M12 + m.M21) * invS,
                (m.M13 + m.M31) * invS,
                (m.M32 - m.M23) * invS
            );
        }
        else if (m.M22 > m.M33)
        {
            var s = MathF.Sqrt(1f + m.M22 - m.M11 - m.M33) * 2f;
            var invS = 1f / s;
            return new Quaternion(
                (m.M12 + m.M21) * invS,
                0.25f * s,
                (m.M23 + m.M32) * invS,
                (m.M13 - m.M31) * invS
            );
        }
        else
        {
            var s = MathF.Sqrt(1f + m.M33 - m.M11 - m.M22) * 2f;
            var invS = 1f / s;
            return new Quaternion(
                (m.M13 + m.M31) * invS,
                (m.M23 + m.M32) * invS,
                0.25f * s,
                (m.M21 - m.M12) * invS
            );
        }
    }

    #endregion

    #region Equality

    private bool NearlyEquals(Quaternion other, float epsilon = 1e-5f)
        => MathF.Abs(X - other.X) < epsilon &&
           MathF.Abs(Y - other.Y) < epsilon &&
           MathF.Abs(Z - other.Z) < epsilon &&
           MathF.Abs(W - other.W) < epsilon;

    /// <summary>
    /// Determines whether this quaternion is equal to another quaternion.
    /// </summary>
    public bool Equals(Quaternion other)
        => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

    /// <summary>
    /// Determines whether this quaternion is equal to another object.
    /// </summary>
    public override bool Equals(object? obj) => obj is Quaternion q && Equals(q);

    /// <summary>
    /// Returns the hash code for this quaternion.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    #endregion

    #region Utility

    /// <summary>
    /// Returns a string representation of the quaternion.
    /// </summary>
    public override string ToString()
        => $"({X:0.###}, {Y:0.###}, {Z:0.###}, {W:0.###})";

    #endregion
}
