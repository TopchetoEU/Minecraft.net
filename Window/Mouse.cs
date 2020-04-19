using OpenTK.Input;
using System;
using System.Runtime.InteropServices;

namespace Minecraft.MainWindow
{
    /// <summary>
    /// A API for the mouse
    /// </summary>
    public class Mouse
    {
        /// <summary>
        /// Move
        /// </summary>
        /// <param name="position"></param>
        public static void SimulateMove(Position position)
        {
            OpenTK.Input.Mouse.SetPosition(position.X, position.Y);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        /// <summary>
        /// Simulates a fake mouse click. NOTE: This is going to actually click the selected mouse button
        /// </summary>
        /// <param name="position">The position at which the mouse click occured</param>
        /// <param name="button">The button that went down</param>
        public void SimulateMouseDown(Position position, MouseButton button)
        {
            uint X = (uint)position.X;
            uint Y = (uint)position.Y;
            if (button == MouseButton.Left)
                mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            if (button == MouseButton.Right)
                mouse_event(MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
        }
        /// <summary>
        /// Simulates a fake mouse release. NOTE: This is going to actually release the selected mouse button
        /// </summary>
        /// <param name="position">The position at which the mouse release occured</param>
        /// <param name="button">The button that went up</param>
        public void SimulateMouseUp(Position position, MouseButton button)
        {
            uint X = (uint)position.X;
            uint Y = (uint)position.Y;
            if (button == MouseButton.Left)
                mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            if (button == MouseButton.Right)
                mouse_event(MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }

        /// <summary>
        /// The last registered mouse position
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// The state of the left button (true for pressed)
        /// </summary>
        public bool Left { get; private set; }
        /// <summary>
        /// The state of the right button (true for pressed)
        /// </summary>
        public bool Right { get; private set; }
        /// <summary>
        /// The state of the middle button (true for pressed)
        /// </summary>
        public bool Middle { get; private set; }

        /// <summary>
        /// Registers new position. NOTE: This will not actually effect 
        /// the real mouse position. To simulate a mouse move, use <see cref="SimulateMove(Position)"/>
        /// </summary>
        /// <param name="position">The new position of the mouse</param>
        public void RegisterMove(Position position)
        {
            Position = position;
            MouseMoved?.Invoke(this, new MouseEventArgs(position, Left, Right, Middle));
        }

        /// <summary>
        /// Registers a mouse click. NOTE: This will not actually simulate a mouse click.
        /// To simulate a click, use <see cref="SimulateMouseDown(Position, MouseButton)"/>
        /// </summary>
        /// <param name="button">The button that went down</param>
        public void RegisterPress(MouseButton button)
        {
            Left = Left || button == MouseButton.Left;
            Right = Right || button == MouseButton.Right;
            Middle = Middle || button == MouseButton.Middle;

            MousePressed?.Invoke(this, new MouseEventArgs(Position, Left, Right, Middle));
        }

        /// <summary>
        /// Registers a mouse release. NOTE: This will not actually simulate a mouse release.
        /// To simulate a release, use <see cref="SimulateMouseUp(Position, MouseButton)"/>
        /// </summary>
        /// <param name="button">The button that went up</param>
        public void RegisterRelease(MouseButton button)
        {
            Left = Left && button != MouseButton.Left;
            Right = Right && button != MouseButton.Right;
            Middle = Middle && button != MouseButton.Middle;

            MouseReleased?.Invoke(this, new MouseEventArgs(Position, Left, Right, Middle));
        }

        /// <summary>
        /// Fires when a mouse move occurs
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseMoved;
        /// <summary>
        /// Occurs when any mouse click occured
        /// </summary>
        public event EventHandler<MouseEventArgs> MousePressed;
        /// <summary>
        /// Occurs when any mouse release occurs
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseReleased;
    }

    /// <summary>
    /// Arguments for mouse event
    /// </summary>
    public class MouseEventArgs
    {
        /// <summary>
        /// The new mouse position
        /// </summary>
        public bool LeftPressed { get; }
        /// <summary>
        /// The state of left mouse button
        /// </summary>
        public Position Position { get; }
        /// <summary>
        /// The new state of right mouse button
        /// </summary>
        public bool RightPressed { get; }
        /// <summary>
        /// The state of middle mouse button
        /// </summary>
        public bool MiddlePressed { get; }

        /// <summary>
        /// Creates arguments for mouse events
        /// </summary>
        /// <param name="position">The new mouse position</param>
        /// <param name="left">The new state of left mouse button</param>
        /// <param name="right">The state of right mouse button</param>
        /// <param name="middle">The state of middle mouse button</param>
        public MouseEventArgs(Position position, bool left, bool right, bool middle)
        {
            Position = position;
            LeftPressed = left;
            RightPressed = right;
            MiddlePressed = middle;
        }
    }

    /// <summary>
    /// Argumants for scroll event
    /// </summary>
    public class ScrollEventArgs
    {
        /// <summary>
        /// The velocity of the scroll
        /// </summary>
        public int ScrollDelta { get; }

        /// <summary>
        /// Creates new arguments for scroll event
        /// </summary>
        /// <param name="delta">The velocity of the scroll</param>
        public ScrollEventArgs(int delta)
        {
            ScrollDelta = delta;
        }
    }
}