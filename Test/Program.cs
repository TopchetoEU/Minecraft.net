using System;
using System.Threading;
using NetGL;
using NetGL.GraphicsAPI;
using NetGL.WindowAPI;

namespace Test
{
    class Program
    {

        static Window wnd = new Window("test");
        static ShaderProgram program;
        static Mesh<TestElement> mesh;
        static void Main(string[] args)
        {
            wnd.Loaded += Wnd_Loaded;
            wnd.Display += Wnd_Display;

            wnd.Show();
        }

        private static void Wnd_Display(object sender, EventArgs e)
        {
            wnd.Graphics.Clear();

            mesh.Draw();
            program.Use();

            wnd.Graphics.SwapBuffers();
        }

        struct TestElement
        {
            public Point2 Location { get; set; }
            public Point3 Color { get; set; }

            public TestElement(float x, float y, float r, float g, float b)
            {
                Location = new Point2(x, y);
                Color = new Point3(r, g, b);
            }
        }

        private static void Wnd_Loaded(object sender, EventArgs e)
        {
            var vert = new Shader(@"#version 330 core
layout(location = 0) in vec2 vertPos;
layout(location = 1) in vec3 vertColor;

out vec4 _color;

void main() {
    gl_Position = vec4(vertPos, 1, 1);
    _color = vec4(vertColor, 1);
}", ShaderType.Vertex);
            var frag = new Shader(@"#version 330 core
out vec4 color;

in vec4 _color;

void main(){
  color = _color;
}", ShaderType.Fragment);

            program = new ShaderProgram(vert, frag);

            mesh = new Mesh<TestElement>();
            mesh.LoadVertices(new [] {
                new TestElement(1, 0, 1, 0, 0),
                new TestElement(0, 0, 0, 1, 0),
                new TestElement(0, 1, 0, 0, 1)
            });

            wnd.Graphics.BackgroundColor = new Point4(0, 0, 1, 1);
        }
    }
}
