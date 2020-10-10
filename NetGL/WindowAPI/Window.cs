using NetGL.GraphicsAPI;
using System;

namespace NetGL.WindowAPI
{
    public class Window: IDisposable
    {
        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;
        public event EventHandler Display;
        public event EventHandler Loaded;

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

        public Graphics Graphics { get; }

        public Window(string title)
        {
            LLWindow.window_setup(new string[0], 0);
            ID = LLWindow.window_createWindow(title);

            this.title = title;
            Graphics = new Graphics(this);
        }
        ~Window()
        {
            Dispose(disposing: false);
        }

        struct TestElement
        {
            public Point2 Point { get; set; }
            public Point3 Color { get; set; }

            public TestElement(Point2 point, Point3 color)
            {
                Point = point;
                Color = color;
            }
        }

        public PointI2 ScreenToClient(PointI2 point)
        {
            int x = point.X;
            int y = point.Y;
            LLWindow.window_screenToClient((uint)ID, ref x, ref y);

            return new PointI2(x, y);
        }
        public PointI2 ClientToScreen(PointI2 point)
        {
            int x = point.X;
            int y = point.Y;
            LLWindow.window_clientToScreen((uint)ID, ref x, ref y);

            return new PointI2(x, y);
        }

        public PointI2 SpaceToClient(Point2 point)
        {
            float x = point.X;
            float y = point.Y;

            LLWindow.window_spaceToClient((uint)ID, ref x, ref y);

            return new PointI2((int)x, (int)y);
        }
        public Point2 ClientToSpace(PointI2 point)
        {
            float x = point.X;
            float y = point.Y;
            LLWindow.window_clientToSpace((uint)ID, ref x, ref y);

            return new Point2(x, y);
        }

        public void Show()
        {
            LLWindow.window_setDisplayFunc(ID, DisplayFunc);
            LLWindow.window_setKeyboardDownFunc(ID, KeyDownFunc);
            LLWindow.window_setKeyboardUpFunc(ID, KeyUpFunc);

            Loaded?.Invoke(this, new EventArgs());

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
            KeyDown?.Invoke(this, new KeyboardEventArgs((Key)key));
        }
        private void KeyUpFunc(int key)
        {
            KeyUp?.Invoke(this, new KeyboardEventArgs((Key)key));
        }

        private void DisplayFunc()
        {
            Display?.Invoke(this, new EventArgs());
        }

        public void Use()
        {
            LLWindow.window_setCurrWindow(ID);
        }
    }
}
