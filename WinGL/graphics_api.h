#pragma once

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

#include "framework.h"

extern "C" API void graphics_init();

extern "C" API void graphics_swapBuffers();

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
extern "C" API uint graphics_loadShaderProgram(uint* shaders, uint count);

extern "C" API void graphics_setShaderProgram(uint program);
extern "C" API uint graphics_getShaderProgram();

extern "C" API nativeString testA(uint a);
extern "C" API bool testB(uint a);

extern "C" API void graphics_setBackground(float r, float g, float b, float a);
extern "C" API void graphics_getBackground(float* r, float* g, float* b, float* a);
extern "C" API void graphics_clear(uint type);

