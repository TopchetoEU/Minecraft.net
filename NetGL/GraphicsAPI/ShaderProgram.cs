using System;
using System.Collections.Generic;

namespace NetGL.GraphicsAPI
{
    public class ShaderProgram: IDisposable, IUsable
    {
        private bool disposedValue;

        private uint Id { get; }
        private List<Shader> attachedShaders = new List<Shader>();

        public Shader[] AttachedShaders {
            get {
                var a = new Shader[attachedShaders.Count];
                attachedShaders.CopyTo(a);

                return a;
            }
        }

        public void AttachShader(Shader shader)
        {
            LLGraphics.graphics_attachShaderToProgram(Id, shader.Id);

            attachedShaders.Add(shader);
        }
        public void DetachShader(Shader shader)
        {
            int index = attachedShaders.FindIndex(v => v.Id == shader.Id);
            if (index < 0)
                throw new Exception(
                    "The shader could not be detached," +
                    "since it wasn't attached to the program in the first place"
                );

            LLGraphics.graphics_detachShaderFromProgram(Id, shader.Id);

            attachedShaders.RemoveAt(index);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LLGraphics.graphics_destroyShaderProgram(Id);
                    attachedShaders = null;
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Use()
        {
            LLGraphics.graphics_setShaderProgram(Id);
        }

        public ShaderProgram(params Shader[] shaders)
        {
            LLGraphics.graphics_init();

            Id = LLGraphics.graphics_createShaderProgram();

            foreach (var shader in shaders)
            {
                AttachShader(shader);
            }

            LLGraphics.graphics_compileShaderProgram(Id);
        }
    }
}
