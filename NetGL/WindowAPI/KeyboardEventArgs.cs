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
}
