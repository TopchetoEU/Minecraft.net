using System;
using System.Runtime.InteropServices;

namespace NetGL.WindowAPI
{
    public delegate void ExDisplayFunc();
    public delegate void ExKeyboardFunc(int key);
    public delegate void ExResizeFunc(int w, int h);
    public delegate void ExMouseMoveFunc(int x, int y);
    public delegate void ExMouseActionFunc(int action, int x, int y);
    public delegate void ExMouseScrollFunc(int x, int y, int delta);
    public delegate void KeyboardFunc(int key);
    public delegate void MouseActionFunc(int data, int x, int y);
    public delegate void MouseMoveFunc(int x, int y);

    public static class LLWindow
    {

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void test();
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setup(string[] args, int length);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setCurrWindow(int window);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static int window_getCurrWindow();


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static int window_createWindow(string title);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_destryWindow(int window);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setDisplayFunc(int window, Action func);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setKeyboardDownFunc(int window, KeyboardFunc func);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setKeyboardUpFunc(int window, KeyboardFunc func);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setMouseDownFunc(int window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setMouseUpFunc(int window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setMouseScrollFunc(int window, MouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setMouseMoveFunc(int window, MouseMoveFunc func);


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_startMainLoop();


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_showWindow(int window);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_hideWindow(int window);


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setWindowTitle(int window, string title);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static string window_getWindowTitle(int window);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setWindowSize(int window, int w, int h);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_getMousePosition(ref int x, ref int y);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setMousePosition(int x, int y);


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_screenToClient(uint wnd, ref int x, ref int y);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_clientToSpace(uint wnd, ref float x, ref float y);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_spaceToClient(uint wnd, ref float x, ref float y);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_clientToScreen(uint wnd, ref int x, ref int y);

    }

    public static class LLExWindow
    {
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint window_ex_createWindow(string title);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_destryWindow(uint id);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_showWindow(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_hideWindow(uint id);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_activateWindow(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setDisplayFunc(uint id, ExDisplayFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setResizeFunc(uint id, ExResizeFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setMouseMoveFunc(uint id, ExMouseMoveFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setMouseDownFunc(uint id, ExMouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setMouseUpFunc(uint id, ExMouseActionFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setScrollFunc(uint id, ExMouseActionFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setWindowTitle(uint id, string title);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setWindowSize(uint id, int w, int h);

        [DllImport(OSDetector.GraphicsDLL)] public static extern float window_ex_getRefreshRate(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setRefreshRate(uint id, float fps);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setKeydownFunc(uint id, ExKeyboardFunc func);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_setKeyupFunc(uint id, ExKeyboardFunc func);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_activateMainLoop();
        [DllImport(OSDetector.GraphicsDLL)] public static extern void window_ex_init();

    }
}
