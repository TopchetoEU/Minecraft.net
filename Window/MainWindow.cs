using System;
using System.Drawing;
using Minecraft.GUI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Minecraft.MainWindow
{
    /// <summary>
    /// A OpenGL window to show
    /// </summary>
    /// <inheritdoc/>
    public class MainWindow : GameWindow
    {
        ushort i = 0;
        /// <summary>
        /// The default logger
        /// </summary>
        public Logger Logger { get; set; } = new Logger("Main");
        /// <summary>
        /// The API to the physical keyboard
        /// </summary>
        public Keyboard Keyboard { get; } = new Keyboard();
        /// <summary>
        /// The API to the physical mouse
        /// </summary>
        public Mouse Mouse { get; } = new Mouse();

        /// <summary>
        /// Gets the inner bounds of the window (excluding title bar and borders)
        /// </summary>
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

        /// <summary>
        /// Starts the appliucation
        /// </summary>
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

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            Logger.Info("Window initialised successfully");
        }

        /// <inheritdoc/>
        protected override void OnUnload(EventArgs e)
        {
            Logger.Info("The window closed successfully!");
        }
        /// <inheritdoc/>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientRectangle);
        }
        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                Logger.Info(e.Key.ToKey() + " KeyDown!");
                Keyboard.RegisterPressKey(e.Key.ToKey());
            }
        }
        /// <inheritdoc/>
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Logger.Info(e.Key.ToKey() + " KeyUp!");
            Keyboard.RegisterReleaseKey(e.Key.ToKey());
        }
        /// <inheritdoc/>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Key.Q)) Mouse.SimulateMove(new Position(0, 0));
            if (Keyboard.IsKeyPressed(Key.ScrollLock)) throw new Exception("Intentional crash!");
        }
        /// <inheritdoc/>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }
        /// <inheritdoc/>
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Mouse.RegisterMove(new Position(e.Position));
        }
        /// <inheritdoc/>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Mouse.RegisterPress(e.Button);
            Keyboard.SimulateKeyPress(i);
        }
        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Mouse.RegisterRelease(e.Button);
            Keyboard.SimulateKeyRelease(i);
            i++;
        }

        private static void Main()
        {
            using (MainWindow example = new MainWindow())
            {
                example.Run();
            }
        }
    }
}