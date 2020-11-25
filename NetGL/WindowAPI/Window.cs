using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGL.WindowAPI
{
    public class Window: IDisposable
    {
        public event CancellableGraphicsEventHandler Drawing;
        public event GraphicsEventHandler Drawn;
        public event GraphicsEventHandler Loaded;

        public event ResizeEventHandler SizeChanged;

        public Keyboard Keyboard { get; } = new Keyboard();
        public Mouse Mouse { get; }

        private string title;
        private bool disposedValue;
        private VectorI2 size;
        private bool cursorLocked = false;

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

        public float DeltaTime { get; private set; }
        public float FPS { get; private set; } = 0;

        public unsafe void Fullscreen(Monitor monitor)
        {
            LLWindow.window_fullscreen(ID, (void*)monitor.ID, null);
        }
        public unsafe void Fullscreen(Monitor monitor, VideoMode mode)
        {
            var a = mode._mode;
            LLWindow.window_fullscreen(ID, (void*)monitor.ID, &a);
        }
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
                Keyboard.PressKey((Key)key);
            };
            KeyupFunc = (int key) => {
                Keyboard.ReleaseKey((Key)key);
            };
            ResizeFunc = (w, h) => {
                Size = new VectorI2(w, h);
                foreach (var scene in Scenes) {
                    scene.ViewRation = w / (float)h;
                }
                SizeChanged?.Invoke(this, new ResizeEventArgs(Size));
            };
            MouseMoveFunc = (int x, int y) => {
                Mouse.RegisterMove(new VectorI2(x, y));
            };
            MousePressFunc = (button, x, y) => {
                MouseMoveFunc(x, y);
                Mouse.RegisterPress((MouseButton)button);
            };
            MouseReleaseFunc = (button, x, y) => {
                MouseMoveFunc(x, y);
                Mouse.RegisterRelease((MouseButton)button);
            };
            MouseScrollFunc = (delta, x, y) => {
                MouseMoveFunc(x, y);
                Mouse.RegisterScroll(delta);
            };

            Mouse = new Mouse((x, y) => LLWindow.window_setMousePosition(ID, x, y));
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

    public struct Monitor
    {
        public int ID { get; }

        internal Monitor(int id)
        {
            ID = id;
        }

        public static bool operator ==(Monitor a, Monitor b) => a.Equals(b);
        public static bool operator !=(Monitor a, Monitor b) => !a.Equals(b);

        public override bool Equals(object obj)
        {
            return obj is Monitor monitor &&
                   ID == monitor.ID;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public static Monitor[] Monitors {
            get {
                unsafe {
                    LLWindow.window_setup();

                    int count = 0;
                    var array = LLWindow.window_getMonitors(ref count);

                    var monitors = new Monitor[count];

                    for (int i = 0; i < count; array++, i++) {
                        monitors[i] = new Monitor(new IntPtr(*array).ToInt32());
                    }

                    return monitors;
                }
            }
        }
        public static Monitor PrimaryMonitor => Monitors[0];

        public VideoMode[] VideoModes {
            get {
                unsafe {
                    int count = 0;

                    var modes = LLWindow.window_getMonitorModes((void*)ID, ref count);

                    var res = new VideoMode[count];

                    for (int i = 0; i < count; i++, modes++) {
                        res[i] = new VideoMode(*modes);
                        Console.WriteLine($"{res[i].Width}X{res[i].Height}");
                    }

                    return res;
                }
            }
        }
        public unsafe VideoMode PrimaryVideoMode => new VideoMode(
            *LLWindow.window_getMonitorMainMode((void*)ID)
        );
    }

    public class VideoMode
    {
        internal LLWindow.VideoMode _mode;

        internal VideoMode(LLWindow.VideoMode _mode)
        {
            this._mode = _mode;
        }

        public int Width => _mode.width;
        public int Height => _mode.height;
        public int RefreshRate => _mode.refreshRate;

        public VideoMode(int width, int height, int refreshRate): this(new LLWindow.VideoMode() {
            width = width,
            height = height,
            refreshRate = refreshRate,
        }) { }
    }

    public delegate void MouseMover(int x, int y);

    public class MouseControl: ICameraController
    {
        public Mouse Mouse { get; }

        public float MinPitch { get; set; } = -90;
        public float MaxPitch { get; set; } = 90;

        public float Smoothness { get; set; } = 2;

        public float Slowness { get; set; } = 15;

        private float pitch = 0;
        private float yaw = 0;
        private float lastPitch = 0;
        private float lastYaw = 0;

        private bool controlMode = true;

        public bool ControlMode {
            get => controlMode;
            set {
                controlMode = value;

                if (controlMode) {

                }
            }
        }

        private void UpdateMouse(int x, int y, out VectorI2? newMousePos)
        {
            yaw = lastYaw - x / Slowness;
            pitch = lastPitch - y / Slowness;

            newMousePos = null;

            float maxPitch = 90;
            float minPitch = -90;

            if (pitch > maxPitch) {
                pitch = maxPitch;
                newMousePos = new VectorI2(x, -(int)((pitch - lastPitch) * Slowness));
            }
            if (pitch < minPitch) {
                pitch = minPitch;
                newMousePos = new VectorI2(x, -(int)((pitch - lastPitch) * Slowness));
            }
        }

        public Vector3 Rotation { get; private set; }

        public void Update()
        {
            if (Smoothness > 0) {
                float newX = (pitch - Rotation.X) / Smoothness;
                float newY = (yaw - Rotation.Y) / Smoothness;

                if (float.IsNaN(newX) || float.IsNegativeInfinity(newX))
                    newX = Rotation.X;
                if (float.IsNaN(newY) || float.IsNegativeInfinity(newY))
                    newY = Rotation.Y;

                Rotation = new Vector3(
                    Rotation.X + newX,
                    Rotation.Y + newY,
                    0
                );

            }
            else
                Rotation = new Vector3(pitch, yaw, Rotation.Z);
        }

        public MouseControl(Mouse mouse)
        {
            Mouse = mouse;

            mouse.Moved += (s, e) => {
                UpdateMouse(e.X, e.Y, out var newPos);

                if (newPos.HasValue) {
                    Mouse.Move(newPos.Value);
                }
            };
        }
    }
    public class KeyboardControl: IMovementController
    {
        public Vector3 Position => CurrPos['x', 'y', 'z'];

        public Keyboard Keyboard { get; }

        public Key ForwardKey { get; set; } = Key.W;
        public Key BackwardKey { get; set; } = Key.S;
        public Key LeftKey { get; set; } = Key.A;
        public Key RightKey { get; set; } = Key.D;
        public Key? ElevateKey { get; set; } = Key.Space;
        public Key? DelevateKey { get; set; } = Key.LeftShift;


        public float Yaw { get; set; } = 0;

        public float MovementAcceleration { get; set; } = .25f;
        public float ElevationAcceleration { get; set; } = 1f;
        public float MovementFriction { get; set; } = .03f;
        public float ElevationFriction { get; set; } = .15f;

        public float Gravity { get; set; } = 0;

        private Vector3 CurrVelocity = Vector3.Zero;
        private Vector3 CurrPos = Vector3.Zero;

        private float ApplyFriction(float vel, float fric)
        {
            return vel - vel * fric;
        }
        private Vector2 ApplyFriction(Vector2 vel, float fric)
        {
            var newVel = vel - vel * fric;

            return newVel;
        }

        public void Update(float delta)
        {
            var currVel = Vector4.Zero;

            if (Keyboard.KeyPressed(ForwardKey))
                currVel.Z -= MovementAcceleration * delta;
            if (Keyboard.KeyPressed(BackwardKey))
                currVel.Z += MovementAcceleration * delta;

            if (Keyboard.KeyPressed(LeftKey))
                currVel.X += MovementAcceleration * delta;
            if (Keyboard.KeyPressed(RightKey))
                currVel.X -= MovementAcceleration * delta;

            if (ElevateKey.HasValue && Keyboard.KeyPressed(ElevateKey.Value))
                currVel.Y += ElevationAcceleration * delta;
            if (DelevateKey.HasValue && Keyboard.KeyPressed(DelevateKey.Value))
                currVel.Y -= ElevationAcceleration * delta;

            currVel = Matrix4.CreateRotationY(Yaw) * currVel;


            var a = new Vector3(currVel.X, currVel.Y, currVel.Z);

            CurrVelocity += a;

            CurrVelocity['x', 'z'] = ApplyFriction(CurrVelocity['x', 'z'], MovementFriction);
            CurrVelocity.Y = ApplyFriction(CurrVelocity.Y, ElevationFriction) - Gravity * delta;

            CurrPos += CurrVelocity;
        }

        public KeyboardControl(Keyboard keyboard)
        {
            Keyboard = keyboard;
        }
    }
}
