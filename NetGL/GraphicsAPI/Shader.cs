using System;
using System.IO;

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

        internal Shader(string source, ShaderType type)
        {
            Source = source;

            Id = LLGraphics.graphics_createShader((uint)type, source, source.Length);

            var i = Id;
            BuildLog = LLGraphics.graphics_getShaderInfoLog(i);
            BuildSuccess = BuildLog.Length == 0 || BuildLog == "No errors.\n";

            if (!BuildSuccess)
            {
                throw new ShaderSyntaxException(BuildLog);
            }
        }
        internal static Shader FromFile(string path, ShaderType type) => new Shader(
            File.ReadAllText(path),
            type
        );

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
