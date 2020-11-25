using System.Collections.Generic;

namespace NetGL.WindowAPI
{
    public class Mouse
    {
        public event MouseEventHandler Moved;
        public event MouseEventHandler Pressed;
        public event MouseEventHandler Released;
        public event MouseEventHandler Scrolled;

        private MouseMover mouseMover;

        private Dictionary<MouseButton, bool> pressedButtons = new Dictionary<MouseButton, bool>();

        public VectorI2 Position { get; private set; }
        public int ScrollAmount { get; private set; } = 0;

        public void RegisterMove(VectorI2 pos)
        {
            if (pos != Position)
                Moved?.Invoke(this, new MouseEventArgs(pos));
        }
        public void RegisterPress(MouseButton button)
        {
            if (!ButtonPressed(button)) {
                pressedButtons[button] = true;
                Pressed?.Invoke(this, new MouseEventArgs(Position, button));
            }
        }
        public void RegisterRelease(MouseButton button)
        {
            if (ButtonPressed(button)) {
                pressedButtons[button] = false;
                Released?.Invoke(this, new MouseEventArgs(Position, button));
            }
        }
        public void RegisterScroll(int delta)
        {
            if (delta != 0) {
                ScrollAmount += delta;

                Scrolled?.Invoke(this, new MouseEventArgs(Position, delta));
            }
        }

        public void Move(VectorI2 pos)
        {
            if (Position != pos) {
                mouseMover(pos.X, pos.Y);
                RegisterMove(pos);
            }
        }
        public bool ButtonPressed(MouseButton button)
        {
            return pressedButtons.TryGetValue(button, out var val) && val;
        }

        public Mouse(MouseMover mouseMover)
        {
            this.mouseMover = mouseMover;
        }
    }
}
