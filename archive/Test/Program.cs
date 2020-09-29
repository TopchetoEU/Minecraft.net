using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Test
{
    public class Main: GameWindow
    {
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };

        int VertexBufferObject = 0;
        int VertexArrayObject = 0;

        string vert = @"#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";
        string frag = @"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}";
        Shader shader;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader(vert, frag);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            base.OnResize(e);
        }

        private PointF NormalizePoint(PointF point)
        {
            return new PointF(point.X * 2 / Width - 1, -(point.Y * 2 / Height - 1));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.Ortho(-1, 1, -1, 1, -1, 1);

            var state = Mouse.GetCursorState();

            var mouse = NormalizePoint(PointToClient(new Point(state.X, state.Y)));

            vertices = new[] {
                -0.5f + mouse.X, -0.5f + mouse.Y, 0.0f, //Bottom-left vertex
                 0.5f + mouse.X, -0.5f + mouse.Y, 0.0f, //Bottom-right vertex
                 0.0f + mouse.X,  0.5f + mouse.Y, 0.0f  //Top vertex
            };

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);

            shader.Use();

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);



            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        public Main() : base(640, 480)
        {

        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            base.OnKeyDown(e);
        }
    }

    public class ArgumentLayout
    {
        public int Location { get; }
        public int Size { get; }

        public ArgumentLayout(int location, int size)
        {
            Location = location;
            Size = size;
        }
    }
    public class Shader
    {
        public int ID { get; } = 0;

        private int VertexShader = 0;
        private int FragmentShader = 0;

        public Shader(string vertex, string fragment)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertex);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragment);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine("Vertex shader info:\n" + infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            Console.WriteLine("Fragment shader info:\n" + infoLogFrag);

            ID = GL.CreateProgram();

            GL.AttachShader(ID, VertexShader);
            GL.AttachShader(ID, FragmentShader);

            GL.LinkProgram(ID);
        }

        public void Use()
        {
            GL.UseProgram(ID);
        }

        public void SetUniformInt(string uniformName, int value)
        {
            var location = GL.GetUniformLocation(ID, uniformName);
            GL.Uniform1(location, value);
        }
        public void SetUniformFloat(string uniformName, float value)
        {
            var location = GL.GetUniformLocation(ID, uniformName);
            GL.Uniform1(location, value);
        }
        public void SetUniformVec2(string uniformName, float x, float y)
        {
            var location = GL.GetUniformLocation(ID, uniformName);
            GL.Uniform2(location, x, y);
        }
        public void SetUniformVec3(string uniformName, float x, float y, float z)
        {
            var location = GL.GetUniformLocation(ID, uniformName);
            GL.Uniform3(location, x, y, z);
        }
        public void SetUniformVec4(string uniformName, float x, float y, float z, float w)
        {
            var location = GL.GetUniformLocation(ID, uniformName);
            GL.Uniform4(location, x, y, z, w);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(ID, attribName);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DetachShader(ID, VertexShader);
                    GL.DetachShader(ID, FragmentShader);
                    GL.DeleteShader(FragmentShader);
                    GL.DeleteShader(VertexShader);
                    GL.DeleteProgram(ID);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Main().Run();
        }
    }
}
