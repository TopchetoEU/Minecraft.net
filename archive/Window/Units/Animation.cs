using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNetWindow.Units
{
    public delegate KeyframeT InterpolationFunction<KeyframeT>(float state, KeyframeT[] keyframes) where KeyframeT: IInterpolateable;

    public class InterpolateableFloat: IInterpolateable
    {
        public float Value { get; set; }

        public Keyframe ToKeyframe()
        {
            var kf = new Keyframe();
            kf.Add("Value", Value);

            return kf;
        }

        public IInterpolateable ApplyKeyframe(Keyframe keyframe)
        {
            if (keyframe.HasProperty("Value"))
            {
                return new InterpolateableFloat(keyframe["Value"]);
            }
            else throw new ArgumentException("The keyframe provided doesn't have the property \"Value\"");
        }

        public static implicit operator InterpolateableFloat(float value)
        {
            return new InterpolateableFloat(value);
        }
        public static implicit operator float(InterpolateableFloat value)
        {
            return value.Value;
        }

        public InterpolateableFloat(float value)
        {
            Value = value;
        }
    }
    public class InterpolateablePointF: IInterpolateable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Keyframe ToKeyframe()
        {
            var kf = new Keyframe();
            kf.Add("X", X);
            kf.Add("Y", Y);

            return kf;
        }

        public IInterpolateable ApplyKeyframe(Keyframe keyframe)
        {
            if (keyframe.HasProperties("X", "Y"))
            {
                return (IInterpolateable)new InterpolateablePointF(keyframe["X"], keyframe["Y"]);
            }
            else throw new ArgumentException("The keyframe provided doesn't have the following properties: \"X\" and \"Y\"");
        }

        public static implicit operator InterpolateablePointF(PointF point)
        {
            return new InterpolateablePointF(point.X, point.Y);
        }
        public static implicit operator PointF(InterpolateablePointF value)
        {
            return new PointF(value.X, value.Y);
        }

        public InterpolateablePointF(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public class InterpolateableSizeF: IInterpolateable
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Keyframe ToKeyframe()
        {
            var kf = new Keyframe();
            kf.Add("Width", Width);
            kf.Add("Height", Height);

            return kf;
        }

        public IInterpolateable ApplyKeyframe(Keyframe keyframe)
        {
            if (keyframe.HasProperties("Width", "Height"))
            {
                return new InterpolateableSizeF(keyframe["Width"], keyframe["Height"]);
            }
            else throw new ArgumentException("The keyframe provided doesn't have the following properties: \"Width\" and \"Height\"");
        }

        public static implicit operator InterpolateableSizeF(SizeF point)
        {
            return new InterpolateableSizeF(point.Width, point.Height);
        }
        public static implicit operator SizeF(InterpolateableSizeF value)
        {
            return new SizeF(value.Width, value.Height);
        }

        public InterpolateableSizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
    public class InterpolateableObject<T>: IInterpolateable
    {
        public T EncapsulatedObject { get; set; }

        public IInterpolateable ApplyKeyframe(Keyframe keyframe)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var floatProps = props.Where(v => v.PropertyType == typeof(float));

            var propNames = floatProps.Select(v => v.Name).ToArray();

            if (!keyframe.HasProperties(propNames))
            {
                var propString = new StringBuilder();

                for (int i = 0; i < propNames.Length; i++)
                {
                    propString.Append("\"" + propNames[i] + "\"");
                    if (i == propNames.Length - 2)
                    {
                        propString.Append(" and ");
                    }
                    else if (i != propNames.Length - 1)
                    {
                        propString.Append(", ");
                    }
                }

                throw new ArgumentException(
                    "The keyframe provided doesn't have the following properties: " + propString.ToString()
                );
            }

            var newObject = EncapsulatedObject.MemberwiseClone();

            foreach (var property in floatProps)
            {
                property.SetValue(newObject, keyframe[property.Name]);
            }

            return new InterpolateableObject<T>(newObject);
        }

        public Keyframe ToKeyframe()
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var floatProps = props.Where(v => v.PropertyType == typeof(float));

            var keyframe = new Keyframe();

            foreach (var property in floatProps)
            {
                keyframe.Add(property.Name, (float)property.GetValue(EncapsulatedObject));
            }

            return keyframe;
        }

        public InterpolateableObject(T obj)
        {
            EncapsulatedObject = obj;
        }
    }

    public interface IAnimation<KeyframeT> where KeyframeT: IInterpolateable
    {
        float Duration { get; set; }
        List<KeyframeT> Keyframes { get; }
        InterpolationFunction<KeyframeT> InterpolationFunction { get; set; }

        KeyframeT Interpolate(float time);
    }

    public class Animation<KeyframeT>: IAnimation<KeyframeT> where KeyframeT : IInterpolateable
    {
        public float Duration { get; set; }

        public List<KeyframeT> Keyframes { get; } = new List<KeyframeT>();
        public InterpolationFunction<KeyframeT> InterpolationFunction { get; set; }

        public KeyframeT Interpolate(float time)
        {
            float state = (time / Duration);

            return InterpolationFunction(state, Keyframes.ToArray());
        }

        public Animation(float duration, InterpolationFunction<KeyframeT> function, params KeyframeT[] keyframes)
        {
            Duration = duration;
            Keyframes = keyframes.ToList();
            InterpolationFunction = function;
        }
        public Animation()
        {
            Duration = 1;
            InterpolationFunction = InterpolationFunctions<KeyframeT>.LinearInterpolation;
        }
    }

    public static class InterpolationFunctions<KeyframeT> where KeyframeT: IInterpolateable
    {
        public static readonly InterpolationFunction<KeyframeT> LinearInterpolation =
            delegate (float state, KeyframeT[] keyframes)
            {
                var properties = keyframes[0].ToKeyframe().GetPropertyNames();
                var frame = (int)Math.Floor(state);

                if (properties.Count() <= 0)
                {
                    return keyframes[frame];
                }

                var newKeyframe = keyframes[frame].ToKeyframe();

                foreach (var property in properties)
                {
                    var sum = 0f;
                    var count = 0f;

                    for (int i = 0; i < keyframes.Length; i++)
                    {
                        var value = keyframes[i].ToKeyframe()[property];
                        var distance = state - i;
                        var weight = 1 / distance;

                        sum += value * weight;
                        count += weight;
                    }

                    newKeyframe[property] = sum / count;
                }

                return (KeyframeT)keyframes[frame].ApplyKeyframe(newKeyframe);
            };
    }
}
