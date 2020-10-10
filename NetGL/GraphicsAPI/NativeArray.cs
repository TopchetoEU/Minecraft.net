using System;

namespace NetGL.GraphicsAPI
{
    internal class NativeArray: IDisposable
    {
        private bool disposedValue;

        public uint Id { get; }
        public NativeArray(uint size)
        {
            Id = LLGraphics.graphics_createNativeArray(size);
        }
        public NativeArray(NativeArrayElement[] elements)
        {
            Id = LLGraphics.graphics_loadNativeArray(elements, (uint)elements.Length);
        }
        public NativeArray(object[] elements)
        {
            Id = LLGraphics.graphics_loadNativeArray(
                NativeArrayElement.GetNativeArray(elements),
                (uint)elements.Length
            );
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LLGraphics.graphics_destroyNativeArray(Id);
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
