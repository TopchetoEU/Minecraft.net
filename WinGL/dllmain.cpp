// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Api.h"
#include <iostream>
#include "GL/glut.h"
#include <fstream>
#include <map>

using namespace std;

typedef void(MouseActionFunc)(int button, int x, int y);
typedef void(__stdcall* KeyboardActionFunc)(int code);

enum class KeyType {
	Unknown,
	Up,
	Down
};
struct window {
public:
	HWND handle;
	MouseActionFunc* mouseDown = nullptr;
	MouseActionFunc* mouseUp = nullptr;
	KeyboardActionFunc* keyUp = nullptr;
	KeyboardActionFunc* keyDown = nullptr;
	window(HWND handle) {
		this->handle = handle;
	}

	bool operator==(window a) {
		return handle == a.handle;
	}
};

bool initialised = false;
HHOOK h;
map<int, window*> handles = map<int, window*>();

void displayFunc() {
}
void mouseEventHandle(int button, int state, int x, int y) {
	for (const auto& pair : handles) {
		window* wnd = pair.second;
		if      (state == GLUT_DOWN && wnd->mouseDown != nullptr) wnd->mouseDown(button, x, y);
		else if (state == GLUT_UP   && wnd->mouseUp   != nullptr) wnd->mouseUp  (button, x, y);
	}
}
long __stdcall keyboardFunction(int code, WPARAM w, LPARAM l) {
	if (code < 0)
		return CallNextHookEx(h, code, w, l);
	else {
		auto* data = (tagKBDLLHOOKSTRUCT *)l;

		cout << "" << data->vkCode << "\n";

		for (const auto& pair : handles) {
			window* wnd = pair.second;
			if (wnd->handle == GetActiveWindow()) {
				KeyboardActionFunc func = NULL;

				if (w == 256) func = *wnd->keyDown;
				if (w == 257) func = *wnd->keyUp;

				if (func != NULL) func(data->vkCode);
			}
		}

		return NULL;
	}
}
long __stdcall mouseFunction(int code, WPARAM w, LPARAM l) {
	if (code < 0)
		return CallNextHookEx(h, code, w, l);
	else {
		auto* data = (tagKBDLLHOOKSTRUCT*)l;

		cout << "" << data->vkCode << "\n";

		for (const auto& pair : handles) {
			window* wnd = pair.second;
			if (wnd->handle == GetActiveWindow()) {
				KeyboardActionFunc func = NULL;

				if (w == 256) func = *wnd->keyDown;
				if (w == 257) func = *wnd->keyUp;

				if (func != NULL) func(data->vkCode);
			}
		}

		return NULL;
	}
}

HHOOK installhook(HINSTANCE hmod) {
	h = SetWindowsHookExA(WH_KEYBOARD_LL, keyboardFunction, NULL, NULL);

	if (h == NULL) {
		cout << "Unable to initialise keyboard listener!\n";
		cout << "Error: " << GetLastError() << "\n";
	}

	return h;
}

void window_setup(char** args, int length) {
	if (!initialised) {
		glutInit(&length, args);
		glutMouseFunc(mouseEventHandle);

		installhook(GetModuleHandle(NULL));

		initialised = true;
	}
}
void window_setup() {
	window_setup(0, {});
}

int window_getCurrWindow() {
	return glutGetWindow();
}
void window_setCurrWindow(int window) {
	if (window != window_getCurrWindow())
		glutSetWindow(window);
}

int window_createWindow(char* title) {
	int id = glutCreateWindow(title);
	HWND handle = GetForegroundWindow();
	window *wnd = new window(handle);

	handles.insert(std::make_pair( id, wnd ));

	glutDisplayFunc(displayFunc);
	return id;
}
void window_destroyWindow(int window) {
	glutDestroyWindow(window);
}

void window_showWindow(int window) {
	window_setCurrWindow(window);
	glutShowWindow();
}
void window_hideWindow(int window) {
	window_setCurrWindow(window);
	glutHideWindow();
}

void window_setDisplayFunc(int window, void(func)()) {
	if (func == nullptr) {
		glutDisplayFunc(displayFunc);
	}
	else {
		window_setCurrWindow(window);
		glutDisplayFunc(func);
	}
}
void window_setResizeFunc(int window, void(func)(int width, int height)) {
	window_setCurrWindow(window);
	glutReshapeFunc(func);
}

void window_setKeyboardUpFunc(int window, void(__stdcall func)(int key)) {
	window_setCurrWindow(window);
	handles[window]->keyUp = &func;
}
void window_setKeyboardDownFunc(int window, void(__stdcall func)(int key)) {
	window_setCurrWindow(window);
	handles[window]->keyDown = &func;
}

void window_setMouseMoveFunc(int window, void(func)(int x, int y)) {
	window_setCurrWindow(window);
	glutMotionFunc(func);
}
void window_setMouseDownFunc(int window, void(func)(int button, int x, int y)) {
	window_setCurrWindow(window);
	handles[window]->mouseDown = func;
}
void window_setMouseUpFunc(int window, void(func)(int button, int x, int y)) {
	window_setCurrWindow(window);
	handles[window]->mouseUp = func;
}

void window_setWindowTitle(int window, char* title) {
	window_setCurrWindow(window);
	glutSetWindowTitle(title);
}

void window_startMainLoop(int window) {
	window_setCurrWindow(window);
	glutMainLoop();
}

void window_test() {
	int a = 0;
	glutInit(&a, {});
    glutInitDisplayMode(GLUT_SINGLE | GLUT_RGB);
    glutInitWindowSize(1000, 250);
    glutInitWindowPosition(100, 100);
    glutCreateWindow("A Simple OpenGL Windows Application with GLUT");
    glutDisplayFunc(displayFunc);
    glutMainLoop();
}

HHOOK hook = NULL;
HWND hwnd = NULL;
