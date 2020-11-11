using System;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// A 3D-object in the space, expressed by points and indicies
    /// </summary>
    public class Mesh: IDrawable, IDisposable, IObject3D
    {
        private bool disposedValue;

        public event CancellableGraphicsEventHandler Rendering;
        public event GraphicsEventHandler Rendered;

        /// <summary>
        /// The array buffer, used by the mesh
        /// </summary>
        public VBO VertexBuffer { get; }
        /// <summary>
        /// The element buffer, used by the mesh
        /// </summary>
        public EBO ElementBuffer { get; }
        /// <summary>
        /// The shader program, used to shade the mesh
        /// </summary>
        public ShaderProgram Program { get; set; }
        /// <summary>
        /// The transformation of the mesh
        /// </summary>
        public Transform Transformation { get; set; } = Transform.Zero;

        /// <summary>
        /// The transformation matrix of the mesh
        /// </summary>
        public Matrix4 TransformMatrix => Transformation.Matrix;

        /// <summary>
        /// The name of the uniform transformation matrix
        /// </summary>
        public string TransformationMatrixName { get; }

        /// <summary>
        /// Creates new mesh, using already created buffers
        /// </summary>
        /// <param name="vbo">The array buffer to use</param>
        /// <param name="ebo">The element buffer to use</param>
        /// <param name="program">The program to use</param>
        /// <param name="transMatrixName">The transformation matirx uniform name to use</param>
        public Mesh(VBO vbo, EBO ebo, ShaderProgram program, string transMatrixName = "meshMatrix")
        {
            VertexBuffer = vbo;
            ElementBuffer = ebo;
            Program = program;
            TransformationMatrixName = transMatrixName;
        }
        /// <summary>
        /// Creates new mesh, using already created buffers
        /// </summary>
        /// <param name="primitive">The type of primitives to use</param>
        /// <param name="program">The program to use</param>
        /// <param name="transMatrixName">The transformation matirx uniform name to use</param>
        /// <param name="g">The graphics object to use in the creation of the mesh</param>
        public Mesh(Graphics g, ShaderProgram program, GraphicsPrimitive primitive = GraphicsPrimitive.Triangles, string transMatrixName = "meshMatrix")
        {
            VertexBuffer = g.CreateVertexBuffer(program);
            ElementBuffer = g.CreateElementBuffer(primitive);
            Program = program;
            TransformationMatrixName = transMatrixName;
        }

        /// <summary>
        /// Loads vertices data to the mesh
        /// </summary>
        /// <typeparam name="T">The type of vertices to use</typeparam>
        /// <param name="data">The vertex data</param>
        public void LoadVertices<T>(params T[] data) where T : struct
        {
            LoadVertices(data, data.Select((v, i) => (uint)i).ToArray());
        }
        /// <summary>
        /// Loads vertices data to the mesh
        /// </summary>
        /// <typeparam name="T">The type of vertices to use</typeparam>
        /// <param name="data">The vertex data</param>
        /// <param name="primitives">The primitive connection data</param>
        public void LoadVertices<T>(T[] data, uint[] primitives) where T : struct
        {
            VertexBuffer.SetData(data);
            ElementBuffer.SetData(primitives);
        }

        /// <summary>
        /// Draws the mesh on screen
        /// </summary>
        public void Draw(Graphics graphics)
        {
            var e = new CancellableGraphicsEventArgs(graphics);
            Rendering?.Invoke(this, e);
            if (!e.Cancelled) {
                graphics.DrawObject(VertexBuffer, ElementBuffer);
                Rendered?.Invoke(this, new GraphicsEventArgs(graphics));
            }
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    VertexBuffer.Dispose();
                    Program.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
