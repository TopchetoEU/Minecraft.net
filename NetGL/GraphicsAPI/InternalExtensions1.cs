using System;
using System.Linq;
using System.Reflection;

namespace NetGL.GraphicsAPI
{
    internal static partial class InternalExtensions
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
        public static Type ToType(this MultiDimensionType type)
        {
            switch (type)
            {
                case MultiDimensionType.Boolean: return typeof(bool);
                case MultiDimensionType.Int: return typeof(int);
                case MultiDimensionType.UnsignedInt: return typeof(uint);
                case MultiDimensionType.Float: return typeof(float);
                case MultiDimensionType.Double: return typeof(double);

                default: throw new Exception();
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
        public static uint GetSize(this VectorAttribute attrib)
        {
            return attrib.Type.ToGraphicsType().GetSize();
        }
        public static uint GetSize(this Type type)
        {
            if (type.ToGraphicsType().TryStruct(out var gType))
            {
                return gType.GetSize();
            }
            else if (type.IsVector(out var attr))
            {
                return attr.GetSize() * attr.Dimensions;
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
        public static MultiDimensionType? ToMultiDimType(this Type type)
        {
            if (type == typeof(bool)) return MultiDimensionType.Boolean;
            if (type == typeof(int)) return MultiDimensionType.Int;
            if (type == typeof(uint)) return MultiDimensionType.UnsignedInt;
            if (type == typeof(float)) return MultiDimensionType.Float;
            if (type == typeof(double)) return MultiDimensionType.Double;

            return null;
        }

        public static GraphicsType ToGraphicsType(this MultiDimensionType type)
        {
            switch (type)
            {
                case MultiDimensionType.Boolean: return GraphicsType.Boolean;
                case MultiDimensionType.Int: return GraphicsType.Int;
                case MultiDimensionType.UnsignedInt: return GraphicsType.UnsignedInt;
                case MultiDimensionType.Float: return GraphicsType.Float;
                case MultiDimensionType.Double: return GraphicsType.Double;

                default: throw new Exception("How did we get here?");
            }
        }
        public static MultiDimensionType? ToMultiDimType(this GraphicsType type)
        {
            switch (type)
            {
                case GraphicsType.Boolean: return MultiDimensionType.Boolean;
                case GraphicsType.Int: return MultiDimensionType.Int;
                case GraphicsType.UnsignedInt: return MultiDimensionType.UnsignedInt;
                case GraphicsType.Float: return MultiDimensionType.Float;
                case GraphicsType.Double: return MultiDimensionType.Double;

                default: return null;
            }
        }

        public static bool IsTypeGraphics(this Type type)
        {
            var isVector = type.IsVector();
            if (isVector) return true;
            return ToGraphicsType(type).TryStruct();
        }
        public static bool IsTypeMultiDim(this Type type)
        {
            return type.ToMultiDimType().TryStruct();
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
            var a = type.GetCustomAttribute<VectorAttribute>().TryClass(out attr);
            var b = IsAssignableToGenericType(type, typeof(IVector<>));

            return a && b;
        }
        public static bool IsVector(this Type type)
        {
            return type.IsVector(out _);
        }

        public static bool IsMatrix(this Type type, out MatrixAttribute attr)
        {
            var a = type.GetCustomAttribute<MatrixAttribute>().TryClass(out attr);
            var b = IsAssignableToGenericType(type, typeof(IMatrix<>));

            return a && b;
        }
        public static bool IsMatrix(this Type type)
        {
            return type.IsMatrix(out _);
        }

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }

            return givenType == genericType
              || givenType.MapsToGenericTypeDefinition(genericType)
              || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }
        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }
        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && givenType.IsGenericType
              && givenType.GetGenericTypeDefinition() == genericType;
        }
    }
}
