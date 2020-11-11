using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NetGL.WindowAPI
{
    public class Window: IDisposable
    {
        public event KeyboardEventHandler KeyPressed;
        public event KeyboardEventHandler KeyReleased;

        public event CancellableGraphicsEventHandler Drawing;
        public event GraphicsEventHandler Drawn;
        public event GraphicsEventHandler Loaded;

        public event ResizeEventHandler SizeChanged;

        public event MouseEventHandler MouseMoved;
        public event MouseEventHandler MousePressed;
        public event MouseEventHandler MouseReleased;
        public event MouseEventHandler MouseScrolled;

        private VectorI2 mousePosition = new VectorI2(0, 0);
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private string title;
        private bool disposedValue;
        private VectorI2 size;
        private bool cursorLocked = false;
        private bool fullscreen = false;

        public VectorI2 MousePosition {
            get => mousePosition;
            set {
                LLWindow.window_setMousePosition(ID, value.X, value.Y);
            }
        }
        public VectorI2 Size {
            get => size;
            set {
                size = value;
                if (Opened)
                    LLWindow.window_setWindowSize(ID, size.X, size.Y);
            }
        }
        public int Width {
            get => Size.X;
            set => Size = new VectorI2(value, Size.Y);
        }
        public int Height {
            get => Size.Y;
            set => Size = new VectorI2(Size.X, value);
        }
        public string Title {
            get => title;
            set {
                title = value;
                if (Opened)
                    LLWindow.window_setWindowTitle(ID, title);
            }
        }

        public bool Opened { get; private set; }
        public uint ID { get; private set; } = 0;
        public bool IsKeyPressed(Key key) => pressedKeys.Contains(key);

        public float DeltaTime { get; private set; }
        public float FPS { get; private set; } = 0;

        public List<Scene> Scenes { get; } = new List<Scene>();

        public bool LockCursor {
            get => cursorLocked;
            set {
                if (cursorLocked != value) {
                    cursorLocked = value;

                    LLWindow.window_setMouseLocked(ID, value);
                }
            }
        }

        Action DisplayFunc;
        KeyboardFunc KeydownFunc;
        KeyboardFunc KeyupFunc;
        ResizeFunc ResizeFunc;
        MouseMoveFunc MouseMoveFunc;
        MouseActionFunc MousePressFunc;
        MouseActionFunc MouseReleaseFunc;
        MouseActionFunc MouseScrollFunc;

        private Graphics g = new Graphics();

        public Window(string title) : this(title, new VectorI2(300, 300)) { }
        public Window(string title, VectorI2 size)
        {
            this.title = title;
            this.size = size;
            var sw = new Stopwatch();
            sw.Start();
            DisplayFunc = () => {
                DeltaTime = (float)sw.Elapsed.TotalMilliseconds / 1000;
                if (DeltaTime != 0)
                    FPS = 1 / DeltaTime;
                sw.Reset();
                sw.Start();

                var e = new CancellableGraphicsEventArgs(g, false);
                Drawing?.Invoke(this, e);

                if (!e.Cancelled) {
                    foreach (var scene in Scenes) {
                        scene.Draw(g);
                    }
                }
                Drawn?.Invoke(this, new GraphicsEventArgs(g));
            };
            KeydownFunc = (int key) => {
                KeyPressed?.Invoke(this, new KeyboardEventArgs((Key)key));
                pressedKeys.Add((Key)key);
            };
            KeyupFunc = (int key) => {
                KeyReleased?.Invoke(this, new KeyboardEventArgs((Key)key));
                pressedKeys.Remove((Key)key);
            };
            ResizeFunc = (w, h) => {
                Size = new VectorI2(w, h);
                SizeChanged?.Invoke(this, new ResizeEventArgs(Size));
            };
            MouseMoveFunc = (int x, int y) => {
                mousePosition = new VectorI2(x, y);
                MouseMoved?.Invoke(this, new MouseEventArgs(x, y));
            };
            MousePressFunc = (button, x, y) =>
                MousePressed?.Invoke(this, new MouseEventArgs(x, y, (MouseButton)button));
            MouseReleaseFunc = (button, x, y) =>
                MouseReleased?.Invoke(this, new MouseEventArgs(x, y, (MouseButton)button));
            MouseScrollFunc = (delta, x, y) =>
                MouseReleased?.Invoke(this, new MouseEventArgs(x, y, delta));
        }

        public void Show()
        {
            LLWindow.window_setup();
            ID = LLWindow.window_createWindow(title);

            LLWindow.window_activateWindow((uint)ID);

            LLWindow.window_setWindowSize(ID, size.X, size.Y);

            LLWindow.window_setDisplayFunc(ID, DisplayFunc);
            LLWindow.window_setResizeFunc(ID, ResizeFunc);
            LLWindow.window_setKeydownFunc(ID, KeydownFunc);
            LLWindow.window_setKeyupFunc(ID, KeyupFunc);

            LLWindow.window_setMouseMoveFunc(ID, MouseMoveFunc);
            LLWindow.window_setMouseDownFunc(ID, MousePressFunc);
            LLWindow.window_setMouseUpFunc(ID, MouseReleaseFunc);
            LLWindow.window_setScrollFunc(ID, MouseScrollFunc);

            Opened = true;

            Loaded?.Invoke(this, new GraphicsEventArgs(g));
            ;
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

        ~Window()
        {
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
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

        private static void BeginLoop()
        {
            LLWindow.window_activateMainLoop();
        }
    }
}
