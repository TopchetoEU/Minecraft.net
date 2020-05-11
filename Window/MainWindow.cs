using System;
using System.Drawing;
using MinecraftNetWindow.GUI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using MinecraftNetWindow.Units;
using Rectangle = MinecraftNetWindow.Units.Rectangle;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;

namespace MinecraftNetWindow.MainWindow
{
    /// <summary>
    /// A OpenGL window to show
    /// </summary>
    /// <inheritdoc/>
    public class MainWindow : GameWindow
    {
        private Atlas atlas;
        private GUIButton button;

        #region IO shit

        /// <summary>
        /// The default logger
        /// </summary>
        public Logger Logger { get; set; } = new Logger("Main");
        /// <summary>
        /// The API to the physical / virtual keyboard
        /// </summary>
        public Keyboard Keyboard { get; } = new Keyboard();
        /// <summary>
        /// The API to the physical / virtual mouse
        /// </summary>
        public Mouse Mouse { get; } = new Mouse();

        /// <summary>
        /// Gets the inner bounds of the window (excluding title bar and borders)
        /// </summary>
        public Rectangle InnerBounds {
            get {
                var loc = PointToScreen(new Point(ClientRectangle.X, ClientRectangle.Y));
                return new Rectangle(new Point2D(loc), new Size2D(ClientRectangle.Size));
            }
            set {
                var p = value.Position.ToSystemDrawingPointF();
                var loc = PointToClient(new Point((int)p.X, (int)p.Y));

                var s = value.Size.ToSystemDrawingSizeF();

                Bounds = new System.Drawing.Rectangle(loc, new Size((int)s.Width, (int)s.Height));
            }
        }

        #endregion

        /// <summary>
        /// Starts the application
        /// </summary>
        public new void Run()
        {
            Logger.Info("Starting window...");
            //try
            //{
            base.Run();
            //}
            //catch (Exception error)
            //{
            //    Logger.DisplayException(error);
            //}
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            Logger.Info("Window initialized successfully");

            atlas = new Atlas(8192, 8192);
            button = new GUIButton(atlas, new Size2D(100, 30), "Fakh");
            button.BackgroundSprite = new GUIMaterial(Color.FromArgb(255, 255, 255), Transformation2D.Zero, button.Size);

            guishader = new Shader(guivert, guifrag);
            guiMesh = new Mesh2D(guishader, atlas);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            base.OnLoad(e);
        }
        /// <inheritdoc/>
        #region Some shit
        protected override void OnUnload(EventArgs e)
        {
            Logger.Info("The window closed successfully!");
            guishader.Dispose();
        }
        /// <inheritdoc/>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }
        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                Keyboard.RegisterPressKey(e.Key);
            }
        }
        /// <inheritdoc/>
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Keyboard.RegisterReleaseKey(e.Key);
        }
        #endregion
        /// <inheritdoc/>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }
        /// <inheritdoc/>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            RenderGUI();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        /// <inheritdoc/>
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Mouse.RegisterMove(new Point2D(e.Position));
        }
        /// <inheritdoc/>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Mouse.RegisterPress(e.Button);
        }
        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Mouse.RegisterRelease(e.Button);
        }

        private void RenderGUI()
        {
            GL.Ortho(0, Width, Height, 0, .1, 2);

            foreach (var sprite in button.Textures)
            {

                var x = sprite.Transformation.Position.X;
                var y = sprite.Transformation.Position.Y;
                var size = sprite.Size * sprite.Transformation.Scale;
                var actSize = size * sprite.Transformation.Scale;
                var x1 = sprite.Transformation.Position.X + actSize.Width;
                var y1 = sprite.Transformation.Position.Y + actSize.Height;

                float s = 0, t = 0, s1 = 0, t1 = 0;

                var hasTexture = sprite.Texture != null;

                if (hasTexture)
                {
                    s = sprite.Texture.TextureArea.Position.X;
                    t = sprite.Texture.TextureArea.Position.Y;
                    s1 = sprite.Texture.TextureArea.SecondPosition.X;
                    t1 = sprite.Texture.TextureArea.SecondPosition.Y;
                    sprite.Texture.Atlas.Use();
                }


                var newData = new List<Vertice2D>();

                x = (x) / Width * 2 - 1;
                y = (Height - y) / Height * 2 - 1;
                x1 = (x1) / Width * 2 - 1;
                y1 = (Height - y1) / Height * 2 - 1;
                t = 1 - t;
                t1 = 1 - t1;

                // Part 1
                newData.Add(new Vertice2D(x, y, s, t));
                newData.Add(new Vertice2D(x1, y, s1, t));
                newData.Add(new Vertice2D(x1, y1, s1, t1));
                newData.Add(new Vertice2D(x, y1, s1, t1));

                guiMesh.FlushGeometry(newData.ToArray());

                guiMesh.Draw(BeginMode.Quads);
            }

            GL.End();
        }

        private static void Main()
        {
            using (MainWindow example = new MainWindow())
            {
                example.Run();
            }
        }

        private string guifrag = @"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}";
        private string guivert = @"#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";
        private Shader guishader;
        private Mesh2D guiMesh;
    }

    public class Mesh2D: IDisposable
    {
        public Shader Shader { get; }
        public Atlas Atlas { get; }

        public ObservableCollection<Vertice2D> Geometry { get; private set; } = new ObservableCollection<Vertice2D>();

        public void FlushGeometry(Vertice2D[] geometry)
        {
            Geometry = new ObservableCollection<Vertice2D>(geometry);
            Calculate();
        }

        private int VertexBufferObject = 0;
        private int VertexArrayObject = 0;

        private Buffer<float> vertex;
        private Buffer<int> indicies;

        public void Calculate()
        {
            var vertices = Geometry.SelectMany(v => new[] {
                v.Location.X, v.Location.Y,
                v.TextureLocation.X, v.TextureLocation.Y
            }).ToArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Geometry.Count * 4 * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            var attrLoc = Shader.GetAttribLocation("aPosition");

            GL.VertexAttribPointer(attrLoc, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(attrLoc);
        }

        public void Use()
        {
            GL.BindVertexArray(VertexArrayObject);
            Shader.Use();
            Atlas.Use();
        }

        private void init()
        {
            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            Calculate();
        }

        public Mesh2D(Shader shader, Atlas atlas)
        {
            Shader = shader;
            Atlas = atlas;

            init();

            Geometry.CollectionChanged += Geometry_CollectionChanged;
        }

        private void Geometry_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Calculate();
        }

        public void Draw(BeginMode mode)
        {
            Use();
            GL.DrawArrays(mode, 0, Geometry.Count);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Mesh2D()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    
    public class Vertice2D
    {
        public Point2D Location { get; }
        public Point2D TextureLocation { get; }

        public Vertice2D(float x, float y, float s, float t)
        {
            Location = new Point2D(x, y);
            TextureLocation = new Point2D(s, t);
        }

        public static int ByteSize => 16;
    }

    /// <summary>
    /// A OpenGL shader
    /// </summary>
    public class Shader : IDisposable
    {
        private int Handle = 0;

        private int VertexShader = 0;
        private int FragmentShader = 0;

        /// <summary>
        /// Creates new shader
        /// </summary>
        /// <param name="vertex">Vertex shader</param>
        /// <param name="fragment">Fragment shader</param>
        public Shader(string vertex, string fragment)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertex);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragment);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != string.Empty)
                Console.WriteLine(infoLogFrag);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);
        }

        /// <summary>
        /// Loads the shader
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        /// <summary>
        /// Gets a location of an attribute inside a shader
        /// </summary>
        /// <param name="attribName">The name of the attribute</param>
        /// <returns>The location of the specified attribute</returns>
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes unallocated resources
        /// </summary>
        /// <param name="disposing">Is the object disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DetachShader(Handle, VertexShader);
                    GL.DetachShader(Handle, FragmentShader);
                    GL.DeleteShader(FragmentShader);
                    GL.DeleteShader(VertexShader);
                    GL.DeleteProgram(Handle);
                }

                disposedValue = true;
            }
        }


        // ~Shader()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        /// <summary>
        /// Disposes unallocated resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class Buffer<T>: IDisposable where T: struct
    {
        private int id = 0;
        private BufferTarget target;
        private BufferUsageHint usageHint;
        private int elementSize;

        public void BindElements(T[] elements)
        {
            GL.BindVertexArray(id);

            GL.BindBuffer(target, id);
            GL.BufferData(target, elements.Length * elementSize, elements, usageHint);
        }

        public Buffer(int id, BufferTarget target, BufferUsageHint usageHint, int elementSize)
        {
            this.id = id;
            this.target = target;
            this.usageHint = usageHint;
            this.elementSize = elementSize;

            id = GL.GenBuffer();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DeleteBuffer(id);
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
}

