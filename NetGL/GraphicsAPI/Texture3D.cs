using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public class Texture3D: ITexture
    {
        private bool disposedValue;

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public uint Depth { get; private set; }

        public uint Id { get; }

        public uint[] Sizes => new uint[] { Width, Height, Depth };

        public Texture3D(uint width, uint height, uint depth, Vector4? colorFill = null)
        {
            if (!colorFill.HasValue)
                colorFill = new Vector4(0, 0, 0, 0);

            var genData = new float[4 * width * height * depth];
            for (ulong i = 0; i < width * height; i++) {
                genData[i * 4 + 0] = colorFill.Value.X;
                genData[i * 4 + 1] = colorFill.Value.Y;
                genData[i * 4 + 2] = colorFill.Value.Z;
                genData[i * 4 + 3] = colorFill.Value.W;
            }

            Width = width;
            Height = height;
            Depth = depth;

            SetData(genData, PixelFormat.RGBA);
        }

        public void SetData(float[] data, PixelFormat format)
        {
            SetData((IEnumerable<float>)data, format);
        }
        public void SetData(IEnumerable<float> data, PixelFormat format)
        {
            var width = Width;
            var height = Height;
            var depth = Depth;

            var arrId = new NativeArray(data.Cast<object>().ToArray());

            LLGraphics.graphics_setTexture3DData(
                (uint)TextureTarget.Texture3D, width, height, depth,
                (uint)format, (uint)GraphicsType.Float, arrId.Id);

            arrId.Dispose();
        }

        public void SetData(float[] data, uint width, uint height, uint depth, PixelFormat format)
        {
            Width = width;
            Height = height;
            Depth = depth;

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
