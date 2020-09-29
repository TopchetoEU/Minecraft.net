using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace NetGL
{
    public class KeyboardEventArgs
    {
        public char Key { get; }
        public int X { get; }
        public int Y { get; }

        public KeyboardEventArgs(char key, int x, int y)
        {
            Key = key;
            X = x;
            Y = y;
        }
    }
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    public class Window: IDisposable
    {
        #region OS detection
#if (LINUX)
        private const string dllName = "LinGL.dll";
#else
        private const string dllName = "WinGL.dll";
#endif
        #endregion

        private delegate void KeyActionFunc(char key, int x, int y);

        internal static bool Loaded = false;
        private string title;
        private bool disposedValue;

        public int ID { get; }
        public string Title {
            get => title;
            set {
                setWindowTitle(ID, value);
                title = value;
            }
        }

        public event KeyboardEventHandler KeyPressed;
        public event KeyboardEventHandler KeyReleased;

        [DllImport(dllName)]
        private static extern void setup();

        [DllImport(dllName)]
        private static extern void setKeyboardUpFunc(KeyActionFunc func);
        [DllImport(dllName)]
        private static extern void setKeyboardDownFunc(KeyActionFunc func);
        [DllImport(dllName)]
        private static extern int createWindow(string title);
        [DllImport(dllName)]
        private static extern void destroyWindow(int window);
        [DllImport(dllName)]
        private static extern void setWindowTitle(int window, string title);
        [DllImport(dllName)]
        private static extern void startMainLoop(int window);
        [DllImport(dllName)]
        private static extern void showWindow(int window);
        [DllImport(dllName)]
        private static extern void hideWindow(int window);

        private void KeyDown(char key, int x, int y) => KeyPressed?.Invoke(this, new KeyboardEventArgs(key, x, y));
        private void KeyUp(char key, int x, int y) => KeyReleased?.Invoke(this, new KeyboardEventArgs(key, x, y));


        public Window(string title)
        {
            if (!Loaded) setup();
            ID = createWindow(title);

            setKeyboardUpFunc(KeyUp);
            setKeyboardDownFunc(KeyDown);
        }

        public void Show()
        {
            showWindow(ID);
            startMainLoop(ID);
        }
        public void Hide()
        {
            hideWindow(ID);
        }

        #region Disposing
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    destroyWindow(ID);
                }

                disposedValue = true;
            }
        }
        ~Window()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
