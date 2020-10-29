#pragma once

#define API

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

typedef void(__stdcall ExDisplayFunc)();
typedef void(__stdcall ExKeyboardFunc)(int key);
typedef void(__stdcall ExResizeFunc)(int w, int h);
typedef void(__stdcall ExMouseMoveFunc)(int x, int y);
typedef void(__stdcall ExMouseActionFunc)(int action, int x, int y);
typedef void(__stdcall ExMouseScrollFunc)(int x, int y, int delta);

extern "C" API void abc();

extern "C" API uint window_ex_createWindow(const char* title);
extern "C" API void window_ex_destryWindow(uint id);

extern "C" API void window_ex_showWindow(uint id);
extern "C" API void window_ex_hideWindow(uint id);

extern "C" API void window_ex_activateWindow(uint id);
extern "C" API void window_ex_setDisplayFunc(uint id, ExDisplayFunc * func);
extern "C" API void window_ex_setResizeFunc(uint id, ExResizeFunc* func);

extern "C" API void window_ex_setMouseMoveFunc(uint id, ExMouseMoveFunc * func);
extern "C" API void window_ex_setMouseDownFunc(uint id, ExMouseActionFunc * func);
extern "C" API void window_ex_setMouseUpFunc(uint id, ExMouseActionFunc * func);
extern "C" API void window_ex_setScrollFunc(uint id, ExMouseActionFunc * func);

extern "C" API void window_ex_setWindowTitle(uint id, const char* title);
extern "C" API void window_ex_setWindowSize(uint id, int w, int h);

extern "C" API float window_ex_getRefreshRate(uint id);
extern "C" API void window_ex_setRefreshRate(uint id, float fps);

extern "C" API void window_ex_setKeydownFunc(uint id, ExKeyboardFunc * func);
extern "C" API void window_ex_setKeyupFunc(uint id, ExKeyboardFunc * func);

extern "C" API void window_ex_activateMainLoop();
extern "C" API void window_ex_init();
