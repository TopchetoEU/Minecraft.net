﻿using NetGL.GraphicsAPI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetGL.WindowAPI
{
    public class Window: IDisposable
    {
        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;
        public event EventHandler Display;
        public event EventHandler Loaded;

        public bool ConstantRefresh { get; set; }
        private string title;
        private bool disposedValue;
        private VectorI2 size;

        public bool Opened { get; private set; }
        public int ID { get; private set; } = -1;
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
        } 
        ~Window()
        {
            Dispose(disposing: false);
        }

        struct TestElement
        {
            public Vector2 Point { get; set; }
            public Vector3 Color { get; set; }

            public TestElement(Vector2 point, Vector3 color)
            {
                Point = point;
                Color = color;
            }
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
            LLWindow.window_setup(new string[0], 0);
            ID = LLWindow.window_createWindow(title);

            Graphics = new Graphics(this);

            Use();

            LLWindow.window_setWindowSize(ID, size.X, size.Y);
            LLWindow.window_setDisplayFunc(ID, DisplayFunc);
            LLWindow.window_setKeyboardDownFunc(ID, KeyDownFunc);
            LLWindow.window_setKeyboardUpFunc(ID, KeyUpFunc);

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
            KeyDown?.Invoke(this, new KeyboardEventArgs((Key)key));
        }
        private void KeyUpFunc(int key)
        {
            KeyUp?.Invoke(this, new KeyboardEventArgs((Key)key));
        }

        private void DisplayFunc()
        {
            Use();
            //Graphics.Clear();
            Display?.Invoke(this, new EventArgs());
            Graphics.SwapBuffers();
        }

        public void Use()
        {
            LLWindow.window_setCurrWindow(ID);
        }

        public static void BeginLoop()
        {
            LLWindow.window_startMainLoop();
        }
    }
}
