using System;
using System.Reflection;

namespace NetGL.GraphicsAPI
{
    public static class Extensions
    {
        public static Type ToType(this GraphicsType type)
        {
            switch (type)
            {
                case GraphicsType.UnsginedByte: return typeof(byte);
                case GraphicsType.Short: return typeof(short);
                case GraphicsType.UnsignedShort: return typeof(ushort);
                case GraphicsType.Int: return typeof(int);
                case GraphicsType.UnsignedInt: return typeof(uint);
                case GraphicsType.Float: return typeof(float);
                case GraphicsType.Double: return typeof(double);
                default: throw new Exception("Invalid graphics type!");
            }
        }

        public static uint GetSize<T>()
        {
            return GetSize(typeof(T));
        }
        public static uint GetSize(this GraphicsType gType)
        {
            switch (gType)
            {
                case GraphicsType.UnsginedByte: return sizeof(byte);
                case GraphicsType.Short: return sizeof(short);
                case GraphicsType.UnsignedShort: return sizeof(ushort);
                case GraphicsType.Int: return sizeof(int);
                case GraphicsType.UnsignedInt: return sizeof(uint);
                case GraphicsType.Float: return sizeof(float);
                case GraphicsType.Double: return sizeof(double);
            }

            return 0;
        }
        public static uint GetSize<T>(this T vector) where T : struct
        {
            if (vector.GetType().GetCustomAttribute<VectorAttribute>().TryClass(out var attrib))
            {
                return attrib.Type.GetSize() * attrib.Dimensions;
            }
            else return 0;
        }
        public static uint GetSize(this Type type)
        {
            if (type.ToGraphicsType().TryStruct(out var gType))
            {
                return gType.GetSize();
            }
            else if (type.IsVector(out var attr))
            {
                return attr.Type.GetSize() * attr.Dimensions;
            }
            else return 0;
        }

        public static GraphicsType? ToGraphicsType(this Type type)
        {
            if (type == typeof(byte)) return GraphicsType.UnsginedByte;
            if (type == typeof(short)) return GraphicsType.Short;
            if (type == typeof(ushort)) return GraphicsType.UnsignedShort;
            if (type == typeof(int)) return GraphicsType.Int;
            if (type == typeof(uint)) return GraphicsType.UnsignedInt;
            if (type == typeof(float)) return GraphicsType.Float;
            if (type == typeof(double)) return GraphicsType.Double;

            return null;
        }

        public static bool IsTypeGraphics(this Type type)
        {
            if (type.IsVector()) return true;
            return ToGraphicsType(type).TryStruct();
        }

        public static bool TryStruct<T>(this T? obj, out T val, T? faulthyValue = null) where T : struct
        {
            if (obj.HasValue && !obj.Equals(faulthyValue)) val = obj.Value;
            else val = default;

            return obj != null;
        }
        public static bool TryStruct<T>(this T obj, out T val, T faulthyValue) where T : struct
        {
            if (!obj.Equals(faulthyValue)) val = obj;
            else val = default;

            return !obj.Equals(faulthyValue);
        }
        public static bool TryStruct<T>(this T? obj, T? faulthyValue = null) where T : struct
        {
            return TryStruct(obj, out _, faulthyValue);
        }
        public static bool TryStruct<T>(this T obj, T faulthyValue) where T : struct
        {
            return TryStruct(obj, out _, faulthyValue);
        }

        public static bool TryClass<T>(this T obj, out T val, T faulthyValue = null) where T : class
        {
            val = obj;

            return obj != faulthyValue;
        }
        public static bool TryClass<T>(this T obj, T faulthyValue = null) where T : class
        {
            return TryClass(obj, out _, faulthyValue);
        }


        public static bool IsVector(this Type type, out VectorAttribute attr)
        {
            return type.GetCustomAttribute<VectorAttribute>().TryClass(out attr);
        }
        public static bool IsVector(this Type type)
        {
            return type.IsVector(out _);
        }
    }
}
