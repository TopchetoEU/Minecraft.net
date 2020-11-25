using System.Collections.Generic;

namespace NetGL.WindowAPI
{
    public class Keyboard
    {
        public event KeyboardEventHandler Pressed;
        public event KeyboardEventHandler Released;

        private HashSet<Key> pressedKeys = new HashSet<Key>();

        public bool KeyPressed(Key key)
        {
            return pressedKeys.Contains(key);
        }

        public void PressKey(Key key)
        {
            if (pressedKeys.Add(key))
                Pressed?.Invoke(this, new KeyboardEventArgs(key));
        }
        public void ReleaseKey(Key key)
        {
            if (pressedKeys.Remove(key))
                Released?.Invoke(this, new KeyboardEventArgs(key));
        }
    }
}
