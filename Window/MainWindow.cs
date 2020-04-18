using System;
using System.Drawing;
using Minecraft.GUI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Minecraft.MainWindow
{
    partial class MainWindow : GameWindow
    {
        public Logger Logger { get; set; } = new Logger("Main");
        public Keyboard Keyboard { get; } = new Keyboard();
        public Mouse Mouse { get; } = new Mouse();
        private bool NaturalMouseMove = false;

        public Rectangle InnerBounds {
            get {
                var loc = PointToScreen(new Point(ClientRectangle.X, ClientRectangle.Y));
                return new Rectangle(loc, ClientRectangle.Size);
            }
            set {
                var loc = PointToClient(value.Location);

                Bounds = new Rectangle(loc, value.Size);
            }
        }

        public new void Run()
        {
                Logger.Info("Starting window...");
            try
            {
                base.Run();
            }
            catch (Exception error)
            {
                Logger.DisplayException(error);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            Title = "Minecraft.net";
            Logger.Info("Window initialised successfully");
            Mouse.MouseMoved += MouseMoved;
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            Logger.Info($"X: {e.Position.X}, Y: {e.Position.Y}");
            if (!NaturalMouseMove)
            {
                var x = e.Position.X + InnerBounds.X;
                var y = e.Position.Y + InnerBounds.Y;
                if (e.Position.X >= 0 && e.Position.Y >= 0)OpenTK.Input.Mouse.SetPosition(x, y);
                if (e.Position.X < 0) Logger.Error("Mouse X was set to a number, smaller than 0.");
                if (e.Position.Y < 0) Logger.Error("Mouse Y was set to a number, smaller than 0.");

                throw new Exception("Mouse can't be fake moved to a point, outside the window");
            }
            NaturalMouseMove = false;
        }

        protected override void OnUnload(EventArgs e)
        {
            Logger.Info("The window closed successfully!");
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientRectangle);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Keyboard.PressKey(e.Key);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Keyboard.ReleaseKey(e.Key);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Key.Q)) Mouse.Move(new Position(-100, 0));
            if (Keyboard.IsKeyPressed(Key.ScrollLock)) throw new Exception("Intentional crash!");
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            NaturalMouseMove = true;
            Mouse.Move(new Position(e.Position));
        }
        public static void Main()
        {
            using (MainWindow example = new MainWindow())
            {
                example.Run();
            }
        }
    }
}