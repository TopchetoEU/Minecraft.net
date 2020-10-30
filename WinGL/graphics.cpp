#include "framework.h"
#include "pch.h"
#include <map>
#include "graphics_api.h"
#include "window_api.h"
#include "gl/glew.h"
#include <gl/GL.h>
#include <set>
#include <list>
#include <combaseapi.h>

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
	string source;

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

typedef unsigned char ubyte;
typedef unsigned short ushort;

struct NativeArrayElement
{
public:
	uint Type;
	void* Value;

	NativeArrayElement(uint type, void* val)
	{
		Type = type;
		Value = val;
	}
};

class throwable {
public:
	virtual string getName() = 0;
	virtual string getMessage() = 0;
	virtual uint getCode() = 0;
	void throwError() {
		cout << getName() << " (0x" << intToString(getCode(), 16, "0123456789ABCDEF") << ") threw with a message of: " << getMessage() << endl;

		SetLastError(getCode());

		throw getCode();
	}
};

class genericError : public throwable {
private:
	string message = "aaa";
	string name = "genericError";
public:
	string getName() { return name; }
	void setName(string value) { name = value; }
	string getMessage() { return message; }
	void setMessage(string value) { message = value; }

	uint getCode() { return 0xFFFF0000; }

	genericError() { setMessage("A generic error was thrown"); }
	genericError(string message) { setMessage(message); }
	genericError(string message, string name) { setMessage(message); setName(name); }
};

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

nativeString nativiseString(string val) {
	auto ptr = (char*)CoTaskMemAlloc(strlen(val.c_str()) + 1);

	if (!ptr) throw exception("Could not nativise string!");

	strcpy_s(ptr, (val.size() + 1) * sizeof(char), val.c_str());

	return ptr;
}

void graphics_init() {
	if (g_initialised) return;
	g_initialised = true;

	auto err = glewInit(); 
	if (err != GLEW_OK) cout << glewGetErrorString(err);
}

uint graphics_createVAO() {
	cout << glewGetString(GLEW_VERSION);
	uint vaoId = 0;
	glGenVertexArrays(1, &vaoId);

	vaos.insert(make_pair(vaoId, new vao(vaoId)));

	return vaoId;
}
void graphics_destroyVAO(uint vaoId) {
	if (vaos.find(vaoId) != vaos.end()) {
		glDeleteVertexArrays(1, &vaoId);
		vaos.erase(vaoId);
	}
	else genericError(
		"An attempt was made to delete an unexistant vertex array object",
		"objectDeleteError"
	).throwError();
}

uint graphics_createBuffer(uint target) {
	uint bufferId = 0;

	glGenBuffers(1, &bufferId);


	buffers.insert(make_pair(bufferId, new buffer(target, bufferId)));

	return bufferId;
}
void graphics_destroyBuffer(uint buff) {
	if (buffers.find(buff) != buffers.end()) {
		glDeleteBuffers(0, &buff);
		buffers.erase(buff);
	}
	else genericError(
		"An attempt was made to delete an unexistant buffer",
		"objectDeleteError"
	).throwError();
}

bool checkBuffExistance(uint target) {
	bool exists = bountBuffers.find(target) != bountBuffers.end();

	if (!exists)
		bountBuffers.insert(make_pair(target, 0));

	return exists;
}

void graphics_setBuffer(uint target, uint buff) {
	if (buffers.find(buff) == buffers.end()) genericError(
		"An attempt was made to bind an unexistant buffer",
		"bufferBindError"
	).throwError();
	if (bountBuffers[target] != buff) {
		glBindBuffer(target, buff);
		checkBuffExistance(target);
		bountBuffers[target] = buff;
	}
}
uint graphics_getBuffer(uint target) {
	return bountBuffers[target];
}

void graphics_setBufferData(uint target, uint size, uint dataArr, uint usage) {
	glBufferData(target, size, nativeArrays[dataArr], usage);
}
void graphics_getBufferData(uint target, uint size, object data) {
	glGetBufferSubData(target, 0, size, data);
}

void graphics_setVAO(uint vaoId) {
	if (vaos.find(vaoId) == vaos.end()) genericError(
		"An attempt was made to bind an unexistant buffer",
		"bufferBindError"
	).throwError();
	if (bountVAO != vaoId) {
		glBindVertexArray(vaoId);
		bountVAO = vaoId;
	}
}
uint graphics_getVAO() {
	return bountVAO;
}

void graphics_modifyBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, object arrayOffset) {
	graphics_setVAO(buff);

	vaos[buff]->attributes.insert(make_pair(
		attrId,
		new attribute(attrId, byteSize, type, stride, 0)
	));

	glVertexAttribPointer(attrId, byteSize, type, false, stride, arrayOffset);
}
void graphics_createBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, uint offset) {
	graphics_modifyBufferAttribute(buff, attrId, byteSize, type, stride, (void*)offset);
	glEnableVertexAttribArray(attrId);
	cout << "test";
}
void graphics_destroyBufferAttribute(uint buff, uint attrId) {
	graphics_setVAO(buff);
	glDisableVertexArrayAttrib(buff, attrId);
}
void graphics_clearBufferAttributes(uint buff) {
	graphics_setVAO(buff);
	for (auto pair : vaos[buff]->attributes) {
		glDisableVertexArrayAttrib(buff, pair.first);
	}
}

void graphics_drawBuffer(uint vaoId, uint buff, uint amount, uint mode) {
	graphics_setVAO(vaoId);
	graphics_setBuffer(buffers[buff]->type, buff);
	glDrawArrays(mode, 0, amount);
}

uint graphics_loadNativeArray(void* arr, uint size) {
	auto* v = (NativeArrayElement*)arr;
	void* nativeArr = new void* [size];
	uint id = nextArray++;

	void* initArray = nativeArr;

	for (uint i = 0; i < size; i++, v++)
	{
		auto value = *v;

		switch (value.Type)
		{
		case GL_UNSIGNED_BYTE:
			*(ubyte*)nativeArr = *(ubyte*)(ubyte*)value.Value;
			nativeArr = (ubyte*)nativeArr + 1;
			break;
		case GL_SHORT:
			*(short*)nativeArr = *(short*)(short*)value.Value;
			nativeArr = (short*)nativeArr + 1;
			break;
		case GL_UNSIGNED_SHORT:
			*(ushort*)nativeArr = *(ushort*)(ushort*)value.Value;
			nativeArr = (ushort*)nativeArr + 1;
			break;
		case GL_INT:
			*(int*)nativeArr = *(int*)(int*)value.Value;
			nativeArr = (int*)nativeArr + 1;
			break;
		case GL_FLOAT:
			*(float*)nativeArr = *(float*)(float*)value.Value;;
			nativeArr = (float*)nativeArr + 1;
			break;
		case GL_DOUBLE:
			*(double*)nativeArr = *(double*)(double*)value.Value;
			nativeArr = (double*)nativeArr + 1;
			break;
		}
	}

	nativeArrays.insert(make_pair(id, initArray));

	return id;
}
uint graphics_createNativeArray(uint size) {
	void* nativeArr = new void* [size];
	uint id = nextArray++;

	nativeArrays.insert(make_pair(id, nativeArr));

	return id;
}
void graphics_destroyNativeArray(uint arr) {
	delete nativeArrays[arr];

	nativeArrays.erase(arr);
}

void graphics_setShaderProgram(uint program) {
	glUseProgram(program);
	selectedShaderProgram = program;
}
uint graphics_getShaderProgram() {
	return selectedShaderProgram;
}

uint graphics_createShader(uint type, const char* raw, int length) {
	int a = 0;
	uint shaderId = glCreateShader(type);

	auto shdr = new shader(shaderId, raw);

	int success = 0;
	int logSize = 0;

	cout << 1;
	glShaderSource(shaderId, 1, &raw, &length);
	glCompileShader(shaderId);
	glGetShaderiv(shaderId, GL_COMPILE_STATUS, &success);
	glGetShaderiv(shaderId, GL_INFO_LOG_LENGTH, &logSize);

	char* rewritableLog = new char[logSize];
	glGetShaderInfoLog(shaderId, logSize, NULL, rewritableLog);

	string log = (const char*)rewritableLog;

	if (logSize == 0) log = "";

	shdr->buildSuccessfull = logSize == 0;
	shdr->error = log;

	shaders.insert(make_pair(shaderId, shdr));

	return shaderId;
}
void graphics_destroyShader(uint shader) {

	glDeleteShader(shader);
}

uint test = 0;

nativeString graphics_getShaderInfoLog(uint shaderId) {
	auto a = shaders[shaderId];
	return nativiseString(a->error);
}
bool graphics_getShaderBuildSuccess(uint shaderId) {
	auto a = shaders[shaderId]->buildSuccessfull;

	return a;
}
string graphics_getShaderSource(uint shaderId) {
	return shaders[shaderId]->source;
}

void graphics_detachShaderFromProgram(uint program, uint shader) {
	glDetachShader(program, shader);

	shaderPrograms[program]->shaders->remove(shader);
	shaders[shader]->correlatedPrograms->remove(program);
}
void graphics_attachShaderToProgram(uint program, uint shader) {
	glAttachShader(program, shader);

	shaderPrograms[program]->shaders->push_back(shader);
	shaders[shader]->correlatedPrograms->push_back(program);
}

uint graphics_createShaderProgram() {
	uint id = glCreateProgram();

	shaderPrograms.insert(make_pair(id, new shaderProgram(id)));

	return id;
}
void graphics_compileShaderProgram(uint program) {
	glLinkProgram(program);

	int success = 0;
	int logLength = 0;

	glGetProgramiv(program, GL_LINK_STATUS, &success);
	glGetProgramiv(program, GL_INFO_LOG_LENGTH, &logLength);

	char* logError = new char[logLength];

	glGetProgramInfoLog(program, logLength, NULL, logError);

	if (!(logLength == 0 || success == 1)) genericError((const char*)logError).throwError();
}
void graphics_destroyShaderProgram(uint program) {
	for (auto shdr : *shaderPrograms[program]->shaders) {
		graphics_detachShaderFromProgram(program, shdr);
	}

	shaderPrograms.erase(program);

	glDeleteProgram(program);
}
uint graphics_loadShaderProgram(uint* shaders, uint count) {
	uint program = graphics_createShaderProgram();

	for (uint i = 0; i < count; i++)
	{
		graphics_attachShaderToProgram(program, shaders[i]);
	}

	return program;
}

float backR = 0, backG = 0, backB = 0, backA = 0;

void graphics_setBackground(float r, float g, float b, float a) {
	glClearColor(r, g, b, a);

	backR = r;
	backG = g;
	backB = b;
	backA = a;
}
void graphics_getBackground(float* r, float* g, float* b, float* a) {
	r = new float(backR);
	g = new float(backG);
	b = new float(backB);
	a = new float(backA);
}

void graphics_clear(uint type) {
	glClear(type);
}

uint graphics_getUniformLocation(uint program, char* name) {
	return glGetUniformLocation(program, name);
}
uint graphics_getAttribLocation(uint program, char* name) {
	return glGetAttribLocation(program, name);
}

void graphics_setUniformVec2(uint id, float x, float y) {
	glUniform2f(id, x, y);
}
void graphics_setUniformVec3(uint id, float x, float y, float z) {
	glUniform3f(id, x, y, z);
}
void graphics_setUniformVec4(uint id, float x, float y, float z, float w) {
	glUniform4f(id, x, y, z, w);
}

void graphics_setUniformdVec2(uint id, double x, double y) {
	glUniform2d(id, x, y);
}
void graphics_setUniformdVec3(uint id, double x, double y, double z) {
	glUniform3d(id, x, y, z);
}
void graphics_setUniformdVec4(uint id, double x, double y, double z, double w) {
	glUniform4d(id, x, y, z, w);
}

void graphics_setUniformiVec2(uint id, int x, int y) {
	glUniform2i(id, x, y);
}
void graphics_setUniformiVec3(uint id, int x, int y, int z) {
	glUniform3i(id, x, y, z);
}
void graphics_setUniformiVec4(uint id, int x, int y, int z, int w) {
	glUniform4i(id, x, y, z, w);
}

void graphics_setUniformbVec2(uint id, bool x, bool y) {
	graphics_setUniformiVec2(id, x, y);
}
void graphics_setUniformbVec3(uint id, bool x, bool y, bool z) {
	graphics_setUniformiVec3(id, x, y, z);
}
void graphics_setUniformbVec4(uint id, bool x, bool y, bool z, bool w) {
	graphics_setUniformiVec4(id, x, y, z, w);
}

void graphics_setUniformMat2(uint id,
	float x1, float x2,
	float y1, float y2
) {
	glUniformMatrix2fv(id, 1, false, new float[4] {
		x1, x2,
		y1, y2
	});
}
void graphics_setUniformMat3(uint id,
	float x1, float x2, float x3,
	float y1, float y2, float y3,
	float z1, float z2, float z3
) {
	auto a = (float*)new float[9] {
		x1, y1, z1,
		x2, y2, z2,
		x3, y3, z3
	};

	glUniformMatrix3fv(id, 1, false, a);
}
void graphics_setUniformMat4(uint id,
	float x1, float x2, float x3, float x4,
	float y1, float y2, float y3, float y4,
	float z1, float z2, float z3, float z4,
	float w1, float w2, float w3, float w4
) {
	glUniformMatrix3fv(id, 1, false, new float[16] {
		x1, x2, x3, x4,
		y1, y2, y3, y4,
		z1, z2, z3, z4,
		w1, w2, w3, w4
	});
}


void graphics_setUniformMatd2(uint id,
	double x1, double x2,
	double y1, double y2
) {
	glUniformMatrix2dv(id, 1, false, new double[4] {
		x1, x2,
		y1, y2
	});
}
void graphics_setUniformMatd3(uint id,
	double x1, double x2, double x3,
	double y1, double y2, double y3,
	double z1, double z2, double z3
) {
	glUniformMatrix3dv(id, 1, false, new double[9] {
		x1, x2, x3,
		y1, y2, y3,
		z1, z2, z3
	});
}
void graphics_setUniformMatd4(uint id,
	double x1, double x2, double x3, double x4,
	double y1, double y2, double y3, double y4,
	double z1, double z2, double z3, double z4,
	double w1, double w2, double w3, double w4
) {
	glUniformMatrix3dv(id, 1, false, new double[16] {
			x1, x2, x3, x4,
			y1, y2, y3, y4,
			z1, z2, z3, z4,
			w1, w2, w3, w4
		});
}
