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


            var vecAttrib = type.GetCustomAttribute(typeof(VectorAttribute)) as VectorAttribute;

            if (vecAttrib != null)
            {
                var size = vecAttrib.Dimensions;
                var gType = vecAttrib.Type;

                return new AttribPointer(size, gType);
            }
            else if (type.ToGraphicsType().TryStruct(out var gType))
            {
                return new AttribPointer(1, gType);
            }
            else throw new Exception("Invalid graphics type");
        }
        public static AttribPointer FromStruct<T>() where T : struct
        {
            return FromStruct(typeof(T));
        }

        public static bool TryFromStruct(Type type, out AttribPointer? attrib)
        {
            if (!type.IsValueType) attrib = null;


            var vecAttrib = type.GetCustomAttribute(typeof(VectorAttribute)) as VectorAttribute;

            if (vecAttrib != null)
            {
                var size = vecAttrib.Dimensions;
                var gType = vecAttrib.Type;

                attrib = new AttribPointer(size, gType);
            }
            else if (type.ToGraphicsType().TryStruct(out var gType))
            {
                attrib = new AttribPointer(1, gType);
            }
            else attrib = null;

            return attrib.HasValue;
        }
        public static bool TryFromStruct<T>(out AttribPointer? attrib)
        {
            return TryFromStruct(typeof(T), out attrib);
        }
    }
}
