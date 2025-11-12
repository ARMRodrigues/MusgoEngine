using MusgoEngine.Math;

namespace MusgoEngine;

public enum CameraType
{
    Perspective,
    Orthographic
}

public class Camera : GameComponent
{
    private float _fov = 60f;
    private float _near = 0.1f;
    private float _far = 1000f;
    private float _aspect = 16f / 9f;
    private float _orthoSize = 5f;
    private CameraType _type = CameraType.Perspective;

    public bool HasChanged { get; private set; } = true;

    public CameraType Type
    {
        get => _type;
        set { _type = value; HasChanged = true;  }
    }

    public float FieldOfViewDegrees
    {
        get => _fov;
        set { _fov = value; HasChanged = true;  }
    }

    public float NearPlane
    {
        get => _near;
        set {_near = value; HasChanged = true; }
    }

    public float FarPlane
    {
        get => _far;
        set { _far = value; HasChanged = true; }
    }

    public float Aspect
    {
        get => _aspect;
        set { _aspect = value; HasChanged = true; }
    }

    public float OrthographicSize
    {
        get => _orthoSize;
        set { _orthoSize = value; HasChanged = true; }
    }

    private Matrix4 _view = Matrix4.Identity;
    public Matrix4 View
    {
        get => _view;
        set { _view = value; HasChanged = false; }
    }

    private Matrix4 _projection = Matrix4.Identity;
    public Matrix4 Projection
    {
        get => _projection;
        set { _projection = value; HasChanged = false; }
    }
}
