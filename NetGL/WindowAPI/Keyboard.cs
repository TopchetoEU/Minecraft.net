using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetGL.WindowAPI
{
    public static class Keyboard
    {
        public static event KeyboardEventHandler KeyDown {
            add {
                Init();
                keyDown += value;
            }
            remove {
                Init();
                keyDown -= value;
            }
        }
        public static event KeyboardEventHandler KeyUp {
            add {
                Init();
                keyUp += value;
            }
            remove {
                Init();
                keyUp -= value;
            }
        }

        private static event KeyboardEventHandler keyDown;
        private static event KeyboardEventHandler keyUp;

        private static HashSet<Key> pressedKeys = new HashSet<Key>();

        public static bool KeyPressed(Key key)
        {
            return pressedKeys.Contains(key);
        }

        private static bool initialised = false;

        internal static void Init()
        {
            if (initialised) return;

            LLWindow.window_setup(new string[0], 0);

            LLWindow.window_setKeyboardDownFunc(0, (key) => keyDown?.Invoke(null, new KeyboardEventArgs((Key)key)));
            LLWindow.window_setKeyboardUpFunc(0, (key) => keyUp?.Invoke(null, new KeyboardEventArgs((Key)key)));

            initialised = true;
            keyDown += Keyboard_keyDown;
            keyUp += Keyboard_keyUp;
        }

        private static void Keyboard_keyDown(object sender, KeyboardEventArgs e) => pressedKeys.Add(e.Key);
        private static void Keyboard_keyUp(object sender, KeyboardEventArgs e) => pressedKeys.Remove(e.Key);

        static Keyboard()
        {
            Init();
        }
    }
}
