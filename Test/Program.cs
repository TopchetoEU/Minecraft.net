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
        static Mesh<a> c;
        static Mesh<a> b;
        static ShaderProgram shader;

        static Window wnd;

        static void Main(string[] args)
        {
            wnd = new Window("a");

            wnd.Display += A_Displaying;
            wnd.KeyPressed += (s, e) => Console.WriteLine(e.Key);
            wnd.Loaded += Wnd_Loaded;

            wnd.ShowAsMain();
        }

        private static void Wnd_Loaded(object sender, EventArgs e)
        {
            var vert = new Shader(@"#version 330 core
in vec2 position;
in vec3 color;

uniform mat3 matrix;
uniform mat3 cameraMatrix;

out vec3 _color;

void main() {
    gl_Position = vec4(cameraMatrix * matrix * vec3(position, 1), 1);
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

            LLGraphics.graphics_setBackground(1, 0, 0, 0);

            b = new Mesh<a>(shader);
            c = new Mesh<a>(shader);
            b.LoadVertices(new[]
            {
                new a(new Vector2(0, 0), new Vector3(1, 0, 0)),
                new a(new Vector2(1, 0), new Vector3(0, 1, 0)),
                new a(new Vector2(0, 1), new Vector3(0, 0, 1))
            });
            c.LoadVertices(new[]
            {
                new a(new Vector2(0, 0), new Vector3(1, 0, 0)),
                new a(new Vector2(0, 1), new Vector3(0, 1, 0)),
                new a(new Vector2(1, 1), new Vector3(0, 0, 1)),
                new a(new Vector2(1, 0), new Vector3(1, 0, 1))
            }, new uint[] { 0, 1, 2, 0, 3, 2 });
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
            shader.ApplyUniform(Matrix3.CreateScaleX((float)wnd.Size.Y / wnd.Size.X), "cameraMatrix");
            LLGraphics.graphics_clear(0x00004000);
            shader.ApplyUniform(Matrix3.CreateRotation(i), "matrix");

            i += 1;

            b.Draw();
            shader.ApplyUniform(
                Matrix3.CreateTranslation(-.5f, -.5f) *
                Matrix3.CreateScale(((float)Math.Sin(i / 500) + 1) / 2 + 1) *
                Matrix3.Identity
                , "matrix"
            );
            c.Draw();
        }

        private static void A_KeyDown(object sender, KeyboardEventArgs e)
        {
            Console.WriteLine(e.Key);
        }
    }
}
