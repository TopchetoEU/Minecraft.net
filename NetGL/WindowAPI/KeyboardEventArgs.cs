namespace NetGL.WindowAPI
{
    public class KeyboardEventArgs
    {
        public Key Key { get; }

        public KeyboardEventArgs(Key key)
        {
            Key = key;
        }
    }
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    public class ResizeEventArgs
    {
        public int Width { get; }
        public int Height { get; }

        public VectorI2 Size { get; }

        public ResizeEventArgs(int w, int h)
        {
            Width = w;
            Height = h;
            Size = new VectorI2(w, h);
        }
        public ResizeEventArgs(VectorI2 size)
        {
            Width = size.X;
            Height = size.Y;

            Size = size;
        }
    }
    public delegate void ResizeEventHandler(object sender, ResizeEventArgs e);

    public enum MouseButton
    {
        None = -1,
        Left = 0,
        Right = 1,
        Middle
    }

    public class MouseEventArgs
    {
        public int X { get; }
        public int Y { get; }

        public MouseButton Button { get; }
        public int Delta { get; }

        public MouseEventArgs(VectorI2 pos) : this(pos.X, pos.Y) { }
        public MouseEventArgs(VectorI2 pos, int delta) : this(pos.X, pos.Y, delta) { }
        public MouseEventArgs(VectorI2 pos, MouseButton button) : this(pos.X, pos.Y, button) { }

        public MouseEventArgs(int x, int y)
        {
            X = x; Y = y;
            Button = MouseButton.None;
            Delta = 0;
        }
        public MouseEventArgs(int x, int y, int delta)
        {
            X = x; Y = y; Delta = delta;
        }
        public MouseEventArgs(int x, int y, MouseButton button)
        {
            X = x;
            Y = y;
            Button = button;
            Delta = 0;
        }
    }
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
}
