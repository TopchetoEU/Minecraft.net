using System;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public class Mesh<T>: IDrawable, IDisposable where T : struct
    {
        private bool disposedValue;

        public VBO<T> ArrayBuffer { get; }
        public EBO ElementBuffer { get; }
        public ShaderProgram Program { get; set; }

        public Mesh(VBO<T> vbo, EBO ebo, ShaderProgram program)
        {
            ArrayBuffer = vbo;
            ElementBuffer = ebo;
            Program = program;
        }
        public Mesh(ShaderProgram program, GraphicsPrimitive primitive = GraphicsPrimitive.Triangles)
        {
            ArrayBuffer = new VBO<T>(program);
            ElementBuffer = new EBO(primitive);
            Program = program;
        }

        private uint[] getNStripElArray(uint elAmount, uint n)
        {
            var els = new uint[elAmount * n - n];
            for (uint i = 0; i < elAmount - 1; i++) {
                for (uint j = 0; j < n; j++) {
                    els[i * n + i] = i + j;
                }
            }

            return els;
        }
        public void LoadVertices(params T[] data)
        {
            LoadVertices(data, data.Select((v, i) => (uint)i).ToArray());
        }
        public void LoadVertices(T[] data, uint[] primitives)
        {
            ArrayBuffer.SetData(data);
            ElementBuffer.SetData(primitives);
        }

        public void Draw()
        {
            Program?.Use();
            ElementBuffer.Draw(ArrayBuffer);
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    ArrayBuffer.Dispose();
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
