using System.Numerics;

namespace MusgoEngine;

public class Transform : GameComponent
{
    public Vector3 LocalPosition { get; set; } = Vector3.Zero;
    public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
    public Vector3 LocalScale { get; set; } = Vector3.One;

    public Matrix4x4 LocalMatrix { get; private set; } = Matrix4x4.Identity;
    public ref readonly Matrix4x4 WorldMatrix => ref _worldMatrix;
    private Matrix4x4 _worldMatrix = Matrix4x4.Identity;
    public bool HasChanged { get; private set; } = true;

    public void SetLocalPosition(Vector3 p)
    {
        LocalPosition = p;
        HasChanged = true;
    }

    public void SetLocalRotationEuler(Vector3 eulerDeg)
    {
        var radians = new Vector3(
            MathF.PI / 180f * eulerDeg.X,
            MathF.PI / 180f * eulerDeg.Y,
            MathF.PI / 180f * eulerDeg.Z
        );
        LocalRotation = Quaternion.CreateFromYawPitchRoll(radians.Y, radians.X, radians.Z);
        HasChanged = true;
    }

    public void SetLocalRotationQuat(Quaternion q)
    {
        LocalRotation = q;
        HasChanged = true;
    }

    public void SetLocalScale(Vector3 s)
    {
        LocalScale = s;
        HasChanged = true;
    }

    internal void SetWorldMatrix(Matrix4x4 world)
    {
        _worldMatrix = world;
    }

    public void RebuildLocalMatrix()
    {
        LocalMatrix =
            Matrix4x4.CreateScale(LocalScale) *
            Matrix4x4.CreateFromQuaternion(LocalRotation) *
            Matrix4x4.CreateTranslation(LocalPosition);
        _worldMatrix = LocalMatrix;
        HasChanged = false;
    }

    public Vector3 Right => Vector3.Normalize(new Vector3(WorldMatrix.M11, WorldMatrix.M12, WorldMatrix.M13));
    public Vector3 Up => Vector3.Normalize(new Vector3(WorldMatrix.M21, WorldMatrix.M22, WorldMatrix.M23));
    public Vector3 Forward => Vector3.Normalize(-new Vector3(WorldMatrix.M31, WorldMatrix.M32, WorldMatrix.M33));
    public Vector3 Position => new(WorldMatrix.M41, WorldMatrix.M42, WorldMatrix.M43);

    public void LookAt(Vector3 target, Vector3 worldUp)
    {
        var pos = Position;
        var zAxis = Vector3.Normalize(pos - target);
        var xAxis = Vector3.Normalize(Vector3.Cross(worldUp, zAxis));
        var yAxis = Vector3.Cross(zAxis, xAxis);

        var rotMatrix = new Matrix4x4(
            xAxis.X, xAxis.Y, xAxis.Z, 0,
            yAxis.X, yAxis.Y, yAxis.Z, 0,
            zAxis.X, zAxis.Y, zAxis.Z, 0,
            0, 0, 0, 1
        );

        LocalRotation = Quaternion.CreateFromRotationMatrix(rotMatrix);
        HasChanged = true;
    }
}

