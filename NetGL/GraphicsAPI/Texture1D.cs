using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public class Texture1D: ITexture
    {
        private bool disposedValue;

        public uint Width { get; private set; }

        public uint Id { get; }

        public uint[] Sizes => new uint[] { Width };

        public Texture1D(uint width, Vector4? colorFill = null)
        {
            if (!colorFill.HasValue)
                colorFill = new Vector4(0, 0, 0, 0);

            var genData = new float[4 * width];
            for (ulong i = 0; i < width; i++) {
                genData[i * 4 + 0] = colorFill.Value.X;
                genData[i * 4 + 1] = colorFill.Value.Y;
                genData[i * 4 + 2] = colorFill.Value.Z;
                genData[i * 4 + 3] = colorFill.Value.W;
            }

            Width = width;

            SetData(genData, PixelFormat.RGBA);
        }

        public void SetData(float[] data, PixelFormat format)
        {
            SetData((IEnumerable<float>)data, format);
        }
        public void SetData(IEnumerable<float> data, PixelFormat format)
        {
            var width = Width;

            var arrId = new NativeArray(data.Cast<object>().ToArray());

            LLGraphics.graphics_setTexture1DData(
                (uint)TextureTarget.Texture1D, width, (uint)format, (uint)GraphicsType.Float, arrId.Id);

            arrId.Dispose();
        }

        public void SetData(float[] data, uint width, PixelFormat format)
        {
            Width = width;

            SetData(data, format);
        }

        public void Use()
        {
            LLGraphics.graphics_setTexture(Id, (uint)TextureTarget.Texture2D);
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    LLGraphics.graphics_destroyTexture(Id);
                }

                disposedValue = true;
            }
        }
    }
}
