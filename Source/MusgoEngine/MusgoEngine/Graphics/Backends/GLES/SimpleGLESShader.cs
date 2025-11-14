namespace MusgoEngine.Graphics.Backends.GLES;

public class SimpleGLESShader : GLESShader
{
    public SimpleGLESShader() : base("SimpleEGLShader",
        // Vertex Shader
        @"#version 300 es
precision highp float;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 3) in vec4 aColor;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec3 vFragPos;
out vec3 vNormal;
out vec4 vColor;

void main()
{
    vec4 worldPos = uModel * vec4(aPosition, 1.0);
    vFragPos = worldPos.xyz;
    vNormal = mat3(transpose(inverse(uModel))) * aNormal;
    vColor = aColor;

    gl_Position = uProjection * uView * worldPos;
}",
        // Fragment Shader
        @"#version 300 es
precision highp float;

in vec3 vFragPos;
in vec3 vNormal;
in vec4 vColor;

out vec4 FragColor;

layout(std140) uniform SceneUniforms
{
    vec4 uLightDirection;
    vec4 uLightColor;
    vec4 uAmbientColor;
};

void main()
{
    vec3 N = normalize(vNormal);
    //vec3 L = normalize(-uLightDirection.xyz); // inverter direção da luz
    vec3 L = normalize(-vec3(-0.25, -0.866, 0.433));

    float diff = max(dot(N, L), 0.0);

    vec3 diffuse = uLightColor.rgb * diff;
    vec3 ambient = uAmbientColor.rgb;

    vec3 result = (diffuse + ambient) * vColor.rgb;
    //FragColor = vec4(result, 1.0);
    FragColor = vec4(vec3(diff), 1.0);
}")
    {
    }
}
