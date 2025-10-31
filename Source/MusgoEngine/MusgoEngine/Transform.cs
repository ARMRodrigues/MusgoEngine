using System.Numerics;

namespace MusgoEngine.Core;

public class Transform : GameComponent
{
    private Vector3 _localPosition;
    private Vector3 _localRotation;
    private Vector3 _localScale = Vector3.One;
    private Quaternion _rotation = Quaternion.Identity;
    private Matrix4x4 _localMatrix = Matrix4x4.Identity;
    private Matrix4x4 _worldMatrix = Matrix4x4.Identity;

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
            _rotation = Quaternion.CreateFromYawPitchRoll(radians.Y, radians.X, radians.Z);
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

    public Matrix4x4 LocalMatrix
    {
        get
        {
            _localMatrix =
                Matrix4x4.CreateScale(_localScale) *
                Matrix4x4.CreateFromQuaternion(_rotation) *
                Matrix4x4.CreateTranslation(_localPosition);
            return _localMatrix;
        }
    }

    public Matrix4x4 WorldMatrix
    {
        get => _worldMatrix;
        set
        {
            _worldMatrix = value;
            HasChanged = true;
        }
    }

    public Vector3 Right   => Vector3.Normalize(new(WorldMatrix.M11, WorldMatrix.M12, WorldMatrix.M13));
    public Vector3 Up      => Vector3.Normalize(new(WorldMatrix.M21, WorldMatrix.M22, WorldMatrix.M23));
    public Vector3 Forward => Vector3.Normalize(new Vector3(WorldMatrix.M31, WorldMatrix.M32, WorldMatrix.M33));

    public Transform(Vector3 position = default)
    {
        LocalPosition = position;
        RebuildMatrices();
    }

    public void LookAt(Vector3 targetPosition, Vector3 worldUp)
    {
        var direction = Vector3.Normalize(targetPosition - Position);

        if (direction == Vector3.Zero)
            direction = -Vector3.UnitZ;

        var viewMatrix = Matrix4x4.CreateLookAt(Position, targetPosition, worldUp);

        LocalRotation = Quaternion.CreateFromRotationMatrix(Matrix4x4.Transpose(viewMatrix));
    }

    public void RotateAround(Vector3 target, float angleDeg, Vector3 up)
    {
        // Passo 1: Direção atual em relação ao alvo
        var direction = Position - target;

        // Passo 2: Cria o quaternion de rotação
        var rotation = Quaternion.CreateFromAxisAngle(Vector3.Normalize(up), MathUtils.ToRadians(angleDeg));

        // Passo 3: Aplica a rotação
        var rotatedDirection = Vector3.Transform(direction, rotation);

        // Passo 4: Atualiza a posição local
        LocalPosition = target + rotatedDirection;

        // Opcional: rotacionar também o próprio transform (orientação)
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
