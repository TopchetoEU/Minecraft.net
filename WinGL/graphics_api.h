#pragma once

#define API

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif


#include <list>
#include <iostream>
#include <map>

using std::list;
using std::map;

using std::string;

struct attribute {
public:
	uint id;
	uint byteSize;
	uint type;
	uint stride;
	object arrayOffset;

	attribute(uint id, uint size, uint type, uint stride, object offset) {
		this->id = id;
		this->byteSize = size;
		this->type = type;
		this->stride = stride;
		this->arrayOffset = offset;
	}
};
struct buffer {
public:
	uint type;
	uint id;

	buffer(uint type, uint id) {
		this->id = id;
		this->type = type;
	}
};
struct vao {
public:
	uint id;
	map<uint, attribute*> attributes = map<uint, attribute*>();

	vao(uint id) {
		this->id = id;
	}
};
struct shader {
public:
	uint id;
	std::string source;

	list<uint>* correlatedPrograms = new list<uint>();

	string error;
	bool buildSuccessfull = false;

	shader(uint id, string source) {
		this->id = id;
		this->source = source;
	}
};
struct shaderProgram {
public:
	uint id;
	list<uint>* shaders = new list<uint>();

	shaderProgram(uint id) {
		this->id = id;
	}
};

class graphics {
public:
	map<uint, uint> bountBuffers = map<uint, uint>();
	map<uint, buffer*> buffers = map<uint, buffer*>();
	map<uint, vao*> vaos = map<uint, vao*>();
	map<uint, void*> nativeArrays = map<uint, void*>();
	map<uint, shader*> shaders = map<uint, shader*>();
	map<uint, shaderProgram*> shaderPrograms = map<uint, shaderProgram*>();

	uint selectedShaderProgram = 0;

	uint nextArray = 1;

	uint bountVAO = 0;
	bool g_initialised = false;

	float backR, backG, backB, backA;

	graphics();
};

void setCurrContext(graphics* grph);

extern "C" API uint graphics_createVAO();
extern "C" API void graphics_destroyVAO(uint vaoId);

extern "C" API uint graphics_createBuffer(uint target);
extern "C" API void graphics_destroyBuffer(uint buff);

extern "C" API void graphics_setBuffer(uint target, uint buff);
extern "C" API uint graphics_getBuffer(uint target);

extern "C" API void graphics_setBufferData(uint target, uint size, uint dataArr, uint usage);
extern "C" API void graphics_getBufferData(uint target, uint size, object data);

extern "C" API void graphics_clearBufferAttributes(uint buff);
extern "C" API void graphics_createBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, uint offset);
extern "C" API void graphics_destroyBufferAttribute(uint buff, uint attrId);

extern "C" API void graphics_setVAO(uint vaoId);
extern "C" API uint graphics_getVAO();

extern "C" API void graphics_drawBuffer(uint vaoId, uint buff, uint amount, uint mode);
extern "C" API void graphics_drawElement(uint indicieType, uint indicieCount, uint eboLength, uint ebo, uint vbo);

extern "C" API uint graphics_loadNativeArray(void* arr, uint size);
extern "C" API uint graphics_createNativeArray(uint size);
extern "C" API void graphics_destroyNativeArray(uint arr);

extern "C" API uint graphics_createShader(uint type, const char* raw, int length);
extern "C" API void graphics_destroyShader(uint shader);

extern "C" API bool graphics_getShaderBuildSuccess(uint shaderId);
extern "C" API nativeString graphics_getShaderInfoLog(uint shaderId);
extern "C" API string graphics_getShaderSource(uint shaderId);

extern "C" API void graphics_detachShaderFromProgram(uint program, uint shader);
extern "C" API void graphics_attachShaderToProgram(uint program, uint shader);

extern "C" API uint graphics_createShaderProgram();
extern "C" API void graphics_compileShaderProgram(uint program);
extern "C" API void graphics_destroyShaderProgram(uint program);
extern "C" API uint graphics_loadShaderProgram(uint * shaders, uint count);

extern "C" API void graphics_setShaderProgram(uint program);
extern "C" API uint graphics_getShaderProgram();

extern "C" API void graphics_setBackground(float r, float g, float b, float a);
extern "C" API void graphics_getBackground(float* r, float* g, float* b, float* a);
extern "C" API void graphics_clear(uint type);

extern "C" API uint graphics_getUniformLocation(uint program, char* name);
extern "C" API uint graphics_getAttribLocation(uint program, char* name);

extern "C" API void graphics_setUniformVec2(uint id, float x, float y);
extern "C" API void graphics_setUniformVec3(uint id, float x, float y, float z);
extern "C" API void graphics_setUniformVec4(uint id, float x, float y, float z, float w);

extern "C" API void graphics_setUniformdVec2(uint id, double x, double y);
extern "C" API void graphics_setUniformdVec3(uint id, double x, double y, double z);
extern "C" API void graphics_setUniformdVec4(uint id, double x, double y, double z, double w);

extern "C" API void graphics_setUniformiVec2(uint id, int x, int y);
extern "C" API void graphics_setUniformiVec3(uint id, int x, int y, int z);
extern "C" API void graphics_setUniformiVec4(uint id, int x, int y, int z, int w);

extern "C" API void graphics_setUniformbVec2(uint id, bool x, bool y);
extern "C" API void graphics_setUniformbVec3(uint id, bool x, bool y, bool z);
extern "C" API void graphics_setUniformbVec4(uint id, bool x, bool y, bool z, bool w);

extern "C" API void graphics_setUniformMat2(uint id,
	float x1, float x2,
	float y1, float y2
);
extern "C" API void graphics_setUniformMat3(uint id,
	float x1, float x2, float x3,
	float y1, float y2, float y3,
	float z1, float z2, float z3
);
extern "C" API void graphics_setUniformMat4(uint id,
	float x1, float x2, float x3, float x4,
	float y1, float y2, float y3, float y4,
	float z1, float z2, float z3, float z4,
	float w1, float w2, float w3, float w4
);

extern "C" API void graphics_setUniformMatd2(uint id,
	double x1, double x2,
	double y1, double y2
);
extern "C" API void graphics_setUniformMatd3(uint id,
	double x1, double x2, double x3,
	double y1, double y2, double y3,
	double z1, double z2, double z3
);
extern "C" API void graphics_setUniformMatd4(uint id,
	double x1, double x2, double x3, double x4,
	double y1, double y2, double y3, double y4,
	double z1, double z2, double z3, double z4,
	double w1, double w2, double w3, double w4
);
