using System;
using System.Globalization;
using System.Text.RegularExpressions;

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

    public class ShaderSyntaxException: Exception
    {
        private string msg = "";
        public override string Message { get => msg; }

        public ShaderSyntaxException(string log)
        {
            foreach (var error in log.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var a = error.Split(" : ", 2);

                if (a.Length != 2) throw new Exception("Invalid shader error format");

                var locationUnparsed = a[0];
                var errorUnparsed = a[1];

                if (!new Regex("(\\d*\\(\\d*\\))").IsMatch(locationUnparsed))
                    throw new Exception("Invalid shader error loaction format");
                if (!new Regex("(error [A-F0-9]*: .*)").IsMatch(errorUnparsed))
                    throw new Exception("Invalid shader error message format");

                var line = int.Parse(locationUnparsed.Substring(2, locationUnparsed.Length - 3));
                var nativeMessage = errorUnparsed.Substring(errorUnparsed.IndexOf(':') + 2);

                var errorCodeStart = errorUnparsed.IndexOf(' ') + 1;
                var b = errorUnparsed.Substring(errorCodeStart, errorUnparsed.IndexOf(':') - errorCodeStart);
                var errorCode = int.Parse(
                    b,
                    NumberStyles.HexNumber
                );

                var currentMsg = $"\n\tError at line {line}: {errorCode.ToString("X")} - {nativeMessage}";

                msg += currentMsg;
            }
        }
    }
}
