#version 330 core
layout (points) in;
layout (triangle_strip, max_vertices = 6) out;

in VSH {
    vec2 textureSize;
    vec2 charSize;
    int charIndex;

    vec2 windowSize;
    mat4 cameraMatrix;
    mat4 meshMatrix;

    bool enableLight;
} vsh[];

out GSH {
    vec2 texCoord;
    vec4 pos;
} gsh;

vec3 calcNormal(vec4 a, vec4 b, vec4 c) {
    vec4 temp1 = a - b;
    vec4 temp2 = b - c;

    // assume * represents cross-product
    vec3 normal = cross(temp1.xyz, temp2.xyz);  
    normalize(normal);

    return normal;
}

void Emit(vec2 texCoord, vec4 pos, mat4 matrix) {
    gsh.texCoord = texCoord;
    gsh.pos = matrix * pos;
    gl_Position = matrix * pos;
    EmitVertex();
}

void drawOnePoint(int i) {
    vec4 position =     gl_in[i].gl_Position;

    vec2 textureSize =  vsh[i].textureSize;
    vec2 charSize =     vsh[i].charSize;
    int charIndex =     vsh[i].charIndex;

    mat4 cameraMatrix = vsh[i].cameraMatrix;
    mat4 meshMatrix =   vsh[i].meshMatrix;

    int xCharCount = int(textureSize.x / charSize.x);
    int yCharCount = int(textureSize.y / charSize.y);

    bool enableLight = vsh[i].enableLight;

    float charX = mod(charIndex, xCharCount);
    float charY = floor(charIndex / float(xCharCount));


    float x = charSize.x / charSize.y * 2;
    float y = -2;

    float s =   charX *      charSize.x / textureSize.x;
    float t =   charY *      charSize.y / textureSize.y;
    float s1 = (charX + 1) * charSize.x / textureSize.x;
    float t1 = (charY + 1) * charSize.y / textureSize.y;

    mat4 matrix = cameraMatrix * meshMatrix;

    vec2 tca = vec2(s1, t1);
    vec2 tcb = vec2(s,  t1);
    vec2 tcc = vec2(s,  t);
    vec2 tcd = vec2(s1, t);

    vec4 a = position + vec4(x, y, 0, 0);
    vec4 b = position + vec4(0, y, 0, 0);
    vec4 c = position + vec4(0, 0, 0, 0);
    vec4 d = position + vec4(x, 0, 0, 0);

    Emit(tca, a, matrix);
    Emit(tcb, b, matrix);
    Emit(tcc, c, matrix);

    Emit(tca, a, matrix);
    Emit(tcd, d, matrix);
    Emit(tcc, c, matrix);

    EndPrimitive();
}


void main() {
    for(int i = 0; i < gl_in.length(); i++) {
        drawOnePoint(i);
    }
}