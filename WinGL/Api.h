#pragma once

#ifdef API_EXPORTS
#define API __declspec(dllexport)
#else
#define API __declspec(dllimport)
#endif

extern "C"  API void test();
extern "C"  API void setup(char** args, int length);

extern "C"  API void setCurrWindow(int window);
extern "C"  API int getCurrWindow();

extern "C"  API int createWindow(char* title);
extern "C"  API void destroyWindow(int window);

extern "C"  API void showWindow(int window);
extern "C"  API void hideWindow(int window);

extern "C"  API void setDisplayFunc(int window, void(func)());
extern "C"  API void setResizeFunc(int window, void(func)(int width, int height));

extern "C"  API void setKeyboardUpFunc(int window, void(__stdcall func)(int key));
extern "C"  API void setKeyboardDownFunc(int window, void(__stdcall func)(int key));

extern "C"  API void setMouseMoveFunc(int window, void(func)(int x, int y));
extern "C"  API void setMouseDownFunc(int window, void(func)(int button, int x, int y));
extern "C"  API void setMouseUpFunc(int window, void(func)(int button, int x, int y));

extern "C"  API void setWindowTitle(int window, char* title);

extern "C"  API void startMainLoop(int window);
