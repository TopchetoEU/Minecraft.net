using System;
using System.Runtime.InteropServices;

namespace NetGL
{
    public class KeyboardEventArgs
    {
        public int Key { get; }

        public KeyboardEventArgs(int key)
        {
            Key = key;
        }
    }

    internal  delegate void KeyboardFunc(int key);

    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    public enum OS
    {
        Windows,
        Linux,
        Mac
    }

    /// <summary>
    /// A simple os detector, dependant on build type. NOTE! Unsupported operating systems fallback to windows
    /// </summary>
    internal static class OSDetector
    {
        #region OS Detection
#if LINUX
        private const OS os = OS.Linux;
        internal const string GraphicsDLL = "MacGL.dll";
#elif MACOS
        private const OS os = OS.Mac;
        internal const string GraphicsDLL = "LinGL.dll";
#else
        private const OS os = OS.Windows;
        internal const string GraphicsDLL = "WinGL.dll";
#endif
        #endregion

        public static string GetOSString(OS os)
        {
            switch (os)
            {
                case OS.Windows: return "WINDOWS";
                case OS.Linux: return "LINUX";
                case OS.Mac: return "MACOS";
                default: return "DEFAULT";
            }
        }
        public static OS CurrentOS => os;
    }

    internal static class LLWindow
    {

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void test();
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setup(string[] args, int length);

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
        public extern static void window_startMainLoop(int window);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_showWindow(int window);
        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_hideWindow(int window);

        [DllImport(OSDetector.GraphicsDLL)]
        public extern static void window_setWindowTitle(int window, string title);
    }
    public class Window: IDisposable
    {
        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;

        public int ID { get; }
        private string title;
        private bool disposedValue;
        public string Title {
            get => title;
            set {
                title = value;
                LLWindow.window_setWindowTitle(ID, title);
            }
        }


        public Window(string title)
        {
            LLWindow.window_setup(new string[0], 0);
            ID = LLWindow.window_createWindow(title);

            this.title = title;
        }
        ~Window()
        {
            Dispose(disposing: false);
        }

        public void Show()
        {
            Console.WriteLine(ID);
            LLWindow.window_setKeyboardDownFunc(ID, KeyDownFunc);
            LLWindow.window_setKeyboardUpFunc(ID, KeyUpFunc);
            LLWindow.window_showWindow(ID);
            LLWindow.window_startMainLoop(ID);
        }
        public void Hide()
        {
            LLWindow.window_hideWindow(ID);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LLWindow.window_destryWindow(ID);
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void KeyDownFunc(int key)
        {
            KeyDown?.Invoke(this, new KeyboardEventArgs(key));
        }
        private void KeyUpFunc(int key)
        {
            KeyUp?.Invoke(this, new KeyboardEventArgs(key));
        }
    }
}
