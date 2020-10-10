using System;

namespace NetGL.GraphicsAPI
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class IgonrePropertyAttribute: Attribute
    {
        public IgonrePropertyAttribute()
        {
        }
    }
}
