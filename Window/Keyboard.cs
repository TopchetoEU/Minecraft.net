using System;
using System.Collections.Generic;
using OpenTK.Input;

namespace Minecraft.MainWindow
{
    public class Keyboard
    {
        public event EventHandler<KeyboardEventArgs> KeyPressed;
        public event EventHandler<KeyboardEventArgs> KeyReleased;


        private List<Key> pressedKeys = new List<Key>();

        public void PressKey(Key key)
        {
            if (!pressedKeys.Contains(key))
            {
                if (!pressedKeys.Contains(key)) pressedKeys.Add(key);
                KeyPressed?.Invoke(this, new KeyboardEventArgs(key));
            }
        }
        public void ReleaseKey(Key key)
        {
            if (pressedKeys.Contains(key)) pressedKeys.Remove(key);
            KeyReleased?.Invoke(this, new KeyboardEventArgs(key));
        }

        public bool IsKeyPressed(Key key)
        {
            return pressedKeys.Contains(key);
        }
    }
}