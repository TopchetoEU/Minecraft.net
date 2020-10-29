using System;

namespace NetGL.GraphicsAPI
{
    public class Scene: IDrawable, IDisposable
    {
        private bool disposedValue;

        public void Draw()
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

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
