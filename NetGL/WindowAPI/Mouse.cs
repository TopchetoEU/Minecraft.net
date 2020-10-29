using System.Collections.Generic;

namespace NetGL.WindowAPI
{
    public static class Mouse
    {
        private static bool nextFakeMove = false;

        public static event MouseEventHandler Down {
            add {
                Init();
                down += value;
            }
            remove {
                Init();
                down -= value;
            }
        }
        public static event MouseEventHandler Up {
            add {
                Init();
                up += value;
            }
            remove {
                Init();
                up -= value;
            }
        }
        public static event MouseEventHandler Move {
            add {
                Init();
                move += value;
            }
            remove {
                Init();
                move -= value;
            }
        }

        private static event MouseEventHandler down;
        private static event MouseEventHandler up;
        private static event MouseEventHandler move;

        private static VectorI2 position = new VectorI2(0, 0);
        public static VectorI2 Position {
            get {
                Init();
                return position;
            }
            set {
                nextFakeMove = true;
                LLWindow.window_setMousePosition(value.X, value.Y);
            }
        }

        private static HashSet<MouseButton> pressedButtons = new HashSet<MouseButton>();

        public static bool ButtonPressed(MouseButton button)
        {
            if (button == MouseButton.None) return false;
            return pressedButtons.Contains(button);
        }

        private static bool initialised = false;

        internal static void Init()
        {
            if (initialised) return;
            initialised = true;

            int x = 0, y = 0;

            LLWindow.window_getMousePosition(ref x, ref y);

            position = new VectorI2(x, y);

            LLWindow.window_setup(new string[0], 0);

            LLWindow.window_setMouseDownFunc(0,
                (b, x, y) => down?.Invoke(null, new MouseEventArgs(new VectorI2(x, y), (MouseButton)b, 0))
            );
            LLWindow.window_setMouseUpFunc(0,
                (b, x, y) => up?.Invoke(null, new MouseEventArgs(new VectorI2(x, y), (MouseButton)b, 0))
            );
            LLWindow.window_setMouseMoveFunc(0,
                (x, y) =>
                {
                    move?.Invoke(null, new MouseEventArgs(new VectorI2(x, y), 0, 0, nextFakeMove));
                    nextFakeMove = false;
                }
            );

            down += Mouse_keyDown;
            up += Mouse_keyUp;
            move += Mouse_move;
        }

        private static void Mouse_move(object sender, MouseEventArgs e)
        {
            position = e.Position;
        }

        private static void Mouse_keyDown(object sender, MouseEventArgs e) => pressedButtons.Add(e.Button);
        private static void Mouse_keyUp(object sender, MouseEventArgs e) => pressedButtons.Remove(e.Button);

        static Mouse()
        {
            Init();
        }
    }
}
