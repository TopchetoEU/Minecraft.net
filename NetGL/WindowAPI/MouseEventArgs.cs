namespace NetGL.WindowAPI
{
    public class MouseEventArgs
    {
        public PointI2 Position { get; }
        public MouseButton Button { get; }
        public int Delta { get; }
        public bool Fake { get; } = false;

        public MouseEventArgs(PointI2 position, MouseButton button, int delta, bool fake = false)
        {
            Position = position;
            Button = button;
            Delta = delta;
            Fake = fake;
        }
    }
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
}
