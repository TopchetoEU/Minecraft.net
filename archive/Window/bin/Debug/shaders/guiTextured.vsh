#version 330 core
in vec4 inPosition;
in vec2 inTexCoord;
in vec4 inColorisation;

out vec2 texCoord;
out vec4 colorisation;

void main(void)
{
    texCoord = inTexCoord;
    colorisation = inColorisation;

    gl_Position = vec4(inPosition.xy, 1.0, 0.0);
}