using System;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// The standart definition for a buffer
    /// </summary>
    public interface IBuffer: IUsable, IDisposable
    {
        //T[] GetData();
        /// <summary>
        /// The length of the buffer
        /// </summary>
        uint Length { get; }
        /// <summary>
        /// The length in memory of the buffer
        /// </summary>
        uint ByteLength { get; }
        /// <summary>
        /// Sets the data of the buffer
        /// </summary>
        /// <typeparam name="T">The type of data to set</typeparam>
        /// <param name="data">The data to set</param>
        void SetData<T>(T[] data) where T : struct;
    }
}
