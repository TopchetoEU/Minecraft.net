using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinecraftNetWindow.Units;

namespace Tests
{
    public class ExampleClass
    {
        public float FloatProp1 { get; set; } = 102;
        public float FloatProp2 { get; set; } = 42;

        public ExampleClass(float fl1 = 102, float fl2 = 42)
        {
            FloatProp1 = fl1;
            FloatProp2 = fl2;
        }
    }

    [TestClass]
    public class InterpolateableTests
    {
        [TestMethod]
        public void FloatKeyframeTest()
        {
            var a = new InterpolateableFloat(1);

            var kf = a.ToKeyframe();

            Assert.IsTrue(kf.HasProperty("Value"), "Keyframe of an interpolateable doesn't contain \"Value\" property");

            Assert.IsTrue(kf["Value"] == a.Value, "interpolateable.Keyframe.Value isn't equal to the interpolateable.Value");
        }
        [TestMethod]
        public void PointFKeyframeTest()
        {
            var a = new InterpolateablePointF(1, 2);

            var kf = a.ToKeyframe();

            Assert.IsTrue(kf.HasProperties("X", "Y"), "Keyframe of an interpolateable doesn't have the required properties");

            Assert.IsTrue(kf["X"] == a.X, "interpolateable.Keyframe.X isn't equal to the interpolateable.X");
            Assert.IsTrue(kf["Y"] == a.Y, "interpolateable.Keyframe.Y isn't equal to the interpolateable.Y");

            kf["X"]++;

            var clone = (InterpolateablePointF)a.ApplyKeyframe(kf);

            Assert.IsFalse(a.X == clone.X, "Keyframe is applied to the interpolateable");
        }
        [TestMethod]
        public void SizeFKeyframeTest()
        {
            var a = new InterpolateableSizeF(1, 2);

            var kf = a.ToKeyframe();

            Assert.IsTrue(kf.HasProperties("Width", "Height"), "Keyframe of an interpolateable doesn't have the required properties");

            Assert.IsTrue(kf["Width"] == a.Width, "interpolateable.Keyframe.Width isn't equal to the interpolateable.Width");
            Assert.IsTrue(kf["Height"] == a.Height, "interpolateable.Keyframe.Height isn't equal to the interpolateable.Height");

            kf["Width"]++;

            var clone = (InterpolateableSizeF)a.ApplyKeyframe(kf);

            Assert.IsFalse(a.Width == clone.Width, "Keyframe is applied to the interpolateable");
        }
        [TestMethod]
        public void ObjectKeyframeTest()
        {
            var a = new InterpolateableObject<ExampleClass>(new ExampleClass());

            var kf = a.ToKeyframe();

            Assert.IsTrue(kf.HasProperties("FloatProp1", "FloatProp2"), "Keyframe of an interpolateable doesn't have the required properties");

            Assert.IsTrue(
                kf["FloatProp1"] == a.EncapsulatedObject.FloatProp1, 
                "interpolateable.Keyframe.Width isn't equal to the interpolateable.Width"
            );
            Assert.IsTrue(
                kf["FloatProp2"] == a.EncapsulatedObject.FloatProp2, 
                "interpolateable.Keyframe.Height isn't equal to the interpolateable.Height"
            );

            kf["FloatProp2"]++;

            var clone = (InterpolateableObject<ExampleClass>)a.ApplyKeyframe(kf);

            Assert.IsFalse(
                a.EncapsulatedObject.FloatProp2 == clone.EncapsulatedObject.FloatProp2,
                "Keyframe is applied to the interpolateable"
            );
        }
    } 
}