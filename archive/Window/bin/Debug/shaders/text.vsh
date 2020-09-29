#version 330 core

in vec2 inPosition;
in float inCharacter;

out VSH {
    vec2 textureSize;
    vec2 charSize;
    int charIndex;

    vec2 windowSize;
    mat4 cameraMatrix;
    mat4 meshMatrix;

    bool enableLight;
} vsh;

uniform vec2 windowSize;
uniform mat4 cameraMatrix;
uniform mat4 viewMatrix;
uniform mat4 meshMatrix;

uniform vec2 textureSize;
uniform vec2 charSize;

uniform float enableLight;

void main() {
    gl_Position = vec4(inPosition, 0, 1.0);
    gl_PointSize = 3;

    vsh.charIndex = int(inCharacter);
    vsh.textureSize = textureSize;
    vsh.charSize = charSize;

    vsh.windowSize = windowSize;
    vsh.cameraMatrix = cameraMatrix * viewMatrix;
    vsh.meshMatrix = meshMatrix;

    if (enableLight > 0) {
        vsh.enableLight = true;
    }
    else {
        vsh.enableLight = false;
    }
}