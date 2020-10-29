	#include "pch.h"
#include "window_api.h"
#include "framework.h"
#include <iostream>
#include <gl/glut.h>
#include <fstream>
#include <map>

using namespace std;

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
	MouseActionFunc* mouseScroll = nullptr;
	MouseMovementFunc* mouseMove = nullptr;

	KeyboardActionFunc* keyUp = nullptr;
	KeyboardActionFunc* keyDown = nullptr;

	ActionFunc* display = nullptr;

	bool constantRefresh = false;
	float fps = 60;

	string title = "";

	window(HWND handle, string title) {
		this->handle = handle;
		this->title = title;
	}

	bool operator==(window a) {
		return handle == a.handle;
	}
};

bool initialised = false;
HHOOK keyboardHandle;
HHOOK mouseHandle;
map<int, window*> handles = map<int, window*>();

uint currWindowId = 0;

string intToString(uint value, int radix, const char* alphabet) {
	string a = "";

	if (value == 0) {
		a += alphabet[0];
		return a;
	}

	while (value > 0) {
		a = alphabet[value % radix] + a;
		value /= radix;
	}

	return a;
}

class throwable {
public:
	virtual string getName() = 0;
	virtual string getMessage() = 0;
	virtual uint getCode() = 0;
	void throwError() {
		cout << getName() << " (0x" << intToString(getCode(), 16, "0123456789ABCDEF") << ") threw with a message of: " << getMessage() << endl;

		SetLastError(getCode());

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
class windowDeleteError : public throwable {
private:
	string message = "";
public:
	string getName() { return "windowDeleteError"; }
	string getMessage() { return message; }
	void setMessage(string value) { message = value; }

	uint getCode() { return 0xFFFF0001; }

	windowDeleteError() { message = "A invincible window was deleted"; }
	windowDeleteError(string mesage) { this->message = message; }
};
class invalidIDError : public throwable {
private:
	string message = "";
public:
	string getName() { return "invalidIDError"; }
	string getMessage() { return message; }
	void setMessage(string value) { message = value; }

	uint getCode() { return 0xFFFF0002; }

	invalidIDError() { message = "An invalid id was given"; }
	invalidIDError(string mesage) { this->message = message; }
};

long __stdcall keyboardFunction(int code, WPARAM w, LPARAM l) {
	if (code < 0)
		return CallNextHookEx(keyboardHandle, code, w, l);
	else {
		auto* data = (tagKBDLLHOOKSTRUCT*)l;

		auto process = [](tagKBDLLHOOKSTRUCT* data, window* wnd, uint w) -> void {
			KeyboardActionFunc* func = NULL;

			if (w == 256) func = *wnd->keyDown;
			if (w == 257) func = *wnd->keyUp;

			if (func != NULL) func(data->vkCode);
		};

		for (const auto& pair : handles) {
			window* wnd = pair.second;
			if (wnd->handle != 0 && wnd->handle == GetActiveWindow()) process(data, wnd, w);
		}

		process(data, handles[0], w);

		return NULL;
	}
}
long __stdcall mouseFunction(int code, WPARAM w, LPARAM l) {
	if (code < 0)
		return CallNextHookEx(mouseHandle, code, w, l);
	else {
		MSLLHOOKSTRUCT* data = (MSLLHOOKSTRUCT*)l;

		auto process = [](MSLLHOOKSTRUCT* data, window* wnd, uint w) -> void {
			int button = 0;
			int actionType = 0;
			int delta = data->mouseData;

			int x = data->pt.x;
			int y = data->pt.y;

			switch (w)
			{
			case WM_LBUTTONDOWN: actionType = 1; button = 1; break;
			case WM_LBUTTONUP:   actionType = 2; button = 1; break;

			case WM_RBUTTONDOWN: actionType = 1; button = 2; break;
			case WM_RBUTTONUP:   actionType = 2; button = 2; break;

			case WM_MBUTTONDOWN: actionType = 1; button = 3; break;
			case WM_MBUTTONUP:   actionType = 2; button = 3; break;

			case WM_MOUSEWHEEL:  actionType = 3; button = 3; break;
			}

			switch (actionType)
			{
			case 0: if (wnd->mouseDown != NULL) wnd->mouseDown(button, x, y);
			case 1: if (wnd->mouseUp != NULL) wnd->mouseUp(button, x, y);
			case 2: if (wnd->mouseMove != NULL) wnd->mouseMove(x, y);
			case 3: if (wnd->mouseScroll != NULL) wnd->mouseScroll(delta, x, y);
			default:
				break;
			}
		};

		for (const auto& pair : handles) {
			window* wnd = pair.second;
			if (wnd->handle == GetActiveWindow()) {
				RECT rect{ 0, 0, 0, 0 };
				GetWindowRect(wnd->handle, &rect);

				if (wnd->handle == GetActiveWindow() && PtInRect(&rect, data->pt)) process(data, wnd, w);
			}
		}

		process(data, handles[0], w);

		return NULL;
	}
}

void window_setup(char** args, int length) {
	if (!initialised) {
		keyboardHandle = SetWindowsHookExA(WH_KEYBOARD_LL, keyboardFunction, NULL, NULL);
		mouseHandle = SetWindowsHookExA(WH_MOUSE_LL, mouseFunction, NULL, NULL);

		if (keyboardHandle == NULL)
			genericError("Unable to initialise keyboard listener!, Internal error code: " + GetLastError()).throwError();
		if (mouseHandle == NULL)
			genericError("Unable to initialise mouse listener!, Internal error code: " + GetLastError()).throwError();

		glutInit(&length, args);

		handles.insert(make_pair(0, new window(NULL, "global")));

		initialised = true;
	}
}
void window_setup() {
	window_setup(0, {});
}

int window_getCurrWindow() {
	currWindowId = glutGetWindow();
	return glutGetWindow();
}
void window_setCurrWindow(int window) {
		glutSetWindow(window);
}

int window_createWindow(char* title) {
	int id = glutCreateWindow(title);
	HWND handle = GetForegroundWindow();
	window* wnd = new window(handle, title);

	handles.insert(std::make_pair(id, wnd));

	glutDisplayFunc([]() -> void {
		cout << glutGetWindow();
		handles[glutGetWindow()]->display();
	});
	return id;
}
void window_destroyWindow(int window) {
	if (currWindowId == 0) {
		windowDeleteError("An attempt was made to delete the global window (ID - 0)").throwError();
	}
	if (currWindowId < 0) {
		windowDeleteError("An attempt was made to delete window with a negative ID").throwError();
	}
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

void window_setDisplayFunc(int window, ActionFunc* func) {
	handles[window]->display = func;
}
void window_setResizeFunc(int window, void(*func)(int width, int height)) {
	window_setCurrWindow(window);
	glutReshapeFunc(func);
}

void window_setKeyboardUpFunc(int window, KeyboardActionFunc* func) {
	if (window < 0) invalidIDError("A negative number was given as an id").throwError();
	handles[window]->keyUp = func;
}
void window_setKeyboardDownFunc(int window, KeyboardActionFunc* func) {
	handles[window]->keyDown = func;
}

void window_setMouseScrollFunc(int window, MouseActionFunc func) {
	handles[window]->mouseScroll = *func;
}
void window_setMouseMoveFunc(int window, MouseMovementFunc func) {
	handles[window]->mouseMove = *func;
}
void window_setMouseDownFunc(int window, MouseActionFunc func) {
	handles[window]->mouseDown = *func;
}
void window_setMouseUpFunc(int window, MouseActionFunc func) {
	handles[window]->mouseUp = *func;
}

string window_getWindowTitle(int window) {
	window_setCurrWindow(window);
	return handles[window]->title;
}
void window_setWindowTitle(int window, char* title) {
	window_setCurrWindow(window);
	glutSetWindowTitle(title);
	handles[window]->title = title;
}
void window_setWindowSize(int window, int w, int h) {
	window_setCurrWindow(window);
	glutReshapeWindow(w, h);
}

void window_startMainLoop() {
	glutMainLoop();
}

void window_test() {
	int a = 0;
	glutInit(&a, {});
	glutInitDisplayMode(GLUT_SINGLE | GLUT_RGB);
	glutInitWindowSize(1000, 250);
	glutInitWindowPosition(100, 100);
	glutCreateWindow("A Simple OpenGL Windows Application with GLUT");
	//glutDisplayFunc(displayFunc);
	glutMainLoop();
}

void window_getMousePosition(int* x, int* y) {
	POINT p{ 0, 0 };
	GetCursorPos(&p);

	int _x = ((int)p.x);
	int _y = ((int)p.y);

	x = &_x;
	y = &_y;
}
void window_setMousePosition(int x, int y) {
	SetCursorPos(x, y);
}
void window_screenToClient(uint wnd, int& x, int& y) {
	POINT newPt{ x, y };

	ScreenToClient(handles[wnd]->handle, &newPt);

	int newX = (int)newPt.x;
	int newY = (int)newPt.y;

	x = newX;
	y = newY;
}
void window_clientToScreen(uint wnd, int& x, int& y) {
	POINT newPt{ (long)x, (long)y };

	ClientToScreen(handles[wnd]->handle, &newPt);

	int newX = (int)newPt.x;
	int newY = (int)newPt.y;

	x = newX;
	y = newY;
}
void window_spaceToClient(uint wnd, float& x, float& y) {
	RECT newRect{ 0, 0, 1, 1 };

	GetWindowRect(handles[wnd]->handle, &newRect);

	float w = newRect.right - newRect.left;
	float h = newRect.bottom - newRect.top;

	float newX = (x + 1) / 2 * w;
	float newY = (-y + 1) / 2 * h;

	x = newX;
	y = newY;
}
void window_clientToSpace(uint wnd, float& x, float& y) {
	RECT newRect{ 0, 0, 1, 1 };

	GetWindowRect(handles[wnd]->handle, &newRect);

	float w = newRect.right - newRect.left;
	float h = newRect.bottom - newRect.top;

	float newX = x / w * 2 - 1;
	float newY = -(y / h * 2 - 1);

	x = newX;
	y = newY;
}

float window_getRefreshRate(int window) {
	return handles[window]->fps;
}
void window_setRefreshRate(int window, float fps) {
	handles[window]->fps = fps;
}

void window_setConstantRefresh(int window, bool refresh) {
	handles[window]->constantRefresh = refresh;
}
bool window_getConstantRefresh(int window) {
	return handles[window]->constantRefresh;
}