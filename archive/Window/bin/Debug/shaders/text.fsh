#version 330 core

in GSH {
    vec2 texCoord;
    vec4 pos;
} gsh;

out vec4 FragColor;

uniform sampler2D texture0;

float calculateAltAlpha(vec4 color) {
    if (color.r == color.g && color.g == color.b) {
        return color.r;
    } 
    else if (color.g > 0) {
        return color.g;
    }
    else {
        return 0;
    }
}

void main() {
    vec4 color = texture(texture0, gsh.texCoord);
    float alpha = calculateAltAlpha(color);

    FragColor = vec4(1, 1, 1, alpha);
}