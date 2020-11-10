using NetGL.GraphicsAPI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetGL.WindowAPI
{
    public class Window: IDisposable
    {
        public event KeyboardEventHandler KeyPressed;
        public event KeyboardEventHandler KeyReleased;
        public event EventHandler Display;
        public event EventHandler Loaded;
        public event ResizeEventHandler SizeChanged;

        public bool ConstantRefresh { get; set; }
        private string title;
        private bool disposedValue;
        private VectorI2 size;

        public bool Opened { get; private set; }
        public uint ID { get; private set; } = 0;
        public VectorI2 Size {
            get => size;
            set {
                size = value;
                if (Opened) LLWindow.window_setWindowSize(ID, size.X, size.Y);
            }
        }
        public string Title {
            get => title;
            set {
                title = value;
                if (Opened)
                    LLWindow.window_setWindowTitle(ID, title);
            }
        }

        public Graphics Graphics { get; private set; }

        public Window(string title) : this(title, new VectorI2(300, 300)) { }
        public Window(string title, VectorI2 size)
        {
            this.title = title;
            this.size = size;
            DisplayFunc = () =>
            {
                Display?.Invoke(this, new EventArgs());
            };
            KeydownFunc = KeyDownFunc;
            KeyupFunc = KeyUpFunc;
            ResizeFunc = (w, h) => {
                SizeChanged?.Invoke(this, new ResizeEventArgs(w, h));
            };

            SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, ResizeEventArgs e)
        {
            size = e.Size;
        }

        ~Window()
        {
            Dispose(disposing: false);
        }

        public VectorI2 ScreenToClient(VectorI2 point)
        {
            int x = point.X;
            int y = point.Y;
            LLWindow.window_screenToClient((uint)ID, ref x, ref y);

            return new VectorI2(x, y);
        }
        public VectorI2 ClientToScreen(VectorI2 point)
        {
            int x = point.X;
            int y = point.Y;
            LLWindow.window_clientToScreen((uint)ID, ref x, ref y);

            return new VectorI2(x, y);
        }

        public VectorI2 SpaceToClient(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            LLWindow.window_spaceToClient((uint)ID, ref x, ref y);

            return new VectorI2((int)x, (int)y);
        }
        public Vector2 ClientToSpace(VectorI2 point)
        {
            float x = point.X;
            float y = point.Y;
            LLWindow.window_clientToSpace((uint)ID, ref x, ref y);

            return new Vector2(x, y);
        }


        public void Show()
        {
            LLWindow.window_setup();
            ID = LLWindow.window_createWindow(title);

            Graphics = new Graphics(this);

            LLWindow.window_activateWindow((uint)ID);

            LLWindow.window_setWindowSize(ID, size.X, size.Y);

            LLWindow.window_setDisplayFunc(ID, DisplayFunc);
            LLWindow.window_setResizeFunc(ID, ResizeFunc);
            LLWindow.window_setKeydownFunc(ID, KeydownFunc);
            LLWindow.window_setKeyupFunc(ID, KeyupFunc);

            Opened = true;

            Loaded?.Invoke(this, new EventArgs());
        }
        public void ShowAsMain()
        {
            Show();
            BeginLoop();
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
            KeyPressed?.Invoke(this, new KeyboardEventArgs((Key)key));
        }
        private void KeyUpFunc(int key)
        {
            KeyReleased?.Invoke(this, new KeyboardEventArgs((Key)key));
        }

        Action DisplayFunc;
        KeyboardFunc KeydownFunc;
        KeyboardFunc KeyupFunc;
        ResizeFunc ResizeFunc;

        [Obsolete("This metod won't do anything and it's kept just for backward compatibility")]
        public void Use()
        {
        }

        public static void BeginLoop()
        {
            LLWindow.window_activateMainLoop();
        }
    }
}
