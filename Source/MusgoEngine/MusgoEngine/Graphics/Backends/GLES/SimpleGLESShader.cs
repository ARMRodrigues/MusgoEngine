namespace MusgoEngine.Graphics.Backends.GLES;

public class SimpleGLESShader : GLESShader
{
    public SimpleGLESShader() : base("SimpleEGLShader",
        // Vertex Shader
        @"#version 300 es
            precision highp float;
            layout(location = 0) in vec3 aPosition;
            layout(location = 3) in vec3 aColor;

            uniform mat4 uModel;
            uniform mat4 uView;
            uniform mat4 uProjection;

            out vec3 vColor;

            void main()
            {
                vColor = aColor;
                gl_Position = uProjection * uView * uModel * vec4(aPosition, 1.0);
            }",
        // Fragment Shader
        @"#version 300 es
            precision highp float;
            in vec3 vColor;
            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(vColor, 1.0);
            }")
    {
    }
}
