namespace MusgoEngine.Math;

/// <summary>
/// 4x4 matrix used for 3D transformations such as translation, rotation, scaling, and projection.
/// Follows column-major order (OpenGL convention).
/// </summary>
public struct Matrix4 : IEquatable<Matrix4>
{
    public float M11, M12, M13, M14;
    public float M21, M22, M23, M24;
    public float M31, M32, M33, M34;
    public float M41, M42, M43, M44;

    public static readonly Matrix4 Identity = new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f
    );

    /// <summary>
    /// Creates a new matrix with the given component values.
    /// </summary>
    private Matrix4(
        float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        M11 = m11; M12 = m12; M13 = m13; M14 = m14;
        M21 = m21; M22 = m22; M23 = m23; M24 = m24;
        M31 = m31; M32 = m32; M33 = m33; M34 = m34;
        M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    #region Factory Methods

    /// <summary>Creates a translation matrix using the given position.</summary>
    public static Matrix4 CreateTranslation(Vector3 position)
    {
        var result = Identity;
        result.M41 = position.X;
        result.M42 = position.Y;
        result.M43 = position.Z;
        return result;
    }

    /// <summary>Creates a rotation matrix from a quaternion.</summary>
    public static Matrix4 CreateFromQuaternion(Quaternion q)
    {
        // Column-major vectors
        var right   = Vector3.Transform(Vector3.Right, q);
        var up      = Vector3.Transform(Vector3.Up, q);
        var forward = Vector3.Transform(Vector3.Forward, q);

        return new Matrix4(
            right.X, up.X, forward.X, 0f,
            right.Y, up.Y, forward.Y, 0f,
            right.Z, up.Z, forward.Z, 0f,
            0f,      0f,  0f,        1f
        );
    }

    /// <summary>Creates a scaling matrix using the given scale vector.</summary>
    public static Matrix4 CreateScale(Vector3 scale)
    {
        var result = Identity;
        result.M11 = scale.X;
        result.M22 = scale.Y;
        result.M33 = scale.Z;
        return result;
    }

    /// <summary>Creates a rotation matrix from yaw, pitch, and roll angles in degrees.</summary>
    public static Matrix4 CreateRotationYawPitchRoll(float yawDeg, float pitchDeg, float rollDeg)
    {
        var yaw = yawDeg.ToRadians();
        var pitch = pitchDeg.ToRadians();
        var roll = rollDeg.ToRadians();

        var cy = MathF.Cos(yaw);
        var sy = MathF.Sin(yaw);
        var cp = MathF.Cos(pitch);
        var sp = MathF.Sin(pitch);
        var cr = MathF.Cos(roll);
        var sr = MathF.Sin(roll);

        return new Matrix4(
            cy * cr + sy * sp * sr,   sr * cp,   -sy * cr + cy * sp * sr,   0f,
            -cy * sr + sy * sp * cr,  cr * cp,   sr * sy + cy * sp * cr,    0f,
            sy * cp,                 -sp,        cy * cp,                   0f,
            0f,                      0f,         0f,                        1f
        );
    }

    /// <summary>Creates a view matrix that looks from the eye position toward a target using the specified up vector.</summary>
    public static Matrix4 CreateLookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        var zaxis = (target - eye).Normalized();
        var xaxis = Vector3.Cross(up, zaxis).Normalized();
        var yaxis = Vector3.Cross(zaxis, xaxis);

        return new Matrix4(
            xaxis.X, yaxis.X, zaxis.X, 0f,
            xaxis.Y, yaxis.Y, zaxis.Y, 0f,
            xaxis.Z, yaxis.Z, zaxis.Z, 0f,
            -Vector3.Dot(xaxis, eye),
            -Vector3.Dot(yaxis, eye),
            -Vector3.Dot(zaxis, eye),
            1f
        );
    }

    /// <summary>Creates an orthographic projection matrix.</summary>
    public static Matrix4 CreateOrthographic(float width, float height, float near, float far)
    {
        var invDepth = 1f / (far - near);
        return new Matrix4(
            2f / width, 0f, 0f, 0f,
            0f, 2f / height, 0f, 0f,
            0f, 0f, -invDepth, 0f,
            0f, 0f, -near * invDepth, 1f
        );
    }

    /// <summary>Creates an orthographic projection matrix with explicit left, right, bottom, and top bounds.</summary>
    public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        var invRightLeft = 1f / (right - left);
        var invTopBottom = 1f / (top - bottom);
        var invFarNear = 1f / (far - near);

        return new Matrix4(
            2f * invRightLeft, 0f, 0f, 0f,
            0f, 2f * invTopBottom, 0f, 0f,
            0f, 0f, -invFarNear, 0f,
            -(right + left) * invRightLeft,
            -(top + bottom) * invTopBottom,
            -near * invFarNear,
            1f
        );
    }

    /// <summary>Creates a perspective projection matrix using the given field of view (in degrees), aspect ratio, and near/far planes.</summary>
    public static Matrix4 CreatePerspectiveFieldOfView(float fovYDeg, float aspect, float near, float far)
    {
        var fovY = fovYDeg.ToRadians();
        var f = 1f / MathF.Tan(fovY / 2f);
        var invNearFar = 1f / (near - far);

        return new Matrix4(
            f / aspect, 0f, 0f, 0f,
            0f, f, 0f, 0f,
            0f, 0f, (far + near) * invNearFar, -1f,
            0f, 0f, (2f * far * near) * invNearFar, 0f
        );
    }

    #endregion

    #region Transformations

    /// <summary>Returns the transposed version of this matrix.</summary>
    public Matrix4 Transpose()
    {
        return new Matrix4(
            M11, M21, M31, M41,
            M12, M22, M32, M42,
            M13, M23, M33, M43,
            M14, M24, M34, M44
        );
    }

    /// <summary>Returns the transposed version of the specified matrix.</summary>
    public static Matrix4 Transpose(Matrix4 m)
    {
        return new Matrix4(
            m.M11, m.M21, m.M31, m.M41,
            m.M12, m.M22, m.M32, m.M42,
            m.M13, m.M23, m.M33, m.M43,
            m.M14, m.M24, m.M34, m.M44
        );
    }

    #endregion

    #region Operators

    /// <summary>Multiplies two matrices.</summary>
    public static Matrix4 operator *(Matrix4 a, Matrix4 b)
    {
        Matrix4 r;
        r.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
        r.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
        r.M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
        r.M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;

        r.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
        r.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
        r.M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
        r.M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;

        r.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
        r.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
        r.M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
        r.M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;

        r.M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;
        r.M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;
        r.M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;
        r.M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;
        return r;
    }

    /// <summary>Transforms a 3D vector using the given matrix (assumes w = 1).</summary>
    public static Vector3 operator *(Matrix4 m, Vector3 v)
    {
        return new Vector3(
            v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41,
            v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42,
            v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43
        );
    }

    #endregion

    #region Utility

    /// <summary>Converts this matrix to a float array (column-major order).</summary>
    public float[] ToArray() =>
    [
        M11, M12, M13, M14,
        M21, M22, M23, M24,
        M31, M32, M33, M34,
        M41, M42, M43, M44
    ];

    public override string ToString()
        => $"[{M11:0.###}, {M12:0.###}, {M13:0.###}, {M14:0.###}]\n" +
           $"[{M21:0.###}, {M22:0.###}, {M23:0.###}, {M24:0.###}]\n" +
           $"[{M31:0.###}, {M32:0.###}, {M33:0.###}, {M34:0.###}]\n" +
           $"[{M41:0.###}, {M42:0.###}, {M43:0.###}, {M44:0.###}]";

    /// <summary>Checks if two matrices are nearly equal within a small epsilon tolerance.</summary>
    private bool NearlyEquals(Matrix4 other, float epsilon = 1e-5f)
    {
        return MathF.Abs(M11 - other.M11) < epsilon &&
               MathF.Abs(M22 - other.M22) < epsilon &&
               MathF.Abs(M33 - other.M33) < epsilon &&
               MathF.Abs(M44 - other.M44) < epsilon;
    }

    public override bool Equals(object? obj) => obj is Matrix4 m && Equals(m);

    /// <summary>Compares this matrix with another for exact equality.</summary>
    public bool Equals(Matrix4 other)
    {
        return M11.Equals(other.M11) && M12.Equals(other.M12) && M13.Equals(other.M13) && M14.Equals(other.M14) &&
               M21.Equals(other.M21) && M22.Equals(other.M22) && M23.Equals(other.M23) && M24.Equals(other.M24) &&
               M31.Equals(other.M31) && M32.Equals(other.M32) && M33.Equals(other.M33) && M34.Equals(other.M34) &&
               M41.Equals(other.M41) && M42.Equals(other.M42) && M43.Equals(other.M43) && M44.Equals(other.M44);
    }

    public override int GetHashCode()
    {
        var hash1 = HashCode.Combine(M11, M12, M13, M14, M21, M22, M23, M24);
        var hash2 = HashCode.Combine(M31, M32, M33, M34, M41, M42, M43, M44);
        return HashCode.Combine(hash1, hash2);
    }

    public static bool operator ==(Matrix4 left, Matrix4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Matrix4 left, Matrix4 right)
    {
        return !(left == right);
    }

    #endregion
}
