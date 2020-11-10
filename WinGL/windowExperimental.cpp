#include "pch.h"
#include "gl/glew.h"
#include "framework.h"
#include "window_api_experimental.h"
#include "graphics_api.h"
#include <map>
#include <iostream>
#include <sstream>
#include <chrono>

#define out

using namespace std;

class error {
public:
	wstring message;
	void throwError() {
		wcout << "An exception was thrown: " << message << endl;
		throw message;
	}

	error(wstring err) {
		message = err;
	}
};

window::window(uint id) {
	this->id = id;
	this->attachedGraphics = nullptr;
}
const char* uintToString(uint num, int radix, const char* digits) {
	uint len = 0;
	if (num == 0) return "0";

	for (uint tempNum = num; num > 0; num /= radix) len++;

	char* stringified = new char[len];

	for (uint i = 0; num > 0; i++)
	{
		stringified[len - 1] = digits[num % 10];

		num /= 10;
	}

	return stringified;
}

map<uint, window*> windows = map<uint, window*>();

bool initialised = false;

uint nextId = 1;

window* getWindow(uint window) {
	if (window == 0)
		error(L"The window id must be a whole number, bigger than 0").throwError();
	if (windows.find(window) == windows.end())
		throw "The window id specified doesn't exist!";

	return windows[window];
}
window* getNonGlobalWindow(uint window) {
	return getWindow(window);
}

void window_setup() {
	if (!initialised)
		glfwInit();
	initialised = true;
}

window* getWindowByHandle(GLFWwindow* _window) {
	window* wnd = NULL;

	for (auto w : windows) {
		if (w.second->handle == _window && w.second->active) {
			wnd = w.second;
		}
	}

	return wnd;
}

void setCurrWindowContext(window* wnd) {
	glfwMakeContextCurrent(wnd->handle);
	setCurrContext(wnd->attachedGraphics);
}

void initWindowEvents(window* wnd) {
	auto _window = wnd->handle;

	glfwSetKeyCallback(_window, [](GLFWwindow* _window, int key, int scancode, int action, int mods) -> void {
		auto wnd = getWindowByHandle(_window);

		if (wnd) switch (action) {
		case 1:
			if (wnd->keyDown != nullptr) wnd->keyDown(key);
		case 0:
			if (wnd->keyUp != nullptr) wnd->keyUp(key);
		}
		});
	glfwSetMouseButtonCallback(_window, [](auto* _window, int key, int action, int smth) {
		window* wnd = getWindowByHandle(_window);
		if (wnd) {
			switch (action)
			{
			case GLFW_PRESS: if (wnd->mouseDown != nullptr)  wnd->mouseDown(key, wnd->x, wnd->y);
			case GLFW_RELEASE: if (wnd->mouseUp != nullptr)  wnd->mouseUp(key, wnd->x, wnd->y);
			}
		}
		});
	glfwSetScrollCallback(_window, [](auto* _window, double offsetX, double offsetY) {
		window* wnd = getWindowByHandle(_window);
		if (wnd) {
			if (wnd->scroll != nullptr) wnd->scroll(wnd->x, wnd->y, (int)(offsetX + offsetY));
		}
		});
	glfwSetCursorPosCallback(_window, [](auto* _window, double x, double y) {
		window* wnd = getWindowByHandle(_window);
		if (wnd) {
			wnd->x = (int)x;
			wnd->y = (int)y;
			if (wnd->mouseMove != nullptr) wnd->mouseMove((int)x, (int)y);
		}
		});
	glfwSetWindowSizeCallback(_window, [](auto* _window, int x, int y) {
		window* wnd = getWindowByHandle(_window);
		if (wnd) {
			setCurrWindowContext(wnd);
			glViewport(0, 0, x, y);
			if (wnd->resize != nullptr) wnd->resize(x, y);
			if (wnd->display != nullptr) wnd->display();
			glfwSwapBuffers(_window);
		}
		});
}
void initWindowGraphics(window* wnd) {
	auto _window = wnd->handle;
	glfwMakeContextCurrent(_window);

	glewExperimental = GL_TRUE;
	auto err = glewInit();
	if (err != GLEW_OK) {
		cout << glewGetErrorString(err);
		throw err;
	}

	setCurrContext(wnd->attachedGraphics);
}
void initWindow(window* wnd) {
	auto _window = glfwCreateWindow(300, 300, wnd->title.c_str(), NULL, NULL);

	wnd->handle = _window;

	initWindowEvents(wnd);
	initWindowGraphics(wnd);
	glfwSwapInterval(0);
}

uint window_createWindow(const char* title) {
	uint id = nextId++;

	window_setup();

	auto wnd = new window(id);

	wnd->attachedGraphics = new graphics();
	wnd->attachedGraphics->g_initialised = true;
	wnd->title = string(title);
	wnd->shown = true;

	initWindow(wnd);

	windows.insert(make_pair(id, wnd));

	return id;
}
void window_destryWindow(uint id)
{
	glfwDestroyWindow(windows[id]->handle);
	windows.erase(id);
}

void window_showWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);

	if (!wnd->shown)
		glfwShowWindow(wnd->handle);

	wnd->shown = true;
}
void window_hideWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);

	if (wnd->shown)
		glfwHideWindow(wnd->handle);
	wnd->shown = false;
	wnd->active = false;
}

bool window_setVSync(uint id, bool vsync) {
	auto wnd = getWindow(id);
	wnd->vsync = vsync;
	setCurrWindowContext(wnd);
	glfwWindowHint();
}

void window_activateMainLoop() {
	bool active = true;
	while (active) {
		active = false;
		for (auto wnd : windows) {
			if (wnd.second->active) {
				if (glfwWindowShouldClose(wnd.second->handle)) {
					wnd.second->active = false;
					window_hideWindow(wnd.first);
				}
				else {
					typedef chrono::high_resolution_clock clock;
					auto start = clock::now();

					active = true;

					setCurrWindowContext(wnd.second);
					if (wnd.second->display != nullptr) wnd.second->display();
					glfwSwapBuffers(wnd.second->handle);

					auto end = clock::now();

					wnd.second->actualTime = chrono::duration_cast<chrono::milliseconds>(end - start).count();
				}
			}
		}
		glfwPollEvents();
	}
}
void window_activateWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);
	wnd->active = true;
}

void window_setDisplayFunc(uint id, ExDisplayFunc* func) {
	if (id == 0 && func == nullptr) throw "Can't set default display function to null!";

	auto a = getWindow(id);
	setCurrWindowContext(a);
	a->display = func;
}
void window_setResizeFunc(uint id, ExResizeFunc* func)
{
	getWindow(id)->resize = func;
}

void window_setMouseMoveFunc(uint id, ExMouseMoveFunc* func)
{
	getWindow(id)->mouseMove = func;
}
void window_setMouseDownFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->mouseDown = func;
}
void window_setMouseUpFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->mouseUp = func;
}
void window_setScrollFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->scroll = func;
}

void window_setWindowTitle(uint id, const char* title)
{
	glfwSetWindowTitle(getWindow(id)->handle, title);
}

void window_setWindowSize(uint id, int w, int h)
{
	glfwSetWindowSize(getWindow(id)->handle, w, h);
}
void window_getWindowSize(uint id, int* w, int* h) {
	glfwGetWindowSize(getWindow(id)->handle, w, h);
}
float window_getRefreshRate(uint id)
{
	return getWindow(id)->fps;
}
void window_setRefreshRate(uint id, float fps) {
	getWindow(id)->fps = fps;
}

void window_setKeydownFunc(uint id, ExKeyboardFunc* func) {
	getWindow(id)->keyDown = func;
}
void window_setKeyupFunc(uint id, ExKeyboardFunc* func) {
	getWindow(id)->keyUp = func;
}
