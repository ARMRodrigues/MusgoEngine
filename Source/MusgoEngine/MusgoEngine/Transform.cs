using MusgoEngine.Math;

namespace MusgoEngine;

public class Transform : GameComponent
{
    private Vector3 _localPosition;
    private Vector3 _localRotation;
    private Vector3 _localScale = Vector3.One;
    private Quaternion _rotation = Quaternion.Identity;
    private Matrix4 _localMatrix = Matrix4.Identity;
    private Matrix4 _worldMatrix = Matrix4.Identity;

    public bool HasChanged { get; private set; }

    public Vector3 LocalPosition
    {
        get => _localPosition;
        set
        {
            _localPosition = value;
            HasChanged = true;
        }
    }

    public Vector3 Position => new(WorldMatrix.M41, WorldMatrix.M42, WorldMatrix.M43);

    public Vector3 LocalEulerAngles
    {
        get => _localRotation;
        set
        {
            _localRotation = value;
            var radians = value * (MathF.PI / 180f);
            //_rotation = Quaternion.FromYawPitchRoll(radians.Y, radians.X, radians.Z);
            _rotation = Quaternion.FromPitchYawRoll(radians.X, radians.Y, radians.Z);
            HasChanged = true;
        }
    }

    public Quaternion LocalRotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            _localRotation = RotationToEulerAngles(_rotation);
            HasChanged = true;
        }
    }

    public Vector3 LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            HasChanged = true;
        }
    }

    public Matrix4 LocalMatrix
    {
        get
        {
            _localMatrix =
                Matrix4.CreateTranslation(_localPosition) *
                Matrix4.CreateFromQuaternion(_rotation) *
                Matrix4.CreateScale(_localScale);
            return _localMatrix;
        }
    }

    public Matrix4 WorldMatrix
    {
        get => _worldMatrix;
        set
        {
            _worldMatrix = value;
            HasChanged = true;
        }
    }

    public Matrix4 RotationMatrix => Matrix4.CreateFromQuaternion(_rotation);
    public Vector3 Right => new Vector3(RotationMatrix.M11, RotationMatrix.M21, RotationMatrix.M31).Normalized();
    public Vector3 Up => new Vector3(RotationMatrix.M12, RotationMatrix.M22, RotationMatrix.M32).Normalized();
    public Vector3 Forward => new Vector3(RotationMatrix.M13, RotationMatrix.M23, RotationMatrix.M33).Normalized();

    public Transform(Vector3 position = default)
    {
        LocalPosition = position;
        RebuildMatrices();
    }

    public void LookAt(Vector3 targetPosition, Vector3 worldUp)
    {
        var direction = (targetPosition - Position).Normalized();

        if (direction == Vector3.Zero)
            direction = Vector3.Forward;

        var viewMatrix = Matrix4.CreateLookAt(Position, targetPosition, worldUp);
        LocalRotation = Quaternion.FromMatrix((viewMatrix));
    }

    public void RotateAround(Vector3 target, float angleDeg, Vector3 up)
    {
        var direction = Position - target;
        var rotation = Quaternion.FromAxisAngle(up.Normalized(), angleDeg.ToRadians());
        var rotatedDirection = Vector3.Transform(direction, rotation);

        LocalPosition = target + rotatedDirection;
        LocalRotation = rotation * LocalRotation;
    }

    public void RebuildMatrices()
    {
        _worldMatrix = LocalMatrix;
        HasChanged = false;
    }

    public static Vector3 RotationToEulerAngles(Quaternion q)
    {
        var pitch = MathF.Asin(2.0f * (q.W * q.X - q.Y * q.Z));
        var yaw   = MathF.Atan2(2.0f * (q.W * q.Y + q.Z * q.X), 1 - 2 * (q.X * q.X + q.Y * q.Y));
        var roll  = MathF.Atan2(2.0f * (q.W * q.Z + q.X * q.Y), 1 - 2 * (q.Y * q.Y + q.Z * q.Z));
        return new Vector3(pitch, yaw, roll) * (180f / MathF.PI);
    }
}
