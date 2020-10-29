using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;

namespace Test
{
    class Program
    {

        private delegate void KeyboardFunc(int key);
        private class ExWindow
        {
            public static ExWindow Global { get; } = new ExWindow(0);
            public event EventHandler Loaded;
            public event EventHandler Activated;
            public event EventHandler Displayed;
            public event KeyboardEventHandler KeyDown;
            public event KeyboardEventHandler KeyUp;

            public uint ID { get; }

            private void DisplayFunc()
            {
                Displayed?.Invoke(this, new EventArgs());
            }
            private void KeydownFunc(int key)
            {
                KeyDown?.Invoke(this, new KeyboardEventArgs((Key)key));
            }
            private void KeyupFunc(int key)
            {
                KeyUp?.Invoke(this, new KeyboardEventArgs((Key)key));
            }

            public ExWindow(string title)
            {
                ID = window_ex_createWindow(title);
                window_ex_setDisplayFunc(ID, DisplayFunc);
                window_ex_setKeydownFunc(ID, KeydownFunc);
                window_ex_setKeyupFunc(ID, KeyupFunc);
                GC.KeepAlive(Loaded);
                GC.KeepAlive(Activated);
                GC.KeepAlive(Displayed);
                Loaded?.Invoke(this, new EventArgs());
            }
            private ExWindow(uint id)
            {
                ID = id;
            }

            public void Show()
            {
                window_ex_showWindow(ID);
            }
            public void Hide()
            {
                window_ex_hidewindow(ID);
            }
            public void Activate()
            {
                Activated?.Invoke(this, new EventArgs());
                window_ex_activateWindow(ID);
            }

            public static void ActivateMainLoop()
            {
                window_ex_activateMainLoop();
            }
        }
        [DllImport("WinGL.dll")] private static extern uint window_ex_createWindow(string title);
        [DllImport("WinGL.dll")] private static extern void window_ex_showWindow(uint id);
        [DllImport("WinGL.dll")] private static extern void window_ex_hidewindow(uint id);
        [DllImport("WinGL.dll")] private static extern void window_ex_activateWindow(uint id);
        [DllImport("WinGL.dll")] private static extern void window_ex_setDisplayFunc(uint id, Action func);
        [DllImport("WinGL.dll")] private static extern void window_ex_activateMainLoop();
        [DllImport("WinGL.dll")] private static extern void window_ex_init();
        [DllImport("WinGL.dll")] private static extern void window_ex_setKeydownFunc(uint id, KeyboardFunc func);
        [DllImport("WinGL.dll")] private static extern void window_ex_setKeyupFunc(uint id, KeyboardFunc func);

        static VBO<a> b;

        static void Main(string[] args)
        {
            window_ex_init();

            var wnd = new ExWindow("a");

            wnd.Displayed += A_Displaying;
            wnd.KeyDown += (s, e) => Console.WriteLine(e.Key);

            var vert = new Shader(@"#version 330 core
in vec2 position;

void main() {
    gl_Position = vec4(position, 1, 1);
}
", ShaderType.Vertex);
            var frag = new Shader(@"#version 330 core
out vec4 color;

void main() {
    color = vec4(1, 0, 0, 1);
}
", ShaderType.Fragment);

            b = new VBO<a>(new ShaderProgram(vert, frag));
            b.SetData(new[]
            {
                new a(new Vector2(0, 0)),
                new a(new Vector2(1, 0)),
                new a(new Vector2(0, 1))
            });

            wnd.Show();
            wnd.Activate();

            ExWindow.ActivateMainLoop();
        }

        struct a
        {
            public Vector2 position { get; set; }

            public a(Vector2 pos)
            {
                position = pos;
            }
        }

        private static void A_Displaying(object sender, EventArgs e)
        {
            LLGraphics.graphics_clear(0x00004000);
            b.TempDraw();
        }

        private static void A_KeyDown(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine(e.Key);
        }
    }
}
