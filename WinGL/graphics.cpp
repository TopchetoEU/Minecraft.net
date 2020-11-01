#include "pch.h"
#include <map>
#include "graphics_api.h"
#include "gl/glew.h"
#include "window_api_experimental.h"
#include <set>
#include <list>
#include <iostream>
#include <combaseapi.h>

using std::string;

using std::list;
using std::map;
using std::make_pair;
using std::cout;

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
		cout << getName() << " (0x" << uintToString(getCode(), 16, "0123456789ABCDEF") << ") threw with a message of: " << getMessage() << endl;

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

graphics::graphics() {
	backA = 0; backR = 0; backG = 0; backB = 0;
}

nativeString nativiseString(string val) {
	auto ptr = (char*)CoTaskMemAlloc(strlen(val.c_str()) + 1);

	if (!ptr) throw exception("Could not nativise string!");

	strcpy_s(ptr, (val.size() + 1) * sizeof(char), val.c_str());

	return ptr;
}

graphics* currGraphics = nullptr;

bool graphicsCheck() {
	if (currGraphics == nullptr) {
		genericError(
			"There is no current window graphics context selected. Maybe you're not drawing at the moment?"
		).throwError();
		return false;
	}
	else if (!currGraphics->g_initialised) {
		genericError("The currently selected graphic context is not initialised.").throwError();
		return false;
	}

	return true;
}
void setCurrContext(graphics* grph) {
	currGraphics = grph;
}

uint graphics_createVAO() {
	uint vaoId = 0;
	glGenVertexArrays(1, &vaoId);

	currGraphics->vaos.insert(make_pair(vaoId, new vao(vaoId)));

	return vaoId;
}
void graphics_destroyVAO(uint vaoId) {
	if (currGraphics->vaos.find(vaoId) != currGraphics->vaos.end()) {
		glDeleteVertexArrays(1, &vaoId);
		currGraphics->vaos.erase(vaoId);
	}
	else genericError(
		"An attempt was made to delete an unexistant vertex array object",
		"objectDeleteError"
	).throwError();
}

uint graphics_createBuffer(uint target) {
	uint bufferId = 0;

	glGenBuffers(1, &bufferId);


	currGraphics->buffers.insert(make_pair(bufferId, new buffer(target, bufferId)));

	return bufferId;
}
void graphics_destroyBuffer(uint buff) {
	if (currGraphics->buffers.find(buff) != currGraphics->buffers.end()) {
		glDeleteBuffers(0, &buff);
		currGraphics->buffers.erase(buff);
	}
	else genericError(
		"An attempt was made to delete an unexistant buffer",
		"objectDeleteError"
	).throwError();
}

bool checkBuffExistance(uint target) {
	bool exists = currGraphics->bountBuffers.find(target) != currGraphics->bountBuffers.end();

	if (!exists)
		currGraphics->bountBuffers.insert(make_pair(target, 0));

	return exists;
}

void graphics_setBuffer(uint target, uint buff) {
	if (currGraphics->buffers.find(buff) == currGraphics->buffers.end()) genericError(
		"An attempt was made to bind an unexistant buffer",
		"bufferBindError"
	).throwError();
	if (currGraphics->bountBuffers[target] != buff) {
		glBindBuffer(target, buff);
		checkBuffExistance(target);
		currGraphics->bountBuffers[target] = buff;
	}
}
uint graphics_getBuffer(uint target) {
	return currGraphics->bountBuffers[target];
}

void graphics_setBufferData(uint target, uint size, uint dataArr, uint usage) {
	glBufferData(target, size, currGraphics->nativeArrays[dataArr], usage);
}
void graphics_getBufferData(uint target, uint size, object data) {
	glGetBufferSubData(target, 0, size, data);
}

void graphics_setVAO(uint vaoId) {
	if (currGraphics->vaos.find(vaoId) == currGraphics->vaos.end()) genericError(
		"An attempt was made to bind an unexistant buffer",
		"bufferBindError"
	).throwError();
	if (currGraphics->bountVAO != vaoId) {
		glBindVertexArray(vaoId);
		currGraphics->bountVAO = vaoId;
	}
}
uint graphics_getVAO() {
	return currGraphics->bountVAO;
}

void graphics_modifyBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, object arrayOffset) {
	graphics_setVAO(buff);

	currGraphics->vaos[buff]->attributes.insert(make_pair(
		attrId,
		new attribute(attrId, byteSize, type, stride, 0)
	));

	glVertexAttribPointer(attrId, byteSize, type, false, stride, arrayOffset);
}
void graphics_createBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, uint offset) {
	graphics_modifyBufferAttribute(buff, attrId, byteSize, type, stride, (void*)offset);
	glEnableVertexAttribArray(attrId);
}
void graphics_destroyBufferAttribute(uint buff, uint attrId) {
	graphics_setVAO(buff);
	glDisableVertexArrayAttrib(buff, attrId);
}
void graphics_clearBufferAttributes(uint buff) {
	graphics_setVAO(buff);
	for (auto pair : currGraphics->vaos[buff]->attributes) {
		glDisableVertexArrayAttrib(buff, pair.first);
	}
}

void graphics_drawBuffer(uint vaoId, uint buff, uint amount, uint mode) {
	graphicsCheck();
	graphics_setVAO(vaoId);
	graphics_setBuffer(currGraphics->buffers[buff]->type, buff);
	glDrawArrays(mode, 0, amount);
}
void graphics_drawElement(uint indicieType, uint indicieCount, uint eboLength, uint ebo, uint vbo) {
	graphicsCheck();
	graphics_setBuffer(GL_ARRAY_BUFFER, vbo);
	graphics_setBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
	glDrawElements(indicieType, eboLength, GL_UNSIGNED_INT, 0);
}

uint graphics_loadNativeArray(void* arr, uint size) {
	auto* v = (NativeArrayElement*)arr;
	void* nativeArr = new void* [size];
	uint id = currGraphics->nextArray++;

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
		case GL_UNSIGNED_INT:
			*(uint*)nativeArr = *(uint*)(uint*)value.Value;
			cout << "test";
			nativeArr = (uint*)nativeArr + 1;
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

	currGraphics->nativeArrays.insert(make_pair(id, initArray));

	return id;
}
uint graphics_createNativeArray(uint size) {
	void* nativeArr = new void* [size];
	uint id = currGraphics->nextArray++;

	currGraphics->nativeArrays.insert(make_pair(id, nativeArr));

	return id;
}
void graphics_destroyNativeArray(uint arr) {
	delete currGraphics->nativeArrays[arr];

	currGraphics->nativeArrays.erase(arr);
}

void graphics_setShaderProgram(uint program) {
	glUseProgram(program);
	currGraphics->selectedShaderProgram = program;
}
uint graphics_getShaderProgram() {
	return currGraphics->selectedShaderProgram;
}

uint graphics_createShader(uint type, const char* raw, int length) {
	int a = 0;
	uint shaderId = glCreateShader(type);

	auto shdr = new shader(shaderId, raw);

	int success = 0;
	int logSize = 0;

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

	currGraphics->shaders.insert(make_pair(shaderId, shdr));

	return shaderId;
}
void graphics_destroyShader(uint shader) {
	glDeleteShader(shader);

	currGraphics->shaders.erase(shader);
}

uint test = 0;

nativeString graphics_getShaderInfoLog(uint shaderId) {
	auto a = currGraphics->shaders[shaderId];
	return nativiseString(a->error);
}
bool graphics_getShaderBuildSuccess(uint shaderId) {
	auto a = currGraphics->shaders[shaderId]->buildSuccessfull;

	return a;
}
string graphics_getShaderSource(uint shaderId) {
	return currGraphics->shaders[shaderId]->source;
}

void graphics_detachShaderFromProgram(uint program, uint shader) {
	glDetachShader(program, shader);

	currGraphics->shaderPrograms[program]->shaders->remove(shader);
	currGraphics->shaders[shader]->correlatedPrograms->remove(program);
}
void graphics_attachShaderToProgram(uint program, uint shader) {
	glAttachShader(program, shader);

	currGraphics->shaderPrograms[program]->shaders->push_back(shader);
	currGraphics->shaders[shader]->correlatedPrograms->push_back(program);
}

uint graphics_createShaderProgram() {
	uint id = glCreateProgram();

	currGraphics->shaderPrograms.insert(make_pair(id, new shaderProgram(id)));

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
	for (auto shdr : *currGraphics->shaderPrograms[program]->shaders) {
		graphics_detachShaderFromProgram(program, shdr);
	}

	currGraphics->shaderPrograms.erase(program);

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

void graphics_setBackground(float r, float g, float b, float a) {
	glClearColor(r, g, b, a);

	currGraphics->backR = r;
	currGraphics->backG = g;
	currGraphics->backB = b;
	currGraphics->backA = a;
}
void graphics_getBackground(float* r, float* g, float* b, float* a) {
	r = new float(currGraphics->backR);
	g = new float(currGraphics->backG);
	b = new float(currGraphics->backB);
	a = new float(currGraphics->backA);
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
	glUniformMatrix2fv(id, 1, false, new float[4]{
		x1, x2,
		y1, y2
		});
}
void graphics_setUniformMat3(uint id,
	float x1, float x2, float x3,
	float y1, float y2, float y3,
	float z1, float z2, float z3
) {
	auto a = (float*)new float[9]{
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
	glUniformMatrix3fv(id, 1, false, new float[16]{
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
	glUniformMatrix2dv(id, 1, false, new double[4]{
		x1, x2,
		y1, y2
		});
}
void graphics_setUniformMatd3(uint id,
	double x1, double x2, double x3,
	double y1, double y2, double y3,
	double z1, double z2, double z3
) {
	glUniformMatrix3dv(id, 1, false, new double[9]{
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
	glUniformMatrix3dv(id, 1, false, new double[16]{
			x1, x2, x3, x4,
			y1, y2, y3, y4,
			z1, z2, z3, z4,
			w1, w2, w3, w4
		});
}
