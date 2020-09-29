#include "WinGL.h"
#include "pch.h"
#include "glut.h"
#include <gl/GL.h>
#include <gl/GLU.h>
#include "framework.h"

typedef void(MouseActionFunc)(int button, int x, int y);

MouseActionFunc *mouseDown;
MouseActionFunc *mouseUp;

void mf(int button, int state, int x, int y) {
	if (state == GLUT_DOWN && mouseDown != nullptr)  mouseUp(button, x, y);
	else if (state == GLUT_UP && mouseUp != nullptr) mouseDown(button, x, y);
}

void setup(char** args, int length) {
	glutInit(&length, args);
	glutMouseFunc(mf);
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
	return glutCreateWindow(title);
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
	setCurrWindow(window);
	glutDisplayFunc(func);
}
void setResizeFunc(int window, void(func)(int width, int height)) {
	setCurrWindow(window);
	glutReshapeFunc(func);
}

void setKeyboardUpFunc(int window, void(func)(unsigned char key, int x, int y)) {
	setCurrWindow(window);
	glutKeyboardUpFunc(func);
}
void setKeyboardDownFunc(int window, void(func)(unsigned char key, int x, int y)) {
	setCurrWindow(window);
	glutKeyboardFunc(func);
}

void setMouseMoveFunc(int window, void(func)(int x, int y)) {
	setCurrWindow(window);
	glutMotionFunc(func);
}
void setMouseDownFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	mouseDown = func;
}
void setMouseUpFunc(int window, void(func)(int button, int x, int y)) {
	setCurrWindow(window);
	mouseUp = func;
}

void setWindowTitle(int window, char* title) {
	setCurrWindow(window);
	glutSetWindowTitle(title);
}

void startMainLoop(int window) {
	setCurrWindow(window);
	glutMainLoop();
}