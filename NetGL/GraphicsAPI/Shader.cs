using System;

namespace NetGL.GraphicsAPI
{
    public class Shader: IDisposable
    {
        private bool disposedValue;

        public ShaderType Type { get; }
        public string Source { get; }

        internal uint Id { get; }

        public bool BuildSuccess { get; }
        public string BuildLog { get; }

        public Shader(string source, ShaderType type)
        {
            LLGraphics.graphics_init();

            Source = source;

            Id = LLGraphics.graphics_createShader((uint)type, source, source.Length);

            var i = Id;
            BuildSuccess = LLGraphics.graphics_getShaderBuildSuccess(Id);
            BuildLog = LLGraphics.graphics_getShaderInfoLog(i);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LLGraphics.graphics_destroyShader(Id);
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
