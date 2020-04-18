using System;

namespace Minecraft.MainWindow
{
    public class Mouse
    {
        public Position Position { get; private set; }

        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Middle { get; private set; }

        public void Move(Position position)
        {
            Position = position;
            MouseMoved?.Invoke(this, new MouseEventArgs(position, Left, Right, Middle));
        }

        public void Press(MouseButton button)
        {
            Left = Left || button == MouseButton.Left;
            Right = Right || button == MouseButton.Right;
            Middle = Middle || button == MouseButton.Middle;

            MousePressed?.Invoke(this, new MouseEventArgs(Position, Left, Right, Middle));
        }

        public void Release(MouseButton button)
        {
            Left = Left && button != MouseButton.Left;
            Right = Right && button != MouseButton.Right;
            Middle = Middle && button != MouseButton.Middle;

            MouseReleased?.Invoke(this, new MouseEventArgs(Position, Left, Right, Middle));
        }

        public event EventHandler<MouseEventArgs> MouseMoved;
        public event EventHandler<MouseEventArgs> MousePressed;
        public event EventHandler<MouseEventArgs> MouseReleased;
    }

    public enum MouseButton
    {
        Left, Right, Middle
    }

    public class MouseEventArgs
    {
        public Position Position { get; }
        public bool RightPressed { get; }
        public bool LeftPressed { get; }
        public bool MiddlePressed { get; }

        public MouseEventArgs(Position position, bool left, bool right, bool middle)
        {
            Position = position;
            LeftPressed = left;
            RightPressed = right;
            MiddlePressed = middle;
        }
    }

    public class ScrollEventArgs
    {
        public int ScrollDelta { get; }

        public ScrollEventArgs(int delta)
        {
            ScrollDelta = delta;
        }
    }
}