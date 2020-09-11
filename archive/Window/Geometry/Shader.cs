using System;
using OpenTK.Graphics.OpenGL;

namespace MinecraftNetWindow.Geometry
{
    public class Shader: IDisposable
    {
        public ShaderType Type { get; }

        public int ID { get; }

        public string BuildResult => GL.GetShaderInfoLog(ID);

        internal Shader(string source, ShaderType type)
        {
            Type = type;
            ID = GL.CreateShader(Type);

            GL.ShaderSource(ID, source);

            GL.CompileShader(ID);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DeleteShader(ID);
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