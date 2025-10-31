using MusgoEngine.Bindings.OpenGL;

namespace MusgoEngine.Graphics.Backends.GLES;

public class GLESSceneGlobals : ISceneGlobals
{
    private uint _uboHandle;

    public void Initialize()
    {
        _uboHandle = GL.GenBuffer();
        GL.BindBuffer(GLBufferTarget.UniformBuffer, _uboHandle);

        var emptyBuffer = new float[16];
        GL.BufferData(GLBufferTarget.UniformBuffer, emptyBuffer, GLBufferUsageHint.DynamicDraw);

        GL.BindBufferBase(GLBufferRangeTarget.UniformBuffer, 0, _uboHandle);

        GL.BindBuffer(GLBufferTarget.UniformBuffer, 0);
    }

    public void Update(Scene scene)
    {
        GL.BindBuffer(GLBufferTarget.UniformBuffer, _uboHandle);

        var buffer = ToFloatArray(scene.SceneEnvironment);

        GL.BufferSubData(GLBufferTarget.UniformBuffer, 0, buffer);

        GL.BindBuffer(GLBufferTarget.UniformBuffer, 0);
    }

    public void Shutdown()
    {
        if (_uboHandle == 0) return;
        GL.DeleteBuffer(_uboHandle);
        _uboHandle = 0;
    }

    private static float[] ToFloatArray(SceneEnvironment env)
    {
        // Vetor de floats com padding para std140
        return
        [
            env.MainLight.Direction.X, env.MainLight.Direction.Y, env.MainLight.Direction.Z, 0f,
            env.MainLight.Color.R * env.MainLight.Intensity,
            env.MainLight.Color.G * env.MainLight.Intensity,
            env.MainLight.Color.B * env.MainLight.Intensity,
            0f,
            env.AmbientColor.X, env.AmbientColor.Y, env.AmbientColor.Z, 0f
        ];
    }
}
