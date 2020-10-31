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
        static Mesh<a> b;
        static ShaderProgram shader;

        static void Main(string[] args)
        {
            var wnd = new Window("a");
            var wnd2 = new Window("b");

            wnd.Display += A_Displaying;
            wnd2.Display += (s, e) => LLGraphics.graphics_clear(0x4000);
            wnd.KeyDown += (s, e) => Console.WriteLine(e.Key);
            wnd.Loaded += Wnd_Loaded;

            wnd2.Show();
            wnd.ShowAsMain();
        }

        private static void Wnd_Loaded(object sender, EventArgs e)
        {

            var vert = new Shader(@"#version 330 core
in vec2 position;
in vec3 color;

uniform mat3 matrix;

out vec3 _color;

void main() {
    gl_Position = vec4(matrix * vec3(position, 1), 1);
    _color = color;
}
", ShaderType.Vertex);
            var frag = new Shader(@"#version 330 core
in vec3 _color;
out vec4 color;

void main() {
    color = vec4(_color, 1);
}
", ShaderType.Fragment);
            shader = new ShaderProgram(vert, frag);

            b = new Mesh<a>(shader);
            b.LoadVertices(new[]
            {
                new a(new Vector2(0, 0), new Vector3(1, 0, 0)),
                new a(new Vector2(1, 0), new Vector3(0, 1, 0)),
                new a(new Vector2(0, 1), new Vector3(0, 0, 1))
            });
        }

        struct a
        {
            public Vector2 position { get; set; }
            public Vector3 color { get; set; }

            public a(Vector2 pos, Vector3 color)
            {
                position = pos;
                this.color = color;
            }
        }

        static float i = 0;
        private static void A_Displaying(object sender, EventArgs e)
        {
            LLGraphics.graphics_clear(0x00004000);
            shader.ApplyUniform(Matrix3.CreateRotation(i), "matrix");

            i += 1;

            b.Draw();
        }

        private static void A_KeyDown(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine(e.Key);
        }
    }
}
