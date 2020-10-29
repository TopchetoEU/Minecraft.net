
#include <windows.h>
#include <tchar.h>
#include "pch.h"
#include "framework.h"
#include <gl/glew.h>
#include <GL/glfw3native.h>
#include <GL/glfw3.h>
#include <gl/wglew.h>
#include <gl/glut.h>
#include <fstream>
#include <map>
#include "window_api.h"
#include "window_api_experimental.h"
#include <iostream>
#include <sstream>

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

struct window {
public:
	string title = "";
	uint id;
	GLFWwindow* handle;
	bool shown = false;
	bool active = false;

	float fps = 60;

	int x = 0, y = 0;

	ExDisplayFunc* display = nullptr;
	ExResizeFunc* resize = nullptr;

	ExMouseMoveFunc* mouseMove = nullptr;
	ExMouseActionFunc* mouseDown = nullptr;
	ExMouseActionFunc* mouseUp = nullptr;
	ExMouseScrollFunc* scroll = nullptr;

	ExKeyboardFunc* keyDown = nullptr;
	ExKeyboardFunc* keyUp = nullptr;

	window(uint id, GLFWwindow* handle) {
		this->id = id;
		this->handle = handle;
	}
};

map<uint, window*> windows = map<uint, window*>();
window* globalWindow = new window(0, NULL);
WNDCLASSEX* windowClass;

uint nextId = 1;

TCHAR* toWide(const char* source) {
	TCHAR* coverted = new TCHAR[strlen(source) + 1];
	size_t b = 0;
	mbstowcs_s(&b, coverted, strlen(source) + 1, source, _TRUNCATE);

	return coverted;
}
wstring toWide(const string& str)
{
	int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
	std::wstring wstrTo(size_needed, 0);
	MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], size_needed);
	return wstrTo;
}

window* getNonGlobalWindow(uint window) {
	if (window == 0)
		error(L"Can't access the global window from here!").throwError();
	if (windows.find(window) == windows.end())
		error(L"The window id specified doesn't exist!").throwError();

	return windows[window];
}
window* getWindow(uint window) {
	if (window == 0)
		return globalWindow;
	if (windows.find(window) == windows.end())
		throw "The window id specified doesn't exist!";

	return windows[window];
}
wstring GetErrorMessage()
{
	DWORD dw = GetLastError();

	wstringstream displayBuffer;
	displayBuffer << L"Error 0x";
	displayBuffer << toWide(intToString(dw, 16, "0123456789ABCDEF"));

	LPWSTR messageBuffer{};
	if (FormatMessageW(
		FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		0,
		dw,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPWSTR)&messageBuffer,
		0,
		0))
	{
		displayBuffer << L": " << messageBuffer;
		LocalFree(messageBuffer);
	}

	return displayBuffer.str();
}
void dumpLastError() {
	wcout << GetErrorMessage();
	throw;
}

void window_ex_init() {
	glfwInit();
}

window* getWindowByHandle(GLFWwindow* _window) {
	window* wnd = NULL;

	for (auto w : windows) {
		if (w.second->handle == _window) wnd = w.second;
	}

	return wnd;
}

uint window_ex_createWindow(const char* title) {
	uint id = nextId++;
	auto _window = glfwCreateWindow(300, 300, title, NULL, NULL);

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
			glViewport(0, 0, x, y);
			wnd->display();
			glfwSwapBuffers(_window);
		}
	});
	glfwMakeContextCurrent(_window);
	glewInit();

	auto wnd = new window(id, _window);
	wnd->title = string(title);
	wnd->shown = true;

	windows.insert(make_pair(id, wnd));

	return id;
}
void window_ex_destryWindow(uint id)
{
	glfwDestroyWindow(windows[id]->handle);
}

void window_ex_showWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);

	if (!wnd->shown)
		glfwShowWindow(wnd->handle);

	wnd->shown = true;
}
void window_ex_hideWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);

	if (wnd->shown)
		glfwHideWindow(wnd->handle);
	wnd->shown = false;
}

void window_ex_activateMainLoop() {
	bool active = true;
	while (active) {
		active = false;
		for (auto wnd : windows) {
			if (wnd.second->active) {
				if (glfwWindowShouldClose(wnd.second->handle)) wnd.second->active = false;
				else {
					active = true;

					glfwMakeContextCurrent(wnd.second->handle);
					if (wnd.second->display != nullptr) wnd.second->display();
					glfwSwapBuffers(wnd.second->handle);
					glfwPollEvents();
				}
			}
		}
	}
}
void window_ex_activateWindow(uint id) {
	auto wnd = getNonGlobalWindow(id);
	wnd->active = true;
}

void window_ex_setDisplayFunc(uint id, ExDisplayFunc* func) {
	if (id == 0 && func == nullptr) throw "Can't set default display function to null!";

	auto a = getWindow(id);
	a->display = func;
}
void window_ex_setResizeFunc(uint id, ExResizeFunc* func)
{
	getWindow(id)->resize = func;
}

void window_ex_setMouseMoveFunc(uint id, ExMouseMoveFunc* func)
{
	getWindow(id)->mouseMove = func;
}

void window_ex_setMouseDownFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->mouseDown = func;
}
void window_ex_setMouseUpFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->mouseUp = func;
}
void window_ex_setScrollFunc(uint id, ExMouseActionFunc* func)
{
	getWindow(id)->scroll = func;
}

void window_ex_setWindowTitle(uint id, const char* title)
{
	glfwSetWindowTitle(getWindow(id)->handle, title);
}

void window_ex_setWindowSize(uint id, int w, int h)
{
	glfwSetWindowSize(getWindow(id)->handle, w, h);
}

float window_ex_getRefreshRate(uint id)
{
	return getWindow(id)->fps;
}

void window_ex_setKeydownFunc(uint id, ExKeyboardFunc* func) {
	getWindow(id)->keyDown = func;
}
void window_ex_setKeyupFunc(uint id, ExKeyboardFunc* func) {
	getWindow(id)->keyUp = func;
}

void abc() {
	uint a = window_ex_createWindow("test");
	window_ex_showWindow(a);
	window_ex_activateWindow(a);
}
