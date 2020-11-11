using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// A texture that has different dimensions and the abillity to take new data; very similar to the buffer
    /// </summary>
    public interface ITexture: IUsable, IDisposable
    {
        /// <summary>
        /// The different dimensions of the texture, going form Width, Height ...
        /// </summary>
        uint[] Sizes { get; }

        /// <summary>
        /// Sets the data of the texture
        /// </summary>
        /// <param name="data">The data to set</param>
        /// <param name="format">The format of the pixels</param>
        void SetData(float[] data, PixelFormat format);
        /// <summary>
        /// Sets the data of the texture
        /// </summary>
        /// <param name="data">The data to set</param>
        /// <param name="format">The format of the pixels</param>
        void SetData(IEnumerable<float> data, PixelFormat format);
    }
}
