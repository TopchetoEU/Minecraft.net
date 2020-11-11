using NetGL.WindowAPI;
using System;
using System.Reflection;

namespace NetGL.GraphicsAPI
{
    internal struct AttribPointer
    {
        public uint Size { get; set; }
        public GraphicsType Type { get; set; }

        public AttribPointer(uint size, GraphicsType type)
        {
            Size = size;
            Type = type;
        }

        public static AttribPointer FromStruct(Type type)
        {
            if (!type.IsValueType) throw new Exception("Can't get attrib pointer for a non-value type");


            if (type.IsVector(out VectorAttribute vecAttrib))
            {
                var size = vecAttrib.Dimensions;
                var gType = vecAttrib.Type;

                return new AttribPointer(size, gType.ToGraphicsType());
            }
            else if (type.ToGraphicsType().TryStruct(out var gType))
            {
                return new AttribPointer(1, gType);
            }
            else throw new Exception("Invalid graphics type");
        }
    }
}
