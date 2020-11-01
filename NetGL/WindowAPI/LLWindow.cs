using System;
using System.Runtime.InteropServices;

namespace NetGL.WindowAPI
{
    public delegate void ExDisplayFunc();
    public delegate void ExKeyboardFunc(int key);
    public delegate void ResizeFunc(int w, int h);
    public delegate void ExMouseMoveFunc(int x, int y);
    public delegate void ExMouseActionFunc(int action, int x, int y);
    public delegate void ExMouseScrollFunc(int x, int y, int delta);
    public delegate void KeyboardFunc(int key);
    public delegate void MouseActionFunc(int data, int x, int y);
    public delegate void MouseMoveFunc(int x, int y);

    public static class LLWindow
    {

        [DllImport(OSDetector.GraphicsDLL)] public extern static void test();
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setup();


        [DllImport(OSDetector.GraphicsDLL)] public extern static uint window_createWindow(string title);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_destryWindow(uint window);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setDisplayFunc(uint window, Action func);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setKeydownFunc(uint window, KeyboardFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setKeyupFunc(uint window, KeyboardFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setMouseDownFunc(uint window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setMouseUpFunc(uint window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setMouseScrollFunc(uint window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setMouseMoveFunc(uint window, MouseMoveFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setResizeFunc(uint id, ResizeFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_activateWindow(uint wnd);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_activateMainLoop();

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_showWindow(uint window);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_hideWindow(uint window);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setWindowTitle(uint window, string title);
        [DllImport(OSDetector.GraphicsDLL)] public extern static string window_getWindowTitle(uint window);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setWindowSize(uint window, int w, int h);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_getMousePosition(ref int x, ref int y);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_setMousePosition(int x, int y);

        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_screenToClient(uint wnd, ref int x, ref int y);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_clientToSpace(uint wnd, ref float x, ref float y);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_spaceToClient(uint wnd, ref float x, ref float y);
        [DllImport(OSDetector.GraphicsDLL)] public extern static void window_clientToScreen(uint wnd, ref int x, ref int y);

    }

    public static class LLExWindow
    {
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint window_createWindow(string title);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_destryWindow(uint id);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_showWindow(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_hideWindow(uint id);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_activateWindow(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setDisplayFunc(uint id, ExDisplayFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setResizeFunc(uint id, ResizeFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setMouseMoveFunc(uint id, ExMouseMoveFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setMouseDownFunc(uint id, ExMouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setMouseUpFunc(uint id, ExMouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setScrollFunc(uint id, ExMouseActionFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setWindowTitle(uint id, string title);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setWindowSize(uint id, int w, int h);

        [DllImport(OSDetector.GraphicsDLL)] public static extern float window_getRefreshRate(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setRefreshRate(uint id, float fps);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setKeydownFunc(uint id, ExKeyboardFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setKeyupFunc(uint id, ExKeyboardFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_activateMainLoop();
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_setup();

    }
}
