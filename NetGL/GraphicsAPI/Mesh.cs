using System;

namespace NetGL.GraphicsAPI
{
    public class Mesh<T>: IDrawable, IDisposable where T : struct
    {
        private bool disposedValue;

        public VBO<T> Buffer { get; }
        public ShaderProgram Program { get; set; }

        public Mesh(VBO<T> vbo, ShaderProgram program)
        {
            Buffer = vbo;
            Program = program;
        }
        public Mesh(ShaderProgram program)
        {
            Buffer = new VBO<T>(program);
            Program = program;
        }

        public void LoadVertices(params T[] data)
        {
            Buffer.SetData(data);
        }

        public void Draw()
        {
            Program?.Use();
            Buffer.TempDraw();
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Buffer.Dispose();
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
