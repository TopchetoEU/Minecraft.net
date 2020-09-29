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
	KeyboardActionFunc keyUp = nullptr;
	KeyboardActionFunc keyDown = nullptr;
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
void mouseFunction(int button, int state, int x, int y) {
	for (const auto& pair : handles) {
		window* wnd = pair.second;
		if      (state == GLUT_DOWN && wnd->mouseDown != nullptr) wnd->mouseDown(button, x, y);
		else if (state == GLUT_UP   && wnd->mouseUp   != nullptr) wnd->mouseUp  (button, x, y);
	}
}
LRESULT CALLBACK keyboardFunction(int code, WPARAM w, LPARAM l) {
	if (code < 0)
		return CallNextHookEx(h, code, w, l);
	else {
		auto* data = (tagKBDLLHOOKSTRUCT *)l;

		cout << "" << data->vkCode << "\n";

		for (const auto& pair : handles) {
			window* wnd = pair.second;
			if (wnd->handle == GetActiveWindow()) {
				KeyboardActionFunc func = NULL;

				if (w == 256) func = wnd->keyDown;
				if (w == 257) func = wnd->keyUp;

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

void setup(char** args, int length) {
	if (!initialised) {
		glutInit(&length, args);
		glutMouseFunc(mouseFunction);

		installhook(GetModuleHandle(NULL));

		initialised = true;
	}
}
void setup() {
	setup(0, {});
}

int getCurrWindow() {
	return glutGetWindow();
}
void setCurrWindow(int window) {
	if (window != getCurrWindow())
		glutSetWindow(window);
}

int createWindow(char* title) {
	int id = glutCreateWindow(title);
	HWND handle = GetForegroundWindow();
	window *wnd = new window(handle);

	handles.insert(std::make_pair( id, wnd ));

	glutDisplayFunc(displayFunc);
	return id;
}
void destroyWindow(int window) {
	glutDestroyWindow(window);
}

void showWindow(int window) {
	setCurrWindow(window);
	glutShowWindow();
}
void hideWindow(int window) {
	setCurrWindow(window);
	glutHideWindow();
}

void setDisplayFunc(int window, void(func)()) {
	if (func == nullptr) {
		glutDisplayFunc(displayFunc);
	}
	else {
		setCurrWindow(window);
		glutDisplayFunc(func);
	}
}
void setResizeFunc(int window, void(func)(int width, int height)) {
	setCurrWindow(window);
	glutReshapeFunc(func);
}

void setKeyboardUpFunc(int window, void(__stdcall func)(int key)) {
	setCurrWindow(window);
	handles[window]->keyUp = func;
}
void setKeyboardDownFunc(int window, void(__stdcall func)(int key)) {
	setCurrWindow(window);
	handles[window]->keyDown = func;
}

void setMouseMoveFunc(int window, void(func)(int x, int y)) {
	setCurrWindow(window);
	glutMotionFunc(func);
}
void setMouseDownFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	handles[window]->mouseDown = func;
}
void setMouseUpFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	handles[window]->mouseUp = func;
}

void setWindowTitle(int window, char* title) {
	setCurrWindow(window);
	glutSetWindowTitle(title);
}

void startMainLoop(int window) {
	setCurrWindow(window);
	glutMainLoop();
}

void test() {
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
