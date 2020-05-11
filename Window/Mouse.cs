using OpenTK.Input;
using System;
using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.MainWindow
{
    /// <summary>
    /// A API for the mouse
    /// </summary>
    public class Mouse
    {
        /// <summary>
        /// Move the mouse
        /// </summary>
        /// <param name="position">New position of mouse position</param>
        public static void SimulateMove(Point2D position)
        {
            OpenTK.Input.Mouse.SetPosition(position.X, position.Y);
        }


        /// <summary>
        /// The last registered mouse position
        /// </summary>
        public Point2D Position { get; private set; }

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
        public bool MouseOver { get; private set; }

        /// <summary>
        /// Registers new position. NOTE: This will not actually effect 
        /// the real mouse position. To simulate a mouse move, use <see cref="SimulateMove(Point2D)"/>
        /// </summary>
        /// <param name="position">The new position of the mouse</param>
        public void RegisterMove(Point2D position)
        {
            Position = position;
            MouseMoved?.Invoke(this, new MouseEventArgs(position, Left, Right, Middle));

            if (!MouseOver)
            {
                MouseOver = true;
                MouseEntered?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Registers a mouse click. NOTE: This will not actually simulate a mouse click.
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
        /// Marks the mouse as not being over
        /// </summary>
        public void RegisterMouseLeave()
        {
            MouseOver = false;
            MouseLeft?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires when a mouse move occurs
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseMoved;
        /// <summary>
        /// Occurs when any mouse click occurred
        /// </summary>
        public event EventHandler<MouseEventArgs> MousePressed;
        /// <summary>
        /// Occurs when any mouse release occurs
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseReleased;
        /// <summary>
        /// Occurs when mouse enters
        /// </summary>
        public event EventHandler MouseEntered;
        /// <summary>
        /// Occurs when mouse leaves
        /// </summary>
        public event EventHandler MouseLeft;
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
        public Point2D Position { get; }
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
        public MouseEventArgs(Point2D position, bool left, bool right, bool middle)
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