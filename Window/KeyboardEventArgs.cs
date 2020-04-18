using OpenTK.Input;

namespace Minecraft.MainWindow
{
    public class KeyboardEventArgs
    {
        public Key Key { get; }

        public KeyboardEventArgs(Key key)
        {
            Key = key;
        }
    }
}