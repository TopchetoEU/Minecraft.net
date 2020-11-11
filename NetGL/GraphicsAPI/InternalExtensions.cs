using NetGL.WindowAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetGL.GraphicsAPI
{
    internal static partial class InternalExtensions
    {
        public static AttribPointer ToAttribPointer(this GraphicsType type)
        {
            return new AttribPointer(1, type);
        }
        public static AttribPointer? ToAttribPointer<T>(this T vector) where T : struct
        {
            if (vector.GetType().GetCustomAttribute<VectorAttribute>().TryClass(out var vecAttrib)) {
                return vecAttrib.ToAttribPointer();
            }
            else
                return null;
        }
        public static AttribPointer? ToAttribPointer(this Type type)
        {
            if (type.ToGraphicsType().TryStruct(out var gType))
                return gType.ToAttribPointer();
            else if (type.IsVector(out var vecAttr))
                return vecAttr.ToAttribPointer();
            else
                return null;
        }

        public static PropertyInfo[] GetValidProps(this Type type)
        {
            var a = type;
            var b = type.GetProperties();
            var c = b.Where(prop => {
                var a = !prop.GetCustomAttribute<IgonrePropertyAttribute>().TryClass(null);
                return a;
            });

            var d = c.Where(prop => {
                var b = prop.PropertyType.IsTypeGraphics();

                return b;
            });
            var e = d.ToArray();

            return e;
        }

        public static ValueType[] Flattern<T>(this T obj)
        {
            if (obj.GetType().IsVector(out var vecAttrib)) {
                return obj.GetType()
                    .GetProperties()
                    .Where(prop => prop.PropertyType == vecAttrib.Type.ToType())
                    .Select(prop => (attrib: prop.GetCustomAttribute<VectorDimensionAttribute>(), prop))
                    .Where(dimAttrib => dimAttrib.attrib != null)
                    .OrderBy(dimAttrib => dimAttrib.attrib.DimensionId)
                    .Select(dimAttrib => dimAttrib.prop)
                    .Select(dimAttrib => (ValueType)dimAttrib.GetValue(obj))
                    .ToArray();
            }
            else if (obj.GetType().IsTypeGraphics()) {
                return new ValueType[] { (ValueType)(object)obj };
            }
            else
                return null;
        }

        public static uint GetBufferElementSize(this Type type)
        {
            var size = (uint)0;

            foreach (var prop in type.GetValidProps()) {
                if (prop.PropertyType.GetSize().TryStruct(out var currSize, (uint)0)) {
                    size += currSize;
                }
            }

            return size;
        }

        public static Dictionary<uint, AttribPointer> ExtractAttribPointerMap(this Type type, ShaderProgram program)
        {
            var props = type.GetValidProps();

            var map = new Dictionary<uint, AttribPointer>();
            var i = (uint)0;

            foreach (var prop in props) {
                if (prop.PropertyType.ToAttribPointer().TryStruct(out var attribPointer)) {
                    map[LLGraphics.graphics_getAttribLocation(program.Id, prop.Name)] = attribPointer;
                    i++;
                }
            }

            return map;
        }
    }
}
