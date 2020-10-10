using System;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public static class BufferElementPacker<T> where T : struct
    {
        public static ValueType[] Unpack(T element)
        {
            return typeof(T).GetValidProps()
                .SelectMany(prop => prop.GetValue(element).Flattern()).ToArray();
        }
        public static T Pack(params object[] data)
        {
            var type = typeof(T);

            var validProps = type.GetValidProps();
            var element = default(T);
            var i = 0;

            foreach (var prop in validProps)
            {
                if (prop.GetType().IsVector())
                    prop.SetValue(element, data[i]);

                i++;
            }

            return element;
        }
    }
}
