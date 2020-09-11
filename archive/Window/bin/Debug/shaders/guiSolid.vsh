#version 330 core

// Vertex shader

//in vec4 inColor;
layout (location = 1) in vec4 inPosition;

out vec4 color;

void main(void)
{
    color = vec4(1, 1, 1, 1);

    gl_Position =  inPosition;
}