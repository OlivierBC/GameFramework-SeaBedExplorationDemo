#version 330

in vec3 fragNormal;

out vec4 finalColor;

uniform vec3 lightDir;
uniform vec3 terrainColor;

uniform float ambient;
uniform float shadowStrength;

void main()
{
    vec3 normal = normalize(fragNormal);
    vec3 light = normalize(lightDir);

    float diffuse =
        max(dot(normal, light), 0.0);

    float lighting =
        ambient +
        diffuse * (1.0 - ambient);

    lighting =
        mix(
            1.0,
            lighting,
            shadowStrength
        );

    finalColor = vec4(
        terrainColor * lighting,
        1.0
    );
}