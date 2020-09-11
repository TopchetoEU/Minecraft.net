using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using MinecraftNetWindow.Geometry;
using System.IO;

using MinecraftNetWindow.Units;

using Font = MinecraftNetWindow.Units.Font;

namespace MinecraftNetWindow
{

    public class Window: GameWindow
    {
        public event EventHandler Setup;
        public event EventHandler BeforeDraw;

        public float LastDelta { get; private set; }
        public float LastFPS { get; private set; }

        public KeyboardController Keyboard { get; } = new KeyboardController();

        public List<Scene> Scenes { get; } = new List<Scene>();

        Stopwatch s;

        public Window() : base(1280, 720)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = false;

            MouseController = new MouseGameController(new Point(100, 100),
                () =>
                {
                    var mouseState = Mouse.GetCursorState();

                    return new Point(mouseState.X, mouseState.Y);
                },
                (point) => Mouse.SetPosition(point.X, point.Y)
            );

            Setup?.Invoke(this, new EventArgs());

            base.OnLoad(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            shader.Dispose();
            mesh.Dispose();
            font.Dispose();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            OnRenderFrame(new FrameEventArgs());

            GenText();

            RefreshMatrix();

            MouseController.Center = PointToScreen(new Point(Width / 2, Height / 2));

            base.OnResize(e);
        }
        protected override void OnMove(EventArgs e)
        {

            MouseController.Center = PointToScreen(new Point(Width / 2, Height / 2));
            base.OnMove(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            BeforeDraw?.Invoke(this, new EventArgs());

            foreach (var scene in Scenes)
            {
                scene.Draw();
            }

            Context.SwapBuffers();

            #region Timing stuff (FPS calculation)
            s.Stop();

            LastDelta = s.ElapsedTicks / (float)Stopwatch.Frequency;
            LastFPS = 1 / LastDelta;
            s.Reset();
            s.Start();

            #endregion

            base.OnRenderFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Keyboard.PressKey(e.Key);

            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Keyboard.ReleaseKey(e.Key);

            base.OnKeyUp(e);
        }

        public static void Main()
        {
            using (var w = new Window())
                w.Run(0);
        }

    }

    public class Transformation
    {
        public PointF3D Position { get; set; }
        public SizeF3D Size { get; set; }
        public RotationF3D Rotation { get; set; }

        public Transformation(PointF3D position, SizeF3D size, RotationF3D rotation)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
        }

        public Matrix4 GetMatrix()
        {
            return
                Position.GetMatrix() *
                Size.GetMatrix() *
                Rotation.GetMatrix();
        }
    }

    public delegate Point MouseLocationGetter();
    public delegate void MouseLocationSetter(Point location);

    public class MouseGameController
    {
        public Point MouseDelta { get; private set; }

        public float Sensitivity { get; set; } = -.1f;
        public float MaxPitch { get; set; } = 90;
        public float MinPitch { get; set; } = -90;
        public float Smoothness { get; set; } = 2.5f;

        public RotationF3D Rotation { get; private set; } = new RotationF3D(0, 0, 0);

        public Point Center { get; set; }
        public Point ActualPosition { get; private set; }

        private Point sharpPosition = Point.Empty;

        public PointF Position { get; set; } = Point.Empty;

        public void Update()
        {
            ActualPosition = MouseLocationGetter();

            MouseDelta = new Point(-ActualPosition.X + Center.X,
                                   -ActualPosition.Y + Center.Y);

            sharpPosition = new Point(MouseDelta.X + sharpPosition.X,
                                 MouseDelta.Y + sharpPosition.Y);

            var diffX = (sharpPosition.X - Position.X) / Smoothness;
            var diffY = (sharpPosition.Y - Position.Y) / Smoothness;

            Position = new PointF(
                Position.X + diffX,
                Position.Y + diffY
            );

            MouseLocationSetter(Center);

            Rotation.Yaw = Position.X * Sensitivity;
            Rotation.Pitch = Position.Y * Sensitivity;

            if (sharpPosition.Y * Sensitivity > MaxPitch || sharpPosition.Y * Sensitivity < MinPitch)
            {
                sharpPosition = new Point(
                    sharpPosition.X,
                    sharpPosition.Y - MouseDelta.Y
                );

                Position = new PointF(
                    Position.X,
                    Position.Y - diffY
                );


                diffY = (sharpPosition.Y - Position.Y) / Smoothness;

                Position = new PointF(
                    Position.X,
                    Position.Y + diffY
                );

                Rotation.Pitch = Position.Y * Sensitivity;
            }
        }

        public MouseLocationGetter MouseLocationGetter { get; set; }
        public MouseLocationSetter MouseLocationSetter { get; set; }

        public MouseGameController(Point center,
            MouseLocationGetter mouseLocationGetter,
            MouseLocationSetter mouseLocationSetter)
        {
            Center = center;
            MouseLocationGetter = mouseLocationGetter;
            MouseLocationSetter = mouseLocationSetter;
        }
    }

    public class KeyboardGameController
    {
        private PointF3D offset = new PointF3D(0, 0, 0);

        public PointF3D Acceleration { get; set; } = new PointF3D(0, 0, 0);
        public Vector3 AccelerationVector {
            get => Acceleration;
            set => Acceleration = value;
        }

        public PointF3D Velocity { get; set; } = new PointF3D(0, 0, 0);
        public Vector3 VelocityVector {
            get => Velocity;
            set => Velocity = value;
        }


        public float MaxVelocity { get; set; } = 30f;
        public float Speed { get; set; } = 7.5f;
        public float FrictionAmount { get; set; } = 0.085f;

        public void Up()       => offset.Y -= Speed;
        public void Down()     => offset.Y += Speed; 
        public void Left()     => offset.X += Speed; 
        public void Right()    => offset.X -= Speed;
        public void Forward()  => offset.Z += Speed;
        public void Backward() => offset.Z -= Speed;

        public void Update(float delta, Matrix4? rotation = null)
        {
            var normalisedOffset = ((Vector3)offset).Normalized() * Speed;
            if (((Vector3)offset).LengthSquared == 0) normalisedOffset = Vector3.Zero;

            Acceleration = normalisedOffset * delta;

            Velocity += (PointF3D)((rotation ?? Matrix4.Zero) * (Vector4)Acceleration);
            if (VelocityVector.LengthSquared > 0)
            {
                Velocity = VelocityVector.Normalized() * Math.Min(MaxVelocity, VelocityVector.Length);
                Velocity = VelocityVector.Normalized() * Math.Max(0, VelocityVector.Length * (1 - FrictionAmount));
            }

            offset = Vector3.Zero;
        }

        public void ResetAcceleration() => Acceleration = Vector3.Zero;
    }

    public class KeyboardController
    {
        private HashSet<Key> PressedKeys = new HashSet<Key>();

        public void ReleaseKey(Key key)
        {
            if (PressedKeys.Contains(key))
            {
                PressedKeys.Remove(key);

                KeyReleased?.Invoke(this, new EventArgs());
            }
        }
        public void PressKey(Key key)
        {
            if (!PressedKeys.Contains(key))
            {
                PressedKeys.Add(key);

                KeyPressed?.Invoke(this, new EventArgs());
            }
        }
        public bool IsKeyPressed(Key key)
        {
            return PressedKeys.Contains(key);
        }

        public event EventHandler KeyPressed;
        public event EventHandler KeyReleased;
    }
}