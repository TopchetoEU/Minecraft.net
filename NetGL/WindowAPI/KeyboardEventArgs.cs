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
}
