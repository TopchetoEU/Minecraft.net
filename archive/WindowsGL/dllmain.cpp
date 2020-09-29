#include "WinGL.h"
#include "pch.h"
#include "glut.h"
#include <gl/GL.h>
#include <gl/GLU.h>
#include "framework.h"

typedef void(MouseActionFunc)(int button, int x, int y);

MouseActionFunc *mouseDown;
MouseActionFunc *mouseUp;

void window_mf(int button, int state, int x, int y) {
	if (state == GLUT_DOWN && mouseDown != nullptr)  mouseUp(button, x, y);
	else if (state == GLUT_UP && mouseUp != nullptr) mouseDown(button, x, y);
}

void window_setup(char** args, int length) {
	glutInit(&length, args);
	glutMouseFunc(mf);
}
void window_setup() {
	setup(0, {});
}

int window_getCurrWindow() {
	return glutGetWindow();
}
void window_setCurrWindow(int window) {
	if (window != getCurrWindow())
		glutSetWindow(window);
}

int window_createWindow(char* title) {
	return glutCreateWindow(title);
}
void window_destroyWindow(int window) {
	glutDestroyWindow(window);
}

void window_showWindow(int window) {
	setCurrWindow(window);
	glutShowWindow();
}
void window_hideWindow(int window) {
	setCurrWindow(window);
	glutHideWindow();
}

void window_setDisplayFunc(int window, void(func)()) {
	setCurrWindow(window);
	glutDisplayFunc(func);
}
void window_setResizeFunc(int window, void(func)(int width, int height)) {
	setCurrWindow(window);
	glutReshapeFunc(func);
}

void window_setKeyboardUpFunc(int window, void(func)(unsigned char key, int x, int y)) {
	setCurrWindow(window);
	glutKeyboardUpFunc(func);
}
void window_setKeyboardDownFunc(int window, void(func)(unsigned char key, int x, int y)) {
	setCurrWindow(window);
	glutKeyboardFunc(func);
}

void window_setMouseMoveFunc(int window, void(func)(int x, int y)) {
	setCurrWindow(window);
	glutMotionFunc(func);
}
void window_setMouseDownFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	mouseDown = func;
}
void window_setMouseUpFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	mouseUp = func;
}

void window_setWindowTitle(int window, char* title) {
	setCurrWindow(window);
	glutSetWindowTitle(title);
}

void window_startMainLoop(int window) {
	setCurrWindow(window);
	glutMainLoop();
}