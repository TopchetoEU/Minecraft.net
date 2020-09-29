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
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    public class Window: IDisposable
    {
        #region OS Detection
#if LINUX
        private const string Dll = "LinGL.dll";
#else
        private const string Dll = "WinGL.dll";
        private bool disposedValue;
#endif
        #endregion

        private delegate void KeyboardFunc(int key);

        public int ID { get; }
        private string title;
        public string Title {
            get => title;
            set {
                title = value;
                setWindowTitle(ID, title);
            }
        }

        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;

        [DllImport(Dll)]
        private extern static void test();
        [DllImport(Dll)]
        private extern static void setup(string[] args, int length);

        [DllImport(Dll)]
        private extern static int createWindow(string title);
        [DllImport(Dll)]
        private extern static void destryWindow(int window);

        [DllImport(Dll)]
        private extern static void setDisplayFunc(int window, Action func);
        [DllImport(Dll)]
        private extern static void setKeyboardDownFunc(int window, KeyboardFunc func);
        [DllImport(Dll)]
        private extern static void setKeyboardUpFunc(int window, KeyboardFunc func);

        [DllImport(Dll)]
        private extern static void startMainLoop(int window);

        [DllImport(Dll)]
        private extern static void showWindow(int window);
        [DllImport(Dll)]
        private extern static void hideWindow(int window);

        [DllImport(Dll)]
        private extern static void setWindowTitle(int window, string title);

        public Window(string title)
        {
            setup(new string[0], 0);
            ID = createWindow(title);

            this.title = title;
        }
        ~Window()
        {
            Dispose(disposing: false);
        }

        public void Show()
        {
            Console.WriteLine(ID);
            setKeyboardDownFunc(ID, KeyDownFunc);
            setKeyboardUpFunc(ID, KeyUpFunc);
            showWindow(ID);
            startMainLoop(ID);
        }
        public void Hide()
        {
            hideWindow(ID);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    destryWindow(ID);
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
