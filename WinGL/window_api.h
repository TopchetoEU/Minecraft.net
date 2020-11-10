#pragma once

#define API

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

typedef void(__stdcall DisplayFunc)();
typedef void(__stdcall KeyboardActionFunc)(int code);
typedef void(__stdcall MouseActionFunc)(int data, int x, int y);
typedef void(__stdcall MouseMovementFunc)(int x, int y);
typedef void(__stdcall ResizeFunc)(int w, int h);

typedef unsigned int uint;


extern "C" API void window_test();

extern "C" API void window_setCurrWindow(int window);
extern "C" API int window_getCurrWindow();

extern "C" API void window_screenToClient(uint wnd, int& x, int& y);
extern "C" API void window_clientToSpace(uint wnd, float& x, float& y);
extern "C" API void window_spaceToClient(uint wnd, float& x, float& y);
extern "C" API void window_clientToScreen(uint wnd, int& x, int& y);

extern "C" API void window_getMousePosition(int *x, int *y);
extern "C" API void window_setMousePosition(int x, int y);

extern "C" API void window_setConstantRefresh(int window, bool refresh);
extern "C" API bool window_getConstantRefresh(int window);
