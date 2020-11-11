#pragma once

#define API

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

typedef void(__stdcall ExDisplayFunc)();
typedef void(__stdcall ExKeyboardFunc)(int key);
typedef void(__stdcall ExMouseActionFunc)(int action, int x, int y);
typedef void(__stdcall ExMouseMoveFunc)(int x, int y);
typedef void(__stdcall ExResizeFunc)(int w, int h);
typedef void(__stdcall ExMouseScrollFunc)(int x, int y, int delta);

#include <glfw3.h>
#include <iostream>
#include "graphics_api.h"
#include "framework.h"

using namespace std;

class window {
public:
	std::string title;
	uint id;

	GLFWwindow* handle = NULL;
	graphics* attachedGraphics;

	bool shown = false;
	bool active = false;

	float fps = 60;

	float actualTime = -1;

	bool vsync = true;

	int x = 0, y = 0;

	ExDisplayFunc* display = nullptr;
	ExResizeFunc* resize = nullptr;

	ExMouseMoveFunc* mouseMove = nullptr;
	ExMouseActionFunc* mouseDown = nullptr;
	ExMouseActionFunc* mouseUp = nullptr;
	ExMouseScrollFunc* scroll = nullptr;

	ExKeyboardFunc* keyDown = nullptr;
	ExKeyboardFunc* keyUp = nullptr;

	window(uint id);
};

window* getWindow(uint id);

extern "C" API uint window_createWindow(const char* title);
extern "C" API void window_destryWindow(uint id);

extern "C" API void window_showWindow(uint id);
extern "C" API void window_hideWindow(uint id);

extern "C" API void window_activateWindow(uint id);
extern "C" API void window_setDisplayFunc(uint id, ExDisplayFunc * func);
extern "C" API void window_setResizeFunc(uint id, ExResizeFunc* func);

extern "C" API void window_setMouseMoveFunc(uint id, ExMouseMoveFunc * func);
extern "C" API void window_setMouseDownFunc(uint id, ExMouseActionFunc * func);
extern "C" API void window_setMouseUpFunc(uint id, ExMouseActionFunc * func);
extern "C" API void window_setScrollFunc(uint id, ExMouseActionFunc * func);

extern "C" API void window_setWindowTitle(uint id, const char* title);
extern "C" API void window_setWindowSize(uint id, int w, int h);

extern "C" API float window_getRefreshRate(uint id);
extern "C" API void window_setRefreshRate(uint id, float fps);

extern "C" API void window_setKeydownFunc(uint id, ExKeyboardFunc * func);
extern "C" API void window_setKeyupFunc(uint id, ExKeyboardFunc * func);

extern "C" API void window_activateMainLoop();
extern "C" API void window_setup();

extern "C" API void window_setMousePosition(uint wnd, int x, int y);
extern "C" API void window_setMouseLocked(uint wnd, bool value);