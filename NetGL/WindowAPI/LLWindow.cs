using System;
using System.Runtime.InteropServices;

namespace NetGL.WindowAPI
{
    internal delegate void KeyboardFunc(int key);
    internal delegate void MouseActionFunc(int data, int x, int y);
    internal delegate void MouseMoveFunc(int x, int y);

    internal static class LLWindow
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
        public extern static void window_startMainLoop(int window);


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_showWindow(int window);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_hideWindow(int window);


        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setWindowTitle(int window, string title);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static string window_getWindowTitle(int window);

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
}
