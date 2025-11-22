namespace MusgoEngine;

public class DefaultShader() : Shader(
    """
    #version 300 es
            precision highp float;

            layout(location = 0) in vec3 aPos;
            layout(location = 1) in vec3 aNormal;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 proj;

            out vec3 FragPos;
            out vec3 Normal;

            void main()
            {
                FragPos = vec3(model * vec4(aPos,1.0));
                Normal = mat3(transpose(inverse(model))) * aNormal;
                gl_Position = proj * view * vec4(FragPos,1.0);
            }

    """,
    """
    #version 300 es
            precision highp float;

            in vec3 FragPos;
            in vec3 Normal;

            uniform vec3 lightDir;
            uniform vec3 lightColor;
            uniform vec3 objectColor;

            // --- AO uniforms ---
            uniform vec3 aoColor;     // cor da sombra
            uniform float aoStrength; // intensidade 0..1

            out vec4 FragColor;

            void main()
            {
                // --- Luz difusa ---
                float NdotL = max(dot(normalize(Normal), -lightDir), 0.0);
                vec3 diffuse = NdotL * lightColor;

                // --- Cálculo base do AO (abertura da normal) ---
                float ao = dot(normalize(Normal), vec3(0.0, 1.0, 0.0));
                ao = clamp(ao, 0.0, 1.0);

                // Inverte e aplica força
                ao = 1.0 - ao;         // 1 = máxima oclusão
                ao *= 0.5;      // multiplica intensidade

                // Usa cor do AO
                vec3 aoTerm = vec3(0.6, 0.8, 0.1) * ao;

                // Ambient base
                vec3 ambient = vec3(0.12);

                // Resultado final
                vec3 result = (ambient + diffuse - aoTerm) * objectColor;

                FragColor = vec4(result, 1.0);
            }

    """);
