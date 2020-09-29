#ifdef __cplusplus
extern "C" {
#endif

#ifdef _WIN32
#  ifdef MODULE_API_EXPORTS
#    define MODULE_API __declspec(dllexport)
#  else
#    define MODULE_API __declspec(dllimport)
#  endif
#else
#  define MODULE_API
#endif
MODULE_API void setup();
MODULE_API void setup(char** args, int length);

MODULE_API void setCurrWindow(int window);
MODULE_API int getCurrWindow();
MODULE_API
MODULE_API int createWindow(char* title);
MODULE_API void destroyWindow(int window);
MODULE_API
MODULE_API void showWindow(int window);
MODULE_API void hideWindow(int window);

MODULE_API void setDisplayFunc(int window, void(func)());
MODULE_API void setResizeFunc(int window, void(func)(int width, int height));

MODULE_API void setKeyboardUpFunc(int window, void(func)(unsigned char key, int x, int y));
MODULE_API void setKeyboardDownFunc(int window, void(func)(unsigned char key, int x, int y));

MODULE_API void setMouseMoveFunc(int window, void(func)(int x, int y));
MODULE_API void setMouseDownFunc(int window, void(func)(int button, int x, int y));
MODULE_API void setMouseUpFunc(int window, void(func)(int button, int x, int y));

MODULE_API void setWindowTitle(int window, char* title);

MODULE_API void startMainLoop(int window);

#ifdef __cplusplus
}
#endif

