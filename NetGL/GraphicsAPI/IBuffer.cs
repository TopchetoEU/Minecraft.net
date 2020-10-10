using System;

namespace NetGL.GraphicsAPI
{
    public interface IBuffer<T>: IUsable, IDisposable where T : struct
    {
        //T[] GetData();
        void SetData(T[] data);
    }
}
