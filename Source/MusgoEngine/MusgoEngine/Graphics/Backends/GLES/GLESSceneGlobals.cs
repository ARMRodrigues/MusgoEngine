
using MusgoEngine.Bindings.OpenGL;
using MusgoEngine.Math;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESSceneGlobals : ISceneGlobals
{
    private uint _uboHandle;

    public void Initialize()
    {
        _uboHandle = GL.GenBuffer();
        GL.BindBuffer(GLBufferTarget.UniformBuffer, _uboHandle);

        var emptyBuffer = new float[12];
        GL.BufferData(GLBufferTarget.UniformBuffer, emptyBuffer, GLBufferUsageHint.DynamicDraw);

        GL.BindBufferBase(GLBufferRangeTarget.UniformBuffer, 0, _uboHandle);

        GL.BindBuffer(GLBufferTarget.UniformBuffer, 0);
    }

    public void Update(SceneEnvironment sceneEnvironment, Vector3 directionLight)
    {
        GL.BindBuffer(GLBufferTarget.UniformBuffer, _uboHandle);

        var buffer = ToFloatArray(sceneEnvironment, directionLight);

        GL.BufferSubData(GLBufferTarget.UniformBuffer, 0, buffer);

        GL.BindBuffer(GLBufferTarget.UniformBuffer, 0);
    }

    public void Shutdown()
    {
        if (_uboHandle == 0) return;
        GL.DeleteBuffer(_uboHandle);
        _uboHandle = 0;
    }

    private static float[] ToFloatArray(SceneEnvironment env, Vector3 directionLight)
    {
        return
        [
            //directionLight.X, directionLight.Y, directionLight.Z, 0f,
            0.250f, -0.866f,  0.433f, 0f,
            env.MainLight.Color.R * env.MainLight.Intensity,
            env.MainLight.Color.G * env.MainLight.Intensity,
            env.MainLight.Color.B * env.MainLight.Intensity,
            0f,
            env.AmbientColor.X, env.AmbientColor.Y, env.AmbientColor.Z, 0f
        ];
    }
}
