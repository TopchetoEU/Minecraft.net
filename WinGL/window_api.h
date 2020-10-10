#pragma once

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

typedef void(__stdcall MouseActionFunc)(int data, int x, int y);
typedef void(__stdcall MouseMovementFunc)(int x, int y);
typedef void(__stdcall KeyboardActionFunc)(int code);

extern "C" API void window_test();
extern "C" API void window_setup(char** args, int length);

extern "C" API void window_setCurrWindow(int window);
extern "C" API int window_getCurrWindow();

extern "C" API int window_createWindow(char* title);
extern "C" API void window_destroyWindow(int window);

extern "C" API void window_showWindow(int window);
extern "C" API void window_hideWindow(int window);

extern "C" API void window_setDisplayFunc(int window, void(func)());
extern "C" API void window_setResizeFunc(int window, void(func)(int width, int height));

extern "C" API void window_setKeyboardUpFunc(int window, void(__stdcall func)(int key));
extern "C" API void window_setKeyboardDownFunc(int window, void(__stdcall func)(int key));

extern "C" API void window_setMouseMoveFunc(int window, MouseMovementFunc func);
extern "C" API void window_setMouseDownFunc(int window, MouseActionFunc func);
extern "C" API void window_setMouseUpFunc(int window, MouseActionFunc func);
extern "C" API void window_setMouseScrollFunc(int window, MouseActionFunc func);

extern "C" API void window_setWindowTitle(int window, char* title);

extern "C" API void window_startMainLoop(int window);

extern "C" API void window_screenToClient(uint wnd, int& x, int& y);
extern "C" API void window_clientToSpace(uint wnd, float& x, float& y);
extern "C" API void window_spaceToClient(uint wnd, float& x, float& y);
extern "C" API void window_clientToScreen(uint wnd, int& x, int& y);


extern "C" API void window_getMousePosition(int *x, int *y);
extern "C" API void window_setMousePosition(int x, int y);

