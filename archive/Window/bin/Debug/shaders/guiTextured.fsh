#version 330

in vec2 texCoord;
out vec4 outputColor;


uniform sampler2D texture0;

int NO_TEST = 0;
int DISPLAY_TEXCOORD = 1;
int DISPLAY_TEXTURE = 2;

vec4 getErrorTexture(vec2 pos, float size) {
    vec4 purple = vec4(1.0, 0.0, 1.0, 1.0);
    vec4 black = vec4(0, 0, 0, 0);

    if (int((pos.x + pos.y) / size) % 2 == 0) {
        return black;
    } else {
        return purple;
    }
}

void main() {
    int test = DISPLAY_TEXCOORD;
    if (test == NO_TEST) {
        outputColor = texture(texture0, texCoord);
    } else if (test == DISPLAY_TEXCOORD) {
        outputColor = vec4(texCoord, 0, 1);
    } else if (test == DISPLAY_TEXTURE) {
        outputColor = texture(texture0, texCoord);
    } else {
        outputColor = getErrorTexture(texCoord, 10.0);
    }
}